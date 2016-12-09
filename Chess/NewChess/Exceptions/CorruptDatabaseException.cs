﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    class CorruptDatabaseException : Exception
    {
        public CorruptDatabaseException()
        {
        }

        public CorruptDatabaseException(string message)
        : base(message)
        {
        }

        public CorruptDatabaseException(string message, Exception inner)
        : base(message, inner)
        {

        }
    }
}
