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
	using System.Linq;
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
				var darwinElement = allElements.FirstOrDefault(x => x.Protocol.Name == "Techex Darwin" && x.Protocol.Version == "Production" );

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
					new DarwinTestCase(
						String.Format("Test case for Kafka Module {0}", kafkaModule[0]),
						timeSinceLastMessage));
				}

				// Sending test cases to QA Portal
				darwinTest.Execute(engine);
				darwinTest.PublishResults(engine);
			}
			catch (Exception e)
			{
				engine.ExitFail("Run|Something went wrong: " + e);
			}
		}
	}
}