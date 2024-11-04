using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Input;
using PublicAPI.Models;
using PublicAPI.Views;

namespace PublicAPI.ViewModels
{
    public class TicketsVM : INotifyPropertyChanged
    {
        #region Fields

        private Stack<UndoAction> undoStack = new Stack<UndoAction>();
        private System.Timers.Timer timer;
        private DateTime _currentDateTime;
        private double screenWidth;
        private double screenHeight;

        #endregion

        #region Commands

        public Command<Ticket> CompleteTicketCommand { get; }
        public Command<Items> CompleteItemCommand { get; }
        public ICommand UndoCommand { get; private set; }
        public ICommand NextPageCommand { get; private set; }
        public ICommand PreviousPageCommand { get; private set; }
        public ICommand ShowAllTicketsCommand => new Command(ShowAllTickets);

        #endregion

        #region Properties

        public ObservableCollection<Ticket> Tickets { get; set; } = new ObservableCollection<Ticket>();
        public ObservableCollection<Ticket> PagedTickets { get; set; } = new ObservableCollection<Ticket>();
        public int OriginalIndex { get; set; }

        private int currentPage;
        private int totalPages;

        public DateTime CurrentDateTime
        {
            get => _currentDateTime;
            set
            {
                if (_currentDateTime != value)
                {
                    _currentDateTime = value;
                    RefreshTicketStatuses();
                    OnPropertyChanged();
                }
            }
        }

        public int CurrentPage
        {
            get => currentPage;
            set
            {
                if (currentPage != value)
                {
                    currentPage = value;
                    OnPropertyChanged();
                    UpdatePagedTickets();
                }
            }
        }
        public int TotalPages
        {
            get => totalPages;
            private set
            {
                if (totalPages != value)
                {
                    totalPages = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Constructor

        public TicketsVM()
        {
            CurrentDateTime = DateTime.Now;
            Tickets = new ObservableCollection<Ticket>
 {
     new Ticket
     {
         TicketNumber = "123",
         TableNumber = 1,
         WaiterName = "John Doe",
         OrderDateTime = DateTime.Now.AddMinutes(-30),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "124",
         TableNumber = 2,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "125",
         TableNumber = 3,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(2),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "126",
         TableNumber = 4,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
         }
     },
     new Ticket
     {
         TicketNumber = "127",
         TableNumber = 5,
         WaiterName = "Emily Brown",
         OrderDateTime = DateTime.Now.AddMinutes(10),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "128",
         TableNumber = 6,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "129",
         TableNumber = 7,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(-20),
        Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "118",
         TableNumber = 8,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "138",
         TableNumber = 9,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },

         }
     },
     new Ticket
     {
         TicketNumber = "148",
         TableNumber = 10,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                 }
             },
         }
     },
     new Ticket
     {
         TicketNumber = "18",
         TableNumber = 11,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "131",
         TableNumber = 12,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "132",
         TableNumber = 13,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(-20),
          Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "133",
         TableNumber = 14,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "135",
         TableNumber = 15,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "136",
         TableNumber = 16,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "137",
         TableNumber = 17,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "138",
         TableNumber = 18,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "139",
         TableNumber = 19,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "140",
         TableNumber = 20,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "142",
         TableNumber = 21,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
              new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "Extra Cream" }
                 }
             },
             new Items { ItemName = "Tea", Quantity = 3 },

         }
     },
      new Ticket
     {
         TicketNumber = "128",
         TableNumber = 6,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "129",
         TableNumber = 7,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(-20),
        Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "118",
         TableNumber = 8,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "138",
         TableNumber = 9,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },

         }
     },
     new Ticket
     {
         TicketNumber = "148",
         TableNumber = 10,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                 }
             },
         }
     },
     new Ticket
     {
         TicketNumber = "18",
         TableNumber = 11,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "131",
         TableNumber = 12,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "132",
         TableNumber = 13,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(-20),
          Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "133",
         TableNumber = 14,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "135",
         TableNumber = 15,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "136",
         TableNumber = 16,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "137",
         TableNumber = 17,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "138",
         TableNumber = 18,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "139",
         TableNumber = 19,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "140",
         TableNumber = 20,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "142",
         TableNumber = 21,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
              new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "Extra Cream" }
                 }
             },
             new Items { ItemName = "Tea", Quantity = 3 },

         }
     },
      new Ticket
     {
         TicketNumber = "128",
         TableNumber = 6,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "129",
         TableNumber = 7,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(-20),
        Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "118",
         TableNumber = 8,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "138",
         TableNumber = 9,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },

         }
     },
     new Ticket
     {
         TicketNumber = "148",
         TableNumber = 10,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                 }
             },
         }
     },
     new Ticket
     {
         TicketNumber = "18",
         TableNumber = 11,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "131",
         TableNumber = 12,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "132",
         TableNumber = 13,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(-20),
          Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "133",
         TableNumber = 14,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "135",
         TableNumber = 15,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "136",
         TableNumber = 16,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "137",
         TableNumber = 17,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "138",
         TableNumber = 18,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "139",
         TableNumber = 19,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "140",
         TableNumber = 20,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "142",
         TableNumber = 21,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
              new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "Extra Cream" }
                 }
             },
             new Items { ItemName = "Tea", Quantity = 3 },

         }
     },
      new Ticket
     {
         TicketNumber = "128",
         TableNumber = 6,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "129",
         TableNumber = 7,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(-20),
        Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "118",
         TableNumber = 8,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "138",
         TableNumber = 9,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },

         }
     },
     new Ticket
     {
         TicketNumber = "148",
         TableNumber = 10,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                 }
             },
         }
     },
     new Ticket
     {
         TicketNumber = "18",
         TableNumber = 11,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "131",
         TableNumber = 12,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "132",
         TableNumber = 13,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(-20),
          Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "133",
         TableNumber = 14,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "135",
         TableNumber = 15,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "136",
         TableNumber = 16,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "137",
         TableNumber = 17,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "138",
         TableNumber = 18,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "139",
         TableNumber = 19,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "140",
         TableNumber = 20,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "142",
         TableNumber = 21,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
              new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "Extra Cream" }
                 }
             },
             new Items { ItemName = "Tea", Quantity = 3 },

         }
     },
      new Ticket
     {
         TicketNumber = "128",
         TableNumber = 6,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "129",
         TableNumber = 7,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(-20),
        Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "118",
         TableNumber = 8,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "138",
         TableNumber = 9,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },

         }
     },
     new Ticket
     {
         TicketNumber = "148",
         TableNumber = 10,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                 }
             },
         }
     },
     new Ticket
     {
         TicketNumber = "18",
         TableNumber = 11,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "131",
         TableNumber = 12,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "132",
         TableNumber = 13,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(-20),
          Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "133",
         TableNumber = 14,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "135",
         TableNumber = 15,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "136",
         TableNumber = 16,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "137",
         TableNumber = 17,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "138",
         TableNumber = 18,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "139",
         TableNumber = 19,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "140",
         TableNumber = 20,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "142",
         TableNumber = 21,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
              new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "Extra Cream" }
                 }
             },
             new Items { ItemName = "Tea", Quantity = 3 },

         }
     },
      new Ticket
     {
         TicketNumber = "128",
         TableNumber = 6,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "129",
         TableNumber = 7,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(-20),
        Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "118",
         TableNumber = 8,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "138",
         TableNumber = 9,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },

         }
     },
     new Ticket
     {
         TicketNumber = "148",
         TableNumber = 10,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                 }
             },
         }
     },
     new Ticket
     {
         TicketNumber = "18",
         TableNumber = 11,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "131",
         TableNumber = 12,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "132",
         TableNumber = 13,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(-20),
          Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "133",
         TableNumber = 14,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "135",
         TableNumber = 15,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "136",
         TableNumber = 16,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "137",
         TableNumber = 17,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "138",
         TableNumber = 18,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "139",
         TableNumber = 19,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "140",
         TableNumber = 20,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "142",
         TableNumber = 21,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
              new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "Extra Cream" }
                 }
             },
             new Items { ItemName = "Tea", Quantity = 3 },

         }
     },
      new Ticket
     {
         TicketNumber = "128",
         TableNumber = 6,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "129",
         TableNumber = 7,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(-20),
        Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "118",
         TableNumber = 8,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "138",
         TableNumber = 9,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },

         }
     },
     new Ticket
     {
         TicketNumber = "148",
         TableNumber = 10,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                 }
             },
         }
     },
     new Ticket
     {
         TicketNumber = "18",
         TableNumber = 11,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "131",
         TableNumber = 12,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "132",
         TableNumber = 13,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(-20),
          Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "133",
         TableNumber = 14,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "135",
         TableNumber = 15,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "136",
         TableNumber = 16,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "137",
         TableNumber = 17,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },
             new Items { ItemName = "Chk Fajita", Quantity = 3 },
             new Items { ItemName = "Cheese Puff", Quantity = 5 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
         }
     },
     new Ticket
     {
         TicketNumber = "138",
         TableNumber = 18,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
             new Items { ItemName = "RITA JUM HH", Quantity = 2 },
         }
     },
     new Ticket
     {
         TicketNumber = "139",
         TableNumber = 19,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "140",
         TableNumber = 20,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA SPC sm", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" }
                 }
             },

         }
     },
     new Ticket
     {
         TicketNumber = "142",
         TableNumber = 21,
         WaiterName = "Jane Smith",
         OrderDateTime = DateTime.Now.AddMinutes(20),
         Items = new List<Items>
         {
             new Items { ItemName = "Chk Enchilada", Quantity = 1 },
             new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "No Sour Cream" }
                 }
             },
             new Items { ItemName = "TEA", Quantity = 1 },
             new Items { ItemName = "RITA JUM HH", Quantity = 1 },
              new Items { ItemName = "CCQ BLANCO lg", Quantity = 1,
                 SpecialItems = new List<SpecialItems>
                 {
                     new SpecialItems { SpecialItemName = "Fajita Beef" },
                     new SpecialItems { SpecialItemName = "Extra Cream" }
                 }
             },
             new Items { ItemName = "Tea", Quantity = 3 },

         }
     },

 };

            foreach (var ticket in Tickets)
            {
                ticket.ItemNumber = ticket.Items.Count;
            }


            timer = new System.Timers.Timer(1000);
            timer.Elapsed += TimerElapsed;
            timer.AutoReset = true;
            timer.Start();
            CurrentPage = 1;
            CompleteTicketCommand = new Command<Ticket>(CompleteTicket);
            CompleteItemCommand = new Command<Items>(CompleteItem);
            UndoCommand = new Command(UndoLastAction);
            NextPageCommand = new Command(NextPage);
            PreviousPageCommand = new Command(PreviousPage);
        }

        #endregion

        #region Event Handlers

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Dispatch(() =>
            {
                CurrentDateTime = DateTime.Now;
            });
        }

        #endregion

        #region Methods

        private void RefreshTicketStatuses()
        {
            if (Tickets != null)
            {
                foreach (var ticket in Tickets)
                {
                    ticket.RefreshTimeExceededStatus(CurrentDateTime);
                }
            }
        }

        private void CompleteTicket(Ticket ticket)
        {
            if (ticket != null)
            {
                int index = PagedTickets.IndexOf(ticket);

                ticket.IsCompleted = true;

                undoStack.Push(new UndoAction
                {
                    Type = ActionType.CompleteTicket,
                    Ticket = ticket,
                    OriginalIndex = index 
                });

                PagedTickets.Remove(ticket);
            }
        }

        private void CompleteItem(Items item)
        {
            if (item != null)
            {
                undoStack.Push(new UndoAction { Type = ActionType.CompleteItem, Item = item });
                item.IsCompleted = true;
            }
        }

        private async void ShowAllTickets()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AllTicketsPage());
        }

        private void UndoLastAction()
        {
            if (undoStack.Count > 0)
            {
                var lastAction = undoStack.Pop();
                switch (lastAction.Type)
                {
                    case ActionType.CompleteTicket:
                        lastAction.Ticket.IsCompleted = false;
                        PagedTickets.Insert(lastAction.OriginalIndex, lastAction.Ticket);
                        break;

                    case ActionType.CompleteItem:
                        lastAction.Item.IsCompleted = false;
                        break;

                    default:
                        break;
                }
                OnPropertyChanged(nameof(PagedTickets));
            }
        }


        private const int TicketWidth = 168;

        private const double BottomButtonHeight = 50; 

        private void UpdatePagedTickets()
        {
            PagedTickets.Clear();

            int ticketsPerRow = (int)(screenWidth / TicketWidth);
            double currentRowHeight = 0;
            double accumulatedPageHeight = 0;
            int columnCount = 0;

            double availableHeight = screenHeight - BottomButtonHeight - 15; 

            int startIndex = (CurrentPage - 1) * ticketsPerRow;
            int endIndex = Math.Min(startIndex + ticketsPerRow * (int)(availableHeight / (70 + 30)), Tickets.Count); 

            for (int i = startIndex; i < endIndex; i++)
            {
                double ticketHeight = CalculateTicketHeight(Tickets[i]); 

                if (accumulatedPageHeight + ticketHeight > availableHeight)
                {
                    TotalPages = (int)Math.Ceiling((double)Tickets.Count / ticketsPerRow);
                    break; 
                }

                if (columnCount >= ticketsPerRow)
                {
                    columnCount = 0;
                    accumulatedPageHeight += currentRowHeight; 
                    currentRowHeight = 0; 

                    if (accumulatedPageHeight + ticketHeight > availableHeight)
                    {
                        TotalPages = (int)Math.Ceiling((double)Tickets.Count / ticketsPerRow);
                        break; 
                    }
                }

                PagedTickets.Add(Tickets[i]);
                columnCount++;

                currentRowHeight = Math.Max(currentRowHeight, ticketHeight);
            }

            if (TotalPages == 0)
            {
                TotalPages = (int)Math.Ceiling((double)Tickets.Count / ticketsPerRow);
            }
        }


        /*private void UpdatePagedTickets()
        {
            PagedTickets.Clear();

            // Calculate the number of tickets that fit per row based on fixed width
            int ticketsPerRow = (int)(screenWidth / TicketWidth);
            int ticketsPerPage = ticketsPerRow; // Start with one row and add dynamically

            double currentRowHeight = 0;
            double accumulatedPageHeight = 0;
            int rowCount = 0;
            int columnCount = 0;

            int startIndex = (CurrentPage - 1) * ticketsPerPage;
            int endIndex = Math.Min(startIndex + Tickets.Count, Tickets.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                double ticketHeight = CalculateTicketHeight(Tickets[i]); // Custom method to get ticket height

                // Check if adding this ticket exceeds the row width
                if (columnCount >= ticketsPerRow)
                {
                    // Reset for the next row
                    columnCount = 0;
                    accumulatedPageHeight += currentRowHeight; // Add row height to page height
                    currentRowHeight = 0;
                    rowCount++;

                    // Check if adding this row exceeds the screen height
                    if (accumulatedPageHeight + ticketHeight > screenHeight)
                    {
                        break; // Stop adding tickets as page height is exceeded
                    }
                }

                // Add the ticket to the paged collection
                PagedTickets.Add(Tickets[i]);
                columnCount++;

                // Track maximum height in the current row
                currentRowHeight = Math.Max(currentRowHeight, ticketHeight);
            }
            TotalPages = (int)Math.Ceiling((double)Tickets.Count / (ticketsPerRow * (screenHeight / (70 + 30))));

        }
*/
        private double CalculateTicketHeight(Ticket ticket)
        {
            return ((ticket.ItemNumber *10) + 45);
        }

        public void SetScreenWidth(double width, double height)
        {
            screenWidth = width;
            screenHeight = height;
            UpdatePagedTickets(); 
        }

        public void NextPage()
        {
            if (CurrentPage < totalPages)
                CurrentPage++;
        }

        public void PreviousPage()
        {
            if (CurrentPage > 1)
                CurrentPage--;
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Nested Types

        private enum ActionType
        {
            CompleteTicket,
            CompleteItem
        }

        private class UndoAction
        {
            public ActionType Type { get; set; }
            public Ticket Ticket { get; set; }
            public Items Item { get; set; }
            public int OriginalIndex { get; internal set; }
        }

        #endregion
       
    }
}
