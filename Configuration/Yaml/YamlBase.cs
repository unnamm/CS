using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace Configuration.Yaml
{
    public abstract class YamlBase<T> where T : YamlBase<T>
    {
        private readonly string _filePath;

        protected YamlBase(string filePath)
        {
            _filePath = filePath;
            Load();
        }

        public void Load()
        {
            if (!File.Exists(_filePath))
            {
                Save();
                return;
            }

            string yaml = File.ReadAllText(_filePath);

            var deserializer = new DeserializerBuilder()
                .WithObjectFactory(Factory)
                .Build();

            deserializer.Deserialize<T>(yaml);
        }

        private object Factory(Type type) => type == typeof(T) ? this : Activator.CreateInstance(type)!;

        public void Save()
        {
            var serializer = new SerializerBuilder().Build();
            string yaml = serializer.Serialize((T)(object)this);
            File.WriteAllText(_filePath, yaml);
        }
    }
}
