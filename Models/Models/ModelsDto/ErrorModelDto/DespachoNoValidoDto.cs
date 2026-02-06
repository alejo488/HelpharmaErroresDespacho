using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ModelsDto.ErrorModelDto
{
    public class DespachoNoValidoDto
    {
        public string Orden { get; set; }
        public int Estado { get; set; } = 1;
        public int EstadoDespacho { get; set; } = 2;
        public DateTime FechaNotificacion { get; set; }

        public DespachoNoValidoDto(DateTime fecha)
        {
            Estado = 1;
            EstadoDespacho = 2;

            var fechaConHoraFija = new DateTimeOffset(
                fecha.Year,
                fecha.Month,
                fecha.Day,
                10, 0, 0,
                TimeSpan.FromHours(-5)
            );

            FechaNotificacion = fechaConHoraFija.DateTime;
        }

    }
}
