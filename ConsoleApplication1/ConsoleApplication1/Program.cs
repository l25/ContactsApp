using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;


namespace ConsoleApplication1
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (LogOn())
            {
                //var list = new List<Contact>();
                for (; ; )
                {
                    DataManager dataManager = new DataManager();
                    dataManager.ShowGroups();
                    Console.WriteLine("Enter your choice:\n1 - create new contact\n2 - view contact\n3 - delete contact\n4 - edit contact\n5 - add group\n6 - edit group name\n7 - delete group\n8 - show contacts in group\n9 - add contact to group\n10 - delete contact from group\n11 - add user\n12 - delete user\n13 - edit user\n14 - show junction_table\n15 - show all contacts\n16 - show all users\n17 - show all groups\n18 - show all changes\n19 - show user's contacts\n20 - exit");
                    try
                    {
                        int option = Convert.ToInt32(Console.ReadLine());

                        if (option != 1 & option != 2 & option != 3 & option != 4 & option != 5 & option != 6 & option != 7 & option != 8 & option != 9 & option != 10 & option != 11 & option != 12 & option != 13 & option != 14 & option != 15 & option != 16 & option != 17 & option != 18 & option != 19) { break; }
                        switch (option)
                        {
                            case 1:
                                AddContact();
                                break;
                            case 2:
                                ShowContactByNumber();
                                break;
                            case 3:
                                DeleteContact();
                                break;
                            case 4:
                                EditContact();
                                break;
                            case 5:
                                AddGroup();
                                break;
                            case 6:
                                EditGroup();
                                break;
                            case 7:
                                DeleteGroup();
                                break;
                            case 8:
                                ShowContactsInGroup();
                                break;
                            case 9:
                                AddContactToGroup();
                                break;
                            case 10:
                                DeleteContactFromGroup();
                                break;
                            case 11:
                                AddUser();
                                break;
                            case 12:
                                DeleteUser();
                                break;
                            case 13:
                                EditUser();
                                break;
                            case 14:
                                dataManager.ShowJunctionTable();
                                break;
                            case 15:
                                ShowAllContacts();
                                break;
                            case 16:
                                ShowAllUsers();
                                break;
                            case 17:
                                ShowAllGroups();
                                break;
                            case 18:
                                ShowAllChanges();
                                break;
                            case 19:
                                ShowContacts();
                                break;

                        }
                    }
                    catch (FormatException) { Console.WriteLine("Enter correct symbols"); }
                    catch (Exception) { Console.WriteLine("Something very bad happend..:("); }
                    finally { }
                }
            }
            Console.ReadLine();
        }

        public static void AddContact()
        {
            var contact = new Contact();
            contact.Set();
            DataManager dataManager = new DataManager();
            dataManager.AddContact(contact);
        }

        public static void EditContact()
        {
            DataManager dataManager = new DataManager();
            Contact contact = new Contact();
            Console.WriteLine("Enter number of contact to edit:");
            int d = Convert.ToInt32(Console.ReadLine());
            contact.Id = d;
            contact.Reset();
            if (dataManager.EditContact(contact)) { Console.WriteLine("Contact with number " + d + " does not exist in your account."); }
        }

        public static void DeleteContact()
        {
            bool f = true;
            DataManager dataManager = new DataManager();
            Console.WriteLine("Enter number of contact to delete:");
            int numOfContactToDelete = Convert.ToInt32(Console.ReadLine());
            f = dataManager.DeleteContact(numOfContactToDelete);
            if (f) { Console.WriteLine("Contact with number " + numOfContactToDelete + " does not exist in your account."); }
            else
            {
                Console.WriteLine("Contact with number " + numOfContactToDelete + " is deleted");
            }
        }

        public static void ShowContactByNumber()
        {
            DataManager dataManager = new DataManager();
            Console.WriteLine("Enter number of contact to show:");
            int numberOfContact = Convert.ToInt32(Console.ReadLine());
            bool f = dataManager.ShowContactByNumber(numberOfContact);
            if (f) { Console.WriteLine("Contact with number " + numberOfContact + " does not exist in your account."); }
        }

        public static bool LogOn()
        {
            DataManager dataManager = new DataManager();
            User user = new User();
            Console.WriteLine("Enter your name:");
            user.Name = Console.ReadLine();
            Console.WriteLine("Enter password:");
            user.Password = Console.ReadLine();
            return dataManager.LogOn(user);
        }

        public static void AddGroup()
        {
            Group group = new Group();
            Console.WriteLine("Enter name of new group:");
            group.GroupName = Console.ReadLine();
            DataManager dataManager = new DataManager();
            dataManager.AddGroup(group);
        }

        public static void EditGroup()
        {
            DataManager dataManager = new DataManager();
            Group oldGroup = new Group();
            Group newGroup = new Group();
            Console.WriteLine("Enter old name of group:");
            oldGroup.GroupName = Console.ReadLine();
            Console.WriteLine("Enter new name of group:");
            newGroup.GroupName = Console.ReadLine();
            if (dataManager.EditGroup(oldGroup, newGroup)) { Console.WriteLine("Group with name " + oldGroup.GroupName + " does not exist."); }
        }

        public static void DeleteGroup()
        {
            DataManager dataManager = new DataManager();
            Group group = new Group();
            Console.WriteLine("Enter name of deleting group:");
            group.GroupName = Console.ReadLine();
            Console.WriteLine("Delete group with all contacts it contains?");
            bool withContacts = false;
            if (Convert.ToInt32(Console.ReadLine()) == 1)
            {
                withContacts = true;
            }
            if (dataManager.DeleteGroup(group, withContacts)) { Console.WriteLine("Group with name " + group.GroupName + " does not exist."); }
        }

        public static void ShowAllContacts()
        {
            DataManager dataManager = new DataManager();
            if (dataManager.ShowAllContacts())
            {
                Console.WriteLine("There's no contacts");
            }
        }

        public static void ShowAllGroups()
        {
            DataManager dataManager = new DataManager();
            if (dataManager.ShowAllGroups())
            {
                Console.WriteLine("There's no groups");
            }
        }

        public static void ShowAllChanges()
        {
            DataManager dataManager = new DataManager();
            if (dataManager.ShowAllChanges())
            {
                Console.WriteLine("There's no changes");
            }
        }

        public static void ShowAllUsers()
        {
            DataManager dataManager = new DataManager();
            if (dataManager.ShowAllUsers())
            {
                Console.WriteLine("There's no users");
            }
        }

        public static void ShowContacts()
        {
            DataManager dataManager = new DataManager();
            if (dataManager.ShowContacts())
            {
                Console.WriteLine("There's no contacts");
            }
        }

        public static void ShowContactsInGroup()
        {
            DataManager dataManager = new DataManager();
            Group group = new Group();
            Console.WriteLine("Enter name of group:");
            group.GroupName = Console.ReadLine();
            if (dataManager.ShowContactsInGroup(group)) { Console.WriteLine("Group with name '" + group.GroupName + "' does not exist or it's empty"); }
        }

        public static void AddContactToGroup()
        {
            DataManager dataManager = new DataManager();
            Console.WriteLine("Enter group name:");
            var groupName = Console.ReadLine();
            Console.WriteLine("Enter number of contact:");
            int contactNumber = Convert.ToInt32(Console.ReadLine().ToString());
            if (dataManager.AddContactToGroup(contactNumber, groupName))
            {
                Console.WriteLine("Group or contact doesn't exist or contact already exists in group");
            }
        }

        public static void DeleteContactFromGroup()
        {
            DataManager dataManager = new DataManager();
            Console.WriteLine("Enter group name:");
            var groupName = Console.ReadLine();
            Console.WriteLine("Enter number of contact:");
            int contactNumber = Convert.ToInt32(Console.ReadLine().ToString());
            if (dataManager.DeleteContactFromGroup(contactNumber, groupName))
            {
                Console.WriteLine("Group or contact doesn't exist or contact doesn't exist in group");
            }
        }

        public static void AddUser()
        {
            DataManager dataManager = new DataManager();
            User user = new User();
            Console.WriteLine("Enter username:");
            user.Name = Console.ReadLine();
            Console.WriteLine("Enter password:");
            user.Password = Console.ReadLine();
            if (dataManager.AddUser(user))
            {
                Console.WriteLine("User was not added");
            }
        }

        public static void EditUser()
        {
            DataManager dataManager = new DataManager();
            User oldUser = new User();
            User newUser = new User();
            Console.WriteLine("Enter username:");
            oldUser.Name = Console.ReadLine();
            Console.WriteLine("Enter password:");
            oldUser.Password = Console.ReadLine();
            Console.WriteLine("Enter new username:");
            newUser.Name = Console.ReadLine();
            Console.WriteLine("Enter  new password:");
            newUser.Password = Console.ReadLine();
            if (dataManager.EditUser(oldUser, newUser))
            {
                Console.WriteLine("User was not edited");
            }
        }

        public static void DeleteUser()
        {
            DataManager dataManager = new DataManager();
            User user = new User();
            Console.WriteLine("Enter username:");
            user.Name = Console.ReadLine();
            Console.WriteLine("Enter password:");
            user.Password = Console.ReadLine();
            if (dataManager.DeleteUser(user))
            {
                Console.WriteLine("User was not deleted");
            }
        }

        //public static void DeleteContactFromGroup()
        //{
        //    DataManager dataManager = new DataManager();
        //    Console.WriteLine("Enter group name:");
        //    var groupName = Console.ReadLine();
        //    Console.WriteLine("Enter number of contact:");
        //    int contactNumber = Convert.ToInt32(Console.ReadLine().ToString());
        //    dataManager.AddContactToGroup(contactNumber, groupName);
        //    if (dataManager.DeleteContactFromGroup(contactNumber, groupName))
        //    {
        //        Console.WriteLine("Group or contact doesn't exist");
        //    }
        //}

        //public static void ExampleOfReadingData()
        //{
        //    Console.WriteLine("Example function:");
        //    string connectionString;
        //    connectionString = Properties.Settings.Default.Database1ConnectionString;
        //    SqlConnection connection = new SqlConnection(connectionString);
        //    connection.Open();
        //    DataSet ds = new DataSet();
        //    SqlDataAdapter daUsers = new SqlDataAdapter("select * from users", connection);
        //    SqlDataAdapter daGroups = new SqlDataAdapter("select * from groups", connection);
        //    SqlDataAdapter daContacts = new SqlDataAdapter("select * from contacts", connection);
        //    SqlDataAdapter daJunctionTable = new SqlDataAdapter("select * from junction_table", connection);
        //    SqlDataAdapter daChanges = new SqlDataAdapter("select * from changes", connection);
        //    SqlCommandBuilder bldContacts = new SqlCommandBuilder(daContacts);
        //    SqlCommandBuilder bldUsers = new SqlCommandBuilder(daUsers);
        //    SqlCommandBuilder bldChanges = new SqlCommandBuilder(daChanges);
        //    SqlCommandBuilder bldJunctionTable = new SqlCommandBuilder(daJunctionTable);
        //    SqlCommandBuilder bldGroups = new SqlCommandBuilder(daGroups);
        //    try
        //    {
        //        daUsers.Fill(ds, "users");
        //        daContacts.Fill(ds, "contacts");
        //        daGroups.Fill(ds, "groups");
        //        daJunctionTable.Fill(ds, "junction_table");
        //        daChanges.Fill(ds, "changes");
        //    }
        //    catch
        //    {
        //        Console.WriteLine("Error reading data");
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //    DataRelation relat = new DataRelation("num_u_groups", ds.Tables["users"].Columns["num_u"], ds.Tables["groups"].Columns["num_u"]);
        //    ds.Relations.Add(relat);
        //    DataRelation relat2 = new DataRelation("num_g_junction_table", ds.Tables["groups"].Columns["num_g"], ds.Tables["junction_table"].Columns["num_g"]);
        //    ds.Relations.Add(relat2);
        //    DataRelation relat3 = new DataRelation("num_c_junction_table", ds.Tables["contacts"].Columns["num_c"], ds.Tables["junction_table"].Columns["num_c"]);
        //    ds.Relations.Add(relat3);
        //    DataRelation relat4 = new DataRelation("num_c_changes", ds.Tables["contacts"].Columns["num_c"], ds.Tables["changes"].Columns["num_c"]);
        //    ds.Relations.Add(relat4);
        //    DataColumn[] colArr = new DataColumn[1];
        //    colArr[0] = ds.Tables["users"].Columns[0];
        //    ds.Tables["users"].PrimaryKey = colArr;
        //    DataColumn[] colArr1 = new DataColumn[1];
        //    colArr1[0] = ds.Tables["contacts"].Columns[0];
        //    ds.Tables["contacts"].PrimaryKey = colArr1;
        //    DataColumn[] colArr2 = new DataColumn[1];
        //    colArr2[0] = ds.Tables["groups"].Columns[0];
        //    ds.Tables["groups"].PrimaryKey = colArr2;
        //    DataColumn[] colArr3 = new DataColumn[1];
        //    colArr3[0] = ds.Tables["changes"].Columns[0];
        //    ds.Tables["changes"].PrimaryKey = colArr3;
        //    DataColumn[] colArr4 = new DataColumn[1];
        //    colArr4[0] = ds.Tables["junction_table"].Columns[0];
        //    ds.Tables["junction_table"].PrimaryKey = colArr4;
        //    foreach (DataRow dr in ds.Tables["users"].Rows)
        //    {
        //        Console.WriteLine(dr[0].ToString() + "  " + dr[1].ToString() + " " + dr[2].ToString());
        //    }
        //    foreach (DataRow dr in ds.Tables["contacts"].Rows)
        //    {
        //        Console.WriteLine(dr[0].ToString() + "  " + dr[1].ToString() + "  " + dr[2].ToString() + "  " + dr[3].ToString());
        //    }
        //    foreach (DataRow dr in ds.Tables["groups"].Rows)
        //    {
        //        Console.WriteLine(dr[0].ToString() + "  " + dr[1].ToString() + "  " + dr[2].ToString());
        //    }
        //    foreach (DataRow dr in ds.Tables["junction_table"].Rows)
        //    {
        //        Console.WriteLine(dr[0].ToString() + "  " + dr[1].ToString() + "  " + dr[2].ToString());
        //    }
        //    foreach (DataRow dr in ds.Tables["changes"].Rows)
        //    {
        //        Console.WriteLine(dr[0].ToString() + "  " + dr[1].ToString() + "  " + dr[2].ToString());
        //    }
        //    foreach (DataRow dr in ds.Tables["groups"].Rows)
        //    {
        //        if (Convert.ToInt16(dr[0].ToString()) == 1)
        //        {
        //            Console.WriteLine("First group is " + (dr[1].ToString()));
        //        }
        //    }
        //    DataRow drrr = ds.Tables["contacts"].Rows[0];
        //    Console.WriteLine("First contact is " + (drrr[1].ToString()));
        //    Program.AddContact(ds, daContacts, connection);
        //    Program.DeleteContact(ds, daContacts, connection);
        //    Program.ExampleOfReadingData();
        //    connection.Close();
        //}



        //public static void AddContact(DataSet ds, SqlDataAdapter daContacts, SqlConnection conn)
        //{
        //    Console.WriteLine("from AddContact");
        //    DataTable dataTable = ds.Tables["contacts"];
        //    DataRow newRow = dataTable.NewRow();
        //    newRow.BeginEdit();
        //    int index = 0;
        //    foreach (DataRow dr in ds.Tables["contacts"].Rows)
        //    {
        //        if (index < Convert.ToInt32(dr[0].ToString()))
        //        { index = Convert.ToInt32(dr[0].ToString()); }
        //    }
        //    newRow[0] = index + 1;
        //    Console.WriteLine("Enter name:");
        //    newRow[1] = Console.ReadLine();
        //    Console.WriteLine("Enter surname:");
        //    newRow[2] = Console.ReadLine();
        //    Console.WriteLine("Enter telephone:");
        //    newRow[3] = Console.ReadLine();
        //    Console.WriteLine("Enter address:");
        //    newRow[4] = Console.ReadLine();
        //    Console.WriteLine("Enter country:");
        //    newRow[5] = Console.ReadLine();
        //    Console.WriteLine("Enter email:");
        //    newRow[6] = Console.ReadLine();
        //    newRow.EndEdit();
        //    dataTable.Rows.Add(newRow);
        //    daContacts.Update(ds.Tables["contacts"].GetChanges(DataRowState.Added));
        //    foreach (DataRow dr in ds.Tables["contacts"].Rows)
        //    {
        //        Console.WriteLine(dr[0].ToString() + "  " + dr[1].ToString() + " " + dr[2].ToString() + " " + dr[3].ToString() + " " + dr[4].ToString() + " " + dr[5].ToString() + " " + dr[6].ToString());
        //    }
        //}

        //public static void DeleteContact(DataSet ds, SqlDataAdapter daContacts, SqlConnection conn)
        //{
        //    bool flag=true;
        //    Console.WriteLine("from DeleteContact");
        //    DataTable dataTable = ds.Tables["contacts"];
        //    Console.WriteLine("Enter number of contact to delete:");
        //    int index = Convert.ToInt32(Console.ReadLine().ToString());
        //    foreach (DataRow dr in ds.Tables["contacts"].Rows)
        //    {
        //        if (index == Convert.ToInt16(dr[0].ToString()))
        //        { 
        //            dr.Delete();
        //            flag = false;
        //            daContacts.Update(ds.Tables["contacts"].GetChanges(DataRowState.Deleted));
        //        }
        //    }
        //    if (flag) { Console.WriteLine("Contact with number " + index + " does not exist."); }
        //    //foreach (DataRow dr in ds.Tables["contacts"].Rows)
        //    //{
        //    //    Console.WriteLine(dr[0].ToString() + "  " + dr[1].ToString() + " " + dr[2].ToString() + " " + dr[3].ToString() + " " + dr[4].ToString() + " " + dr[5].ToString() + " " + dr[6].ToString());
        //    //}
        //}
    }
}

