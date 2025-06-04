namespace Library.Consts
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Text;

	internal static class TestInfoConsts
	{
		internal static string Contact => "squad.deploy-genesis@skyline.be";

		internal static List<int> ProjectIds => new List<int> { 18432 };
	}

	internal static class TechexDarwinConstants
	{
		internal static string DarwinElementName => "DARWIN UAT B";

		internal static string DarwinPesSwitch => "PESA669";

		internal static int PesSwitchTablePid => 4000;

		internal static int PesSwitchIdPid_4001 => 4001;

		internal static int PesSwitchActionPid_4007 => 4007;

		internal static int PesSwitchSwitchActionSourceIdx_4008 => 7;

		internal static int PesSwitchSwitchForceValueIdx_4009 => 8;

		internal static int PesSwitchActionDependencyValuesIdx_4011 => 10;

		internal static int PesSwitchKafkaMetricsTablePid => 50000;

		internal static int PesSwitchKafkaMetricsConfigurationKeyPid_50011 => 50011;

		internal static int PesSwitchKafkaMetricsCurrentSourceIdx_50008 => 7;

		internal static int PesSwitchKafkaMetricsConfigurationKeyIdx_50011 => 10;
	}
}
