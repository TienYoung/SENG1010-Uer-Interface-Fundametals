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
        private string pattern;

        public string Pattern
        {
            get => pattern;
            set 
            {
                pattern = value;
                OnPropertyChanged(nameof(Pattern));
                foreach (var soundModel in Sounds)
                {
                    soundModel.UpdateNewName(Pattern);
                }
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
                foreach (var soundModel in Sounds)
                {
                    soundModel.UpdateNewName(Pattern);
                }
            });

            RenameCommand = new RelayCommand((object? parameter) => 
            {
                foreach (var soundModel in Sounds)
                {
                    soundModel.Rename(soundModel.NewName ?? String.Empty);
                }
            });

            pattern = "<Artist>-<Title>-name";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
