/*
****************************************************************************
*  Copyright (c) 2025,  Skyline Communications NV  All Rights Reserved.    *
****************************************************************************

By using this script, you expressly agree with the usage terms and
conditions set out below.
This script and all related materials are protected by copyrights and
other intellectual property rights that exclusively belong
to Skyline Communications.

A user license granted for this script is strictly for personal use only.
This script may not be used in any way by anyone without the prior
written consent of Skyline Communications. Any sublicensing of this
script is forbidden.

Any modifications to this script by the user are only allowed for
personal use and within the intended purpose of the script,
and will remain the sole responsibility of the user.
Skyline Communications will not be responsible for any damages or
malfunctions whatsoever of the script resulting from a modification
or adaptation by the user.

The content of this script is confidential information.
The user hereby agrees to keep this confidential information strictly
secret and confidential and not to disclose or reveal it, in whole
or in part, directly or indirectly to any person, entity, organization
or administration without the prior written consent of
Skyline Communications.

Any inquiries can be addressed to:

	Skyline Communications NV
	Ambachtenstraat 33
	B-8870 Izegem
	Belgium
	Tel.	: +32 51 31 35 69
	Fax.	: +32 51 31 01 29
	E-mail	: info@skyline.be
	Web		: www.skyline.be
	Contact	: Ben Vandenberghe

****************************************************************************
Revision History:

DATE		VERSION		AUTHOR			COMMENTS

dd/mm/2025	1.0.0.1		XXX, Skyline	Initial version
****************************************************************************
*/

namespace RT_PAR_TECHEX_DARWIN_1
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Threading;
	using Library.Consts;
	using Library.TestCases;
	using Library.Tests;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Core.DataMinerSystem.Automation;
	using Skyline.DataMiner.Core.DataMinerSystem.Common;
	using Skyline.DataMiner.Net.Messages;
	using DmsElementState = Skyline.DataMiner.Core.DataMinerSystem.Common.ElementState;

	/// <summary>
	/// Represents a DataMiner Automation script.
	/// </summary>
	public class Script
	{
		private const string TestName = "RT_PAR_TECHEX_DARWIN";
		private const string TestDescription = "Regression Test to Techex Darwin Flows.";

		/// <summary>
		/// The script entry point.
		/// </summary>
		/// <param name="engine">Link with SLAutomation process.</param>
		public void Run(IEngine engine)
		{
			try
			{
				var dms = engine.GetDms();

				// Initializing regression Test
				Test darwinTest = new Test(TestName, TestDescription);

				// Getting First Darwin element
				var allElements = dms.GetElements();
				var darwinElement = allElements.FirstOrDefault(x => x.Protocol.Name == "Techex Darwin" && x.Name == TechexDarwinConstants.DarwinElementName);

				KafkaUptimeTest(darwinTest, darwinElement);

				PesSwitchTest(engine, darwinTest, darwinElement);

				// Sending test cases to QA Portal
				darwinTest.Execute(engine);
				darwinTest.PublishResults(engine);
			}
			catch (Exception e)
			{
				engine.ExitFail("Run|Something went wrong: " + e);
			}
		}

		private static void KafkaUptimeTest(Test darwinTest, IDmsElement darwinElement)
		{
			// Process last Kafka message per Module
			var now = DateTime.Now;
			var modulesTable = darwinElement.GetTable(13000);

			var activeKafkaModules = modulesTable.QueryData(new List<ColumnFilter>
				{
					new ColumnFilter
					{
						Pid = 13004,
						ComparisonOperator = ComparisonOperator.Equal,
						Value = "1", // Enabled
					},
				});

			foreach (var kafkaModule in activeKafkaModules)
			{
				var lastProcessing = DateTime.FromOADate(Convert.ToDouble(kafkaModule[4]));
				var timeSinceLastMessage = now - lastProcessing;

				darwinTest.AddTestCase(
				new DarwinTimespanTestCase(
					String.Format("Test case for Kafka Module {0}", kafkaModule[0]),
					timeSinceLastMessage));
			}
		}

		private static void PesSwitchTest(IEngine engine, Test darwinTest, IDmsElement darwinElement)
		{
			var pesSwitchName = TechexDarwinConstants.DarwinPesSwitch;
			var pesSwitchTable = darwinElement.GetTable(TechexDarwinConstants.PesSwitchTablePid);
			var testPesSwitchConfig = pesSwitchTable.QueryData(new List<ColumnFilter>
				{
					new ColumnFilter
					{
						Pid = TechexDarwinConstants.PesSwitchIdPid_4001,
						ComparisonOperator = ComparisonOperator.Equal,
						Value = pesSwitchName,
					},
				}).ToArray();

			if (testPesSwitchConfig.Length == 1)
			{
				var pesSwitchRow = testPesSwitchConfig[0];
				var currentSource = Convert.ToString(pesSwitchRow[TechexDarwinConstants.PesSwitchSwitchActionSourceIdx_4008]);
				var availabeSources = new List<string>(Convert.ToString(pesSwitchRow[TechexDarwinConstants.PesSwitchActionDependencyValuesIdx_4011]).Split(';'));
				if (availabeSources.Contains(currentSource))
				{
					availabeSources.Remove(currentSource);
				}

				var newSource = availabeSources.FirstOrDefault();
				if (!String.IsNullOrEmpty(newSource))
				{
					pesSwitchRow[TechexDarwinConstants.PesSwitchSwitchActionSourceIdx_4008] = newSource;
					pesSwitchRow[TechexDarwinConstants.PesSwitchSwitchForceValueIdx_4009] = 0;

					pesSwitchTable.SetRow(TechexDarwinConstants.DarwinPesSwitch, pesSwitchRow);
					var engineElement = engine.FindElement(darwinElement.Name);
					engineElement.SetParameterByPrimaryKey(TechexDarwinConstants.PesSwitchActionPid_4007, TechexDarwinConstants.DarwinPesSwitch, 4);
					Thread.Sleep(60000);
					var pesMetricsTable = darwinElement.GetTable(TechexDarwinConstants.PesSwitchKafkaMetricsTablePid);
					var testPesSwitchMetrics = pesMetricsTable.QueryData(new List<ColumnFilter>
						{
							new ColumnFilter
							{
								Pid = TechexDarwinConstants.PesSwitchKafkaMetricsConfigurationKeyPid_50011,
								ComparisonOperator = ComparisonOperator.Equal,
								Value = TechexDarwinConstants.DarwinPesSwitch,
							},
						}).ToArray();

					foreach (var metricsRow in testPesSwitchMetrics)
					{
						darwinTest.AddTestCase(
							new DarwinStringTestCase(
								$"Test case for PES Switch {TechexDarwinConstants.DarwinPesSwitch} from {currentSource} to {newSource}",
								Convert.ToString(metricsRow[TechexDarwinConstants.PesSwitchKafkaMetricsCurrentSourceIdx_50008]),
								newSource));
					}

					pesSwitchRow[TechexDarwinConstants.PesSwitchSwitchActionSourceIdx_4008] = currentSource;
					pesSwitchRow[TechexDarwinConstants.PesSwitchSwitchForceValueIdx_4009] = 0;

					pesSwitchTable.SetRow(TechexDarwinConstants.DarwinPesSwitch, pesSwitchRow);
					engineElement.SetParameterByPrimaryKey(TechexDarwinConstants.PesSwitchActionPid_4007, TechexDarwinConstants.DarwinPesSwitch, 4);
				}
			}
		}
	}
}