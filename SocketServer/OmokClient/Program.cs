using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OmokClient
{
    [SupportedOSPlatform("windows10.0.177630")]
    static class Program
    {
        /// <summary>
        /// �ش� ���� ���α׷��� �� �������Դϴ�.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new mainForm());
        }
    }
}
