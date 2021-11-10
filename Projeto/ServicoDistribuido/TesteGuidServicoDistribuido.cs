using Dominio.Models;
using Dominio.ViewMode;
using Framework.Application.Service.Interfaces;
using Framework.Service.Distributed.Common;
using Servico;
using System;

namespace ServicoDistribuido
{
    public class TesteGuidServicoDistribuido : AppServiceDistributedBase<TesteGuid, TesteGuidViewModel, Guid, TesteGuidServico>, ITesteGuidServicoDistribuido
    {
        public TesteGuidServicoDistribuido(IServiceUnitOfWork unitOfWork) : base(unitOfWork)
        { }
    }
}