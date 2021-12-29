using Microsoft.Extensions.CommandLineUtils;
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
            string helpTemplate = "-h | --help";
            string serialiseTemplate = "-s | --serialise";
            string deserialiseTemplate = "-d | --deserialise";
            string configFileTemplate = "-c | --configfile";

            var application = new CommandLineApplication();

            application.HelpOption(helpTemplate);
            var serialiseOption = application.Option(serialiseTemplate, "serialise mode", CommandOptionType.NoValue);
            var deserailiseOption = application.Option(deserialiseTemplate, "deserialise", CommandOptionType.NoValue);
            var configFileOption = application.Option(configFileTemplate, "Configuration file (optional)", CommandOptionType.SingleValue);

            application.OnExecute(() =>
            {
                string configurationFile = configFileOption.HasValue() ? configFileOption.Value() : "Config.json";
                var configuration = Configuration.LoadConfiguration(configurationFile);
                Console.WriteLine($"Starting DatabaSeserialiser with action {configuration.Action}");
                Console.WriteLine($"Using database {configuration.Datasource}");
                var database = new SQLServerDatabase(configuration.Datasource);
                if (serialiseOption.HasValue())
                {
                    SerialiseTables(configuration.Tables, database, configuration.DataFolder);
                    Console.WriteLine("Serialisation complete");
                }
                else if (deserailiseOption.HasValue())
                {
                    DeserialiseTables(configuration.Tables, database, configuration.DataFolder);
                    Console.WriteLine("Deserialisation complete");
                }
                else
                {
                    Console.WriteLine("Must specify an action (-s/-d)");
                    return 1;
                }

                return 0;
            });

            try
            {
                application.Execute(args);
            }
            catch (Exception ex)
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
