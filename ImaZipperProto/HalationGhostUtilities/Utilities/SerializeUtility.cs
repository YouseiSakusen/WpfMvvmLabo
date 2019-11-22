using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace HalationGhost.WinApps.Utilities
{
	/// <summary>
	/// シリアライズユーティリティを表します。
	/// </summary>
	public static class SerializeUtility
	{
		#region メソッド

		/// <summary>
		/// ファイルへシリアライズします。
		/// </summary>
		/// <typeparam name="T">シリアライズするオブジェクトの型を表します。</typeparam>
		/// <param name="xmlFilePath">シリアライズするXMLファイルのパスを表します。</param>
		/// <param name="data">シリアライズするオブジェクトを表すT。</param>
		public static void SerializeToFile<T>(string xmlFilePath, T data) where T : class
		{
			var dirPath = Path.GetDirectoryName(xmlFilePath);
			if (!Directory.Exists(dirPath))
				return;

			var writerSettings = new XmlWriterSettings()
			{
				Encoding = new UTF8Encoding(false),
				Indent = true
			};

			using (var writer = XmlWriter.Create(xmlFilePath, writerSettings))
			{
				var serializer = new DataContractSerializer(typeof(T));
				serializer.WriteObject(writer, data);
			}
		}

		/// <summary>
		/// XMLファイルからデシリアライズします。
		/// </summary>
		/// <typeparam name="T">デシリアライズするオブジェクトの型を表します。</typeparam>
		/// <param name="xmlFilePath">デシリアライズするXMLファイルのパスを表します。</param>
		/// <returns>XMLファイルからデシリアライズするT。</returns>
		public static T DeserializeFromFile<T>(string xmlFilePath) where T : class
		{
			if (!File.Exists(xmlFilePath))
				return null;

			using (var reader = XmlReader.Create(xmlFilePath))
			{
				var serializer = new DataContractSerializer(typeof(T));

				return serializer.ReadObject(reader) as T;
			}
		}

		public static void Serialize<T>(string xmlFilePath, T data) where T : class
		{
			var dirPath = Path.GetDirectoryName(xmlFilePath);
			if (!Directory.Exists(dirPath))
				return;

			using (var stream = File.Open(xmlFilePath, FileMode.Create))
			{
				using (var writer = XmlDictionaryWriter.CreateBinaryWriter(stream))
				{
					var writerSettings = new XmlWriterSettings()
					{
						Encoding = new UTF8Encoding(false),
						Indent = true
					}; 
					var serializer = new DataContractSerializer(typeof(T));
					serializer.WriteObject(writer, data);
				}
			}
		}

		public static T Deserialize<T>(string xmlFilePath) where T : class
		{
			using (var stream = File.OpenRead(xmlFilePath))
			{
				using (var reader = XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max))
				{
					var serializer = new DataContractSerializer(typeof(T));

					return serializer.ReadObject(reader) as T;
				}
			}
			
		}

		#endregion
	}
}
