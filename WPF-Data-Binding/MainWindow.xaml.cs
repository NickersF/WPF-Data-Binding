using iRely.Common.Ioc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Data_Binding.learningSQLDataSetTableAdapters;
using WPF_Data_Binding.ToolsWindows;

namespace WPF_Data_Binding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Window instances
        ManageJobDescriptions manageJobDescriptions = new ManageJobDescriptions();

        // These private fields are for uopdating records
        private int originalId;
        private int? originalJobId;
        private string originalFirstName;
        private string originalLastName;
        private string originalEmail;

        // Initial data load
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            learningSQLDataSet learningSQLDataSet = ((learningSQLDataSet)(this.FindResource("learningSQLDataSet")));

            // Load data into the table employee. You can modify this code as needed.
            employeeTableAdapter learningSQLDataSetemployeeTableAdapter = new employeeTableAdapter();
            learningSQLDataSetemployeeTableAdapter.Fill(learningSQLDataSet.employee);
            CollectionViewSource employeeViewSource = ((CollectionViewSource)(this.FindResource("employeeViewSource")));
            employeeViewSource.View.MoveCurrentToFirst();

            numOfTables_label.Text      = learningSQLDataSet.Tables.Count.ToString();
            tableName_label.Text        = learningSQLDataSet.employee.TableName;
            numOfEmployees_label.Text   = learningSQLDataSet.employee.Rows.Count.ToString();
        }

        // Adds a new row to the database
        private void AddUser(object sender, RoutedEventArgs e)
        {
            learningSQLDataSet learningSQLDataSet = ((learningSQLDataSet)(this.FindResource("learningSQLDataSet")));
            employeeTableAdapter addEmployeeDataAdapter = new employeeTableAdapter();
            DataRowView dataRowView = (DataRowView)employeeDataGrid.SelectedItem;

            Random random = new Random();

            int employeeId      = random.Next(0, 1000);
            string firstName    = addUserFirstName.Text;
            string lastName     = addUserLastName.Text;
            string email        = addUserEmail.Text;
            int? jobId          = 0;

            addEmployeeDataAdapter.Insert(employeeId, firstName, lastName, email, jobId);

            addUserFirstName.Text   = "";
            addUserLastName.Text    = "";
            addUserEmail.Text       = "";

            addEmployeeDataAdapter.ClearBeforeFill = true;
            addEmployeeDataAdapter.Fill(learningSQLDataSet.employee);
            numOfEmployees_label.Text = learningSQLDataSet.employee.Rows.Count.ToString();
        }

        // Edits a selected data row
        private void EditUser(object sender, RoutedEventArgs e)
        {
            learningSQLDataSet learningSQLDataSet = ((learningSQLDataSet)(this.FindResource("learningSQLDataSet")));
            employeeTableAdapter editEmployeeTableAdapter = new employeeTableAdapter();
            DataRowView dataRowView = (DataRowView)employeeDataGrid.SelectedItem;

            int id              = int.Parse(editUserId.Text);
            string firstName    = editUserFirstName.Text;
            string lastName     = editUserLastName.Text;
            string email        = editUserEmail.Text;
            int? jobId          = ConvertFromDBVal<int>(dataRowView.Row[4]);

            editEmployeeTableAdapter.Update(firstName, lastName, email, jobId, originalId, originalFirstName, originalLastName, originalEmail, originalJobId);
            editEmployeeTableAdapter.ClearBeforeFill = true;
            editEmployeeTableAdapter.Fill(learningSQLDataSet.employee);
        }

        // Selection changed event to capture the original field values
        private void employeeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)employeeDataGrid.SelectedItem;

            if ((dataRowView != null) && (employeeDataGrid.SelectedItem.GetType() == typeof(DataRowView)))
            {
                originalId = Convert.ToInt32(dataRowView.Row[0]);
                originalFirstName = (string)dataRowView.Row[1];
                originalLastName = (string)dataRowView.Row[2];
                originalEmail = (string)dataRowView.Row[3];
                originalJobId = ConvertFromDBVal<int>(dataRowView.Row[4]);
            }
            else
            {
                return;
            }
        }

        // Deletes a selected user from the database
        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            learningSQLDataSet learningSQLDataSet = ((learningSQLDataSet)(this.FindResource("learningSQLDataSet")));
            employeeTableAdapter deleteEmployeeTableAdapter = new employeeTableAdapter();
            DataRowView dataRowView = (DataRowView)employeeDataGrid.SelectedItem;

            if ((dataRowView != null) && (employeeDataGrid.SelectedItem.GetType() == typeof(DataRowView)))
            {
                originalJobId = ConvertFromDBVal<int>(dataRowView.Row[4]);
                deleteEmployeeTableAdapter.Delete(originalId, originalFirstName, originalLastName, originalEmail, originalJobId);
                deleteEmployeeTableAdapter.ClearBeforeFill = true;
                deleteEmployeeTableAdapter.Fill(learningSQLDataSet.employee);
            }
            else
            {
                return;
            }
        }

        // Refreshes the employee grid view
        private void RefreshEmployeeGrid(object sender, RoutedEventArgs e)
        {
            learningSQLDataSet learningSQLDataSet = ((learningSQLDataSet)(this.FindResource("learningSQLDataSet")));
            employeeTableAdapter refreshEmployeeTableAdapter = new employeeTableAdapter();

            refreshEmployeeTableAdapter.ClearBeforeFill = true;
            refreshEmployeeTableAdapter.Fill(learningSQLDataSet.employee);
        }

        // Close button
        private void ExitApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Opens job description manager window
        private void Show_ManageJobDescriptions(object sender, RoutedEventArgs e)
        {
            ManageJobDescriptions manageJobDescriptions = new ManageJobDescriptions();
            manageJobDescriptions.Show();
        }

        // Opens add job descriptions window
        private void Show_AssignJobDescriptions(object sender, RoutedEventArgs e)
        {
            AssignJobDescriptions assignJobDescriptions = new AssignJobDescriptions();
            assignJobDescriptions.Show(); 
        }

        // Generic utility function to convert DBNull
        public static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T); // returns the default value for the type
            }
            else
            {
                return (T)obj;
            }
        }

    }
}
