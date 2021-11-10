using NHibernate;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Data.Nhibernate.Interceptors
{
    public class NoLockInterceptor : EmptyInterceptor
    {
        public override SqlString OnPrepareStatement(SqlString sql)
        {
            if (sql.StartsWithCaseInsensitive("select"))
            {
                IList<string> parts = sql.ToString().Split().ToList();
                string fromItem = parts.FirstOrDefault(p => p.Trim().Equals("from", StringComparison.OrdinalIgnoreCase));
                int fromIndex = fromItem != null ? parts.IndexOf(fromItem) : -1;
                string whereItem = parts.FirstOrDefault(p => p.Trim().Equals("where", StringComparison.OrdinalIgnoreCase));
                int whereIndex = whereItem != null ? parts.IndexOf(whereItem) : parts.Count;

                if (fromIndex == -1)
                {
                    return sql;
                }

                parts.Insert(parts.IndexOf(fromItem) + 3, "WITH (NOLOCK)");

                for (int i = fromIndex; i < whereIndex; i++)
                {
                    if (parts[i - 1].Equals(","))
                    {
                        parts.Insert(i + 3, "WITH (NOLOCK)");
                        i += 3;
                    }

                    if (parts[i].Trim().Equals("on", StringComparison.OrdinalIgnoreCase))
                    {
                        parts[i] = "WITH (NOLOCK) on";
                    }
                }

                sql = SqlString.Parse(string.Join(" ", parts));
            }

            return sql;
        }
    }
}
