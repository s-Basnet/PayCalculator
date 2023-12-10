using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using CsvHelper;
using System.Security.Cryptography;
using CsvHelper.Configuration;
using System.Globalization;

namespace OO_programming
{
    /// <summary>
    /// The applications main form.
    /// </summary>
    public partial class Form1 : Form
    {
        private PayCalculator payCalculator;
        /// <summary>
        /// Initialises a new instance of the Form 1 class.
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Form1()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            InitializeComponent();

            // Populate the listBox
            // by reading the employee.csv
            // CSV file format: <employee ID>, <first name>, <last name>, <hourly rate>,<taxthreshold>
            payCalculator = new PayCalculator();
            payCalculator.PopulateEmployeeListBox(listBox1);

        }
     

        // Declaring the variables for PaySlip items
        int SelectedEmployeeID = 0;
        string EnteredHoursText = "";
        double EmployeeRate = 0;
        double GrossPay = 0;
        double Tax = 0;
        string CheckTaxThreshold = "";
        double NetPay = 0;
        double Super = 0;
        string EmpFullName = "";

        /// <summary>
        /// Handles the button click.
        /// </summary>
        // Make the button click event accessible for testing
        public event EventHandler ButtonClick;


        /// <summary>
        /// Payslip of selected employee is calculated when button is clicked.
        /// </summary>
        /// <param name="sender"> Object that raised event.</param>
        /// <param name="e"> Event arguments.</param>
        // Button to calculate the payslip of the selected employee
        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                employee selectEmployeeObject = (employee)(listBox1.SelectedItem);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                if (selectEmployeeObject != null)
                {
                    var taxBrackets = payCalculator.LoadTaxBrackets(selectEmployeeObject.TaxThreshold);

                    EmpFullName = $"{selectEmployeeObject.FirstName} {selectEmployeeObject.LastName}";
                    textBox2.Text = $"Employee: {EmpFullName}\r\n \r\n";

                    // Convert textBox1.Text to decimal with up to 2 decimal places
                    if (decimal.TryParse(textBox1.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal enteredHours))
                    {
                        if (enteredHours <= 0 || enteredHours > 40)
                        {
                            MessageBox.Show("Please enter Hours Between 1 - 40.");
                        } else
                        {
                            // Check if the entered hours have more than 2 decimal places
                            if (Decimal.Round(enteredHours, 2) != enteredHours)
                            {
                                MessageBox.Show("Please enter Hours up to 2 decimal places.");
                                return;
                            }

                            string EnteredHours = enteredHours.ToString();

                            GrossPay = payCalculator.CalculateGrossPay(EnteredHours, selectEmployeeObject.Rates);
                            payCalculator.CalculateTax(taxBrackets, GrossPay);
                            payCalculator.CalculateNetPayAndSuper();

                            // Display results in textBox2
                            textBox2.Text += $"Hours Worked: {enteredHours:F2} \r\n";
                            textBox2.Text += $"Hourly Rate: {selectEmployeeObject.Rates} \r\n";
                            textBox2.Text += $"Tax Threshold Claimed: {(selectEmployeeObject.TaxThreshold == "Y" ? "Yes" : "No")} \r\n";
                            textBox2.Text += $"Gross Pay: ${payCalculator.GrossPay:F2} \r\n";
                            textBox2.Text += $"Taxed Amount: ${payCalculator.Tax:F2} \r\n";
                            textBox2.Text += $"Net Pay: ${payCalculator.NetPay:F2} \r\n";
                            textBox2.Text += $"Superannuation: ${payCalculator.Super:F2} \r\n";

                            // Update variables for saving payment data
                            SelectedEmployeeID = int.Parse(selectEmployeeObject.id);
                            EnteredHoursText = enteredHours.ToString("F2", CultureInfo.InvariantCulture);
                            EmployeeRate = selectEmployeeObject.Rates;
                            CheckTaxThreshold = selectEmployeeObject.TaxThreshold;
                            GrossPay = payCalculator.GrossPay;
                            Tax = payCalculator.Tax;
                            NetPay = payCalculator.NetPay;
                            Super = payCalculator.Super;
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid numeric value for Hours Worked.");
                    }
                }
                else
                {
                    MessageBox.Show($"Error: Employee Not Selected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            // Trigger the ButtonClick event
            ButtonClick?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Simulates a button click to test if it works.
        /// </summary>
        // Introduce a method to simulate the button click for testing
        public void SimulateButtonClick()
        {
            button1_Click(this, EventArgs.Empty);
        }

        /// <summary>
        /// Saves the payslip details for the selected employee when the button is clicked.
        /// </summary>
        /// <param name="sender">Object that raise the event.</param>
        /// <param name="e">Event arguments.</param>
        // Button to save the payslip details for the selected employee
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if the user has selected the employee from the list
                if (EmpFullName == null)
                {
                    MessageBox.Show($"Error: Employee's Pay Not Calculated Yet.");
                }
                else
                {
                    // Parsing the entered hours text into 2 d.p number
                    if (decimal.TryParse(EnteredHoursText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal enteredHours))
                    {
                        // Save the payslip details into csv
                        PayCalculator.SavePaySlip(SelectedEmployeeID, EmpFullName, enteredHours, EmployeeRate, CheckTaxThreshold, GrossPay, Tax, NetPay, Super);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
