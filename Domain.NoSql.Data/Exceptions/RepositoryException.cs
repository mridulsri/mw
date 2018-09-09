using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.NoSql.Data.Exceptions
{
    public class RepositoryException : Exception
    {
        public RepositoryException(string message, Exception innerException) : base(message, innerException)
        {
           throw innerException;
        }

        public RepositoryException(string message) : base(message)
        {
            CustomLogger.Logger.LogError(message);
        }
    }
}
