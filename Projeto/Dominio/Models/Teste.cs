using Framework.Domain.Base;

namespace Dominio.Models
{
    public class Teste : Entity<int>
    {
        public virtual string Nome { get; set; }
    }
}
