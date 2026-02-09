using Business.Interfaces;
using Data.Interfaces;
using Models.ModelsDto.OfimaDto;

namespace Business.Business.P2hBll
{
    public class OfimaBusiness : IOfimaBusiness
    {
        private readonly IOfimaServices _repository;

        public OfimaBusiness(IOfimaServices ofservice)
        {
            _repository = ofservice;
        }

        public Task<List<FacturaOfimaDto>> ObtenerFacturasMvAsync(string ordenMv, string cliente)
        {
            return _repository.ObtenerFacturasMvAsync(ordenMv, cliente);
        }
    }
}
