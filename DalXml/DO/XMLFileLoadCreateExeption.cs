using System;
using System.Runtime.Serialization;

namespace DO
{
    [Serializable]
    internal class XMLFileLoadCreateExeption : Exception
    {
        private string filePath;
        private string v;
        private Exception ex;

        public XMLFileLoadCreateExeption()
        {
        }

        public XMLFileLoadCreateExeption(string message) : base(message)
        {
        }

        public XMLFileLoadCreateExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        public XMLFileLoadCreateExeption(string filePath, string v, Exception ex)
        {
            this.filePath = filePath;
            this.v = v;
            this.ex = ex;
        }

        protected XMLFileLoadCreateExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}