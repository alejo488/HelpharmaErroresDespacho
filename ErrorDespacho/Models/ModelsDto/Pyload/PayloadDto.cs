using System.Text.Json.Serialization;

namespace ErrorDespacho.Models.ModelsDto.Pyload
{

    public class PayloadDto
    {
        [JsonPropertyName("numDespacho")]
        public string? NumDespacho { get; set; }

        [JsonPropertyName("cdFormula")]
        public string? CdFormula { get; set; }

        [JsonPropertyName("cdTipoIdPaciente")]
        public string? CdTipoIdPaciente { get; set; }

        [JsonPropertyName("dniPaciente")]
        public string? DniPaciente { get; set; }

        [JsonPropertyName("cdIpsEmite")]
        public string? CdIpsEmite { get; set; }

        [JsonPropertyName("cdPuntoVenta")]
        public string? CdPuntoVenta { get; set; }

        [JsonPropertyName("nombrePuntoVenta")]
        public string? NombrePuntoVenta { get; set; }

        [JsonPropertyName("feSolicitud")]
        public string? FeSolicitud { get; set; }

        [JsonPropertyName("feDespacho")]
        public string? FeDespacho { get; set; }

        [JsonPropertyName("cdEstadoFormula")]
        public int? CdEstadoFormula { get; set; }

        [JsonPropertyName("nmValorPagado")]
        public decimal? NmValorPagado { get; set; }

        [JsonPropertyName("boVentaDirecta")]
        public bool? BoVentaDirecta { get; set; }

        [JsonPropertyName("nmTiempoEspera")]
        public string? NmTiempoEspera { get; set; }

        [JsonPropertyName("cdTipoOperacion")]
        public int? CdTipoOperacion { get; set; }

        [JsonPropertyName("cdOrigenFormula")]
        public int? CdOrigenFormula { get; set; }

        [JsonPropertyName("sistemaOrigen")]
        public string? SistemaOrigen { get; set; }

        [JsonPropertyName("cdTipoIdFarmacia")]
        public string? CdTipoIdFarmacia { get; set; }

        [JsonPropertyName("dniFarmacia")]
        public string? DniFarmacia { get; set; }

        [JsonPropertyName("listaMedicamentos")]
        public List<Medicamento>? ListaMedicamentos { get; set; }
    }

    public class Medicamento
    {
        [JsonPropertyName("cdSura")]
        public string? CdSura { get; set; }

        [JsonPropertyName("cdPluFarmacia")]
        public string? CdPluFarmacia { get; set; }

        [JsonPropertyName("nmCantidadEntregaAcum")]
        public int? NmCantidadEntregaAcum { get; set; }

        [JsonPropertyName("feInicioDespacho")]
        public string? FeInicioDespacho { get; set; }

        [JsonPropertyName("feFinDespacho")]
        public string? FeFinDespacho { get; set; }

        [JsonPropertyName("cdMotivoEntregaParcial")]
        public int? CdMotivoEntregaParcial { get; set; }
    }


}
