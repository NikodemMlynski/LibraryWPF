using Library.Models;
using Library.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Library.ViewModels.Reader
{
    public class BookListPageViewModel : BaseViewModel
    {
        private readonly ReaderService _readerService;

        // --- Dane Listy ---
        private ObservableCollection<Book> _books;
        public ObservableCollection<Book> Books { get => _books; set => SetValue(ref _books, value); }

        // --- Dane Modala ---
        private bool _isModalVisible;
        public bool IsModalVisible { get => _isModalVisible; set => SetValue(ref _isModalVisible, value); }

        private DateTime _selectedReturnDate;
        public DateTime SelectedReturnDate { get => _selectedReturnDate; set => SetValue(ref _selectedReturnDate, value); }

        private Book _selectedBookToRent; // Przechowujemy książkę, którą użytkownik chce wypożyczyć

        // --- Komendy ---
        public ICommand OpenRentModalCommand { get; } // Otwiera okienko
        public ICommand CloseRentModalCommand { get; } // Anuluje
        public ICommand ConfirmRentCommand { get; }    // Zatwierdza wypożyczenie
        public ICommand LoadBooksCommand { get; }

        public BookListPageViewModel()
        {
            _readerService = new ReaderService();
            Books = new ObservableCollection<Book>();

            // Ustaw domyślną datę zwrotu na "za tydzień"
            SelectedReturnDate = DateTime.Now.AddDays(7);

            // Logika komend
            OpenRentModalCommand = new RelayCommand(OpenModal);
            CloseRentModalCommand = new RelayCommand(_ => IsModalVisible = false);
            ConfirmRentCommand = new RelayCommand(async _ => await ConfirmRent());
            LoadBooksCommand = new RelayCommand(async _ => await LoadBooks());

            _ = LoadBooks();
        }

        private void OpenModal(object param)
        {
            if (param is Book book)
            {
                _selectedBookToRent = book; // Zapamiętujemy, co kliknął użytkownik
                SelectedReturnDate = DateTime.Now.AddDays(7); // Reset daty przy otwarciu
                IsModalVisible = true;
            }
        }

        private async Task ConfirmRent()
        {
            if (_selectedBookToRent == null) return;

            // Walidacja: Data nie może być z przeszłości
            if (SelectedReturnDate < DateTime.Now.Date)
            {
                MessageBox.Show("Data zwrotu nie może być wcześniejsza niż dzisiaj.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Wywołanie serwisu z datą
            string resultMessage = await _readerService.AddRentalAsync(_selectedBookToRent, SelectedReturnDate);

            if (resultMessage.StartsWith("Sukces"))
            {
                MessageBox.Show(resultMessage, "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadBooks();
                IsModalVisible = false;
            }
            else
            {
                MessageBox.Show(resultMessage, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadBooks()
        {
            var dbBooks = await _readerService.GetAllBooksAsync();
            Books.Clear();
            foreach (var book in dbBooks) Books.Add(book);
        }
    }
}