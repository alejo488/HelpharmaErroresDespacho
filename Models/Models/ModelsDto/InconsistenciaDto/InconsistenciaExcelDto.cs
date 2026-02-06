using Models.Converter;
using Models.Models.P2h;
using Models.ModelsDto.OfimaDto;
using System.Text.Json.Serialization;

namespace Models.ModelsDto.InconsistenciaDto
{
    public class InconsistenciaExcelDto : OrdenDto
    {
        public string? DWTipoRegistro { get; set; }
        public string? DWNumeroRecetario { get; set; }
        public string? DWCodigoIpsEmite { get; set; }
        public string? DWConsecutivoAutorizacion { get; set; }
        public string? DWCodigoTipoPrestacion { get; set; }
        public string? DWCodigoOrigenAutorizacion { get; set; }
        public string? DWOrden { get; set; }
        public string? DWTipoIdAfiliado { get; set; }
        public string? DWNroIdAfiliado { get; set; }
        public string? DWMedicamento { get; set; }
        public int? DWCantEntregada { get; set; }
        public string? DWNroOrden { get; set; }
        public string? DWNroIdMedico { get; set; }
        public string? DWDiagnostico { get; set; }
        public string? DWFarmacia { get; set; }
        public decimal? DWCostoUnitario { get; set; }
        public string? DWPlu { get; set; }
        public decimal? DWTotalCuotaModeradora { get; set; }
        public DateTime? DWFechaDespacho { get; set; }
        public decimal? DWValorRecetario { get; set; }
        public string? DWClasificacionIngresos { get; set; }
        public string? DWConsecutivoAutorizacionEnRisc { get; set; }
        public string? DWEstadoDelRegistro { get; set; }
        public string? DWCodigoInconsistencia { get; set; }
        public string? DWClasificacionInconsistencia { get; set; }
        public string? DWDescripcionInconsistencia { get; set; }

        [JsonPropertyName("getResponse")]
        [JsonConverter(typeof(FlexibleBoolConverter))]
        public P2hGetResponse? P2hResponse { get; set; }
        public List<FacturaOfimaDto>? FacturaOfimaDto { get; set; }

        public int? P2hResponseCode { get; set; }
        public string? P2hResponseMessage { get; set; }
        public bool P2hHasPayload { get; set; }

        public InconsistenciaExcelDto()
        {
            FacturaOfimaDto = new List<FacturaOfimaDto>();
        }
    }
}
