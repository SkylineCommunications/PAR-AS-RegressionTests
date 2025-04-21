using System.Runtime.CompilerServices;

namespace Library.Consts
{
	internal class Constants
	{
		public const string Source1Name = "LMC-ENC-B104-RX-AWS";
		public const string Source1StreamFK = "65facedf1be229a121ab7170/3c4b1bcd3981c13d4c6758c3f819cc59094d4f81529cb29b";
		public const string Source1StreamID = "3c4b1bcd3981c13d4c6758c3f819cc59094d4f81529cb29b";
		public const string Source2Name = "LMC-ENC-B101-RX-AWS";
		public const string Source2StreamFK = "65facedf1be229a121ab7170/dfdafc123fddec083195d7fc70994d80964a18c0f9a880a2";
		public const string Source2StreamID = "dfdafc123fddec083195d7fc70994d80964a18c0f9a880a2";

		public const string Output1Name = "TX612";
		public const string Output1ID = "65facedf1be229a121ab7170/cff69cc75f366dd30cce8034c656e186fb70cff286249838";
		public const string Output1StreamFK = "65facedf1be229a121ab7170/dfdafc123fddec083195d7fc70994d80964a18c0f9a880a2";
		public const string Output1StreamID = "dfdafc123fddec083195d7fc70994d80964a18c0f9a880a2";

		public const string Output2Name = "TX613";
		public const string Output2ID = "65facedf1be229a121ab7170/d553b85578b3acba81c42930321fe98707565eb0994739cc";
		public const string Output2StreamFK = "65facedf1be229a121ab7170/dfdafc123fddec083195d7fc70994d80964a18c0f9a880a2";
		public const string Output2StreamID = "dfdafc123fddec083195d7fc70994d80964a18c0f9a880a2";

		public const string TestName = "Regression Test - MWCore SetCrosspoint";
		public const string TestDescription = "Regression test that connects a stream to an output and then verifies that the crosspoint was set";
	}
}