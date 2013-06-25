using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    public class Group
    {
        public int Id { get; set; }
        public String GroupName { get; set; }
        public int UserId { get; set; }

        public Group()
        {
        }

        public Group(int numOfUser)
        {
            UserId = numOfUser;
        }

        public void Set()
        {
            Console.WriteLine("Enter group name:");
            this.GroupName = Console.ReadLine();
        }

        public void Show()
        {
            Console.WriteLine("Group number " + Id + " " + GroupName + ", user with number " + UserId);
        }
    }
}
