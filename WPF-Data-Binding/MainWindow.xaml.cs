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

        private int originalId;
        private string originalFirstName;
        private string originalLastName;
        private string originalEmail;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            learningSQLDataSet learningSQLDataSet = ((learningSQLDataSet)(this.FindResource("learningSQLDataSet")));
            // Load data into the table employee. You can modify this code as needed.
            employeeTableAdapter learningSQLDataSetemployeeTableAdapter = new employeeTableAdapter();
            learningSQLDataSetemployeeTableAdapter.Fill(learningSQLDataSet.employee);
            CollectionViewSource employeeViewSource = ((CollectionViewSource)(this.FindResource("employeeViewSource")));
            employeeViewSource.View.MoveCurrentToFirst();
        }

        private void AddUser(object sender, RoutedEventArgs e)
        {
            learningSQLDataSet learningSQLDataSet = ((learningSQLDataSet)(this.FindResource("learningSQLDataSet")));
            employeeTableAdapter addEmployeeDataAdapter = new employeeTableAdapter();
            Random random = new Random();

            int employeeId = random.Next(0, 1000);
            string firstName = addUserFirstName.Text;
            string lastName = addUserLastName.Text;
            string email = addUserEmail.Text;

            addEmployeeDataAdapter.Insert(employeeId, firstName, lastName, email);

            addUserFirstName.Text = "";
            addUserLastName.Text = "";
            addUserEmail.Text = "";

            addEmployeeDataAdapter.ClearBeforeFill = true;
            addEmployeeDataAdapter.Fill(learningSQLDataSet.employee);
        }

        private void EditUser(object sender, RoutedEventArgs e)
        {
            learningSQLDataSet learningSQLDataSet = ((learningSQLDataSet)(this.FindResource("learningSQLDataSet")));
            employeeTableAdapter editEmployeeTableAdapter = new employeeTableAdapter();

            int id = int.Parse(editUserId.Text);
            string firstName = editUserFirstName.Text;
            string lastName = editUserLastName.Text;
            string email = editUserEmail.Text;

            learningSQLDataSet.employeeDataTable updatedEmployeeTable = new learningSQLDataSet.employeeDataTable();

            editEmployeeTableAdapter.Update(firstName, lastName, email, originalId, originalFirstName, originalLastName, originalEmail);
            editEmployeeTableAdapter.ClearBeforeFill = true;
            editEmployeeTableAdapter.Fill(learningSQLDataSet.employee);
        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            learningSQLDataSet learningSQLDataSet = ((learningSQLDataSet)(this.FindResource("learningSQLDataSet")));
            employeeTableAdapter deleteEmployeeTableAdapter = new employeeTableAdapter();
            DataRowView dataRowView = (DataRowView)employeeDataGrid.SelectedItem;

            if ((dataRowView != null) && (employeeDataGrid.SelectedItem.GetType() == typeof(System.Data.DataRowView)))
            {
                deleteEmployeeTableAdapter.Delete(originalId, originalFirstName, originalLastName, originalEmail);
                deleteEmployeeTableAdapter.ClearBeforeFill = true;
                deleteEmployeeTableAdapter.Fill(learningSQLDataSet.employee);
            }
            else
            {
                return;
            }

        }

        private void employeeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)employeeDataGrid.SelectedItem;

            if ((dataRowView != null) && (employeeDataGrid.SelectedItem.GetType() == typeof(System.Data.DataRowView)))
            {
                originalId = Convert.ToInt32(dataRowView.Row[0]);
                originalFirstName = (string)dataRowView.Row[1];
                originalLastName = (string)dataRowView.Row[2];
                originalEmail = (string)dataRowView[3];
            } else
            {
                return;
            }
        }
    }
}
