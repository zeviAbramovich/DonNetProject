using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace BE
{

    [Serializable]
    public class CannotUpdate : Exception
    {
        public CannotUpdate() : base() { }
        public CannotUpdate(string message) : base(message) { }
        public CannotUpdate(string message, Exception inner) : base(message, inner) { }
        protected CannotUpdate(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class IdAlreadyExsist : Exception
    {
        public IdAlreadyExsist() : base() { }
        public IdAlreadyExsist(string message) : base(message) { }
        public IdAlreadyExsist(string message, Exception inner) : base(message, inner) { }
        protected IdAlreadyExsist(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class CannotDelete : Exception
    {
        public CannotDelete() : base() { }
        public CannotDelete(string message) : base(message) { }
        public CannotDelete(string message, Exception inner) : base(message, inner) { }
        protected CannotDelete(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

}
