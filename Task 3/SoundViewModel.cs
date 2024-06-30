using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Win32;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace Task_3
{
    class SoundViewModel : INotifyPropertyChanged
    {
        private string? artist;
        private string? album;
        private string? track;
        private string? title;
        private string? filename;

        public string? Artist
        {
            get => artist;
            private set
            {
                artist = value;
                OnPropertyChanged(nameof(Artist));
            }
        }

        public string? Album
        {
            get => album;
            private set
            {
                album = value;
                OnPropertyChanged(nameof(Album));
            }
        }

        public string? Track
        {
            get => track;
            private set
            {
                track = value;
                OnPropertyChanged(nameof(Track));
            }
        }

        public string? Title
        {
            get => title;
            private set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string? Filename
        {
            get => filename;
            private set
            {
                filename = value;
                OnPropertyChanged(nameof(Filename));
            }
        }

        public ICommand ImportCommand { get; }
        public ICommand RenameCommand { get; }

        public class RelayCommand : ICommand
        {
            private readonly Action<object?> _execute;
            private readonly Func<object?, bool>? _canExecute;

            public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
            {
                _execute = execute;// ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            }

            public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);
            public void Execute(object? parameter) => _execute(parameter);
            public event EventHandler? CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
        }

        private ObservableCollection<SoundModel> _sounds = new ObservableCollection<SoundModel>();
        public ObservableCollection<SoundModel> Sounds
        {
            get => _sounds;
            set
            {
                _sounds = value;
                OnPropertyChanged(nameof(Sounds));
            }
        }

        public SoundViewModel()
        {
            ImportCommand = new RelayCommand((object? parameter) => 
            { 
                Sounds = new ObservableCollection<SoundModel>(SoundModel.LoadSoundsViaDialog()); 
            });

            RenameCommand = new RelayCommand((object? parameter) => { });
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
