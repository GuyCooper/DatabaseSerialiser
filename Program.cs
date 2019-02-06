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
            try
            {
                //deserailise Config.json to create a list of Tables
                var configuration = Configuration.LoadConfiguration("Config.json");
                Console.WriteLine($"Starting DatabaSeserialiser with action {configuration.Action}");
                var database = new SQLServerDatabase(configuration.Datasource);
                if(configuration.Action == "S")
                {
                    SerialiseTables(configuration.Tables, database);
                    Console.WriteLine("serialisation complete");
                }
                else if(configuration.Action == "D")
                {
                    DeserialiseTables(configuration.Tables, database);
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
