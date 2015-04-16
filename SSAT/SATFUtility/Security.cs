using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATFUtilities
{
  public static  class Security
    {
      public static System.Security.SecureString MakeSecureString(string text)
      {
          System.Security.SecureString secure = new System.Security.SecureString();
          foreach (char c in text)
          {
              secure.AppendChar(c);
          }

          return secure;
      }
    }
}
