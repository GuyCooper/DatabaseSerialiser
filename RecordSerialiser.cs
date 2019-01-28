using System;
using System.IO;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json;

namespace DatabaseSerialiser
{
    /// <summary>
    /// Class for serialising database records to a file
    /// </summary>
    class RecordSerialiser : IDisposable
    {
        #region Public Methods

        /// <summary>
        /// Constructor. creates a file stream
        /// </summary>
        public RecordSerialiser(string filename)
        {
            m_filename = filename;
            m_stream = File.Create(filename);
            m_writer = new BinaryWriter(m_stream);
            m_writer.BaseStream.Position = 0;
        }

        /// <summary>
        /// serialise a record and add it to the stream.
        /// </summary>
        public void SerialiseRecord(Record record)
        {
            if(m_stream.CanWrite == false)
            {
                throw new FileLoadException($"unable to write to file {m_filename}");
            }

            var buffer = ToByteArray(record);
            m_writer.Write(buffer.Length);
            m_writer.Write(buffer);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Serialises a Record into a byte array
        /// </summary>
        private Byte[] ToByteArray(object record)
        {
            MemoryStream ms = new MemoryStream();
            using (BsonDataWriter writer = new BsonDataWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, record);
            }
            return ms.GetBuffer();
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// DIspose method. close stream
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed) return;

            m_writer.Dispose();
            m_stream.Dispose();

            IsDisposed = true;
        }

        private bool IsDisposed;

        #endregion

        #region Private Data

        private readonly FileStream m_stream;
        private readonly BinaryWriter m_writer;
        private readonly string m_filename;

        #endregion

    }

    /// <summary>
    /// Class deserialises a list of records from a file
    /// </summary>
    class RecordDeSerialiser : IDisposable
    {
        #region Public Methods

        /// <summary>
        /// Constructor. creates a file stream
        /// </summary>
        public RecordDeSerialiser(string filename)
        {
            m_stream = File.Open(filename, FileMode.Open);
            m_reader = new BinaryReader(m_stream);
            m_reader.BaseStream.Position = 0;
        }

        /// <summary>
        /// Deserialises a record from the current position of the stream
        /// </summary>
        /// <returns></returns>
        public Record DeserialiseRecord()
        {
            if (m_stream.CanRead == false || m_stream.Position >= m_stream.Length)
            {
                return null;
            }
            
            var size = m_reader.ReadInt32();
            if (size > 0)
            {
                var buffer = m_reader.ReadBytes(size);
                return fromByteArray(buffer);
            }
            return null;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Deserialises a record from the current position of the stream
        /// </summary>
        private Record fromByteArray(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                using (BsonDataReader reader = new BsonDataReader(stream))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    return serializer.Deserialize<Record>(reader);
                }
            }
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// DIspose method. close stream
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed) return;

            m_reader.Dispose();
            m_stream.Dispose();

            IsDisposed = true;
        }

        private bool IsDisposed;

        #endregion

        #region Private Data

        private readonly FileStream m_stream;
        private readonly BinaryReader m_reader;

        #endregion

    }
}
