namespace Skyline.Automation.Utils
{
	using System;
	using Newtonsoft.Json;
	using Skyline.Automation.Utils.MWCore;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Core.DataMinerSystem.Automation;
	using Skyline.DataMiner.Core.DataMinerSystem.Common;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Net.Messages.Advanced;
	using Skyline.DataMiner.Utils.ConnectorAPI.Techex.MWCore.InterAppMessages.Messages;
	using Skyline.DataMiner.Utils.ConnectorAPI.Techex.MWCore;

	namespace MWCore
	{
		public class MWEdgeStream
		{
			public MWEdgeStream(MWCoreElement mwcore, string mwedgeName)
			{
				MwEdgeName = mwedgeName;

				var table = mwcore.MWCoreDms.GetTable((int)ParameterPid.MwEdgesTable);
				var primaryKey = table.GetPrimaryKey(mwedgeName);
				var row = table.GetRow(primaryKey);
				MwEdgeGuid = Convert.ToString(row[0]);
				MwEdgeIpAddress = Convert.ToString(row[9]);
			}

			public string EdgeStreamGuid { get; set; }

			public string StreamGuid { get; set; }

			public string EdgeInterAppStreamGuid { get; set; }

			public string InterAppStreamGuid { get; set; }

			public string MwEdgeGuid { get; set; }

			public string InterAppMwEdgeGuid { get; set; }

			public string MwEdgeName { get; set; }

			public string MwEdgeIpAddress { get; set; }
		}

		public class MWCoreElement
		{
			public MWCoreElement(IEngine engine, string element)
			{
				ElementName = element;
				Engine = engine;
				MWCore = engine.FindElement(ElementName)
					?? throw new ElementNotFoundException($"Couldn't find MW Core element with name {ElementName}");
				Dms = Engine.GetDms();
				MWCoreDms = Dms.GetElement(ElementName);
			}

			public Element MWCore { get; }

			public IDmsElement MWCoreDms { get; }

			public string ElementName { get; }

			private IEngine Engine { get; }

			private IDms Dms { get; }
		}
	}

	namespace InputStream
	{
		public class FailoverTriggers
		{
			[JsonProperty("zeroBitrate")]
			public bool ZeroBitrate { get; set; }

			[JsonProperty("TSSyncLoss")]
			public bool TSSyncLoss { get; set; }

			[JsonProperty("lowBitrateThreshold")]
			public int LowBitrateThreshold { get; set; }

			[JsonProperty("CCErrorsInPeriodThreshold")]
			public int CCErrorsInPeriodThreshold { get; set; }

			[JsonProperty("CCErrorsInPeriodTime")]
			public int CCErrorsInPeriodTime { get; set; }

			[JsonProperty("missingPacketsInPeriodThreshold")]
			public int MissingPacketsInPeriodThreshold { get; set; }

			[JsonProperty("missingPacketsInPeriodTime")]
			public int MissingPacketsInPeriodTime { get; set; }

			[JsonProperty("missingPacketsInPeriod")]
			public bool MissingPacketsInPeriod { get; set; }

			[JsonProperty("CCErrorsInPeriod")]
			public bool CCErrorsInPeriod { get; set; }

			[JsonProperty("lowBitrate")]
			public bool LowBitrate { get; set; }
		}

		public class OutputMuteTriggers
		{
			[JsonProperty("zeroBitrate")]
			public bool ZeroBitrate { get; set; }

			[JsonProperty("TSSyncLoss")]
			public bool TSSyncLoss { get; set; }

			[JsonProperty("lowBitrateThreshold")]
			public int LowBitrateThreshold { get; set; }

			[JsonProperty("CCErrorsInPeriodThreshold")]
			public int CCErrorsInPeriodThreshold { get; set; }

			[JsonProperty("CCErrorsInPeriodTime")]
			public int CCErrorsInPeriodTime { get; set; }

			[JsonProperty("missingPacketsInPeriodThreshold")]
			public int MissingPacketsInPeriodThreshold { get; set; }

			[JsonProperty("missingPacketsInPeriodTime")]
			public int MissingPacketsInPeriodTime { get; set; }

			[JsonProperty("lowBitrate")]
			public bool LowBitrate { get; set; }

			[JsonProperty("CCErrorsInPeriod")]
			public bool CCErrorsInPeriod { get; set; }

			[JsonProperty("missingPacketsInPeriod")]
			public bool MissingPacketsInPeriod { get; set; }
		}

		public class Options
		{
			[JsonProperty("failoverTriggers")]
			public FailoverTriggers FailoverTriggers { get; set; }

			[JsonProperty("outputMuteTriggers")]
			public OutputMuteTriggers OutputMuteTriggers { get; set; }

			[JsonProperty("failoverMode")]
			public string FailoverMode { get; set; }

			[JsonProperty("failoverRevertTime")]
			public int FailoverRevertTime { get; set; }

			[JsonProperty("showXOROMT")]
			public bool ShowXoromt { get; set; }
		}

		public class InputStream
		{
			[JsonProperty("options")]
			public Options Options { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="InputStream"/> class.
			/// </summary>
			/// <param name="name">Name of stream.</param>
			/// <returns><see cref="InputStream"/> instance.</returns>
			public static InputStream GenerateStream(string name)
			{
				return new InputStream
				{
					Name = name,
					Options = new Options
					{
						FailoverMode = "none",
						FailoverRevertTime = -1,
						FailoverTriggers = new FailoverTriggers
						{
							ZeroBitrate = false,
							TSSyncLoss = false,
							LowBitrate = false,
							LowBitrateThreshold = -1,
							CCErrorsInPeriod = false,
							CCErrorsInPeriodThreshold = -1,
							CCErrorsInPeriodTime = -1,
							MissingPacketsInPeriod = false,
							MissingPacketsInPeriodThreshold = -1,
							MissingPacketsInPeriodTime = -1,
						},
						ShowXoromt = false,
						OutputMuteTriggers = new OutputMuteTriggers
						{
							ZeroBitrate = false,
							TSSyncLoss = false,
							LowBitrate = false,
							LowBitrateThreshold = -1,
							CCErrorsInPeriod = false,
							CCErrorsInPeriodThreshold = -1,
							CCErrorsInPeriodTime = -1,
							MissingPacketsInPeriod = false,
							MissingPacketsInPeriodThreshold = -1,
							MissingPacketsInPeriodTime = -1,
						},
					},
				};
			}
		}
	}

	namespace InputSrt
	{
		public class Options
		{
			[JsonProperty("type")]
			public int Type { get; set; }

			[JsonProperty("hostAddress")]
			public string HostAddress { get; set; }

			[JsonProperty("address")]
			public string Address { get; set; }

			[JsonProperty("port")]
			public int Port { get; set; }

			[JsonProperty("chunkSize")]
			public int ChunkSize { get; set; }

			[JsonProperty("latency")]
			public int Latency { get; set; }

			[JsonProperty("encryption")]
			public int Encryption { get; set; }

			[JsonProperty("passphrase")]
			public string Passphrase { get; set; }

			[JsonProperty("decryptionType")]
			public string DecryptionType { get; set; }

			[JsonProperty("decryptionOddKey")]
			public string DecryptionOddKey { get; set; }

			[JsonProperty("decryptionEvenKey")]
			public string DecryptionEvenKey { get; set; }

			[JsonProperty("fecEnabled")]
			public bool FecEnabled { get; set; }

			[JsonProperty("fecColumns")]
			public int? FecColumns { get; set; }

			[JsonProperty("fecRows")]
			public int? FecRows { get; set; }

			[JsonProperty("fecLayout")]
			public string FecLayout { get; set; }

			[JsonProperty("fecArq")]
			public string FecArq { get; set; }

			[JsonProperty("logLevel")]
			public int LogLevel { get; set; }
		}

		public class SrtSource
		{
			[JsonProperty("passive")]
			public bool PassiveSource { get; set; }

			[JsonProperty("protocol")]
			public string Protocol { get; set; }

			[JsonProperty("stream")]
			public string Stream { get; set; }

			[JsonProperty("priority")]
			public int Priority { get; set; }

			[JsonProperty("options")]
			public Options Options { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("active")]
			public bool Active { get; set; }

			[JsonProperty("paused")]
			public bool Paused { get; set; }

			public static SrtSource GenerateInput(string name, string streamId, string ipAddress, int port)
			{
				return new SrtSource
				{
					Name = name,
					Priority = 0,
					Protocol = "SRT",
					Stream = streamId,
					PassiveSource = true,
					Active = true,
					Paused = false,
					Options = new Options
					{
						Type = (int)StreamModeSource.Pull,
						Address = ipAddress,
						Port = port,
						Latency = 1000,
						Encryption = (int)TransportEncryption.None,
						Passphrase = null,
						ChunkSize = (int)ChunkSize.UDP,
					},
				};
			}
		}
	}

	namespace Output
	{
		public class OutputsRequest
		{
			[JsonProperty("stream")]
			public string Stream { get; set; }

			[JsonProperty("options")]
			public Options Options { get; set; }

			[JsonProperty("name")]
			public string Name { get; set; }

			[JsonProperty("protocol")]
			public string Protocol { get; set; }

			[JsonProperty("paused")]
			public bool Paused { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="OutputsRequest"/> class.
			/// </summary>
			/// <param name="name">Output name.</param>
			/// <param name="streamId">Stream id where Output will be insert.</param>
			/// <param name="port">The port where the output is being generated.</param>
			/// <param name="hostAddress">The host address of the SRT Output.</param>
			/// <returns><see cref="OutputsRequest"/> instance.</returns>
			public static OutputsRequest GenerateSrtOutput(string name, string streamId, int port, string hostAddress)
			{
				return new OutputsRequest
				{
					Name = name,
					Stream = streamId,
					Protocol = "SRT",
					Paused = false,
					Options = new Options
					{
						type = (int)StreamModeOutput.Listener,
						address = String.Empty, /* usually this is: streamMode == StreamModeOutput.Listener ? String.Empty : ipAddress, but we are currently hard-coding Listener so that's why it's String.Empty */
						hostAddress = hostAddress.Contains("default") ? null : hostAddress,
						port = port,
						ttl = 64,
						encryptionType = null,
						maxConnections = 0,
						maxBandwidth = 2147483647,
						encryption = (int)TransportEncryption.AES256,
						encryptionPercentage = -1,
						encryptionEvenKey = "-1",
						encryptionKeyParity = "-1",
						encryptionOddKey = "-1",
						chunkSize = 1316,
						serviceType = null,
						logLevel = 0,
						passphrase = "regressionTestPasshrase",
					},
				};
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="OutputsRequest"/> class.
			/// </summary>
			/// <param name="name">Output name.</param>
			/// <param name="streamId">Stream id where Output will be insert.</param>
			/// <param name="ipAddress">The IP Address to which the output is sending data.</param>
			/// <param name="port">The port where the output is being generated.</param>
			/// <param name="networkInterface">The host address of the SRT Output.</param>
			/// <returns><see cref="OutputsRequest"/> instance.</returns>
			public static OutputsRequest GenerateUdpOutput(string name, string streamId, string ipAddress, int port, string networkInterface)
			{
				return new OutputsRequest
				{
					Name = name,
					Stream = streamId,
					Protocol = "UDP",
					Paused = false,
					Options = new Options
					{
						address = ipAddress,
						networkInterface = networkInterface,
						port = port,
						ttl = 5,
						encryptionType = null,
						encryptionPercentage = -1,
						encryptionEvenKey = -1,
						encryptionKeyParity = -1,
						encryptionOddKey = -1,
						logLevel = 0,
						chunkSize = 1316,
						tosField = 0,
					},
				};
			}
		}

		public class Options
		{
			public int type { get; set; }

			public int ttl { get; set; }

			public object networkInterface { get; set; }

			public object encryptionType { get; set; }

			public string address { get; set; }

			public int port { get; set; }

			public long? maxConnections { get; set; }

			public int maxBandwidth { get; set; }

			public object hostAddress { get; set; }

			public int chunkSize { get; set; }

			public long? serviceType { get; set; }

			public int encryption { get; set; }

			public string passphrase { get; set; }

			public int encryptionPercentage { get; set; }

			public object encryptionKeyParity { get; set; }

			public object encryptionOddKey { get; set; }

			public object encryptionEvenKey { get; set; }

			public int logLevel { get; set; }

			public int? tosField { get; set; }
		}
	}

	public enum ParameterPid
	{
		SrmStreamListener = 8670,
		SrmInputListener = 10101,
		SrmOutputListener = 8889,
		StreamsTable = 8700,
		StreamsTableNameColumn = 8702,
		StreamsStreamResourceNameColum = 8708,
		InputsTable = 9600,
		OutputsTable = 8900,
		MwEdgesTable = 8500,
		StreamsTableFK = 8707,
		InterAppReceive = 9000000,
		InterAppReturn = 9000001,
	}

	public enum StreamType
	{
		SRT = 1,
		UDP = 2,
	}

	public enum StreamModeSource
	{
		Pull = 0,
		Listener = 1,
	}

	public enum StreamModeOutput
	{
		Listener = 0,
		Push = 1,
	}

	public enum TransportEncryption
	{
		None = 0,
		AES128 = 16,
		AES192 = 24,
		AES256 = 32,
	}

	public enum ChunkSize
	{
		RTP = 1328,
		UDP = 1316,
		Custom = -1,
	}

	public static class Utils
	{
		public static readonly int SleepTime = 3000;

		public static string GenerateStreamName(string resource, string edgeName)
		{
			return String.Format("{0}_{1}_[DM]_[RT]", resource, edgeName);
		}

		public static string GenerateInputName(string resource, string server, string protocol, string priOrSec)
		{
			return String.Format("IN_{0}_{1}_{2}_{3}_[RT]", resource, priOrSec, protocol, server);
		}

		public static string GenerateOutputName(string resource, string edgeName, string protocol)
		{
			return String.Format("OUT_{0}_{1}_{2}_[RT]", resource, protocol, edgeName);
		}

		public static string GenerateOutputDisplayKey(string edgeName, string source, string streamName)
		{
			return edgeName + "/" + source + "_" + streamName + "/" + source + "_" + streamName;
		}

		public static string GenerateRecordingOutputDisplayKey(string edgeName, string source, string streamName, string outputName)
		{
			return edgeName + "/" + streamName + "/" + source + "_" + outputName;
		}

		/// <summary>
		/// Return a string with the Display Key for the Input table with the format {edgeName}/{source}_{streamName}/{source}_{streamName}.
		/// </summary>
		/// <param name="edgeName">The edge name of the source.</param>
		/// <param name="streamName">The name of the stream.</param>
		/// <param name="inputOrOutputName">The name of the input or output.</param>
		/// <returns>Returns a string with the display key with the above format.</returns>
		public static string GenerateInputOrOutputDisplayKey(string edgeName, string streamName, string inputOrOutputName)
		{
			return edgeName + "/" + streamName + "/" + inputOrOutputName;
		}

		public static MWCoreRequest CreateMessage(string pk, string stream, InterAppAction action, InterAppResourceType type)
		{
			return new MWCoreRequest
			{
				Action = action,
				Type = type,
				ResourceToUpdate = pk,
				Body = $"{{\"stream\": \"{stream}\"}}",
			};
		}

		public static void SetContextMenu(int iParam, string[] contextMenuDataArray, MWCoreElement mwcore)
		{
			SetDataMinerInfoMessage setDataMinerInfoMessage = new SetDataMinerInfoMessage
			{
				DataMinerID = mwcore.MWCore.DmaId,
				HostingDataMinerID = mwcore.MWCore.DmaId,
				What = 287,
				Uia1 = new UIA(new uint[] { (uint)mwcore.MWCore.DmaId, (uint)mwcore.MWCore.ElementId, (uint)iParam }),
				Sa2 = new SA(contextMenuDataArray),
			};

			Engine.SLNet.SendSingleResponseMessage(setDataMinerInfoMessage);
		}
	}
}