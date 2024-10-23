using PublicAPI.Models;
using PublicAPI.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PublicAPI.Services.PublicAPI.Services;

namespace PublicAPI.ViewModels
{
    public class MainPageVM : INotifyPropertyChanged
    {
        #region Fields

        private CurrentPrice _currentPrice;
        private ObservableCollection<CurrencyInfo> _bpiList;
        public readonly APIFetch aPIFetch;

        #endregion

        #region Properties

        public event PropertyChangedEventHandler PropertyChanged;

        public CurrentPrice CurrentPrice
        {
            get => _currentPrice;
            set
            {
                _currentPrice = value;
                OnPropertyChanged();
                // Update BpiList based on the new CurrentPrice data
                if (_currentPrice?.bpi != null)
                {
                    BpiList = new ObservableCollection<CurrencyInfo>
                    {
                        _currentPrice.bpi.USD,
                        _currentPrice.bpi.GBP,
                        _currentPrice.bpi.EUR
                    };
                }
            }
        }

        public ObservableCollection<CurrencyInfo> BpiList
        {
            get => _bpiList;
            set
            {
                _bpiList = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructor

        public MainPageVM()
        {
            aPIFetch = new APIFetch();
            BpiList = new ObservableCollection<CurrencyInfo>(); // Initialize an empty list
            _ = LoadData();
        }

        #endregion

        #region Methods

        public async Task LoadData()
        {
            var url = "https://api.coindesk.com/v1/bpi/currentprice.json";
            var data = await aPIFetch.GetResultAsync<CurrentPrice>(url);

            if (data != null)
            {
                CurrentPrice = data;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
