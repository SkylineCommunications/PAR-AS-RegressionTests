namespace Library.TestCases
{
	using System;
	using Library.Tests.TestCases;
	using QAPortalAPI.Models.ReportingModels;
	using Skyline.DataMiner.Automation;

	public class DarwinStringTestCase : ITestCase
	{
		public DarwinStringTestCase(string name, string receivedValue, string expectedValue)
		{
			if (String.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("name");
			}

			Name = name;
			ReceivedValue = receivedValue;
			ExpectedValue = expectedValue;
		}

		public string Name { get; set; }

		public TestCaseReport TestCaseReport { get; private set; }

		public PerformanceTestCaseReport PerformanceTestCaseReport { get; }

		public string ReceivedValue { get; set; }

		public string ExpectedValue { get; set; }

		public void Execute(IEngine engine)
		{
			if (ReceivedValue == ExpectedValue)
			{
				TestCaseReport = TestCaseReport.GetSuccessTestCase(Name);
			}
			else
			{
				TestCaseReport = TestCaseReport.GetFailTestCase(Name, $"{Name} failed, expected {ExpectedValue}, received {ReceivedValue}");
			}
		}
	}
}