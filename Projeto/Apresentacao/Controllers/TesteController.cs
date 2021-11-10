using Dominio.Models;
using Dominio.ViewMode;
using Framework.Presentation.Mvc.Common;
using Servico;
using ServicoDistribuido;

namespace ApresentacaoFramework.Controllers
{
    public class TesteController : BaseController<Teste, TesteViewModel, int, ITesteServico>
    {
        public TesteController(ITesteServico servico) : base(servico)
        { }
    }
}