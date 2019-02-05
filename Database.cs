using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

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
                            var allColumns = table.Columns.SelectMany(subCol => subCol.Columns);
                            foreach(var column in allColumns)
                            {
                                record.Fields.Add(reader[column.Name]);
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
            while(record != null)
            {
                DisplayRecord(table, record);
                AddRecord(table, record);
                record = serialiser.DeserialiseRecord();
            }
        }

        /// <summary>
        /// Adds a record to the database
        /// </summary>
        private void AddRecord(Table table, Record record)
        {
            var sql = new StringBuilder();
            sql.Append($"INSERT INTO {NormaliseNameForQuery(table.Name)} (");
            sql.Append(string.Join(",", table.Columns.Select(col => NormaliseNameForQuery(col.InsertName ?? col.Name))));
            sql.Append(") VALUES (");
            sql.Append(ResolveColumns(table, record));
            sql.Append(")");

            //now run the query..
            using (var connection = OpenConnection())
            {
                using (var command = new SqlCommand(sql.ToString(), connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// This method resolves all the column values into their correct format for insertion
        /// into the table
        /// </summary>
        private string ResolveColumns(Table table, Record record)
        {
            var result = new StringBuilder();
            int i = 0;
            while(i < table.Columns.Count)
            {
                if (i > 0) result.Append(",");

                if(table.Columns[i].Columns != null)
                {
                    var subValues = new List<object>();
                    var parentColumn = table.Columns[i];
                    foreach (var subColumn in parentColumn.Columns)
                    {
                        subValues.Add(record.Fields[i++]);
                    }
                    result.Append(ResolveMultipleColumns(parentColumn, subValues));
                }
                else
                {
                    result.Append(ResolveColumn(table.Columns[i], record.Fields[i]));
                    i++;
                }
            }            
            return result.ToString();
        }

        /// <summary>
        /// This methods resolves a multiple column value list. This is where the value is joined on multiple tables
        /// </summary>
        private object ResolveMultipleColumns(Column parentColumn, List<object> values)
        {
            var sql = new StringBuilder();
            sql.Append(string.Format("SELECT {0} FROM {1} WHERE ",
                NormaliseNameForQuery(parentColumn.ForeignKeyName),
                NormaliseNameForQuery(parentColumn.ForeignTable)));

            var subFilters = new List<string>();
            for (var i = 0; i < parentColumn.Columns.Count; i++)
            {
                var column = parentColumn.Columns[i];
                subFilters.Add(string.Format("{0} = {1}",
                        NormaliseNameForQuery(column.InsertName),
                        ResolveJoinedValue(column, values[i])));
            }

            sql.Append(string.Join(" AND ", subFilters));

            return RetrieveData(sql.ToString(), parentColumn.ForeignKeyName);
        }

        /// <summary>
        /// This method resolves a single column vaue into its correct format for insertion
        /// </summary>
        /// <returns></returns>
        private string ResolveColumn(Column column, object value)
        {
            var resolvedValue = value;
            if(string.IsNullOrWhiteSpace(column.ForeignTable) == false)
            {
                //this vaue is joined on another table
                //TODO 
                resolvedValue = ResolveJoinedValue(column, value);
            }
            return NormaliseValueForQuery(resolvedValue);
        }

        /// <summary>
        /// This method just adds the sql protectors around the name to prevent keyword clashes
        /// </summary>
        /// <returns></returns>
        private string NormaliseNameForQuery(string name)
        {
            return $"[{name}]";
        }

        /// <summary>
        /// This method normalises the value into an insert string value. i.e If the value
        /// is a string type then surround it with quotes otherwise just tostring it
        /// </summary>
        private string NormaliseValueForQuery(object value)
        {
            if (value == null)
                return "null";

            if(value.GetType() == typeof(string))
                return $"'{value}'";
            if (value.GetType() == typeof(DateTime))
                return $"'{((DateTime)value).ToString("MM/dd/yyyy HH:mm:ss")}'";

            return value.ToString();
        }

        /// <summary>
        /// Method resolves a value that is joined on a other table
        /// </summary>
        private object ResolveJoinedValue(Column column, object value)
        {
            var sql = string.Format("SELECT {0} FROM {1} WHERE {2} = {3}",
                NormaliseNameForQuery(column.ForeignKeyName),
                NormaliseNameForQuery(column.ForeignTable),
                NormaliseNameForQuery(column.ForeignTableColumnName ?? column.Name),
                NormaliseValueForQuery(value));

            return RetrieveData(sql, column.ForeignKeyName);
        }

        /// <summary>
        /// Helper method for retrieving a single record from the database from the given query string
        /// </summary>
        private object RetrieveData(string sql, string columnName)
        {
            using (var connection = OpenConnection())
            {
                using (var command = new SqlCommand(sql, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader[columnName];
                        }
                    }
                }
            }
            throw new ArgumentException($"Query failed to return any results: {sql}");
        }

        /// <summary>
        /// Displays the contents of a record.
        /// </summary>
        private void DisplayRecord(Table table, Record record)
        {
            for(int i = 0 ; i < table.Columns.Count; i++)
            {
                Console.WriteLine($"{table.Columns[i].Name} : {record.Fields[i]}");
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
