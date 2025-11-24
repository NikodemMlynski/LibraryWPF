using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Library.ViewModels
{
    // Podstawowa klasa dla wszystkich ViewModeli, implementująca interfejs INotifyPropertyChanged
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Metoda pomocnicza do zgłaszania zmian właściwości
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Metoda pomocnicza do ustawiania wartości właściwości i zgłaszania zmian
        protected bool SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
            {
                return false;
            }
            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}