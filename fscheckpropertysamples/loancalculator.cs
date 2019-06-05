using System;

namespace FsCheckPropertySamples
{
    public enum RiskFactor
    {
        None,
        Low,
        Average,
        High
    }

    public class Person
    {
        public Person(string name, int age, RiskFactor risk)
        {
            Name = name;
            Age = age;
            RiskFactor = risk;
        }
        public string Name { get; }
        public int Age { get; }
        public RiskFactor RiskFactor { get; }

        public override string ToString()
        {
            return $"Name: {Name}\nAge: {Age}\nRisk Factor: {RiskFactor}";
        }
    }

    public enum ApprovalStatus
    {
        Approved,
        Unapproved
    }

    public class LoanResults
    {
        public LoanResults(ApprovalStatus status, decimal interestRate)
        {
            ApprovalStatus = status;
            InterestRate = interestRate;
        }
        public decimal InterestRate { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }

    public class LoanCalculator
    {
        public static LoanResults Calculate(Person p)
        {
            if (p == null)
                return new LoanResults(ApprovalStatus.Unapproved, 0m);

            LoanResults result;
            if (p.Name.Equals("kevin", StringComparison.CurrentCultureIgnoreCase))
                result = new LoanResults(ApprovalStatus.Approved, 1.1m);

            else if (p.Age > 30 && (p.RiskFactor == RiskFactor.None || p.RiskFactor == RiskFactor.Low))
            {
                result = p.RiskFactor == RiskFactor.Low 
                    ? new LoanResults(ApprovalStatus.Approved, 1.7m) 
                    : new LoanResults(ApprovalStatus.Approved, 1.2m);
            }
            else if (30 < p.Age && p.Age < 50 && p.RiskFactor == RiskFactor.Average)
            {
                result = new LoanResults(ApprovalStatus.Approved, 2.02m);
            }
            else
            {
                result = new LoanResults(ApprovalStatus.Unapproved, 0m);
            }

            return result;
        }
    }
}
