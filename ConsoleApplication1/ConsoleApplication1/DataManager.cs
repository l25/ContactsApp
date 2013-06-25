using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;


namespace ConsoleApplication1
{
    class DataManager
    {
        private string _connectionString;
        private static int _numberOfUser;

        public DataManager()
        {
            _connectionString = Properties.Settings.Default.Database1ConnectionString;
        }

        public void AddContact(Contact contact)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "(select max(num_c)+1 from contacts)";
            try
            {
                connection.Open();
                var maxIdC = cmd.ExecuteScalar();
                int index;
                if (maxIdC == DBNull.Value)
                {
                    index = 0;
                }
                else
                {
                    index = Convert.ToInt32(maxIdC);
                }
                //connection.Close();
                String sql = "insert into contacts  values (" + index + ", '" + contact.Name + "', '" + contact.Surname + "', '" 
                    + contact.TelNum + "', '" + contact.Address + "', '" + contact.Country + "', '" + contact.Email + "', " + _numberOfUser + ")";
                SqlCommand cmd2 = new SqlCommand(sql, connection);
                //connection.Open();
                int numAdd = cmd2.ExecuteNonQuery();
                Console.WriteLine(numAdd + " contact(s) is added");
                connection.Close();
            }
            catch (SqlException exc)
            {
                Console.WriteLine("Error: " + exc.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public bool EditContact(Contact contact)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            bool flag = false;
            String sql = "select * from contacts where num_c=" + contact.Id.ToString() + " and num_u=" + _numberOfUser;
            SqlCommand cmd2 = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader reader = cmd2.ExecuteReader();
            if (reader.Read())
            {
                Contact contact2 = new Contact();
                contact2.Id = Convert.ToInt32(reader[0].ToString());
                contact2.Name = reader[1].ToString();
                contact2.Surname = reader[2].ToString();
                contact2.TelNum = reader[3].ToString();
                contact2.Address = reader[4].ToString();
                contact2.Country = reader[5].ToString();
                contact2.Email = reader[6].ToString();
                if (reader[7] == DBNull.Value)
                {
                    contact2.UserID = 0;
                }
                else
                {
                    contact2.UserID = Convert.ToInt32(reader[7].ToString());
                }
                SaveChanges(contact2);
            }
            reader.Close();
            sql = "update contacts set name='" + contact.Name + "', surname='" + contact.Surname + "', tel='" + contact.TelNum
                + "', addr='" + contact.Address + "', country='" + contact.Country + "', email='" + contact.Email
                + "' where num_c=" + contact.Id.ToString() + " and num_u=" + _numberOfUser;
            SqlCommand cmd = new SqlCommand(sql, connection);
            try
            {
                //connection.Open();
                int numUpd = cmd.ExecuteNonQuery();
                if (numUpd == 0) { flag = true; }
                connection.Close();
                Console.WriteLine(numUpd + " contact(s) is edited");
            }
            catch (SqlException exc)
            {
                Console.WriteLine("Error: " + exc.Message);
                flag = true;
            }
            finally
            {
                connection.Close();
            }
            return flag;
        }

        public bool DeleteContact(int numOfContact)
        {
            bool flag = false;
            SqlConnection connection = new SqlConnection(_connectionString);
            String sql = "select * from contacts where num_c=" + numOfContact + " and num_u=" + _numberOfUser;
            SqlCommand cmd2 = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader reader = cmd2.ExecuteReader();
            if (reader.Read())
            {
                Contact contact2 = new Contact();
                contact2.Id = Convert.ToInt32(reader[0].ToString());
                contact2.Name = reader[1].ToString();
                contact2.Surname = reader[2].ToString();
                contact2.TelNum = reader[3].ToString();
                contact2.Address = reader[4].ToString();
                contact2.Country = reader[5].ToString();
                contact2.Email = reader[6].ToString();
                if (reader[7] == DBNull.Value)
                {
                    contact2.UserID = 0;
                }
                else
                {
                    contact2.UserID = Convert.ToInt32(reader[7].ToString());
                }
                SaveChanges(contact2);
            }
            reader.Close();
            String sql2 = "delete from junction_table where num_c=" + numOfContact.ToString()
                + "; delete from contacts where num_c=" + numOfContact.ToString() + " and num_u=" + _numberOfUser;
            SqlCommand cmd = new SqlCommand(sql2, connection);
            try
            {
                //connection.Open();
                int del = cmd.ExecuteNonQuery();
                if (del == 0)
                {
                    flag = true;
                }
            }
            catch (SqlException exc)
            {
                Console.WriteLine("Error: " + exc.Message);
                flag = true;
            }
            finally
            {
                connection.Close();
            }
            return flag;
        }

        public void SaveChanges(Contact contact)
        {
            var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "(select max(num_changes)+1 from changes)";
            try
            {
                connection.Open();
                int index;
                var maxIdC = cmd.ExecuteScalar();
                if (maxIdC == DBNull.Value)
                {
                    index = 0;
                }
                else
                {
                    index = Convert.ToInt32(maxIdC);
                }
                var newSql = new StringBuilder("insert into changes ");
                newSql.Append("(num_changes, num_c, name, surname, tel,addr, country, email, date, num_u) ");
                newSql.AppendFormat("values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',GETDATE(),'{8}')",
                                        index.ToString(),
                                        contact.Id.ToString(),
                                        contact.Name,
                                        contact.Surname,
                                        contact.TelNum,
                                        contact.Address,
                                        contact.Country,
                                        contact.Email,
                                        contact.UserID);

                SqlCommand cmd2 = new SqlCommand(newSql.ToString(), connection);
                int numAdd = cmd2.ExecuteNonQuery();
                Console.WriteLine(numAdd + " change is saved");
                connection.Close();
            }
            catch (SqlException exc)
            {
                Console.WriteLine("Error: " + exc.Message);
            }
            finally
            {
                connection.Close();
            }
            ShowAllChanges();
        }

        public bool LogOn(User user)
        {
            bool presenceOfUserName = true;
            bool presenceOfUser = false;
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "(select * from users)";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var nameInBase = reader[1];
                    if (nameInBase.ToString() == user.Name)
                    {
                        presenceOfUserName = false;
                        var passwordInBase = reader[2];
                        var userNum = reader[0];
                        if (user.Password == passwordInBase.ToString())
                        {
                            presenceOfUser = true;
                            _numberOfUser = Convert.ToInt32(userNum.ToString());
                            Console.WriteLine("Hello, " + nameInBase);
                        }
                        else
                        {
                            Console.WriteLine("Wrong password. Logon denied.");
                        }
                    }
                }
                if (presenceOfUserName)
                {
                    Console.WriteLine("No such name in database. Logon denied.");
                }
            }
            catch
            {
                Console.WriteLine("Error reading the database... ");
            }
            finally
            {
                connection.Close();
            }
            return presenceOfUser;
        }

        public bool ShowContactByNumber(int numberOfContact)
        {
            bool flag = false;
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "select * from contacts where num_c=" + numberOfContact.ToString() + " and num_u=" + _numberOfUser;
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Console.WriteLine(reader[0] + "  " + reader[1] + " " + reader[2] + "  " + reader[3] + "  " + reader[4] + "  " 
                        + reader[5] + "  " + reader[6]);
                }
                else { flag = true; }
                connection.Close();
            }
            catch (SqlException exc)
            {
                Console.WriteLine(exc.Message);
                flag = true;
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine(exc.Message);
                flag = true;
            }
            finally
            {
                connection.Close();
            }
            return flag;
        }

        public void AddGroup(Group group)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "(select max(num_g)+1 from groups)";
            try
            {
                connection.Open();
                var maxIdC = cmd.ExecuteScalar();
                int index;
                if (maxIdC == DBNull.Value)
                {
                    index = 0;
                }
                else
                {
                    index = Convert.ToInt32(maxIdC);
                }
                //connection.Close();
                String sql = "insert into groups values (" + index + ", '" + group.GroupName + "', " + _numberOfUser + ")";
                SqlCommand cmd2 = new SqlCommand(sql, connection);
                //connection.Open();
                int numAdd = cmd2.ExecuteNonQuery();
                Console.WriteLine(numAdd + " group(s) is added");
                connection.Close();
            }
            catch (SqlException exc)
            {
                Console.WriteLine("Error: " + exc.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public bool EditGroup(Group oldGroup, Group newGroup)
        {
            bool flag = false;
            SqlConnection connection = new SqlConnection(_connectionString);
            String sql = "update groups set group_name='" + newGroup.GroupName + "' where group_name='" + oldGroup.GroupName 
                + "' and num_u='" + _numberOfUser + "'";
            SqlCommand cmd = new SqlCommand(sql, connection);
            try
            {
                connection.Open();
                int numUpd = cmd.ExecuteNonQuery();
                if (numUpd == 0) { flag = true; }
                connection.Close();
                Console.WriteLine(numUpd + " contact(s) is edited");
            }
            catch (SqlException exc)
            {
                Console.WriteLine("Error: " + exc.Message);
                flag = true;
            }
            finally
            {
                connection.Close();
            }
            return flag;
        }

        public bool DeleteGroup(Group group, bool withContacts)
        {
            bool flag = false;
            SqlConnection connection = new SqlConnection(_connectionString);
            String sql = "delete from groups where group_name='" + group.GroupName + "' and num_u='" + _numberOfUser + "'";
            SqlCommand cmd = new SqlCommand(sql, connection);
            try
            {
                connection.Open();
                int del = cmd.ExecuteNonQuery();
                if (del == 0) { flag = true; }
                connection.Close();
                //if (withContacts)
                //{

                //}
                Console.WriteLine(del + " contact(s) is deleted");
            }
            catch (SqlException exc)
            {
                Console.WriteLine("Error: " + exc.Message);
                flag = true;
            }
            finally
            {
                connection.Close();
            }
            return flag;
        }

        public void ShowGroups()
        {
            Console.WriteLine("---------Groups-----------");
            var connectionString3 = Properties.Settings.Default.Database1ConnectionString;
            SqlConnection connection3 = new SqlConnection(connectionString3);
            connection3.Open();
            SqlCommand cmd3 = new SqlCommand();
            cmd3.Connection = connection3;
            cmd3.CommandType = System.Data.CommandType.Text;
            cmd3.CommandText = "SELECT groups.group_name, Count(junction_table.num_c) AS [Count-num_c] FROM groups left JOIN junction_table ON groups.num_g = junction_table.num_g GROUP BY groups.group_name, groups.num_u HAVING (groups.num_u)=" + _numberOfUser.ToString();
            SqlDataReader readerOfNumberOfContacts = cmd3.ExecuteReader();
            while (readerOfNumberOfContacts.Read())
            {
                Console.WriteLine(readerOfNumberOfContacts[0] + "  (" + readerOfNumberOfContacts[1] + ")");
            }
            connection3.Close();
        }

        public bool ShowAllChanges()
        {
            bool flag = true;
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "select * from changes";
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader[0].ToString() + "  " + reader[1].ToString() + " " + reader[2] + "  " + reader[3] + "  " 
                        + reader[4] + "  " + reader[5] + "  " + reader[6] + " " + reader[7] + " " + reader[8]);
                    flag = false;
                }
                connection.Close();
            }
            catch (SqlException exc)
            {
                Console.WriteLine("Error: " + exc.Message);
                return true;
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine("Error: " + exc.Message);
            }
            finally
            {
                connection.Close();
            }
            return flag;
        }

        public bool ShowAllUsers()
        {
            bool flag = true;
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "select * from users";
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader[0].ToString() + "  " + reader[1] + " " + reader[2]);
                    flag = false;
                }
                connection.Close();
            }
            catch (SqlException exc)
            {
                Console.WriteLine("Error: " + exc.Message);
                return true;
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine("Error: " + exc.Message);
            }
            finally
            {
                connection.Close();
            }
            return flag;
        }

        public bool ShowAllGroups()
        {
            bool flag = true;
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "select * from groups";
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader[0].ToString() + "  " + reader[1] + " " + reader[2].ToString());
                    flag = false;
                }
                connection.Close();
            }
            catch (SqlException exc)
            {
                Console.WriteLine("Error: " + exc.Message);
                return true;
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine("Error: " + exc.Message);
            }
            finally
            {
                connection.Close();
            }
            return flag;
        }

        public bool ShowAllContacts()
        {
            bool flag = true;
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "select * from contacts";
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader[0].ToString() + "  " + reader[1] + " " + reader[2] + "  " + reader[3] + "  " + reader[4] + "  " 
                        + reader[5] + "  " + reader[6] + "  " + reader[7]);
                    flag = false;
                }
                connection.Close();
            }
            catch (SqlException exc)
            {
                Console.WriteLine("Error: " + exc.Message);
                return true;
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine("Error: " + exc.Message);
            }
            finally
            {
                connection.Close();
            }
            return flag;
        }

        public bool ShowContactsInGroup(Group group)
        {
            bool flag = true;
            var connectionString3 = Properties.Settings.Default.Database1ConnectionString;
            SqlConnection connection3 = new SqlConnection(connectionString3);
            connection3.Open();
            SqlCommand cmd3 = new SqlCommand();
            cmd3.Connection = connection3;
            cmd3.CommandType = System.Data.CommandType.Text;
            cmd3.CommandText = "SELECT * FROM contacts INNER JOIN (groups INNER JOIN junction_table ON groups.num_g = junction_table.num_g) ON contacts.num_c = junction_table.num_c WHERE (groups.group_name)='" + group.GroupName + "' and contacts.num_u=" + _numberOfUser;
            SqlDataReader reader = cmd3.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[0].ToString() + "  " + reader[1] + " " + reader[2] + "  " + reader[3] + "  " + reader[4] + "  " + reader[5] + "  " + reader[6]);
                flag = false;
            }
            connection3.Close();
            return flag;
        }

        public bool AddContactToGroup(int numberOfContact, String groupName)
        {
            int groupNumber = GroupNumberByName(groupName);
            bool flag = false;
            if (groupNumber == 0)
            {
                flag = true;
            }
            else
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "(select max(num_j)+1 from junction_table)";
                try
                {
                    connection.Open();
                    var maxIdJ = cmd.ExecuteScalar();
                    int index;
                    if (maxIdJ == DBNull.Value)
                    {
                        index = 0;
                    }
                    else
                    {
                        index = Convert.ToInt32(maxIdJ);
                    }
                    //connection.Close();
                    String sql2 = "select * from contacts where num_c=" + numberOfContact + " and num_u=" + _numberOfUser;
                    SqlCommand cmd2 = new SqlCommand(sql2, connection);
                    SqlDataReader reader = cmd2.ExecuteReader();
                    if (reader.Read())
                    {
                        reader.Close();
                        String sql3 = "select * from junction_table where num_c=" + numberOfContact + " and num_g=" + groupNumber;
                        SqlCommand cmd3 = new SqlCommand(sql3, connection);
                        SqlDataReader reader2 = cmd3.ExecuteReader();
                        if (reader2.Read())
                        {
                            //Console.WriteLine(reader2[0].ToString()+" "+reader2[1].ToString()+" "+reader2[2].ToString());
                            flag = true;
                            reader2.Close();
                        }
                        else
                        {
                            reader2.Close();
                            String sql = "insert into junction_table values (" + index + ", " + groupNumber + ", " + numberOfContact + ")";
                            SqlCommand cmd4 = new SqlCommand(sql, connection);
                            //connection.Open();
                            int numAdd = cmd4.ExecuteNonQuery();
                            Console.WriteLine(numAdd + " contact is added");
                        }
                    }
                    else
                    {
                        reader.Close();
                        flag = true;
                    }
                    connection.Close();
                }
                catch (SqlException exc)
                {
                    Console.WriteLine("Error: " + exc.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public bool DeleteContactFromGroup(int numberOfContact, String groupName)
        {
            int groupNumber = GroupNumberByName(groupName);
            bool flag = false;
            if (groupNumber == 0)
            { flag = true; }
            else
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                String sql = "delete from junction_table where num_g=" + groupNumber + " and num_c=" + numberOfContact;
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Connection = connection;
                try
                {
                    connection.Open();
                    var del = cmd.ExecuteNonQuery();
                    if (del == 1)
                    {
                        Console.WriteLine(del + " contact is deleted from group");
                    }
                    else
                    {
                        flag = true;
                    }
                    connection.Close();
                }
                catch (SqlException exc)
                {
                    Console.WriteLine("Error: " + exc.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return flag;
        }

        public int GroupNumberByName(String groupName)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "(select num_g from groups where group_name='" + groupName + "' and num_u=" + _numberOfUser + ")";
            connection.Open();
            var num_g = cmd.ExecuteScalar();
            int index;
            if (num_g == DBNull.Value)
            {
                index = 0;
            }
            else
            {
                index = Convert.ToInt32(num_g);
            }
            return index;
        }

        public void ShowJunctionTable()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            String sql = "select * from junction_table";
            SqlCommand cmd = new SqlCommand(sql, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader[0].ToString() + " " + reader[1].ToString() + " " + reader[2].ToString());
            }
            reader.Close();
            connection.Close();
        }

        public bool AddUser(User user)
        {
            bool flag = false;
            SqlConnection connection = new SqlConnection(_connectionString);
            String sql1 = "(select max(num_u)+1 from users)";
            SqlCommand cmd = new SqlCommand(sql1, connection);
            try
            {
                connection.Open();
                var maxIdJ = cmd.ExecuteScalar();
                int index;
                if (maxIdJ == DBNull.Value)
                {
                    index = 0;
                }
                else
                {
                    index = Convert.ToInt32(maxIdJ);
                }
                String sql2 = "insert into users values (" + index + ",'" + user.Name + "', '" + user.Password + "')";
                SqlCommand cmd2 = new SqlCommand(sql2, connection);
                int del = cmd2.ExecuteNonQuery();
                if (del == 0)
                {
                    flag = true;
                }
                Console.WriteLine(del + " user is added");
                connection.Close();
            }
            catch (SqlException exc)
            {
                Console.WriteLine(exc.Message);
            }
            return flag;
        }

        public bool DeleteUser(User user)
        {
            bool flag = false;
            SqlConnection connection = new SqlConnection(_connectionString);
            String sql = "delete from users where name='" + user.Name + "' and password='" + user.Password + "'";
            SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Connection = connection;
            try
            {
                connection.Open();
                var del = cmd.ExecuteNonQuery();
                if (del != 0)
                {
                    Console.WriteLine(del + " user is deleted");
                }
                else
                {
                    flag = true;
                }
                connection.Close();
            }
            catch (SqlException exc)
            {
                Console.WriteLine(exc.Message);
            }
            finally
            {
                connection.Close();
            }
            return flag;
        }

        public bool EditUser(User oldUser, User newUser)
        {
            bool flag = false;
            SqlConnection connection = new SqlConnection(_connectionString);
            String sql = "update users set name='" + newUser.Name + "', password='" + newUser.Password + "' where name='" + oldUser.Name
                + "' and password='" + oldUser.Password + "'";
            SqlCommand cmd = new SqlCommand(sql, connection);
            try
            {
                connection.Open();
                int edit = cmd.ExecuteNonQuery();
                if (edit == 0)
                {
                    flag = true;
                }
                connection.Close();
                Console.WriteLine(edit + " user is edited");
            }
            catch (SqlException exc)
            {
                Console.WriteLine(exc.Message);
                flag = true;
            }
            finally
            {
                connection.Close();
            }
            return flag;
        }

        public bool ShowContacts()
        {
            bool flag = true;
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "select * from contacts where num_u='" + _numberOfUser + "'";
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader[0].ToString() + "  " + reader[1] + " " + reader[2] + "  " + reader[3] + "  " + reader[4] + "  " 
                        + reader[5] + "  " + reader[6]);
                    flag = false;
                }
                connection.Close();
            }
            catch (SqlException exc)
            {
                Console.WriteLine("Error: " + exc.Message);
                return true;
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine("Error: " + exc.Message);
            }
            finally
            {
                connection.Close();
            }
            return flag;
        }
    }
}
