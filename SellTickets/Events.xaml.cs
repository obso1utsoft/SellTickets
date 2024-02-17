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
    /// Логика взаимодействия для Events.xaml
    /// </summary>
    public partial class Events : Page
    {
        public Events()
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
                cmd.CommandText = "SELECT * FROM Events";
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
                    cmd.CommandText = "DELETE FROM Events WHERE event_id   = @num; " +
                                      "UPDATE Events SET event_id   = event_id  -1  WHERE event_id  >@num;" +
                                      "UPDATE sqlite_sequence SET seq = @row -1 WHERE name = 'Events';";
                    cmd.Parameters.AddWithValue("@num", mydatagrid.SelectedIndex + 1);
                    cmd.Parameters.AddWithValue("@row", mydatagrid.Items.Count - 1);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Мероприятие удалено", Title = "Успех");
                }
            }
            catch
            {
                MessageBox.Show("Мероприятие не найдено", Title = "Ошибка");
            }
        }
    }
}
