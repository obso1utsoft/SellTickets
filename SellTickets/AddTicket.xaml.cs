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
    /// Логика взаимодействия для AddTicket.xaml
    /// </summary>
    public partial class AddTicket : Page
    {
        SQLiteDataReader dr;
        public AddTicket()
        {
            InitializeComponent();

            using (SQLiteConnection con = new SQLiteConnection(@"Data Source=" + System.IO.Path.Combine(Environment.CurrentDirectory, @"TicketSystem.db")))
            {
                con.Open();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = con;
                cmd.CommandText = "SELECT * FROM Events";
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    eventBox.Items.Add(dr["event_name"].ToString());
                }
                dr.Close();
                con.Close();
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(nameBox.Text) || string.IsNullOrEmpty(emailBox.Text) || string.IsNullOrEmpty(phoneBox.Text) || eventBox.SelectedItem == null || string.IsNullOrEmpty(seatBox.Text) || statusBox.SelectedItem == null)
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
                    cmd.CommandText = "INSERT INTO Customers ( customer_id  , customer_name, customer_email, customer_phone ) VALUES ( NULL, @customer_name, @customer_email, @customer_phone )";
                    cmd.Parameters.AddWithValue("@customer_name", nameBox.Text);
                    cmd.Parameters.AddWithValue("@customer_email", emailBox.Text);
                    cmd.Parameters.AddWithValue("@customer_phone", phoneBox.Text);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT customer_id FROM Customers WHERE customer_name =  '" + nameBox.Text + "' AND customer_email = '" + emailBox.Text + "' AND customer_phone = '" + phoneBox.Text + "'";
                    dr = cmd.ExecuteReader();
                    dr.Read();
                    string customer_id = (dr["customer_id"]).ToString();
                    dr.Close();

                    cmd.CommandText = "SELECT event_id FROM Events WHERE event_name =  '" + eventBox.SelectedItem.ToString() + "'";
                    dr = cmd.ExecuteReader();
                    dr.Read();
                    string event_id = (dr["event_id"]).ToString();
                    dr.Close();

                    cmd.CommandText = "INSERT INTO Tickets ( ticket_id, event_id, customer_id, seat_number, ticket_status ) VALUES ( NULL, @event_id, @customer_id, @seat_number, @ticket_status )";
                    cmd.Parameters.AddWithValue("@event_id", int.Parse(event_id));
                    cmd.Parameters.AddWithValue("@customer_id", int.Parse(customer_id));
                    cmd.Parameters.AddWithValue("@seat_number", int.Parse(seatBox.Text));
                    cmd.Parameters.AddWithValue("@ticket_status", ((ComboBoxItem)statusBox.SelectedItem).Content.ToString());
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Билет Выдан", Title = "Успех");
                }
            }
        }

        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
    }
}
