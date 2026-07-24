using System;
using System.Collections.Generic;
using System.Text;

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
        }

        public void Save()
        {
            
        }
    }
}
