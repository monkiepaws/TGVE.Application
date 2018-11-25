using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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
using TGVE.Api;
using TGVE.WebApi.Models;
using TMM.UI;

namespace TGVE.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ClientsClient clientsClient { get; } = new ClientsClient();
        public ToursClient toursClient { get; } = new ToursClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Btn_View_Click(object sender, RoutedEventArgs e)
        {
            switch (Combo_View.SelectionBoxItem)
            {
                case "Clients":
                    await ViewClients();
                    break;
                case "Tours":
                    await ViewTours();
                    break;
                default:
                    break;
            }
        }

        private async void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            if (DataView.SelectedItem.GetType() == typeof(Client))
            {
                await AddClient();
            }
            else if (DataView.SelectedItem.GetType() == typeof(Tour))
            {
                await AddTour();
            }

            DataView.Items.Refresh();
        }

        private async void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (DataView.SelectedItem.GetType() == typeof(Client))
            {
                await EditClient();
            }
            else if (DataView.SelectedItem.GetType() == typeof(Tour))
            {
                await EditTour();
            }
            else
            {
                return;
            }
        }

        private async void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (DataView.SelectedItem.GetType() == typeof(Client))
            {
                await DeleteClient();
            }
            else if (DataView.SelectedItem.GetType() == typeof(Tour))
            {
                await DeleteTour();
            }

            DataView.Items.Refresh();
        }

        private async Task ViewClients()
        {
            DataView.ItemsSource = await clientsClient.GetAsync();
        }

        private async Task ViewTours()
        {
            DataView.ItemsSource = await toursClient.GetAsync();
        }

        private async Task EditClient()
        {
            Client client = (Client)DataView.SelectedItem;

            if (!IsValidClient(client))
            {
                return;
            }

            Client response = await clientsClient.PutAsync(client);
            MessageBox.Show($"Client {client.FirstName} {client.LastName} successfully changed", "Edit", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        private async Task EditTour()
        {
            Tour tour = (Tour)DataView.SelectedItem;

            if (!IsValidTour(tour))
            {
                return;
            }

            Tour response = await toursClient.PutAsync(tour);
            MessageBox.Show($"Tour {tour.Name} at {tour.Location} successfully changed", "Edit", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        private async Task AddClient()
        {
            Client client = (Client)DataView.SelectedItem;

            if (!IsValidClient(client))
            {
                return;
            }

            Client response = await clientsClient.PostAsync(client);
            List<Client> list = (List<Client>)DataView.ItemsSource;
            int lastAdded = list.FindIndex(s => s.Id == client.Id);
            list[lastAdded] = response;
            DataView.ItemsSource = list;
            MessageBox.Show($"Client {response.FirstName} {response.LastName} successfully added", "Add", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        private async Task AddTour()
        {
            Tour tour = (Tour)DataView.SelectedItem;

            if (!IsValidTour(tour))
            {
                return;
            }

            Tour response = await toursClient.PostAsync(tour);
            List<Tour> list = (List<Tour>)DataView.ItemsSource;
            int lastAdded = list.FindIndex(s => s.Id == tour.Id);
            list[lastAdded] = response;
            DataView.ItemsSource = list;
            MessageBox.Show($"Tour {response.Name} at {response.Location} successfully added", "Add", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        private async Task DeleteClient()
        {
            Client client = (Client)DataView.SelectedItem;
            string response = await clientsClient.DeleteAsync(client.Id);
            List<Client> list = (List<Client>)DataView.ItemsSource;
            list.Remove(client);
            DataView.ItemsSource = list;
            MessageBox.Show($"Client {client.FirstName} {client.LastName} successfully deleted", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async Task DeleteTour()
        {
            Tour tour = (Tour)DataView.SelectedItem;
            string response = await toursClient.DeleteAsync(tour.Id);
            List<Tour> list = (List<Tour>)DataView.ItemsSource;
            list.Remove(tour);
            DataView.ItemsSource = list;
            MessageBox.Show($"Tour {tour.Name} at {tour.Location} successfully deleted", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool IsValidClient(Client client)
        {
            try
            {
                if (DateTime.Now <= client.DateOfBirth)
                {
                    throw new ValidationFailureException($"{client.DateOfBirth} is not a valid date.");
                }

                Regex nameRegex = new Regex("^[a-zA-Z]*$");

                if (!nameRegex.IsMatch(client.FirstName) || !nameRegex.IsMatch(client.LastName))
                {
                    throw new ValidationFailureException($"{client.FirstName} {client.LastName} is not a valid name.");
                }

                Regex phoneRegex = new Regex(@"\+\d* \d{3}\ *\d{3} \d{4}");

                if (!phoneRegex.IsMatch(client.PhoneNumber))
                {
                    throw new ValidationFailureException($"{client.PhoneNumber} not valid. Phone number must be in +123 555 555 5555 format.");
                }

                MailAddress email = new MailAddress(client.Email);
                return true;
            }
            catch (ValidationFailureException error)
            {
                MessageBox.Show($"Could not add Client\n\n{error.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            catch (Exception error)
            {
                MessageBox.Show($"Could not add Client\n\n{error.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
        }

        private bool IsValidTour(Tour tour)
        {
            try
            {
                if (DateTime.Now > tour.TourStartTime || DateTime.Now > tour.TourEndTime || (tour.TourStartTime >= tour.TourEndTime))
                {
                    throw new ValidationFailureException($"{tour.TourStartTime} or {tour.TourEndTime} is not a valid date.");
                }

                return true;
            }
            catch (ValidationFailureException error)
            {
                MessageBox.Show($"Could not add Tour\n\n{error.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            catch (Exception error)
            {
                MessageBox.Show($"Could not add Tour\n\n{error.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
        }
    }
}
