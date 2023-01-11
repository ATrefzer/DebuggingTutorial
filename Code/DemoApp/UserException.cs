using System;
using System.Runtime.Serialization;

namespace DemoApp
{
    [Serializable]
    public class UserException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public UserException()
        {
        }

        public UserException(string message) : base(message)
        {
        }

        public UserException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UserException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}