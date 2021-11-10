using Dominio.Models;
using Framework.Application.Service.Interfaces;
using System;

namespace Servico
{
    public interface ITesteGuidServico : IAppServiceBase<TesteGuid, Guid>
    { }
}