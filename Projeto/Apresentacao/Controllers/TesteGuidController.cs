using Dominio.Models;
using Dominio.ViewMode;
using Framework.Presentation.Mvc.Common;
using Servico;
using ServicoDistribuido;
using System;

namespace ApresentacaoFramework.Controllers
{
    public class TesteGuidController : BaseController<TesteGuid, TesteGuidViewModel, Guid, ITesteGuidServico>
    {
        public TesteGuidController(ITesteGuidServico servico) : base(servico)
        { }
    }
}