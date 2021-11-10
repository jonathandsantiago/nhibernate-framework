using Framework.Data.Nhibernate.Mapping;

namespace Apresentacao.Console
{
    public class SettingMap : EntityIdMap<int, Setting>
    {
        public SettingMap()
            : base()
        {
            Map(c => c.UrlApi);
            Map(c => c.TokenApi);
            Map(c => c.DataBaseName);
            Map(c => c.Password);
            Map(c => c.ServerName);
            Map(c => c.Type).CustomType<SettingType>().Not.Nullable();
            Map(c => c.Login);
            Map(c => c.ConnectionString);
        }
    }
}