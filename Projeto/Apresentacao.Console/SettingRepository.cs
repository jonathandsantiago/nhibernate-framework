using Framework.Data.Repository.Common;
using Framework.Data.Repository.Interfaces;

namespace Apresentacao.Console
{
    public class SettingRepository : Repository<Setting, int>, ISettingRepository
    {
        public SettingRepository(IDataContext dataContext)
            : base(dataContext)
        { }
    }
}