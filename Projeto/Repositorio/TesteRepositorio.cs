using Dominio.Models;
using Framework.Data.Repository.Common;
using Framework.Data.Repository.Interfaces;

namespace Repositorio
{
    public class TesteRepositorio : Repository<Teste, int>
    {
        public TesteRepositorio(IDataContext dataContext) : base(dataContext)
        { }
    }
}