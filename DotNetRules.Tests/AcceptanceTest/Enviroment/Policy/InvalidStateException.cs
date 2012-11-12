using System;
using System.Runtime.Serialization;

namespace DotNetRules.Tests.AcceptanceTest.Enviroment.Policy
{
    [Serializable]
    public class InvalidStateException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public InvalidStateException()
        {
        }

        public InvalidStateException(string message) : base(message)
        {
        }

        public InvalidStateException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidStateException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}