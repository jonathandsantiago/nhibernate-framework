using AutoMapper;
using Framework.Service.Distributed.Mapping;

namespace ServicoDistribuido
{
    public class AutoMapperConfiguration : AutoMapperBase
    {
        protected override void GetConfiguration(IMapperConfigurationExpression cfg)
        {
            cfg.AddProfile<TesteMapProfile>();
        }
    }
}
