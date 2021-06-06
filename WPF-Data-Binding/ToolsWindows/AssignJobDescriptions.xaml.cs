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
using System.Windows.Shapes;
using WPF_Data_Binding.learningSQLDataSetTableAdapters;

namespace WPF_Data_Binding.ToolsWindows
{
    /// <summary>
    /// Interaction logic for AssignJobDescriptions.xaml
    /// </summary>
    public partial class AssignJobDescriptions : Window
    {
        public AssignJobDescriptions()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            learningSQLDataSet learningSQLDataSet = ((learningSQLDataSet)(this.FindResource("learningSQLDataSet")));

            // Load data into the table employee. You can modify this code as needed.
            learningSQLDataSetTableAdapters.employeeTableAdapter learningSQLDataSetemployeeTableAdapter = new learningSQLDataSetTableAdapters.employeeTableAdapter();
            learningSQLDataSetemployeeTableAdapter.Fill(learningSQLDataSet.employee);
            CollectionViewSource employeeViewSource = ((CollectionViewSource)(this.FindResource("employeeViewSource")));
            employeeViewSource.View.MoveCurrentToFirst();

            // Load data into the table job_descriptions. You can modify this code as needed.
            learningSQLDataSetTableAdapters.job_descriptionsTableAdapter learningSQLDataSetjob_descriptionsTableAdapter = new learningSQLDataSetTableAdapters.job_descriptionsTableAdapter();
            learningSQLDataSetjob_descriptionsTableAdapter.Fill(learningSQLDataSet.job_descriptions);
            CollectionViewSource job_descriptionsViewSource = ((CollectionViewSource)(this.FindResource("job_descriptionsViewSource")));
            job_descriptionsViewSource.View.MoveCurrentToFirst();
        }

        private void AssignJobDescription(object sender, RoutedEventArgs e)
        {
            learningSQLDataSet learningSQLDataSet = ((learningSQLDataSet)(this.FindResource("learningSQLDataSet")));
            employeeTableAdapter assignJobIdEmployeeTableAdapter = new employeeTableAdapter();
            DataRowView employeeDataRowView = (DataRowView)selectEmployee_CmbBx.SelectedItem;
            DataRowView jobDescDataRowView = (DataRowView)selectJobDesc_CmbBx.SelectedItem;

            learningSQLDataSet.employeeRow selectedEmployeeRow = ((learningSQLDataSet.employeeRow)employeeDataRowView.Row);
            learningSQLDataSet.job_descriptionsRow selectedJobDescRow = ((learningSQLDataSet.job_descriptionsRow)jobDescDataRowView.Row);

            int? selectedJobId = selectedJobDescRow.job_id;
            int selectedEmployeeId = selectedEmployeeRow.Id;

            assignJobIdEmployeeTableAdapter.UpdateJobIdOnEmployee(selectedJobId, selectedEmployeeId, selectedEmployeeId);
            assignJobIdEmployeeTableAdapter.ClearBeforeFill = true;
            assignJobIdEmployeeTableAdapter.Fill(learningSQLDataSet.employee);
        }
    }
}
