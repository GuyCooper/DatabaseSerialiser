﻿using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace DatabaseSerialiser
{
    /// <summary>
    /// Configuration class.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Type Of Serialisation. S = Serialise. D = deserialise
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// name of database.
        /// </summary>
        public string Datasource { get; set; }

        /// <summary>
        /// Location of folder to store data files.
        /// </summary>
        public string DataFolder { get; set; }

        /// <summary>
        /// Definition of tables required for serialising / deserialising.
        /// </summary>
        public List<Table> Tables { get; set; }

        public static Configuration LoadConfiguration(string configFile)
        {
            using (var stream = File.OpenText(configFile))
            {
                using (var reader = new JsonTextReader(stream))
                {
                    var serialiser = new JsonSerializer();
                    return serialiser.Deserialize<Configuration>(reader);
                }
            }
        }
    }
}
