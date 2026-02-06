using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ModelsDto.Payload
{
    public class PayloadRoot
    {
        public Payload payload { get; set; } = null!;
    }
    public class Payload
    {
        public string? numDespacho { get; set; }
        public string? cdFormula { get; set; }
        public string? cdTipoIdPaciente { get; set; }
        public string? dniPaciente { get; set; }
        public string? cdIpsEmite { get; set; }
        public string? cdPuntoVenta { get; set; }
        public string? nombrePuntoVenta { get; set; }
        public DateTimeOffset? feSolicitud { get; set; }
        public DateTimeOffset? feDespacho { get; set; }
        public int? cdEstadoFormula { get; set; }
        public decimal? nmValorPagado { get; set; }
        public bool? boVentaDirecta { get; set; }
        public string? nmTiempoEspera { get; set; }
        public int? cdTipoOperacion { get; set; }
        public int? cdOrigenFormula { get; set; }
        public string? sistemaOrigen { get; set; }
        public string? cdTipoIdFarmacia { get; set; }
        public string? dniFarmacia { get; set; }
        public List<ListaMedicamentos>? listaMedicamentos { get; set; }
    }
    public class ListaMedicamentos
    {
        public int? cdMotivoEntregaParcial { get; set; }
        public string? cdPluFarmacia { get; set; }
        public string? cdSura { get; set; }
        public DateTimeOffset? feInicioDespacho { get; set; }
        public DateTimeOffset? feFinDespacho { get; set; }
        public int? nmCantidadEntregaAcum { get; set; }
    }
}
