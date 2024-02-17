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

namespace SellTickets
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            mainFrame.Content = new TabControl();
        }

        private void addEvent_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new AddEvent();
            addEvent.Visibility = Visibility.Collapsed;
            buyTicket.Visibility = Visibility.Collapsed;
            back.Visibility = Visibility.Visible;
        }

        private void buyTicket_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new AddTicket();
            addEvent.Visibility = Visibility.Collapsed;
            buyTicket.Visibility = Visibility.Collapsed;
            back.Visibility = Visibility.Visible;
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new TabControl();
            addEvent.Visibility = Visibility.Visible;
            buyTicket.Visibility = Visibility.Visible;
            back.Visibility = Visibility.Collapsed;
        }
    }
}
