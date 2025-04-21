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
	using Skyline.Automation.Utils;
	using Skyline.DataMiner.Automation;
	using Skyline.Automation.Utils.MWCore;
	using Skyline.DataMiner.Utils.ConnectorAPI.Techex.MWCore;
	using Skyline.DataMiner.Core.DataMinerSystem.Common.Selectors;
	using Constants = Library.Consts.Constants;
	using System.Security.Policy;

	public class SetCrosspoint : ITestCase
	{
		public SetCrosspoint(string name, MWCoreElement mwCore, List<ColumnFilter> updatedStreamFilter)
		{
			Name = name;
			MWCore = mwCore;
			UpdatedStreamFilter = updatedStreamFilter;
		}

		public string Name { get; set; }
		public PerformanceTestCaseReport PerformanceTestCaseReport { get; }
		public TestCaseReport TestCaseReport { get; private set; }
		private List<ColumnFilter> UpdatedStreamFilter { get; set; }


		private MWCoreElement MWCore { get; set; }


		public void Execute(IEngine engine)
		{
			try
			{
				
				//if (_table == null)
				//{
				//	TestCaseReport = TestCaseReport.GetFailTestCase($"Foreign Keys validation for {Name}", "Table not found.");
				//	return;
				//}

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

		private bool RunTestCaseSetCrosspoint(IEngine engine, int maxAttempts = 3)
		{
			try
			{
				var sourcesTable = MWCore.MWCoreDms.GetTable((int)ParameterPid.InputsTable);
				var destinationsTable = MWCore.MWCoreDms.GetTable((int)ParameterPid.OutputsTable);
				var element = new MWCoreInterAppCalls(engine.GetUserConnection(), MWCore.ElementName);
				var outputMessage = Utils.CreateMessage(Constants.Output1ID, Constants.Source1StreamID, InterAppAction.Update, InterAppResourceType.Output);
				var response = element.SendSingleResponseMessage(outputMessage);

				if (string.IsNullOrEmpty(response.Error))
				{
					if (destinationsTable.QueryData(UpdatedStreamFilter).Any())
					{
						engine.GenerateInformation($"RegressionTest|SetCrosspoint|Output-Stream connection successfully created.");
						return true;
					}
				}
				else
				{
					engine.GenerateInformation($"Response error {response.Error} resource dk {response.Resource}");
				}

				return false;
			}
			catch (Exception ex)
			{
				engine.GenerateInformation($"RegressionTest|Couldn't complete Stream Creation. Exception: {ex}");
				return false;
			}
		}
	}
}
