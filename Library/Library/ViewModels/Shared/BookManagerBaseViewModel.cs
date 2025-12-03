using Library.Models;
using Library.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Library.ViewModels.Shared
{
    public class BookManagerBaseViewModel : BaseViewModel
    {
        public string Title => "Zarządzanie Książkami";
        private readonly LibrarianService _librarianService;

        // --- Kolekcja ---
        private ObservableCollection<Book> _books;
        public ObservableCollection<Book> Books { get => _books; set => SetValue(ref _books, value); }

        // --- Pola formularza ---
        private string _newTitle;
        public string NewTitle { get => _newTitle; set => SetValue(ref _newTitle, value); }

        private string _newAuthor;
        public string NewAuthor { get => _newAuthor; set => SetValue(ref _newAuthor, value); }

        private int _newQuantity;
        public int NewQuantity { get => _newQuantity; set => SetValue(ref _newQuantity, value); }

        // --- Stan Modala ---
        private bool _isModalVisible;
        public bool IsModalVisible { get => _isModalVisible; set => SetValue(ref _isModalVisible, value); }

        // Czy jesteśmy w trybie edycji?
        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                SetValue(ref _isEditMode, value);
                // Aktualizuj tekst przycisku w zależności od trybu
                OnPropertyChanged(nameof(ModalButtonText));
                OnPropertyChanged(nameof(ModalTitle));
            }
        }

        // ID edytowanej książki (null jeśli dodajemy nową)
        private int? _editingBookId = null;

        // Dynamiczne teksty
        public string ModalButtonText => IsEditMode ? "Zaktualizuj" : "Dodaj";
        public string ModalTitle => IsEditMode ? "Edytuj książkę" : "Dodaj nową książkę";

        // --- Komendy ---
        public ICommand OpenAddModalCommand { get; }
        public ICommand CloseAddModalCommand { get; }
        public ICommand SaveOrUpdateCommand { get; } // Jedna komenda do zapisu (robi Add lub Update)
        public ICommand DeleteBookCommand { get; }   // Komenda usuwania (z parametrem ID)
        public ICommand EditBookCommand { get; }     // Komenda otwierania edycji (z parametrem Book)

        public BookManagerBaseViewModel()
        {
            _librarianService = new LibrarianService();
            Books = new ObservableCollection<Book>();

            // Otwieranie modala "Dodaj" (czyścimy pola)
            OpenAddModalCommand = new RelayCommand(_ =>
            {
                ClearForm();
                IsEditMode = false;
                IsModalVisible = true;
            });

            // Otwieranie modala "Edytuj" (wypełniamy pola)
            EditBookCommand = new RelayCommand(param =>
            {
                if (param is Book bookToEdit)
                {
                    NewTitle = bookToEdit.Title;
                    NewAuthor = bookToEdit.Author;
                    NewQuantity = bookToEdit.Quantity;
                    _editingBookId = bookToEdit.Id;

                    IsEditMode = true;
                    IsModalVisible = true;
                }
            });

            CloseAddModalCommand = new RelayCommand(_ => IsModalVisible = false);

            // Logika przycisku w modalu (Add vs Update)
            SaveOrUpdateCommand = new RelayCommand(async _ => await SaveOrUpdate());

            // Usuwanie
            DeleteBookCommand = new RelayCommand(async id => await DeleteBook((int)id));

            _ = LoadBooks();
        }

        private void ClearForm()
        {
            NewTitle = "";
            NewAuthor = "";
            NewQuantity = 0;
            _editingBookId = null;
        }

        private async Task LoadBooks()
        {
            var dbBooks = await _librarianService.GetAllBooksAsync();
            Books.Clear();
            foreach (var book in dbBooks) Books.Add(book);
        }

        private async Task DeleteBook(int id)
        {
            var result = MessageBox.Show("Czy na pewno chcesz usunąć tę książkę?", "Potwierdź", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                bool success = await _librarianService.DeleteBookAsync(id);
                if (success) await LoadBooks();
            }
        }

        private async Task SaveOrUpdate()
        {
            if (string.IsNullOrWhiteSpace(NewTitle) || string.IsNullOrWhiteSpace(NewAuthor)) return;

            bool success;

            if (IsEditMode && _editingBookId.HasValue)
            {
                // UPDATE
                success = await _librarianService.UpdateBookAsync(_editingBookId.Value, NewTitle, NewAuthor, NewQuantity);
            }
            else
            {
                // ADD
                success = await _librarianService.AddBookAsync(NewTitle, NewAuthor, NewQuantity);
            }

            if (success)
            {
                await LoadBooks();
                IsModalVisible = false;
                ClearForm();
            }
            else
            {
                MessageBox.Show("Wystąpił błąd zapisu.");
            }
        }
    }
}