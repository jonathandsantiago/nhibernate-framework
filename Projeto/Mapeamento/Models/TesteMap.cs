using Dominio.Models;
using Framework.Data.Nhibernate.Mapping;

namespace Mapeamento.Models
{
    public class TesteMap : EntityIdMap<int, Teste>
    {
        public TesteMap()
            : base()
        {
            Table("TBGERTESTE");
            MapIndex(c => c.Nome).Not.Nullable();
        }
    }
}