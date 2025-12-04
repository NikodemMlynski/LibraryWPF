using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Library.Models;
using Library.Services;
using Library.ViewModels;
namespace Library.ViewModels.Reader
{
    public class UserRentalsPageViewModel : BaseViewModel
    {
        private readonly ReaderService _readerService;
        private ObservableCollection<Rental> _rentals;
        public ObservableCollection<Rental> Rentals { get => _rentals; set => SetValue(ref _rentals, value); }
        private bool _isModalVisible;
        public bool IsModalVisible { get => _isModalVisible; set => SetValue(ref _isModalVisible, value); }

        private DateTime _selectedReturnDate;
        public DateTime SelectedReturnDate { get => _selectedReturnDate; set => SetValue(ref _selectedReturnDate, value); }
        private Rental _selectedRentalToUpdate;

        public ICommand ReturnBookCommand { get; }
        public ICommand OpenEditReturnDateModalCommand { get; }
        public ICommand CloseEditReturnDateModalCommand { get; }
        public ICommand ConfirmEditReturnCommand { get; }
        public ICommand LoadRentalsCommand { get; }

        public UserRentalsPageViewModel()
        {
            _readerService = new ReaderService();
            Rentals = new ObservableCollection<Rental>();
            SelectedReturnDate = DateTime.Now.AddDays(7);

            OpenEditReturnDateModalCommand = new RelayCommand(OpenModal);
            CloseEditReturnDateModalCommand = new RelayCommand(_ => IsModalVisible = false);
            ConfirmEditReturnCommand = new RelayCommand(async _ => await ConfirmReturnDateUpdate());
            LoadRentalsCommand = new RelayCommand(async _ => await LoadRentals());
            ReturnBookCommand = new RelayCommand(async param => await ReturnBook(param));

            _ = LoadRentals();

        }
        private void OpenModal(object param)
        {
            if (param is Rental rental)
            {
                _selectedRentalToUpdate = rental;
                SelectedReturnDate = DateTime.Now.AddDays(7);
                IsModalVisible = true;
            }
        }
        private async Task ConfirmReturnDateUpdate()
        {
            if (_selectedRentalToUpdate == null) return;

            if (SelectedReturnDate < DateTime.Now.Date)
            {
                MessageBox.Show("Return date cannot be earlier than today.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string resultMessage = await _readerService.UpdateRentalReturnDate(_selectedRentalToUpdate, SelectedReturnDate);

            if (resultMessage.StartsWith("Success"))
            {
                await LoadRentals();
                IsModalVisible = false;
            }
            else
            {
                MessageBox.Show(resultMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task LoadRentals()
        {
            var dbRentals = await _readerService.GetAllRentalsAsync();
            Rentals.Clear();
            foreach (var rental in dbRentals) Rentals.Add(rental);
        }
        private async Task ReturnBook(object param)
        {
            if (param is Rental rental)
            {
                var result = MessageBox.Show($"Are you sure you want to return the book: {rental.Book.Title}?", "Confirm Return",
                                            MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    string message = await _readerService.ReturnBookAsync(rental);

                    if (message.StartsWith("Success"))
                    {
                        await LoadRentals();
                    }
                    else
                    {
                        MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Error: Cannot find rental data to return the book.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}