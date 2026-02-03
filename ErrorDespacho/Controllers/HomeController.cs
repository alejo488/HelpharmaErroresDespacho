using ClosedXML.Excel;
using ErrorDespacho.Models.ModelsDto.InconsistenciaDto;
using ErrorDespacho.Models.P2h;
using ErrorDespacho.Services.P2HServices;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ErrorDespacho.Controllers
{
    public class HomeController : Controller
    {
        private readonly P2hService _p2h;

        public HomeController(P2hService p2hService)
        {
            _p2h = p2hService;
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

                resultado.Add(new InconsistenciaExcelDto
                {
                    DWTipoRegistro = fila.Cell(1).GetString(),
                    DWNumeroRecetario = fila.Cell(2).GetString(),
                    DWCodigoIpsEmite = fila.Cell(3).GetString(),
                    DWConsecutivoAutorizacion = fila.Cell(4).GetString(),
                    DWCodigoTipoPrestacion = fila.Cell(5).GetString(),
                    DWCodigoOrigenAutorizacion = fila.Cell(6).GetString(),
                    DWOrden = fila.Cell(7).GetString(),
                    DWTipoIdAfiliado = fila.Cell(8).GetString(),
                    DWNroIdAfiliado = fila.Cell(9).GetString(),
                    DWMedicamento = fila.Cell(10).GetString(),
                    DWCantEntregada = fila.Cell(11).IsEmpty() ? null : cantEntregada,
                    DWNroOrden = fila.Cell(12).GetString(),
                    DWNroIdMedico = fila.Cell(13).GetString(),
                    DWDiagnostico = fila.Cell(14).GetString(),
                    DWFarmacia = fila.Cell(15).GetString(),
                    DWCostoUnitario = fila.Cell(16).IsEmpty() ? null : costoUnitario,
                    DWPlu = fila.Cell(17).GetString(),
                    DWTotalCuotaModeradora = fila.Cell(18).IsEmpty() ? null : totalCuota,
                    DWFechaDespacho = fila.Cell(19).IsEmpty() ? null : fechaDespacho,
                    DWValorRecetario = fila.Cell(20).IsEmpty() ? null : valorRecetario,
                    DWClasificacionIngresos = fila.Cell(21).GetString(),
                    DWConsecutivoAutorizacionEnRisc = fila.Cell(22).GetString(),
                    DWEstadoDelRegistro = fila.Cell(23).GetString(),
                    DWCodigoInconsistencia = partes.ElementAtOrDefault(0)?.Trim(),
                    DWClasificacionInconsistencia = partes.ElementAtOrDefault(1)?
                                                    .Replace("(", "")
                                                    .Replace(")", "")
                                                    .Trim(),
                    DWDescripcionInconsistencia = partes.ElementAtOrDefault(3)?.Trim()
                });
            }

            foreach (var item in resultado)
            {
                var despacho = await _p2h.GetDespachoAsync(item.DWOrden);
                item.P2hResponse = despacho;
            }


            return resultado;
        }


    }
}
