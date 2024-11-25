using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Yaml
{
    /// <summary>
    /// Microsoft.OpenApi.Readers 1.6.22
    /// </summary>
    internal class YamlProcess
    {
        public static void Read()
        {
            using var streamReader = new StreamReader(@"pet-store.yaml");
            var document = new OpenApiStreamReader().Read(streamReader.BaseStream, out var diagnostic);

            if (diagnostic.Errors.Count > 0)
            {
                throw new Exception(string.Join(',', diagnostic.Errors));
            }

            // Serialize and save the OpenAPI document to a JSON file
            //using var streamWriter = new StreamWriter("YamlProcess.json");
            //var writer = new OpenApiJsonWriter(streamWriter);
            //document.SerializeAsV3(writer);

            Console.WriteLine("OpenAPI document converted from YAML to JSON.");
        }

        public static void Create()
        {
            var document = new OpenApiDocument
            {
                Info = new OpenApiInfo
                {
                    Title = "Title",
                    Version = "0.0.1",
                },
                Paths = new OpenApiPaths
                {
                    ["/pathItem1"] = new OpenApiPathItem
                    {
                        Description = "description",
                    },
                    ["/item2"] = new OpenApiPathItem
                    {
                        Description = "a",
                    },
                },
            };

            // Serialize the OpenAPI document to a YAML file
            using var streamWriter = new StreamWriter("pet-store.yaml");
            document.SerializeAsV3(new OpenApiYamlWriter(streamWriter));
            streamWriter.Dispose();
        }

    }
}
