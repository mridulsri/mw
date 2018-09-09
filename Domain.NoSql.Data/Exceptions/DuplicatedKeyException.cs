using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.NoSql.Data.Exceptions
{
    public class DuplicatedKeyException : Exception
    {
        public string Key { get; set; }

        public DuplicatedKeyException(string errorMessage)
            : base(errorMessage)
        {
            Key = GetKey(errorMessage);
        }

        private static string GetKey(string errorMessage)
        {
            var startIndex = errorMessage.IndexOf('$') + 1;
            var endIndex = errorMessage.IndexOf('_', startIndex + 1);
            var length = endIndex - startIndex;
            return errorMessage.Substring(startIndex, length);
        }
    }
}
