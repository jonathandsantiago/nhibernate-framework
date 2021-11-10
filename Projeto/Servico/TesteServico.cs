using Dominio.Models;
using Framework.Application.Service.Common;
using Framework.Application.Service.Interfaces;
using Repositorio;

namespace Servico
{
    public class TesteServico : AppServiceBase<Teste, int, TesteRepositorio, ITesteValidador>, ITesteServico
    {
        public TesteServico(IServiceUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _repository = new TesteRepositorio(unitOfWork.Data);
            AddValidator<ITesteValidador, TesteValidador>();
        }

        protected override bool Validate(Teste entity)
        {
            return base.Validate(entity) || (GetValidator<TesteValidador>()?.Validar() ?? true);
        }
    }
}