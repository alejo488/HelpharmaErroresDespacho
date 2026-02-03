namespace ErrorDespacho.Models.ModelsDto.OfimaDto
{
    public class OrdenDto
    {
        public DateTime? OFFh_Entrega { get; set; }
        public string? OFOrdenMv { get; set; }
        public int? OFCantidad { get; set; }
        public decimal? OFVlr_Unitario { get; set; }
        public decimal? OFTotal_Prod { get; set; }
        public decimal? OFIVA { get; set; }
        public string? OFMYPRES { get; set; }
        public string? OFIdMipres { get; set; }
        public string? OFCUFE { get; set; }
    }
}
