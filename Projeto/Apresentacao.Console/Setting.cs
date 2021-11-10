using Framework.Domain.Base;

namespace Apresentacao.Console
{
    public class Setting : Entity<int>
    {
        public virtual string UrlApi { get; set; }
        public virtual string TokenApi { get; set; }
        public virtual string DataBaseName { get; set; }
        public virtual string Password { get; set; }
        public virtual string ServerName { get; set; }
        public virtual SettingType Type { get; set; }
        public virtual string Login { get; set; }
        public virtual string ConnectionString { get; set; }

        public Setting()
        {
            Type = SettingType.Odbc;
        }
    }
}
