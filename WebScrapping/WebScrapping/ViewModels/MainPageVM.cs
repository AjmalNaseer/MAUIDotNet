using CommunityToolkit.Mvvm.Input;
using HtmlAgilityPack;
using OfficeOpenXml;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using WebScrapping.Models;
using WebScrapping.Services;

namespace WebScrapping.ViewModels
{
    public class MainPageVM : INotifyPropertyChanged
    {
        public ObservableCollection<CustomerData> ExcelData { get; set; } = new ObservableCollection<CustomerData>();

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public ICommand LoadExcelCommand { get; }
        public ICommand WebScrapingCommand { get; }

        private DataFetcher _dataFetcher;

        public MainPageVM()
        {
            _dataFetcher = new DataFetcher();
            LoadExcelCommand = new AsyncRelayCommand(OnLoadExcelAsync);
            WebScrapingCommand = new AsyncRelayCommand(OnWebScrapingAsync);
        }

        // Load Excel file and populate the data
        private async Task OnLoadExcelAsync()
        {
            IsBusy = true;
            try
            {
                Debug.WriteLine("Load Excel Command executed");
                // Your file picking code here...
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                // Handle the exception (e.g., show an alert to the user)
            }
            finally
            {
                IsBusy = false;
            }
        }


        // Perform web scraping and update the data
        private async Task OnWebScrapingAsync()
        {
            IsBusy = true;
            try
            {
                // Log in first
                if (await _dataFetcher.LoginAsync())
                {
                    foreach (var customer in ExcelData)
                    {
                        var updatedData = await _dataFetcher.FetchCustomerDataAsync(customer.CardNumber);
                        if (updatedData != null)
                        {
                            customer.CustomerNumber = updatedData.CustomerNumber;
                            customer.Email = updatedData.Email;
                            customer.MobileNumber = updatedData.MobileNumber;
                        }
                    }

                    OnPropertyChanged(nameof(ExcelData));
                }
                else
                {
                    Debug.WriteLine("Login failed");
                    // Handle login failure (e.g., show an alert to the user)
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                // Handle any exceptions (e.g., display an error message)
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
