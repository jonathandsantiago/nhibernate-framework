using Framework.Validator.Validation;
using Framework.Validator.Validation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apresentacao.Console
{
  public  class SettingValidador : Validation<Setting>, ISettingValidador
    {
        public SettingValidador()
        {        }
    }

    public interface ISettingValidador : IValidation<Setting>
    {}
}