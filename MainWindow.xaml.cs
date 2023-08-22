using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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

namespace ProjectSocilalNetwork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection connection;
        public ObservableCollection<String> columns { get; set; } = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            connection = null;
            this.DataContext = this;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                connection = new SqlConnection(App.ConnectionString);
                connection.Open();
                LoadGroups();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void LoadGroups()
        {
            SqlCommand command = new SqlCommand()
            {
                Connection = connection,
                CommandText = "SELECT * FROM TestProduct"
            };
            try
            {
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())  // get result's one row
                {
                    columns.Add(
                        $"Id: {reader.GetGuid(0).ToString()[0]}, Name: {reader.GetString(1)}, Price: {reader.GetInt32(3)}, Quantity: {reader.GetInt32(4)}"
                    );
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Query error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            connection?.Close();
        }

        private void CreateGroup_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand
            {
                Connection = connection,
                CommandText = @"CREATE TABLE [dbo].[TestProduct] (
                            [Id]          UNIQUEIDENTIFIER NOT NULL,
                            [name]        NVARCHAR (50)    NOT NULL,
                            [id_category] INT              NOT NULL,
                            [price]       INT              NOT NULL,
                            [quantity] INT NOT NULL, 
                            PRIMARY KEY CLUSTERED ([Id] ASC));"
                };
            try
            {
                sqlCommand.ExecuteNonQuery();
                System.Windows.MessageBox.Show("Table Crated");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Create Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InsertGroup_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = @"INSERT INTO TestProduct( Id, name, id_category, price, quantity)
VALUES
( '089015F4-31B5-4F2B-BA05-A813B5419285', N'Картопля', N'1', N'23', N'100' )
                ";
            try
            {
                command.ExecuteNonQuery();
                System.Windows.MessageBox.Show("Data inserted");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Insert Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GroupCount_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = @"SELECT COUNT(*) FROM TestProduct";
            try
            {
                UInt16 temp = Convert.ToUInt16(command.ExecuteScalar());
                System.Windows.MessageBox.Show(temp.ToString());
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Querry Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
