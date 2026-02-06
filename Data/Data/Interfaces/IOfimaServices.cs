using Models.ModelsDto.OfimaDto;
using System.Data;

namespace Data.Interfaces
{
    public interface IOfimaServices
    {
        Task<List<FacturaOfimaDto>> ObtenerFacturasMvAsync(string ordenMv, string cliente);
        Task<DataTable> ObtenerDatosAsync();

    }
}
