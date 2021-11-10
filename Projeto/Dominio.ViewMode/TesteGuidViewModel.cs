using Framework.Domain.Model.Common;
using System;

namespace Dominio.ViewMode
{
    public class TesteGuidViewModel : ModelBase<Guid>
    {
        public string Nome { get; set; }
        public int Index { get; set; }
    }
}
