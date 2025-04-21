namespace RT_PAR_MWCoreTests_1.TestCases
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	using Skyline.DataMiner.Core.DataMinerSystem.Common;
	using Library.Tests.TestCases;
	using QAPortalAPI.Models.ReportingModels;
	using Skyline.DataMiner.Automation;

	public class SetCrosspoint : ITestCase
	{

		private readonly IDmsTable _table;
		private readonly Dictionary<IDmsTable, IDictionary<string, object[]>> _allTableData;

		public string Name { get; set; }
		public PerformanceTestCaseReport PerformanceTestCaseReport { get; }
		public TestCaseReport TestCaseReport { get; private set; }


		public void Execute(IEngine engine)
		{
			try
			{
				if (_table == null)
				{
					TestCaseReport = TestCaseReport.GetFailTestCase($"Foreign Keys validation for {Name}", "Table not found.");
					return;
				}

				//var missingRelations = ValidateTableRelations();

				//TestCaseReport = missingRelations.Any()
				//	? TestCaseReport.GetFailTestCase($"Foreign Keys validation for {Name}", string.Join("| ", missingRelations))
				//	: TestCaseReport.GetSuccessTestCase($"Foreign Keys validation for {Name}");
			}
			catch (Exception e)
			{
				TestCaseReport = TestCaseReport.GetFailTestCase($"Foreign Keys validation for {Name}", $"Exception: {e}");
			}
		}
	}
}
