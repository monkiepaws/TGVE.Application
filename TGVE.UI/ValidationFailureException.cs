using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMM.UI
{
    [Serializable]
    public class ValidationFailureException : Exception
    {
        public ValidationFailureException(string message) : base(message)
        {

        }
    }
}