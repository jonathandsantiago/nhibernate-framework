using Dominio.Models;
using Dominio.ViewMode;
using Framework.Service.Distributed.Interfaces;

namespace ServicoDistribuido
{
    public interface ITesteServicoDistribuido : IAppServiceDistributedBase<Teste, TesteViewModel, int>
    { }
}
