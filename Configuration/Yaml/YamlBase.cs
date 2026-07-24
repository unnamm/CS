using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace Configuration.Yaml
{
    public abstract class YamlBase
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

            deserializer.Deserialize(yaml, GetType());
        }

        private object Factory(Type t) => t == GetType() ? this : Activator.CreateInstance(t)!;

        public void Save()
        {
            var serializer = new SerializerBuilder().Build();
            string yaml = serializer.Serialize(this, GetType());
            File.WriteAllText(_filePath, yaml);
        }
    }
}
