using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ModelsDto.OfimaDto
{
    public class FacturaOfimaDto
    {
        public DateTime? FECORDEN { get; set; } = null;
        public string? Cliente { get; set; } = null;

        public decimal? Cant_Original { get; set; } = null;
        public decimal? Total_Prod { get; set; } = null;
        public decimal? Vlr_Unitario { get; set; } = null;

        public DateTime? Fecha_Inicial { get; set; } = null;
        public DateTime? Fecha_Final { get; set; } = null;

        public decimal? Cantidad { get; set; } = null;

        public DateTime? Fh_Entrega { get; set; } = null;
        public DateTime? Fecha_Dcto { get; set; } = null;
    }
}
