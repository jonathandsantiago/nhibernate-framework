using Dominio.Models;
using Dominio.ViewMode;
using Framework.Service.Distributed.Interfaces;
using System;

namespace ServicoDistribuido
{
    public interface ITesteGuidServicoDistribuido : IAppServiceDistributedBase<TesteGuid, TesteGuidViewModel, Guid>
    { }
}
