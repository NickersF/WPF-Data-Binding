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
    /// Interaction logic for AddJobDescription.xaml
    /// </summary>
    public partial class ManageJobDescriptions : Window
    {
        public ManageJobDescriptions()
        {
            InitializeComponent();
        }

        // These private fields are for uopdating records
        private int originalJobId;
        private int originalHourlyPayRate;
        private string originalJobTitle;
        private string originalJobDescription;

        // Private field to pass the generated id around methods
        private int generatedJobId;

        // Main grid load
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            learningSQLDataSet learningSQLDataSet = ((learningSQLDataSet)(this.FindResource("learningSQLDataSet")));
            // Load data into the table job_descriptions. You can modify this code as needed.
            job_descriptionsTableAdapter learningSQLDataSetjob_descriptionsTableAdapter = new learningSQLDataSetTableAdapters.job_descriptionsTableAdapter();
            learningSQLDataSetjob_descriptionsTableAdapter.Fill(learningSQLDataSet.job_descriptions);
            CollectionViewSource job_descriptionsViewSource = ((CollectionViewSource)(this.FindResource("job_descriptionsViewSource")));
            job_descriptionsViewSource.View.MoveCurrentToFirst();
        }

        // Adds a job description to the list
        private void AddJobDescription(object sender, RoutedEventArgs e)
        {
            learningSQLDataSet learningSQLDataSet = ((learningSQLDataSet)(this.FindResource("learningSQLDataSet")));
            job_descriptionsTableAdapter addJob_DescriptionsTableAdapter = new job_descriptionsTableAdapter();

            if (generatedJobId == 0)
            {
                MessageBox.Show("Please generate a Job Id before adding a new job description.", "Missing Job Id Field", MessageBoxButton.OK);
            }

            if (addHrlyPay_TxtBx.Text == "")
            {
                return;
            }

            int jobId = generatedJobId;
            int hourlyPayRate = int.Parse(addHrlyPay_TxtBx.Text);
            string jobTitle = addJobTitle_TxtBx.Text;
            string jobDescription = addJobDesc_TxtBx.Text;

            addJob_DescriptionsTableAdapter.Insert(jobTitle, jobDescription, hourlyPayRate, jobId);

            generatedJobId = 0;
            addJobId_Label.Text = "-";
            addHrlyPay_TxtBx.Text = "";
            addJobTitle_TxtBx.Text = "";
            addJobDesc_TxtBx.Text = "";

            addJob_DescriptionsTableAdapter.ClearBeforeFill = true;
            addJob_DescriptionsTableAdapter.Fill(learningSQLDataSet.job_descriptions);
        }

        // Edits a job description
        private void EditJobDescription(object sender, RoutedEventArgs e)
        {
            learningSQLDataSet learningSQLDataSet = ((learningSQLDataSet)(this.FindResource("learningSQLDataSet")));
            job_descriptionsTableAdapter editJob_DescriptionsTableAdapter = new job_descriptionsTableAdapter();

            string editedJobTitle = editJobTitle_TxtBx.Text;
            int editedHourlyPayRate = int.Parse(editHrlyPay_TxtBx.Text);
            int editedJobId = int.Parse(editJobId_TxtBx.Text);
            string editedJobDesc = editJobDesc_TxtBx.Text;


            editJob_DescriptionsTableAdapter.Update(editedJobTitle, editedJobDesc, editedHourlyPayRate, editedJobId, originalJobTitle, originalJobDescription, originalHourlyPayRate, originalJobId);
            editJob_DescriptionsTableAdapter.ClearBeforeFill = true;
            editJob_DescriptionsTableAdapter.Fill(learningSQLDataSet.job_descriptions);
        }

        // Deletes a job description
        private void DeleteJobDescription(object sender, RoutedEventArgs e)
        {
            learningSQLDataSet learningSQLDataSet = ((learningSQLDataSet)(this.FindResource("learningSQLDataSet")));
            job_descriptionsTableAdapter deleteJob_descriptionsTableAdapter = new job_descriptionsTableAdapter();

            DataRowView dataRowView = (DataRowView)job_descriptionsDataGrid.SelectedItem;

            if ((dataRowView != null) && (job_descriptionsDataGrid.SelectedItem.GetType() == typeof(DataRowView)))
            {
                deleteJob_descriptionsTableAdapter.Delete(originalJobTitle, originalJobDescription, originalHourlyPayRate, originalJobId);
                deleteJob_descriptionsTableAdapter.ClearBeforeFill = true;
                deleteJob_descriptionsTableAdapter.Fill(learningSQLDataSet.job_descriptions);
            }
            else
            {
                return;
            }
        }

        // Stores the selected gridView row for various data operations
        private void job_descriptionsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)job_descriptionsDataGrid.SelectedItem;

            if ((dataRowView != null) && (job_descriptionsDataGrid.SelectedItem.GetType() == typeof(DataRowView)))
            {
                originalJobTitle = (string)dataRowView.Row[0];
                originalJobDescription = (string)dataRowView.Row[1];
                originalHourlyPayRate = Convert.ToInt32(dataRowView[2]);
                originalJobId = Convert.ToInt32(dataRowView.Row[3]);
            }
            else
            {
                return;
            }
        }

        // Generates a job id
        private void GenerateJobId(object sender, RoutedEventArgs e)
        {
            Random random = new Random();

            generatedJobId = random.Next(1, 50);
            addJobId_Label.Text = generatedJobId.ToString();
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
