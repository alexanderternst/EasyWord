using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasyWord.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// PropertyChangedEventHandler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Methode die PropertyChanged event ausführt und zusätzlich Name des Properties automatisch extrahiert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storage">vergleichswert</param>
        /// <param name="value">neuer wert</param>
        /// <param name="property">eigentliche property</param>
        protected void SetProperty<T>(ref T storage, T value, [CallerMemberName] string property = null)
        {
            if (Object.Equals(storage, value)) return;
            storage = value;
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Methode die PropertyChanged event ausführt, bei der wir Name des Properties mitgeben müssen
        /// </summary>
        /// <param name="propertyName">Name des Properties</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
