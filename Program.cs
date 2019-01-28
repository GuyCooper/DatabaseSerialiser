using System;
using System.Collections.Generic;

namespace DatabaseSerialiser
{
    /// <summary>
    /// This application serialises and deserialies database into normalised tables so they can be 
    /// shared across different databases (e.g. SQL Server and MySQL)
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //deserailise Config.json to create a list of Tables
            Console.WriteLine("Starting Databaseserialiser...");
            var configuration = Configuration.LoadConfiguration("Config.json");
            var database = new SQLServerDatabase(configuration.Datasource);
            //SerialiseTables(configuration.Tables, database);
            DeserialiseTables(configuration.Tables, database);
            Console.WriteLine("serialisation complete");
        }

        /// <summary>
        /// Serialise the list of configured tables
        /// </summary>
        static void SerialiseTables(List<Table> tables, IDatabase database)
        {
            foreach(var table in tables)
            {
                SerialiseTable(table, database);
            }
        }

        /// <summary>
        /// Serialise a single table
        /// </summary>
        static void SerialiseTable(Table table, IDatabase database)
        {
            Console.WriteLine($"serialising table {table.Name} to file {table.Filename}");
            using (var serialiser = new RecordSerialiser(table.Filename))
            {
                database.SerialiseTable(table, serialiser);
            }
        }

        /// <summary>
        /// Deserialise the list of configured tables
        /// </summary>
        static void DeserialiseTables(List<Table> tables, IDatabase database)
        {
            foreach(var table in tables)
            {
                DeSerialiseTable(table, database);
            }           
        }

        /// <summary>
        /// Deserialise a sinbgle table.
        /// </summary>
        static void DeSerialiseTable(Table table, IDatabase database)
        {
            Console.WriteLine($"deserialising table {table.Name} from file {table.Filename}");
            using (var deserialiser = new RecordDeSerialiser(table.Filename))
            {
                database.DeSerialiseTable(table, deserialiser);
            }
        }
    }
}
