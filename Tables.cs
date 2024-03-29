﻿
using System.Collections.Generic;

namespace DatabaseSerialiser
{
    /// <summary>
    /// Enum of different native types supported
    /// </summary>
    public enum FieldValueType
    {
        STRING,
        DOUBLE,
        INTEGER,
        BOOLEAN,
        DATETIME
    }

    /// <summary>
    /// abstract class defines a field value
    /// </summary>
    abstract class FieldValue
    {
        /// <summary>
        /// FieldValueType
        /// </summary>
        public FieldValueType ValueType { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        protected FieldValue(FieldValueType valueType)
        {
            ValueType = valueType;
        }
    }

    /// <summary>
    /// Class defines a string value
    /// </summary>
    class StringFieldValue : FieldValue
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StringFieldValue(string value) : base(FieldValueType.STRING)
        {
        }
    }

    /// <summary>
    /// Class defines a field in a record
    /// </summary>
    class Field
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Field(string name, object value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Name of field.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Value of field.
        /// </summary>
        public object Value { get; set; }
    }

    /// <summary>
    /// Class defines a record in a table
    /// </summary>
    class Record
    {
        /// <summary>
        /// List of fields in record
        /// </summary>
        public List<object> Fields { get; private set; } = new List<object>();
    }

    /// <summary>
    /// Class defines a table in a datbase
    /// </summary>
    public class Table
    {
        /// <summary>
        /// Name of table
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Database query for table contents
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Filename to store serialised table.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Identifier columns for table
        /// </summary>
        public List<string> IdentifierFields { get; set; }

        /// <summary>
        /// Table columns definition.
        /// </summary>
        public List<Column> Columns { get; set; }
    }

    /// <summary>
    /// Class defines a column in a table
    /// </summary>
    public class Column
    {
        /// <summary>
        /// Name of column as it appears in the select statement from the table
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of column as it appears in the INSERT statement for the table
        /// </summary>
        public string InsertName { get; set; }

        /// <summary>
        /// Foreign table name if column joined on another table
        /// </summary>
        public string ForeignTable { get; set; }

        /// <summary>
        /// ForeginTable Column Name if it is different to name
        /// </summary>
        public string ForeignTableColumnName { get; set; }

        /// <summary>
        /// Foreign key name if column joined on another table
        /// </summary>
        public string ForeignKeyName { get; set; }

        /// <summary>
        /// Sub columns that make up this column. 
        /// </summary>
        public List<Column> Columns { get; set; }
    }
}
