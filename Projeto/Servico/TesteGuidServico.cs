using Dominio.Models;
using Framework.Application.Service.Common;
using Framework.Application.Service.Interfaces;
using Repositorio;
using System;
using System.Threading.Tasks;

namespace Servico
{
    public class TesteGuidServico : AppServiceBase<TesteGuid, Guid, TesteGuidRepositorio, ITesteGuidValidador>, ITesteGuidServico
    {
        public TesteGuidServico(IServiceUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _repository = new TesteGuidRepositorio(unitOfWork.Data);
            AddValidator<ITesteGuidValidador, TesteGuidValidador>();
            AddValidator<ITesteValidador, TesteValidador>();
        }

        public override bool Insert(TesteGuid entity)
        {
            if (!GetValidator<ITesteValidador>().ValidarTexto(entity.Nome))
            {
                return false;
            }

            return base.Insert(entity);
        }

        public override bool Save(TesteGuid entity)
        {
            if (!GetValidator<ITesteValidador>().ValidarTexto(entity.Nome))
            {
                return false;
            }

            return base.Save(entity);
        }

        public override async Task<bool> SaveAsync(TesteGuid entity)
        {
            if (!GetValidator<ITesteValidador>().ValidarTexto(entity.Nome))
            {
                return false;
            }

            return await base.SaveAsync(entity);
        }
    }
}