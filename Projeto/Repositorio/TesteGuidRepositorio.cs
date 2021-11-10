using Dominio.Models;
using Framework.Data.Repository.Common;
using Framework.Data.Repository.Interfaces;
using System;

namespace Repositorio
{
    public class TesteGuidRepositorio : Repository<TesteGuid, Guid>
    {
        public TesteGuidRepositorio(IDataContext dataContext) : base(dataContext)
        { }
    }
}