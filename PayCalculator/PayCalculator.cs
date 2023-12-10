using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace OO_programming
{
    /// <summary>
    /// PayCalculator class.
    /// </summary>
    public class PayCalculator
    {
        // Getter and Setter of Properties for calculator
        public int Id { get; set; }
        public string FullName { get; set; }
        public decimal HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public string TaxThreshold { get; set; }
        public double GrossPay { get; set; }
        public double Tax { get; set; }
        public double NetPay { get; set; }
        public double Super { get; set; }


        //Methods
        /// <summary>
        /// Return All Calculations for PaySlip
        /// </summary>
        /// <param name="listBox"></param>
        /// <returns></returns>

        // Populate the employee list box
        public void PopulateEmployeeListBox(ListBox listBox, string filePath = "employee.csv")
        {
            try
            {
                filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var employees = csv.GetRecords<employee>().ToList();

                    foreach (var employee in employees)
                    {
                        var newEmployee = new employee
                        {
                            id = employee.id,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Rates = employee.Rates,
                            TaxThreshold = employee.TaxThreshold
                        };

                        listBox.Items.Add(newEmployee);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        /// <summary>
        /// Loads the tax brakets based on tax threshold.
        /// </summary>
        /// <param name="taxThreshold"></param>
        /// <returns></returns>
        // Load tax brackets based on tax threshold
        public List<PaySlip> LoadTaxBrackets(string taxThreshold)
        {
            var taxBrackets = new List<PaySlip>();

            try
            {
                string taxPath = GetTaxFilePath(taxThreshold);

                using (var reader = new StreamReader(taxPath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false }))
                {
                    taxBrackets = csv.GetRecords<PaySlip>().ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            return taxBrackets;
        }
        /// <summary>
        /// Calculates gross pay.
        /// </summary>
        /// <param name="enteredHours"></param>
        /// <param name="employeeRate"></param>
        /// <returns></returns>
        // Calculate gross pay
        public double CalculateGrossPay(string enteredHours, double employeeRate)
        {
            if (IsNumeric(enteredHours))
            {
                double workedHours = Convert.ToDouble(enteredHours);
                double grossPay = workedHours * employeeRate;
                GrossPay = grossPay;
                return grossPay;
            }
            else
            {
                // If enteredHours is symbol
                return GrossPay = -1;
            }
        }
        /// <summary>
        /// Helper method to check if a string is numeric and does not contain "$"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsNumeric(string value)
        {
            // Checking if the value is numeric and does not contain "$"
            return double.TryParse(value, out _) && !value.Contains("$");
        }
        /// <summary>
        /// Calculates tax amount.
        /// </summary>
        /// <param name="taxBrackets"></param>
        /// <param name="GrossPay"></param>
        // Calculate tax
        public void CalculateTax(List<PaySlip> taxBrackets, double GrossPay)
        {
            foreach (PaySlip taxBracket in taxBrackets)
            {
                double start = taxBracket.start;
                double end = taxBracket.end;

                if (GrossPay >= start && GrossPay <= end)
                {
                    Tax = ((double)taxBracket.a) * (GrossPay + 0.99) - ((double)taxBracket.b);
                }
            }
        }
        /// <summary>
        /// Calculation for net pay and super annuation.
        /// </summary>
        // Calculate net pay and super
        public void CalculateNetPayAndSuper()
        {
            NetPay = GrossPay - Tax;
            Super = GrossPay * 0.11;
        }
        /// <summary>
        /// Saves the payslip to a csv file.
        /// </summary>
        /// <param name="selectedEmployeeId"></param>
        /// <param name="empFullName"></param>
        /// <param name="enteredHours"></param>
        /// <param name="employeeRate"></param>
        /// <param name="taxThreshold"></param>
        /// <param name="grossPay"></param>
        /// <param name="tax"></param>
        /// <param name="netPay"></param>
        /// <param name="super"></param>
        // Save payslip to csv
        public static void SavePaySlip(int selectedEmployeeId, string empFullName, decimal enteredHours, double employeeRate, string taxThreshold, double grossPay, double tax, double netPay, double super)
        {
            try
            {
                var payCalculatorList = new List<PayCalculator>
                {
                    new PayCalculator
                    {
                        Id = selectedEmployeeId,
                        FullName = empFullName,
                        HoursWorked = enteredHours,
                        HourlyRate = employeeRate,
                        TaxThreshold = taxThreshold,
                        GrossPay = grossPay,
                        Tax = tax,
                        NetPay = netPay,
                        Super = super
                    }
                };

                var currentDateTime = DateTime.Now;
                var formattedDateTime = currentDateTime.ToString("yyyyMMdd-HHmmss");

                var filePath = $"Pay-{selectedEmployeeId}-{empFullName}-{formattedDateTime}.csv";

                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    csv.WriteRecords(payCalculatorList);
                }

                MessageBox.Show($"CSV file '{filePath}' created successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        /// <summary>
        /// File path for tax threshold.
        /// </summary>
        /// <param name="taxThreshold"></param>
        /// <returns></returns>
        // Construct tax file path
        private string GetTaxFilePath(string taxThreshold)
        {
            string taxFileName = taxThreshold == "Y" ? "taxrate-withthreshold.csv" : "taxrate-nothreshold.csv";
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, taxFileName);
        }
    }
}