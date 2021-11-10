using Framework.Data.Repository.Interfaces;
using System;

namespace Framework.Data.Repository.Common
{
    public abstract class RepositoryBase
    {
        protected readonly IDataContext _dataContext;

        protected RepositoryBase(IDataContext dataContext)
        {
            this._dataContext = dataContext ?? throw new ArgumentNullException("unitOfWork");
        }
    }
}
