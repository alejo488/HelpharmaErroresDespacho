using AutoMapper;
using Business.Interfaces;
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
        private readonly IP2hBusiness _p2h;
        private readonly IOfimaBusiness _iOfimaBusiness;
        private readonly IMapper _mapper;

        public HomeController(IP2hBusiness p2hService, IOfimaBusiness iOfimaService, IMapper mapper)
        {
            _p2h = p2hService;
            _iOfimaBusiness = iOfimaService;
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

                // ===== Inconsistencias(col 23) =====
                var textoInconsistencia = fila.Cell(24).GetString()?.Trim();

                // Formato esperado:
                // 90418-(E)-PRESTADOR-LA FECHA DE ENTREGA NO COINCIDE...
                string? codigoInconsistencia = null;
                string? clasificacionInconsistencia = null;
                string? descripcionInconsistencia = null;

                if (!string.IsNullOrWhiteSpace(textoInconsistencia))
                {
                    var match = System.Text.RegularExpressions.Regex.Match(
                        textoInconsistencia,
                        @"^(?<codigo>\d+)-\((?<clasif>[^)]+)\)-(?<desc>.+)$");

                    if (match.Success)
                    {
                        codigoInconsistencia = match.Groups["codigo"].Value.Trim();
                        clasificacionInconsistencia = match.Groups["clasif"].Value.Trim();
                        descripcionInconsistencia = match.Groups["desc"].Value.Trim();
                    }
                }

                // ===== Tipos =====
                fila.Cell(10).TryGetValue(out int cantEntregada);
                fila.Cell(15).TryGetValue(out decimal costoUnitario);
                fila.Cell(17).TryGetValue(out decimal totalCuota);
                fila.Cell(18).TryGetValue(out DateTime fechaDespacho);
                fila.Cell(19).TryGetValue(out decimal valorRecetario);

                // ===== DTO =====
                var inc = new InconsistenciaExcelDto
                {
                    DWTipoRegistro = fila.Cell(1).GetString(),
                    DWNumeroRecetario = fila.Cell(2).GetString(),
                    DWCodigoIpsEmite = fila.Cell(3).GetString(),
                    DWConsecutivoAutorizacion = fila.Cell(4).GetString(),
                    DWCodigoTipoPrestacion = fila.Cell(5).GetString(),
                    DWCodigoOrigenAutorizacion = fila.Cell(6).GetString(),

                    // Se construye, NO se lee
                    DWOrden = $"{fila.Cell(3).GetString()}-{fila.Cell(4).GetString()}{fila.Cell(5).GetString()}{fila.Cell(6).GetString()}",

                    DWTipoIdAfiliado = fila.Cell(7).GetString(),
                    DWNroIdAfiliado = fila.Cell(8).GetString(),
                    DWMedicamento = fila.Cell(9).GetString(),
                    DWCantEntregada = fila.Cell(10).IsEmpty() ? null : cantEntregada,
                    DWNroOrden = fila.Cell(11).GetString(),
                    DWNroIdMedico = fila.Cell(12).GetString(),
                    DWDiagnostico = fila.Cell(13).GetString(),
                    DWFarmacia = fila.Cell(14).GetString(),
                    DWCostoUnitario = fila.Cell(15).IsEmpty() ? null : costoUnitario,
                    DWPlu = fila.Cell(16).GetString(),
                    DWTotalCuotaModeradora = fila.Cell(17).IsEmpty() ? null : totalCuota,
                    DWFechaDespacho = fila.Cell(18).IsEmpty() ? null : fechaDespacho,
                    DWValorRecetario = fila.Cell(19).IsEmpty() ? null : valorRecetario,
                    DWClasificacionIngresos = fila.Cell(20).GetString(),
                    DWConsecutivoAutorizacionEnRisc = fila.Cell(21).GetString(),
                    DWEstadoDelRegistro = fila.Cell(22).GetString(),

                    DWCodigoInconsistencia = codigoInconsistencia,
                    DWClasificacionInconsistencia = clasificacionInconsistencia,
                    DWDescripcionInconsistencia = descripcionInconsistencia
                };

                resultado.Add(inc);
            }

            foreach (var item in resultado)
            {
                var despacho = await _p2h.GetDespachoAsync(item.DWOrden);
                item.P2hResponse = despacho;

                var facDto = _iOfimaBusiness.ObtenerFacturasMvAsync(item.DWOrden,item.DWNroIdAfiliado).Result;

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

                var responseGet = _p2h.GetDispatchAsync(item.DWOrden).Result;//queda pendiente por saber esto que hace

                item.FacturaOfimaDto.AddRange(facDto);
            }


            return resultado;
        }


    }
}
