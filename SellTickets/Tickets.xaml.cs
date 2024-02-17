using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

namespace SellTickets
{
    /// <summary>
    /// Логика взаимодействия для Tickets.xaml
    /// </summary>
    public partial class Tickets : Page
    {
        public Tickets()
        {
            InitializeComponent();
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            refresh();
        }

        public void refresh()
        {
            using (SQLiteConnection con = new SQLiteConnection(@"Data Source=" + System.IO.Path.Combine(Environment.CurrentDirectory, @"TicketSystem.db")))
            {
                con.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = con;
                cmd.CommandText = "SELECT * FROM Tickets";
                using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(cmd.CommandText, con))
                {
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    mydatagrid.ItemsSource = dataTable.AsDataView();
                }
                con.Close();

            }
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            refresh();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(@"Data Source=" + System.IO.Path.Combine(Environment.CurrentDirectory, @"TicketSystem.db")))
                {
                    connection.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "DELETE FROM Tickets WHERE ticket_id   = @num; " +
                                      "UPDATE Tickets SET ticket_id   = ticket_id  -1  WHERE ticket_id  >@num;" +
                                      "UPDATE sqlite_sequence SET seq = @row -1 WHERE name = 'Tickets'; " +
                                      "DELETE FROM Customers WHERE customer_id = @num; " +
                                      "UPDATE Customers SET customer_id  = customer_id -1  WHERE customer_id >@num;" +
                                      "UPDATE sqlite_sequence SET seq = @row -1 WHERE name = 'Customers';";
                    cmd.Parameters.AddWithValue("@num", mydatagrid.SelectedIndex + 1);
                    cmd.Parameters.AddWithValue("@row", mydatagrid.Items.Count - 1);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Билет и покупатель удален", Title = "Успех");
                }
            }
            catch
            {
                MessageBox.Show("Билет не найден", Title = "Ошибка");
            }
        }
    }
}
