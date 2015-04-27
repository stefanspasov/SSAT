using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class AppException
    {
        public static void Handle(Exception ex)
        {
            if (ex != null)
            {
                string errorMsg = "An application error occurred. Please contact the adminstrator " +
                                  "with the following information:\n\n";
                errorMsg = errorMsg + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace;
                Console.WriteLine(errorMsg);
                // TODO: Trace exception
            }
        }
    }
}
