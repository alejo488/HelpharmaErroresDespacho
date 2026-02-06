using AutoMapper;
using Models.ModelsDto.Payload;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ErrorDespacho.Mapper
{
    public class PayloadProfile : Profile
    {
        public PayloadProfile()
        {
            CreateMap<PayloadDto, Payload>();
            CreateMap<Medicamento, ListaMedicamentos>();
        }
    }
}
