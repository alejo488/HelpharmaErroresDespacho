using Business.Interfaces;
using Data.Interfaces;
using Models.Models.P2h;
using Models.ModelsDto.Payload;

namespace Business.Business.P2hBll
{
    public class P2hBusiness : IP2hBusiness
    {

        private readonly IP2hService _repository;
        public P2hBusiness(IP2hService ph)
        {
            _repository = ph;
        }

        public Task<P2hGetResponse> GetDespachoAsync(string autorizacion)
        {
           return _repository.GetDespachoAsync(autorizacion);
        }

        public Task<P2hGetResponse?> GetDispatchAsync(string autorizacion)
        {
            return _repository.GetDispatchAsync(autorizacion);
        }

        public Task<P2hGetResponse?> SendDespachoAsync(PayloadRoot payload)
        {
            return _repository.SendDespachoAsync(payload);
        }
    }
}
