using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OO_programming
{
    /// <summary>
    /// employee class.
    /// </summary>
    public class employee
    {
        public string id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Rates { get; set; }
        public string TaxThreshold { get; set; }

        /// <summary>
        /// handles employee.
        /// </summary>
        public employee() { }
        /// <summary>
        /// Contains constructor for employee payslip.
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="hourlyRate"></param>
        /// <param name="taxThreshold"></param>
        //Constructor
        public employee(string employeeID, string firstName, string lastName, double hourlyRate, string taxThreshold)
        {
            id = employeeID;
            FirstName = firstName;
            LastName = lastName;
            Rates = hourlyRate;
            TaxThreshold = taxThreshold;
        }
        /// <summary>
        /// Populate the employeeID and full name in the list box as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{id}: {FirstName} {LastName}";
        }
    }
}
