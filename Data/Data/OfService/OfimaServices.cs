using Data.Interfaces;
using Microsoft.Data.SqlClient;
using Models.ModelsDto.OfimaDto;
using System.Data;
using System.Text;
using Microsoft.Extensions.Configuration;


namespace Data.OfService
{
    public class OfimaServices : IOfimaServices
    {
        private readonly string _connectionString;

        public OfimaServices(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OfimaConnection");
        }

        public Task<DataTable> ObtenerDatosAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<FacturaOfimaDto>> ObtenerFacturasMvAsync(string ordenMv, string cliente)
        {
            var resultado = new List<FacturaOfimaDto>();

            const string sql = @"
            SELECT
                FECORDEN,
                Cliente,
                Cant_Original,
                Total_Prod,
                Vlr_Unitario,
                Fecha_Inicial,
                Fecha_Final,
                Cantidad,
                Fh_Entrega,
                Fecha_Dcto
            FROM [HELPHARMA].[dbo].[J_vReporteMvFacturas]
            WHERE OrdenMv = @OrdenMv
              AND Cliente = @Cliente
              AND ISNULL(T_Fact_Remi, '') = ''
            ORDER BY FECORDEN DESC";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.Add("@OrdenMv", SqlDbType.VarChar).Value = ordenMv;
            command.Parameters.Add("@Cliente", SqlDbType.VarChar).Value = cliente;

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var fact = new FacturaOfimaDto
                {
                    FECORDEN = reader.IsDBNull("FECORDEN") ? null : reader.GetDateTime("FECORDEN"),
                    Cliente = reader.IsDBNull("Cliente") ? null : reader.GetString("Cliente"),

                    Cant_Original = reader.IsDBNull("Cant_Original") ? null : reader.GetDecimal("Cant_Original"),
                    Total_Prod = reader.IsDBNull("Total_Prod") ? null : reader.GetDecimal("Total_Prod"),
                    Vlr_Unitario = reader.IsDBNull("Vlr_Unitario") ? null : reader.GetDecimal("Vlr_Unitario"),

                    Fecha_Inicial = reader.IsDBNull("Fecha_Inicial") ? null : reader.GetDateTime("Fecha_Inicial"),
                    Fecha_Final = reader.IsDBNull("Fecha_Final") ? null : reader.GetDateTime("Fecha_Final"),

                    Cantidad = reader.IsDBNull("Cantidad") ? null : reader.GetDecimal("Cantidad"),

                    Fh_Entrega = reader.IsDBNull("Fh_Entrega") ? null : reader.GetDateTime("Fh_Entrega"),
                    Fecha_Dcto = reader.IsDBNull("Fecha_Dcto") ? null : reader.GetDateTime("Fecha_Dcto")
                };

                resultado.Add(fact);
            }

            return resultado;
        }
    }
}
