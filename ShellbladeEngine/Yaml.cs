using System.IO;
using YamlDotNet.Serialization;

namespace Shellblade
{
	public abstract class Yaml
	{
		protected ISerializer   Serializer   = new SerializerBuilder().Build();
		protected IDeserializer Deserializer = new DeserializerBuilder().Build();

		public abstract void Load(string filepath);

		public virtual void Save(string filepath) => File.WriteAllText(filepath, Serializer.Serialize(this));
	}
}
