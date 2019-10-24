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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ADODisConnected
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;
            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter("Select * from Employee", connection);
            commandBuilder = new SqlCommandBuilder(adapter);
            DataSet = new DataSet();
        }
        string connectionString;
        SqlConnection connection;
        SqlDataAdapter adapter;
        SqlCommandBuilder commandBuilder;
        DataSet DataSet;
        public void GetAll()
        {
            connection.Open();
            adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            adapter.Fill(DataSet,"emp");
            connection.Close();
            datagrid1.DataContext = DataSet.Tables["emp"];
            //datagrid1.DataContext = DataSet.Tables[0];
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetAll();
        }

        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            foreach (DataRow item in DataSet.Tables["emp"].Rows)
            {
                MessageBox.Show(item.RowState.ToString());
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                adapter.Update(DataSet.Tables["emp"]);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //DataRow newRow = DataSet.Tables["emp"].NewRow();
            //newRow["Id"] = txtId.Text;
            //newRow["Name"] = txtName.Text;
            //newRow["Salary"] = txtSalary.Text;
            //DataSet.Tables["emp"].Rows.Add(newRow);




            DataSet.Tables["emp"].Rows.Add(txtId.Text, txtName.Text, txtSalary.Text);
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            //foreach (DataRow item in DataSet.Tables["emp"].Rows)
            //{
            //    if(item["Id"].ToString()==txtId.Text)
            //    {
            //        item.Delete();
            //        break;
            //    }
            //}

          DataRow row=  DataSet.Tables["emp"].Rows.Find(txtId.Text);
            if(row!=null)
            {
                row["Name"] = txtName.Text;
                row["Salary"] = txtSalary.Text;
            }
            else
            {
                MessageBox.Show("No User Found !!");
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            datagrid1.DataContext = DataSet.Tables["emp"]
                .Select($"name like '{txtSearch.Text}%'").CopyToDataTable();
        }
    }
}
