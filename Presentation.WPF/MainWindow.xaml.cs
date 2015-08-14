using ATRealize.Mobilution.ImageUploader.Presentation.WPF.ViewModels;
using Microsoft.Win32;
using System;
using System.Windows;

namespace ATRealize.Mobilution.ImageUploader.Presentation.WPF
{
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; private set; }

        public MainWindow()
        {
            ViewModel = new MainViewModel();
            DataContext = ViewModel;
            InitializeComponent();
        }

        protected async override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            await ViewModel.SearchAsync();
        }

        private async void Upload(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = "*.jpg";
            if (dialog.ShowDialog() == true)
            {
                await ViewModel.CreateAsync(dialog.FileName);
                await ViewModel.SearchAsync();
                MessageBox.Show("OK");
            }
        }
    }
}
