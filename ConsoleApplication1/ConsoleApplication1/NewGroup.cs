using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    public class NewGroup
    {
        public int Id { get; set; }
        public String GroupName { get; set; }
        public int UserId { get; set; }

        public NewGroup()
        {
        }

        public NewGroup(int userId)
        {
            UserId = userId;
        }

        public void Set()
        {
            Console.WriteLine("Enter group name:");
            GroupName = Console.ReadLine();
        }

        public void Show()
        {
            Console.WriteLine("Group number " + Id + " " + GroupName + ", user with number " + UserId);
        }
    }
}
