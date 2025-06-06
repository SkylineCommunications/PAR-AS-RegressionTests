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

namespace RT_PAR_MWCoreTests_1
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Text;
	using Skyline.DataMiner.Automation;

	using Library.Tests;
	using RT_PAR_MWCoreTests_1;
	using RT_PAR_MWCoreTests_1.TestCases;
	using Skyline.Automation.Utils.MWCore;
	using Skyline.DataMiner.Core.DataMinerSystem.Common;
	using Constants = Library.Consts.Constants;

	/// <summary>
	/// Represents a DataMiner Automation script.
	/// </summary>
	public class Script
	{
		private const string TestName = "RT_PAR_MWCoreTests";
		private const string TestDescription = "Regression Test to validate crosspoint connection.";
		private List<ColumnFilter> UpdateCrosspointsFilter { get; set; }

		private MWCoreElement MWCore { get; set; }
		/// <summary>
		/// The Script entry point.
		/// </summary>
		/// <param name="engine">Link with SLAutomation process.</param>
		public void Run(IEngine engine)
		{
			try
			{

				UpdateCrosspointsFilter = new List<ColumnFilter>
				{
					new ColumnFilter{ Pid = 8908, Value = Constants.Source1StreamFK }
				};
				Test test = new Test(TestName, TestDescription);
				var element = engine.GetScriptParam("MWCore Element").Value;

				MWCore = new MWCoreElement(engine, element);
				test.AddTestCase(new SetCrosspoint(TestName, MWCore, UpdateCrosspointsFilter));

				test.Execute(engine);
				test.PublishResults(engine);

				//Test myTest = new Test(TestName, TestDescription);
				//myTest.AddTestCase(
				//	new TestCaseExample("Test 1"),
				//	new TestCaseExample("Test 2"));

				//myTest.Execute(engine);
				//myTest.PublishResults(engine);
			}
			catch (Exception e)
			{
				engine.Log($"{TestName} failed: {e}");
			}
			finally
			{
				// TODO: add cleanup here (if applicable)
			}
		}
	}
}