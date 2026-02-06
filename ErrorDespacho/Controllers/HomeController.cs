using AutoMapper;
using ClosedXML.Excel;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.ModelsDto.InconsistenciaDto;
using Models.ModelsDto.Payload;
using System.Text.Json;

namespace ErrorDespacho.Controllers
{
    public class HomeController : Controller
    {
        private readonly IP2hService _p2h;
        private readonly IOfimaServices _iOfimaService;
        private readonly IMapper _mapper;

        public HomeController(IP2hService p2hService, IOfimaServices iOfimaService, IMapper mapper)
        {
            _p2h = p2hService;
            _iOfimaService = iOfimaService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CargarSoportes(IFormFile archivoExcel)
        {
            if (archivoExcel == null || archivoExcel.Length == 0)
                return BadRequest("Debe seleccionar un archivo Excel");

            if (!Path.GetExtension(archivoExcel.FileName)
                    .Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest("El archivo debe ser un Excel (.xlsx)");

            var registros = await LeerExcelInconsistencias(archivoExcel);

            return Ok(registros);
        }


        private async Task<List<InconsistenciaExcelDto>> LeerExcelInconsistencias(IFormFile archivo)
        {
            var resultado = new List<InconsistenciaExcelDto>();
            var listaEncontrados = new List<InconsistenciaExcelDto>();
            var listaFallidos = new List<InconsistenciaExcelDto>();


            using var stream = archivo.OpenReadStream();
            using var workbook = new XLWorkbook(stream);

            var hoja = workbook.Worksheets.FirstOrDefault();
            if (hoja == null)
                return resultado;

            foreach (var fila in hoja.RowsUsed().Skip(1)) // fila 1 = encabezados
            {
                // Si no hay ORDEN, se ignora la fila
                if (fila.Cell(7).IsEmpty())
                    continue;

                // Columna combinada de inconsistencias (24)
                var textoInconsistencia = fila.Cell(24).GetString();
                var partes = textoInconsistencia.Split('-', 4);

                fila.Cell(11).TryGetValue(out int cantEntregada);
                fila.Cell(16).TryGetValue(out decimal costoUnitario);
                fila.Cell(18).TryGetValue(out decimal totalCuota);
                fila.Cell(19).TryGetValue(out DateTime fechaDespacho);
                fila.Cell(20).TryGetValue(out decimal valorRecetario);

                InconsistenciaExcelDto inc = new InconsistenciaExcelDto();
                inc.DWTipoRegistro = fila.Cell(1).GetString();
                inc.DWNumeroRecetario = fila.Cell(2).GetString();
                inc.DWCodigoIpsEmite = fila.Cell(3).GetString();
                inc.DWConsecutivoAutorizacion = fila.Cell(4).GetString();
                inc.DWCodigoTipoPrestacion = fila.Cell(5).GetString();
                inc.DWCodigoOrigenAutorizacion = fila.Cell(6).GetString();
                inc.DWOrden = $"{inc.DWCodigoIpsEmite}-{inc.DWConsecutivoAutorizacion}{inc.DWCodigoTipoPrestacion}{inc.DWCodigoOrigenAutorizacion}";
                inc.DWTipoIdAfiliado = fila.Cell(8).GetString();
                inc.DWNroIdAfiliado = fila.Cell(9).GetString();
                inc.DWMedicamento = fila.Cell(10).GetString();
                inc.DWCantEntregada = fila.Cell(11).IsEmpty() ? null : cantEntregada;
                inc.DWNroOrden = fila.Cell(12).GetString();
                inc.DWNroIdMedico = fila.Cell(13).GetString();
                inc.DWDiagnostico = fila.Cell(14).GetString();
                inc.DWFarmacia = fila.Cell(15).GetString();
                inc.DWCostoUnitario = fila.Cell(16).IsEmpty() ? null : costoUnitario;
                inc.DWPlu = fila.Cell(17).GetString();
                inc.DWTotalCuotaModeradora = fila.Cell(18).IsEmpty() ? null : totalCuota;
                inc.DWFechaDespacho = fila.Cell(19).IsEmpty() ? null : fechaDespacho;
                inc.DWValorRecetario = fila.Cell(20).IsEmpty() ? null : valorRecetario;
                inc.DWClasificacionIngresos = fila.Cell(21).GetString();
                inc.DWConsecutivoAutorizacionEnRisc = fila.Cell(22).GetString();
                inc.DWEstadoDelRegistro = fila.Cell(23).GetString();
                inc.DWCodigoInconsistencia = partes.ElementAtOrDefault(0)?.Trim();
                inc.DWClasificacionInconsistencia = partes.ElementAtOrDefault(1)?
                                                .Replace("(", "")
                                                .Replace(")", "")
                                                .Trim();
                inc.DWDescripcionInconsistencia = partes.ElementAtOrDefault(3)?.Trim();
                resultado.Add(inc);
            }

            foreach (var item in resultado)
            {
                var despacho = await _p2h.GetDespachoAsync(item.DWOrden);
                item.P2hResponse = despacho;

                var facDto = _iOfimaService.ObtenerFacturasMvAsync(item.DWOrden,item.DWNroIdAfiliado).Result;

                var order = despacho.DispatchOrders.ElementAtOrDefault(0);
                if (order?.Payload == null)
                    continue;

                var payloadDto = JsonSerializer.Deserialize<PayloadDto>(
                    order.Payload,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

                payloadDto.CdEstadoFormula = 2;//error por estado
                payloadDto.ListaMedicamentos[0].NmCantidadEntregaAcum = item.DWCantEntregada;//error por cantidad

                payloadDto.FeDespacho = item.DWFechaDespacho.ToString();//error por fecha
                payloadDto.ListaMedicamentos[0].FeFinDespacho = item.DWFechaDespacho.ToString();//error por fecha
                PayloadRoot pyloadSend = new PayloadRoot
                {
                    payload = _mapper.Map<Payload>(payloadDto)

                };
               var response= _p2h.SendDespachoAsync(pyloadSend);
                var responseGet = _p2h.GetDispatchAsync(item.DWOrden).Result;

                item.FacturaOfimaDto.AddRange(facDto);
            }


            return resultado;
        }


    }
}
