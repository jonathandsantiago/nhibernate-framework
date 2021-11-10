using Framework.Validator.Validation;
using Framework.Validator.Validation.Interfaces;

namespace Dominio.Models
{
    public class TesteValidador : Validation<Teste>, ITesteValidador
    {
        public TesteValidador()
        {
            base.AddRule(new ValidationRule<Teste>(c => string.IsNullOrEmpty(c.Nome), TesteMensagem.NomeObrigatorio));
        }

        public bool Validar()
        {
            Result.Add("teste");
            return Result.IsValid;
        }

        public bool ValidarTexto(string texto)
        {
            if (texto.Equals("teste"))
            {
                Result.Add("teste já cadastrado");
            }

            return Result.IsValid;
        }
    }

    public interface ITesteValidador : IValidation<Teste>
    {
        bool Validar();
        bool ValidarTexto(string texto);
    }
}