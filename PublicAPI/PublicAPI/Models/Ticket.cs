using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PublicAPI.Models
{
    public class Ticket : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string TicketNumber { get; set; }
        public int TableNumber { get; set; }
        public List<Items> Items { get; set; }
        public string WaiterName { get; set; }
        public DateTime OrderDateTime { get; set; }
        public int ItemNumber { get; set; }


        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyChanged(); // Notify that IsCompleted has changed
                }
            }
        }
        private bool _isTimeExceeded; // New property to track if time exceeded
        public bool IsTimeExceeded
        {
            get => _isTimeExceeded;
            set
            {
                if (_isTimeExceeded != value)
                {
                    _isTimeExceeded = value;
                    OnPropertyChanged();
                }
            }
        }

        public void RefreshTimeExceededStatus(DateTime currentDateTime)
        {
            // Update the IsTimeExceeded property based on the OrderDateTime and current time
            IsTimeExceeded = !IsCompleted && OrderDateTime < currentDateTime;
        }



        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
