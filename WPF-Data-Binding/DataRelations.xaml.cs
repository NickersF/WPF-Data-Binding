using System;
using System.Collections.Generic;
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

namespace WPF_Data_Binding
{
    /// <summary>
    /// Interaction logic for DataRelations.xaml
    /// </summary>
    public partial class DataRelations : Window
    {
        public DataRelations()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            WPF_Data_Binding.learningSQLDataSet learningSQLDataSet = ((WPF_Data_Binding.learningSQLDataSet)(this.FindResource("learningSQLDataSet")));
            // Load data into the table job_descriptions. You can modify this code as needed.
            WPF_Data_Binding.learningSQLDataSetTableAdapters.job_descriptionsTableAdapter learningSQLDataSetjob_descriptionsTableAdapter = new WPF_Data_Binding.learningSQLDataSetTableAdapters.job_descriptionsTableAdapter();
            learningSQLDataSetjob_descriptionsTableAdapter.Fill(learningSQLDataSet.job_descriptions);
            System.Windows.Data.CollectionViewSource job_descriptionsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("job_descriptionsViewSource")));
            job_descriptionsViewSource.View.MoveCurrentToFirst();
        }
    }
}
