using PublicAPI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicAPI.ViewModels
{
    public class AllTicketsVM
    {
        public ObservableCollection<Ticket> Tickets { get; set; }

        public AllTicketsVM()
        {
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
                    TicketNumber = "125",
                    TableNumber = 3,
                    WaiterName = "Jane Smith",
                    OrderDateTime = DateTime.Now.AddMinutes(-20),
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
                    OrderDateTime = DateTime.Now.AddMinutes(-20),
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
                    OrderDateTime = DateTime.Now.AddMinutes(-10),
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
                    OrderDateTime = DateTime.Now.AddMinutes(-20),
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
                    TicketNumber = "138",
                    TableNumber = 9,
                    WaiterName = "Jane Smith",
                    OrderDateTime = DateTime.Now.AddMinutes(-20),
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
                    OrderDateTime = DateTime.Now.AddMinutes(-20),
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
                    TicketNumber = "131",
                    TableNumber = 12,
                    WaiterName = "Jane Smith",
                    OrderDateTime = DateTime.Now.AddMinutes(-20),
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
                    TicketNumber = "135",
                    TableNumber = 15,
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
                    }
                },
                new Ticket
                {
                    TicketNumber = "136",
                    TableNumber = 16,
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

                    }
                },
                new Ticket
                {
                    TicketNumber = "137",
                    TableNumber = 17,
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
                    TicketNumber = "138",
                    TableNumber = 18,
                    WaiterName = "Jane Smith",
                    OrderDateTime = DateTime.Now.AddMinutes(-20),
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

                    }
                },
                new Ticket
                {
                    TicketNumber = "140",
                    TableNumber = 20,
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

                    }
                },
                new Ticket
                {
                    TicketNumber = "142",
                    TableNumber = 21,
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
        }
    }
}
