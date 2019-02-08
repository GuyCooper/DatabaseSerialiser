using System;
using System.Collections.Generic;
using System.IO;

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
            try
            {
                //deserailise Config.json to create a list of Tables
                var configuration = Configuration.LoadConfiguration("Config.json");
                Console.WriteLine($"Starting DatabaSeserialiser with action {configuration.Action}");
                var database = new SQLServerDatabase(configuration.Datasource);
                if(configuration.Action == "S")
                {
                    SerialiseTables(configuration.Tables, database, configuration.DataFolder);
                    Console.WriteLine("serialisation complete");
                }
                else if(configuration.Action == "D")
                {
                    DeserialiseTables(configuration.Tables, database, configuration.DataFolder);
                    Console.WriteLine("Deserialisation complete");
                }
                else
                {
                    Console.WriteLine($"Invalid action {configuration.Action}");
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Serialise the list of configured tables
        /// </summary>
        static void SerialiseTables(List<Table> tables, IDatabase database, string outputFolder)
        {
            foreach(var table in tables)
            {
                SerialiseTable(table, database, outputFolder);
            }
        }

        /// <summary>
        /// Serialise a single table
        /// </summary>
        static void SerialiseTable(Table table, IDatabase database, string outputFolder)
        {
            Console.WriteLine($"serialising table {table.Name} to file {table.Filename}");
            using (var serialiser = new RecordSerialiser(Path.Combine(outputFolder, table.Filename)))
            {
                database.SerialiseTable(table, serialiser);
            }
        }

        /// <summary>
        /// Deserialise the list of configured tables
        /// </summary>
        static void DeserialiseTables(List<Table> tables, IDatabase database, string inputFolder)
        {
            foreach(var table in tables)
            {
                DeSerialiseTable(table, database, inputFolder);
            }           
        }

        /// <summary>
        /// Deserialise a sinbgle table.
        /// </summary>
        static void DeSerialiseTable(Table table, IDatabase database, string inputFolder)
        {
            Console.WriteLine($"deserialising table {table.Name} from file {table.Filename}");
            using (var deserialiser = new RecordDeSerialiser(Path.Combine(inputFolder, table.Filename))) 
            {
                database.DeSerialiseTable(table, deserialiser);
            }
        }
    }
}
