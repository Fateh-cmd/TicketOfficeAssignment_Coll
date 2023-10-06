using System;
using System.Collections.Generic;
using System.Linq;

namespace TicketOffice3
{
    public enum Place
    {
        Seated,
        Standing
    }

    public class Ticket
    {
        public int Age { get; private set; }
        public Place Place { get; private set; }
        public int Number { get; private set; }

        public Ticket(int age, Place place)
        {
            Age = age;
            Place = place;
            Number = TicketSalesManager.NextTicketNumber();
        }

        public int Price()
        {
            int seatedPrice = 0;
            int standingPrice = 0;

            if (Age < 11)
            {
                seatedPrice = 50;
                standingPrice = 25;
            }
            else if (Age >= 12 && Age <= 64)
            {
                seatedPrice = 170;
                standingPrice = 110;
            }
            else if (Age > 65)
            {
                seatedPrice = 100;
                standingPrice = 60;
            }

            return Place == Place.Seated ? seatedPrice : standingPrice;
        }

        public decimal Tax()
        {
            int price = Price();
            decimal tax = (1 - 1 / 1.06m) * price;
            return Math.Round(tax, 2);
        }
    }

    public class TicketSalesManager
    {
        private List<Ticket> tickets;

        public TicketSalesManager()
        {
            tickets = new List<Ticket>();
        }

        public static int NextTicketNumber()
        {
            Random random = new Random();
            return random.Next(1, 8000);
        }

        public void AddTicket(Ticket ticket)
        {
            tickets.Add(ticket);
        }

        public bool RemoveTicket(Ticket ticket)
        {
            return tickets.Remove(ticket);
        }

        public decimal SalesTotal()
        {
            decimal total = tickets.Sum(ticket => ticket.Price());
            return total;
        }

        public int AmountOfTickets()
        {
            return tickets.Count;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Ticket Office App!");

            TicketSalesManager ticketSalesManager = new TicketSalesManager();

            int age = GetCustomerAge();
            Place place = GetCustomerPlacePreference();

            Ticket ticket = new Ticket(age, place);

            ticketSalesManager.AddTicket(ticket);

            Console.WriteLine($"Ticket Price: {ticket.Price()} SEK");
            Console.WriteLine($"Tax: {ticket.Tax()} SEK");
            Console.WriteLine($"Ticket Number: {ticket.Number}");
            Console.WriteLine($"Total Sales: {ticketSalesManager.SalesTotal()} SEK");
            Console.WriteLine($"Number of Tickets Sold: {ticketSalesManager.AmountOfTickets()}");
        }

        static int GetCustomerAge()
        {
            while (true)
            {
                Console.Write("Please enter your age: ");
                string input = Console.ReadLine();
                if (input.Length >= 1 && input.Length <= 3 && IsNumeric(input))
                {
                    return int.Parse(input);
                }
                else
                {
                    Console.WriteLine("Invalid age format or range. Please enter a valid age between 1 and 3 characters.");
                }
            }
        }

        static bool IsNumeric(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        static Place GetCustomerPlacePreference()
        {
            while (true)
            {
                Console.Write("Do you want a standing or seated ticket: ");
                string input = Console.ReadLine().Trim().ToLower();

                if (input == "s" || input == "seated")
                {
                    return Place.Seated;
                }
                else if (input == "standing")
                {
                    return Place.Standing;
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }
        }
    }
}
