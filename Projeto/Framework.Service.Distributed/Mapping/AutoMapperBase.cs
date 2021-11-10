using AutoMapper;

namespace Framework.Service.Distributed.Mapping
{
    public class AutoMapperBase
    {
        private static AutoMapperBase instance;

        public static AutoMapperBase Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AutoMapperBase();
                }

                return instance;
            }
        }

        public static void Configure()
        {
            Instance.Initialize();
            Instance.AssertConfigurationIsValid();
        }

        protected virtual void Initialize()
        {
            Mapper.Initialize(cfg => GetConfiguration(cfg));
        }

        protected virtual void AssertConfigurationIsValid()
        {
            Mapper.AssertConfigurationIsValid();
        }

        protected virtual void GetConfiguration(IMapperConfigurationExpression cfg)
        { }
    }
}
