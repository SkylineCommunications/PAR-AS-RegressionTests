namespace Library.TestCases
{
	using System;
	using Library.Tests.TestCases;
	using QAPortalAPI.Models.ReportingModels;
	using Skyline.DataMiner.Automation;

	public class DarwinTimespanTestCase : ITestCase
	{
		public DarwinTimespanTestCase(string name, TimeSpan timeSinceLastMessage)
		{
			if (String.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("name");
			}

			Name = name;
			TimeSinceLastMessage = timeSinceLastMessage;
		}

		public string Name { get; set; }

		public TestCaseReport TestCaseReport { get; private set; }

		public PerformanceTestCaseReport PerformanceTestCaseReport { get; }

		public TimeSpan TimeSinceLastMessage { get; set; }

		public void Execute(IEngine engine)
		{
			if (TimeSinceLastMessage.TotalSeconds < 60)
			{
				TestCaseReport = TestCaseReport.GetSuccessTestCase(Name);
			}
			else
			{
				TestCaseReport = TestCaseReport.GetFailTestCase(Name, String.Format("{0} failed, expected  last message to be less than a minute, received {2}", Name, TimeSinceLastMessage.TotalSeconds));
			}
		}
	}
}