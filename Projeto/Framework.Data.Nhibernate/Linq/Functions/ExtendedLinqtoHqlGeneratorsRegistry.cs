using NHibernate.Linq.Functions;

namespace Framework.Data.Nhibernate.Linq.Functions
{

    public class ExtendedLinqtoHqlGeneratorsRegistry : DefaultLinqToHqlGeneratorsRegistry
    {
        public ExtendedLinqtoHqlGeneratorsRegistry()
        {
            this.Merge(new AddDaysGenerator());
            this.Merge(new BirthdayGenerator());
            this.Merge(new InGenerator());
            this.Merge(new TryConvertIntGenerator());
            this.Merge(new AddTimeSpanGenerator());
            this.Merge(new SubtractTimeSpanGenerator());
            this.Merge(new IsCurrentDateGenerator());
            this.Merge(new BetweenGenerator());
            this.Merge(new GetAgeGenerator());
            this.Merge(new IsLessOrEqualCurrentDateTimeGenerator());
            this.Merge(new DiffTotalHoursGenerator());
            this.Merge(new DiffTotalDaysGenerator());
            this.Merge(new IsNullGenerator());
        }
    }
}
