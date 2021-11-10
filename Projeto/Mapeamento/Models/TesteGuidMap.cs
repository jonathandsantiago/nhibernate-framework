using Dominio.Models;
using Framework.Data.Nhibernate.Mapping;
using System;

namespace Mapeamento.Models
{
    public class TesteGuidMap : EntityIdMap<Guid, TesteGuid>
    {
        public TesteGuidMap()
            : base()
        {
            Table("TBGERTESTEGUID");
            MapIndex(c => c.Nome).Not.Nullable();
        }
    }
}