using Framework.Domain.Base;
using Framework.Validator.Interfaces;
using System;

namespace Dominio.Models
{
    public class TesteGuid : Entity<Guid>
    {
        public virtual string Nome { get; set; }
        public virtual int Index { get; set; }
    }
}
