using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServer.MsSql
{
    public static class ExceptionExtensions
    {
        public static string[] GetMessages(this Exception ex)
        {
            IEnumerable<string> result = new List<string> { ex.Message };
            if (ex is AggregateException)
            {
                var aex = (AggregateException)ex;
                result = result.Concat(aex.InnerExceptions.SelectMany(iex => GetMessages(iex)));
            }
            if (ex.InnerException != null)
            {
                result = result.Concat(GetMessages(ex.InnerException));
            }
            return result.ToArray();
        }
    }
}
