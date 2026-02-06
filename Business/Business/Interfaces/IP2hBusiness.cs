using Models.Models.P2h;
using Models.ModelsDto.Payload;

namespace Business.Interfaces
{
    public interface IP2hBusiness
    {
        Task<P2hGetResponse> GetDespachoAsync(string autorizacion);
        Task<P2hGetResponse?> SendDespachoAsync(PayloadRoot payload);
        Task<P2hGetResponse?> GetDispatchAsync(string autorizacion);
    }
}
