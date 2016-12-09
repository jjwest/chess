using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    class InvalidBoardPositionException : Exception
    {
        public InvalidBoardPositionException()
        {
        }

        public InvalidBoardPositionException(string message)
        : base(message)
        {
        }

        public InvalidBoardPositionException(string message, Exception inner)
        : base(message, inner)
        {

        }
    }
}
