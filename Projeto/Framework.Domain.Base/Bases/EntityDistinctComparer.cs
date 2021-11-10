using Framework.Domain.Base.Helper;
using Framework.Domain.Base.Interfaces;
using System.Collections.Generic;

namespace Framework.Domain.Base.Bases
{
    public class EntityDistinctComparer<T> : IEqualityComparer<T>
       where T : IEntity
    {
        public bool Equals(T x, T y)
        {
            return EntityHelper.Equals(x, y, true);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetId().GetHashCode();
        }
    }
}
