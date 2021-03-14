using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Shellblade
{
	public class Localization
	{
		private string _language;

		public string LangPath { get; set; }

		public string Language
		{
			get => _language;
			set
			{
				_language = value;
				var decoder = new DeserializerBuilder().WithNamingConvention(HyphenatedNamingConvention.Instance).Build();

				Strings = decoder.Deserialize<Dictionary<string, string>>(File.ReadAllText($"{LangPath}/{_language}.lang"));
			}
		}

		public Dictionary<string, string> Strings { get; private set; }
	}
}
