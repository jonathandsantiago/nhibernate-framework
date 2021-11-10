using Dominio.Models;
using Dominio.ViewMode;
using Framework.Application.Service.Interfaces;
using Framework.Service.Distributed.Common;
using Servico;

namespace ServicoDistribuido
{
    public class TesteServicoDistribuido : AppServiceDistributedBase<Teste, TesteViewModel, int, TesteServico>, ITesteServicoDistribuido
    {
        public TesteServicoDistribuido(IServiceUnitOfWork unitOfWork) : base(unitOfWork)
        { }
    }
}