using System;
using System.Data.SqlClient;

namespace DatabaseSerialiser
{
    /// <summary>
    /// This class interfaces to the datbase to perform the query and populate the 
    /// Table object
    /// </summary>
    interface IDatabase
    {
        /// <summary>
        /// Serialise a table
        /// </summary>
        void SerialiseTable(Table table, RecordSerialiser serialiser);
        /// <summary>
        /// Deserialise a table and add it to the database.
        /// </summary>
        void DeSerialiseTable(Table table, RecordDeSerialiser serialiser);
    }

    /// <summary>
    /// SQL Server implementation of Query class
    /// </summary>
    class SQLServerDatabase : IDatabase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SQLServerDatabase(string connectionStr)
        {
            m_connectionStr = connectionStr;
        }

        /// <summary>
        /// Serialise a table
        /// </summary>
        public void SerialiseTable(Table table, RecordSerialiser serialiser)
        {
            using (var connection = OpenConnection())
            {
                using (var command = new SqlCommand(table.Query, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var record = new Record();
                            foreach(var column in table.Columns)
                            {
                                record.Fields.Add(new Field(column.Name, reader[column.Name]));
                            }

                            //serialise the record object into the serialiser
                            serialiser.SerialiseRecord(record);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deserisalise a table from file and add it to the database
        /// </summary>
        public void DeSerialiseTable(Table table, RecordDeSerialiser serialiser)
        {
            Console.WriteLine($"table {table.Name}");
            Record record = serialiser.DeserialiseRecord();
            DisplayRecord(record);
        }

        /// <summary>
        /// Displays the contents of a record.
        /// </summary>
        private void DisplayRecord(Record record)
        {
            foreach(var field in record.Fields)
            {
                Console.WriteLine($"Name: {field.Name}. Value: {field.Value}");
            }
        }

        /// <summary>
        /// Method opens a connection to the sql server database. Implementation uses a connection 
        /// pooling technique so is not an expensive operation.
        /// </summary>
        private SqlConnection OpenConnection()
        {
            try
            {
                var connection = new SqlConnection(m_connectionStr);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private readonly string m_connectionStr;
    }
}
