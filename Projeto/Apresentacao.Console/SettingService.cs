using Framework.Application.Service.Common;
using Framework.Application.Service.Interfaces;

namespace Apresentacao.Console
{
    public class SettingService : AppServiceBase<Setting, int, ISettingRepository, ISettingValidador>
    {
        public SettingService(IServiceUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _repository = new SettingRepository(unitOfWork.Data);
        }
    }
}