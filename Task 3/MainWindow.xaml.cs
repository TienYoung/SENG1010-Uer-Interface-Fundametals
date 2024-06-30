﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace Task_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is SoundViewModel soundViewModel && soundViewModel.ImportCommand.CanExecute(null))
            {
                soundViewModel.ImportCommand.Execute(null);
                if (sender is Grid grid)
                {
                    grid.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}