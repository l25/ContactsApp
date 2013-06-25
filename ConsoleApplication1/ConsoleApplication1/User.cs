using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    public class User
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Password { get; set; }

        public void Set()
        {
            Console.WriteLine("Enter name:");
            Name = Console.ReadLine();
            Console.WriteLine("Enter password:");
            Password = Console.ReadLine();
        }
    }
}
