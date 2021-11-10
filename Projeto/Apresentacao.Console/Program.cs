using Framework.Application.Service.Common;
using Framework.Data.Nhibernate;

namespace Apresentacao.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            NhibernateSessionFactory.Initializar(true, DBType.SqlLite);
            SettingService appService = new SettingService(new ServiceUnitOfWork());
            string name = appService.Get(3)?.DataBaseName;
            System.Console.WriteLine($"{name}");

            Setting setting = new Setting();
            setting.Type = SettingType.SqlServer;
            appService.Save(setting);
            System.Console.ReadLine();
        }
    }
}
