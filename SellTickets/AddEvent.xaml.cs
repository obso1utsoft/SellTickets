using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
    /// Логика взаимодействия для AddEvent.xaml
    /// </summary>
    public partial class AddEvent : Page
    {
        public AddEvent()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(nameBox.Text) || string.IsNullOrEmpty(dateBox.Text) || string.IsNullOrEmpty(timeBox.Text) || string.IsNullOrEmpty(locationBox.Text) || string.IsNullOrEmpty(priceBox.Text))
            {
                MessageBox.Show("Заполните все поля", Title = "Ошибка");
            }
            else
            {
                using (SQLiteConnection con = new SQLiteConnection(@"Data Source=" + System.IO.Path.Combine(Environment.CurrentDirectory, @"TicketSystem.db")))
                {
                    con.Open();
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "INSERT INTO Events ( event_id , event_name , event_date, event_time, event_location, ticket_price ) VALUES ( NULL, @event_name, @event_date, @event_time, @event_location, @ticket_price)";
                    cmd.Parameters.AddWithValue("@event_name", nameBox.Text);
                    cmd.Parameters.AddWithValue("@event_date", dateBox.Text);
                    cmd.Parameters.AddWithValue("@event_time", timeBox.Text);
                    cmd.Parameters.AddWithValue("@event_location", locationBox.Text);
                    cmd.Parameters.AddWithValue("@ticket_price", priceBox.Text);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Мероприятие добавлено", Title = "Успех");
                }
            }
        }

        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) || (e.Text == ".")
               && (!priceBox.Text.Contains(".")
               && priceBox.Text.Length != 0)))
            {
                e.Handled = true;
            }
        }
    }
}
