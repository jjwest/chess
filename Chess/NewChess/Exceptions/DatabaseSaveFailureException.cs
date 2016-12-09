using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    class DatabaseSaveFailureException: Exception
    {
        public DatabaseSaveFailureException()
        {
        }

        public DatabaseSaveFailureException(string message)
        : base(message)
        {
        }

        public DatabaseSaveFailureException(string message, Exception inner)
        : base(message, inner)
        {

        }
    }
}
