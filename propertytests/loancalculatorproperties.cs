using System.Linq;
using FsCheck;
using FsCheck.Xunit;
using FsCheckPropertySamples;
using Xunit;

namespace PropertyTests
{

    public static class PeopleGenerators
    {
        public static string[] Names =
        {
            "Bob", "Chuck", "Kenworth", "Shirley",
            "Susan", "Dawn", "Tim", "Tammy",
            "Sir Purrs Alot", "Heather",
            "Hamilton", "Kenny"
        };

        public static Gen<string> ValidName()
        {
            return Gen.Elements(Names);
        }

        public static Gen<int> AgeUnder18()
        {
            return Gen.Choose(0, 17);
        }

        public static Gen<int> AgeOver30()
        {
            return Gen.Choose(31, 110);
        }

        public static Gen<int> AgeBetween30And50()
        {
            return Gen.Choose(30, 50);
        }

        public static Gen<RiskFactor> NoneOrLowRisk()
        {
            return Gen.Frequency(new[]
            {
                    System.Tuple.Create(1, Gen.Constant(RiskFactor.None)),
                    System.Tuple.Create(4, Gen.Constant(RiskFactor.Low))
            });
        }


        public static Gen<Person> AnyPerson()
        {
            return from age in Arb.Default.PositiveInt().Generator
                   from name in ValidName()
                   from risk in Arb.Generate<RiskFactor>()
                   select new Person(name, age.Get, risk);
        }

        public static Gen<Person> Over30WithNoneOrLowRisk()
        {
            return from age in AgeOver30()
                   from name in ValidName()
                   from risk in NoneOrLowRisk()
                   select new Person(name, age, risk);
        }

        public static Gen<Person> PersonUnder18()
        {
            return from age in AgeUnder18()
                   from name in ValidName()
                   from risk in Arb.Generate<RiskFactor>()
                   select new Person(name, age, risk);
        }

        public static Gen<Person> PersonWithHighRisk()
        {
            return from age in Arb.Default.PositiveInt().Generator
                   from name in ValidName()
                   select new Person(name, age.Get, RiskFactor.High);
        }
    }

    public static class PersonUnder18
    {
        public static Arbitrary<Person> Generate()
        {
            return PeopleGenerators.PersonUnder18().ToArbitrary();
        }
    }



    public class LoanCalculatorProperties
    {
        [Fact]
        public void PersonUnder18ShouldBeUnapprovedProperty()
        {
            Prop.ForAll(PeopleGenerators.PersonUnder18().ToArbitrary(),
                    p => LoanCalculator.Calculate(p).ApprovalStatus == ApprovalStatus.Unapproved)
                .QuickCheckThrowOnFailure();
        }



        [Property(Verbose = true, Arbitrary = new[] { typeof(PersonUnder18) })]
        public void PersonUnder18ShouldBeUnapprovedProperty_2(Person p)
        {
            var status = LoanCalculator.Calculate(p).ApprovalStatus;
            Assert.Equal(ApprovalStatus.Unapproved, status);
        }
    }
}