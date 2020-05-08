using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudTestCSharp
{
    class GlobalVariables
    {
        internal static string ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Z:\\test\\CrudTestCSharp\\CrudTestCSharp\\bin\\Debug\\CrudTest.accdb;Persist Security Info=False";

        internal static string PathButton = string.Empty;
        internal static int GetRow = 0;

        internal static string GlobalFname = string.Empty;
        internal static string GlobalLname = string.Empty;
        internal static string GlobalEmail = string.Empty;
        internal static string GlobalPhoneNo = string.Empty;
        internal static bool GlobalActive = false;
    }
}
