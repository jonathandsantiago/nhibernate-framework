using Framework.Validator.Validation;
using Framework.Validator.Validation.Interfaces;

namespace Dominio.Models
{
    public class TesteGuidValidador : Validation<TesteGuid>, ITesteGuidValidador
    {
        public TesteGuidValidador()
        {
            base.AddRule(new ValidationRule<TesteGuid>(c => string.IsNullOrEmpty(c.Nome), TesteMensagem.NomeObrigatorio));
        }
    }

    public interface ITesteGuidValidador : IValidation<TesteGuid>
    { }
}