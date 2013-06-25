using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    public class Contact
    {
        private static int _staticId = 0;
        public int Id { get; set; }
        public int UserID { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public String TelNum { get; set; }
        public String Address { get; set; }
        public String Country { get; set; }
        public String Email { get; set; }
      
        public void IncNum()
        {
            _staticId++;
        }

        public void Set()
        {
            this.Id = Contact._staticId;
            Contact._staticId++;
            Console.WriteLine("Enter Name:");
            this.Name = Console.ReadLine();
            Console.WriteLine("Enter Surname:");
            this.Surname = Console.ReadLine();
            Console.WriteLine("Enter Telephone Number:");
            this.TelNum = Console.ReadLine();
            Console.WriteLine("Enter Address:");
            this.Address = Console.ReadLine();
            Console.WriteLine("Enter Country:");
            this.Country = Console.ReadLine();
            Console.WriteLine("Enter Email:");
            this.Email = Console.ReadLine();
        }

        public void Show()
        {
            Console.WriteLine("Number " + this.Id);
            Console.WriteLine("Name " + this.Name);
            Console.WriteLine("Surname " + this.Surname);
            Console.WriteLine("Telephone Number " + this.TelNum);
            Console.WriteLine("Address " + this.Address);
            Console.WriteLine("Country " + this.Country);
            Console.WriteLine("Email " + this.Email);
        }

        public void Reset()
        {
            Console.WriteLine("Enter Name:");
            this.Name = Console.ReadLine();
            Console.WriteLine("Enter Surname:");
            this.Surname = Console.ReadLine();
            Console.WriteLine("Enter Telephone Number:");
            this.TelNum = Console.ReadLine();
            Console.WriteLine("Enter Address:");
            this.Address = Console.ReadLine();
            Console.WriteLine("Enter Country:");
            this.Country = Console.ReadLine();
            Console.WriteLine("Enter Email:");
            this.Email = Console.ReadLine();
        }
    }
}
