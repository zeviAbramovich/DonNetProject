using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace BE
{

    [Serializable]
    public class CannotUpdateException : Exception
    {
        public CannotUpdateException() : base() { }
        public CannotUpdateException(string message) : base(message) { }
        public CannotUpdateException(string message, Exception inner) : base(message, inner) { }
        protected CannotUpdateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class IdAlreadyExsistException : Exception
    {
        public IdAlreadyExsistException() : base() { }
        public IdAlreadyExsistException(string message) : base(message) { }
        public IdAlreadyExsistException(string message, Exception inner) : base(message, inner) { }
        protected IdAlreadyExsistException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class CannotDeleteException : Exception
    {
        public CannotDeleteException() : base() { }
        public CannotDeleteException(string message) : base(message) { }
        public CannotDeleteException(string message, Exception inner) : base(message, inner) { }
        protected CannotDeleteException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

}
