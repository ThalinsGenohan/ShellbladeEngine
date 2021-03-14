using YamlDotNet.Serialization;

namespace Shellblade
{
	public abstract class Config
	{
		protected ISerializer   _serializer   = new SerializerBuilder().Build();
		protected IDeserializer _deserializer = new DeserializerBuilder().Build();

		public abstract void Load(string filename);

		public abstract void Save(string filename);
	}
}
