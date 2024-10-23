using System.ComponentModel;

namespace PublicAPI.Models
{
    public class Items : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public List<SpecialItems> SpecialItems { get; set; }
        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCompleted)));
            }
        }

    }
}