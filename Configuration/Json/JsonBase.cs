using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Configuration.Json
{
    public abstract class JsonBase
    {
        private readonly string _filePath;

        protected JsonBase(string filePath)
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

            string json = File.ReadAllText(_filePath);
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;

            foreach (var property in GetType().GetProperties())
            {
                var name = property.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? property.Name;
                if (root.TryGetProperty(name, out var element))
                {
                    property.SetValue(this, element.Deserialize(property.PropertyType));
                }
            }
        }

        public void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(this, GetType(), options);
            File.WriteAllText(_filePath, json);
        }
    }
}
