using Models.ModelsDto.OfimaDto;

namespace Business.Interfaces
{

    public interface IOfimaBusiness
    {
        Task<List<FacturaOfimaDto>> ObtenerFacturasMvAsync(string ordenMv, string cliente);
    }
}
