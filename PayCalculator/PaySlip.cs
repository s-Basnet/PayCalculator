using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OO_programming
{
    /// <summary>
    /// Payslip class to import tax bracket from csv file.
    /// </summary>
    // Class constructed to determine the tax bracket of a payslip
    public class PaySlip
    {
        public int start { get; set; }
        public int end { get; set; }
        public decimal a { get; set; }
        public decimal b { get; set; }

        /// <summary>
        /// Tax bracket constructor.
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        // Constructor
        public PaySlip(int Start, int End, decimal A, decimal B)
        {
            this.start = Start;
            this.end = End;
            this.a = A;
            this.b = B;

        }
    }
}
