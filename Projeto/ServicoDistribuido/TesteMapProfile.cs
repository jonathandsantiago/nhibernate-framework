using AutoMapper;
using Dominio.Models;
using Dominio.ViewMode;

namespace ServicoDistribuido
{
    public class TesteMapProfile : Profile
    {
        public TesteMapProfile()
        {
            CreateMap<Teste, TesteViewModel>()
                .ReverseMap();

            CreateMap<TesteGuid, TesteGuidViewModel>()
                .ReverseMap();
        }
    }
}
