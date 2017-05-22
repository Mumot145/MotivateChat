using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;
using MotivationAdmin.Models;
using System.Linq;
using System.Threading;

namespace MotivationAdmin
{
    public class AzureDataService
    {

        SqlConnection connection = new SqlConnection();
        static AzureDataService defaultInstance = new AzureDataService();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader reader;
        private AzureDataService()
        {
            this.connection = new SqlConnection(Constants.AzureSQLConnection);
        }
        public static AzureDataService DefaultService
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public User GetUser(string Info, string Method)
        {
            User _user = new User();
            string query="";
            if (Method == "fbId")
            {
                query = "SELECT Id, Name, FacebookId, AdminBool FROM Users WHERE FacebookId = '" + Info + "'";
             
            } else if(Method == "rId")
            {
                query = "SELECT Id, Name, FacebookId, AdminBool FROM Users WHERE Id = '" + Info + "'";
            } else if(Method == "Name")
            {
                query = "SELECT Id, Name, FacebookId, AdminBool FROM Users WHERE Name = '" + Info + "'";
            }
                         
            _user = (User) AzureConnect(query, "User");
            return _user;
        }
        public Schedule GetSchedule(string Info)
        {
            Schedule _schedule = new Schedule();
            string query = "";

            query = "SELECT Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday FROM Schedule WHERE Id = '" + Info + "'";

            _schedule = (Schedule)AzureConnect(query, "Schedule");
            _schedule.Id = Convert.ToInt32(Info);
            return _schedule;
        }
        public void RegisterUser(string facebookName, string facebookId)
        {
            string query = "INSERT INTO Users (Name, FacebookId, AdminBool) VALUES ('" + facebookName + "', '" + facebookId + "', 1)";
            AzureConnect(query, "Edit");
        }
        public void AddNewGroup(string cgName, User _user)
        {
            string query = "INSERT INTO ChatGroups (Name) VALUES ('" + cgName + "'); SELECT SCOPE_IDENTITY();";
            int lastId = (int) AzureConnect(query, "CheckId");
            if (lastId != 0)
            {
                AddUserToGroup(_user, lastId);
                AddScheduleToGroup(lastId);
            }          
        }
        public void AddUserToGroup(User _user, int _id)
        {
            string query = "INSERT INTO UserChatGroups (UserId, ChatGroupId) VALUES ('" + _user.Id + "', '" + _id + "')";
            AzureConnect(query, "Edit");
        }
        public void AddScheduleToGroup(int _id)
        {
            string query = "INSERT INTO Schedule (Id) VALUES ('" + _id + "')";
            AzureConnect(query, "Edit");
        }
        public void UpdateSchedule(Schedule _schedule)
        {
            string query = "UPDATE Schedule SET Monday = " + ((_schedule.Monday) ? 1 : 0) + ", Tuesday = " + ((_schedule.Tuesday) ? 1 : 0) + ", "
                                + "Wednesday = " + ((_schedule.Wednesday) ? 1 : 0) + ", Thursday = " + ((_schedule.Thursday) ? 1 : 0) + ", "
                                + "Friday = " + ((_schedule.Friday) ? 1 : 0) + ", Saturday = " + ((_schedule.Saturday) ? 1 : 0) + ", "
                                + "Sunday = " + ((_schedule.Sunday) ? 1 : 0) + " WHERE Id = '" + _schedule.Id + "'";
            AzureConnect(query, "Edit");
        }
        public void ResetCompletion(string _userId)
        {
            string query = "UPDATE Users SET Complete = null WHERE Id = '" + _userId + "'";
            AzureConnect(query, "Edit");
        }
        public List<ChatGroup> GetGroups(int _userId)
        {
            //List<String> _groupIdList = new List<String>();
            List<ChatGroup> _groupList = new List<ChatGroup>();


            string query = "SELECT cg.Id Id, cg.Name Name FROM ChatGroups cg INNER JOIN UserChatGroups ucg"
                                + " ON ucg.ChatGroupId = cg.Id"
                                + " WHERE ucg.UserId =" + _userId;

            _groupList = (List<ChatGroup>)AzureConnect(query, "ChatGroups");

            if (_groupList != null)
            {

                _groupList = GetGroupUsers(_groupList);
                _groupList = GetGroupToDos(_groupList);
            }


            return _groupList;
        }
        public List<ChatGroup> GetGroupUsers(List<ChatGroup> groupList)
        {
            List<User> _userList = new List<User>();
            User _user = new User();
            string groups = "";
            foreach (var g in groupList)
            {
                if (groups == "")               
                    groups = g.Id + ", ";                
                else               
                    groups = groups + g.Id + ", ";            
            }
            groups = groups.Remove(groups.Length - 2);
            string query = "SELECT u.Id, u.Name, u.FacebookId, u.AdminBool, u.Complete, ucg.ChatGroupId FROM Users u INNER JOIN UserChatGroups ucg"
                                + " ON ucg.UserId = u.Id"
                                + " WHERE ucg.ChatGroupId IN (" + groups + ") AND u.AdminBool != 1";
 
            List<ChatGroup> chatGroupList = (List<ChatGroup>)AzureConnect(query, "GroupUserList");
            foreach (var gl in groupList)
            {
                List<User> ul  =  chatGroupList.Where(cg => cg.Id == gl.Id).Select(x => x.UserList).FirstOrDefault();
                gl.UserList = ul;
               
            }

            return groupList;
        }
        public List<User> GetSingleGroupUsers(ChatGroup chatGroup)
        {
            List<User> _userList = new List<User>();
            User _user = new User();

            string query = "SELECT u.Id, u.Name, u.FacebookId, u.AdminBool, u.Complete, ucg.ChatGroupId FROM Users u INNER JOIN UserChatGroups ucg"
                                + " ON ucg.UserId = u.Id"
                                + " WHERE ucg.ChatGroupId ='" + chatGroup.Id.ToString() + "'";

            List<User> chatGroupList = (List<User>)AzureConnect(query, "UserList");
  

            return chatGroupList;
        }
        public List<ChatGroup> GetGroupToDos(List<ChatGroup> groupList)
        {
            string groups = "";
            //ChatGroup cg = new ChatGroup();
            List<TodoItem> toDoList = new List<TodoItem>();

            foreach (var g in groupList)
            {
                if (groups == "")
                    groups = "'" + g.Id + "', '";

                groups = groups + g.Id + "', '";
            }
            groups = groups.Remove(groups.Length - 3);
            string query = "SELECT id, text, groupId, complete, sendTime FROM ToDoItem WHERE groupId IN (" + groups + ") AND deleted != 'True'";
            List<ChatGroup> chatGroupList = (List<ChatGroup>)AzureConnect(query, "ToDoList");
            foreach (var gl in groupList)
            {
                List<TodoItem> td = chatGroupList.Where(cg => cg.Id == gl.Id).Select(x => x.ToDoList).FirstOrDefault();
                gl.ToDoList = td;

            }
            return groupList;
            
        }
        public void DeleteFromGroup(int id, ChatGroup chatGroup)
        {
            string query = "DELETE FROM UserChatGroups WHERE UserId = '" + id.ToString() + "' AND ChatGroupId = '" + chatGroup.Id + "'";
            AzureConnect(query, "Edit");
        }

        private static readonly int[] RetriableClasses = { 13, 16, 17, 18, 19, 20, 21, 22, 24 };

        public object AzureConnect(string Query, string taskType)
        {
            bool rebuildConnection = true; // First try connection must be open
            object returnValue = null;
            for (int i = 0; i < RetriableClasses[5]; ++i)
            {
                try
                {
                    // (Re)Create connection to SQL Server
                    if (rebuildConnection)
                    {
                        if (connection != null)
                            connection.Dispose();

                        // Create connection and open it...                        
                        cmd.CommandText = Query;
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = connection;
                        connection.Open();

                    }

                    // inserts information
                    
                    if (taskType == "Edit")
                    {
                        int rows = cmd.ExecuteNonQuery();
                        Console.WriteLine($"edited {rows} row(s).");
                        connection.Close();
                        return rows;
                    }                  
                    else //finds information
                    {
                        reader = cmd.ExecuteReader();
                        if (taskType == "User")
                        {
                            returnValue = readUser(reader);
                        } else if (taskType == "ChatGroups")
                        {
                            returnValue = readChatGroups(reader);
                        } else if (taskType == "CheckId")
                        {
                            returnValue = readLastId(reader);
                        } else if (taskType == "GroupUserList")
                        {
                            returnValue = readGroupUsers(reader);
                        } else if (taskType == "UserList")
                        {
                            returnValue = readSingleGroupUsers(reader);
                        } else if (taskType =="ToDoList")
                        {
                            returnValue = readToDoList(reader);
                        } else if (taskType == "Schedule")
                        {
                            returnValue = readSchedule(reader);
                        }
                    }
                    

                    // No exceptions, task has been completed
                    return returnValue;
                }
                catch (SqlException e)
                {
                    if (e.Errors.Cast<SqlError>().All(x => CanRetry(x)))
                    {
                        // What to do? Handle that here, also checking Number property.
                        // For Class < 20 you may simply Thread.Sleep(DelayOnError);
                        Thread.Sleep(2500);
                        rebuildConnection = e.Errors
                            .Cast<SqlError>()
                            .Any(x => x.Class >= 20);

                        continue;
                    }

                    throw;
                }
            }
            return null;
        }
        private List<ChatGroup> readToDoList(SqlDataReader _reader)
        {
            List<TodoItem> todoList = new List<TodoItem>();
            ChatGroup _chatGroup = new ChatGroup();
            List<ChatGroup> _chatGroupList = new List<ChatGroup>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    TodoItem toDo = new TodoItem();
                    toDo.Id = String.Format("{0}", reader[0]);
                    toDo.ToDo = String.Format("{0}", reader[1]);
                    toDo.GroupId = String.Format("{0}", reader[2]);
                    toDo.Done = Convert.ToBoolean(reader[3]);
                    toDo.SendTime = String.Format("{0}", reader[4]);
                    //toDo.GroupId = String.Format("{0}", reader[4]);
                    todoList.Add(toDo);

                    _chatGroup.Id = Convert.ToInt32(toDo.GroupId);
                    
                    var check = _chatGroupList.Any(item => item.Id == _chatGroup.Id);

                    if (check == false)
                    {
                        _chatGroup.ToDoList.Add(toDo);
                        _chatGroupList.Add(_chatGroup);
                        _chatGroup = new ChatGroup();
                    }
                    else
                    {
                        foreach (ChatGroup item in _chatGroupList.Where(c => c.Id == _chatGroup.Id))
                        {
                            item.ToDoList.Add(toDo);                         
                        }

                    }
                }
            }

            //sGroup.ToDoList.Add(toDoList);
            connection.Close();


            //coupons = await GetCouponImages(place);
            return _chatGroupList;
            
        }
        private Schedule readSchedule(SqlDataReader _reader)
        {
            Schedule schedule = new Schedule();
            if (reader != null)
            {
                while (reader.Read())
                {

                    schedule.Monday = Convert.ToBoolean(reader[0]);
                    schedule.Tuesday = Convert.ToBoolean(reader[1]);
                    schedule.Wednesday = Convert.ToBoolean(reader[2]);
                    schedule.Thursday = Convert.ToBoolean(reader[3]);
                    schedule.Friday = Convert.ToBoolean(reader[4]);
                    schedule.Saturday = Convert.ToBoolean(reader[5]);
                    schedule.Sunday = Convert.ToBoolean(reader[6]);
                }
            }
            connection.Close();
            return schedule;
        }
        private int readLastId(SqlDataReader _reader)
        {
            int _lastId = 0;          
            if (_reader != null)
            {
                while (reader.Read())
                {
                    _lastId = Convert.ToInt32(reader[0]);
                }
            }

            connection.Close();
            return _lastId;
        }
        private User readUser(SqlDataReader _reader)
        {
            User _user = new User();
            if (_reader != null)
            {
                while (reader.Read())
                {
                    _user.Id = Convert.ToInt32(reader[0]);
                    _user.Name = String.Format("{0}", reader[1]);
                    _user.FacebookId = String.Format("{0}", reader[2]);
                    _user.Admin = Convert.ToBoolean(reader[3]);
                   // _user.CompleteDate = Convert.ToDateTime(reader[4]);
                }
            }
            connection.Close();
            return _user;
        }
        private List<ChatGroup> readGroupUsers(SqlDataReader _reader)
        {
            
            List<User> _userList = new List<User>();
            ChatGroup _chatGroup = new ChatGroup();
            List<ChatGroup> _chatGroupList = new List<ChatGroup>();
            int groupNo = 0;
            if (_reader != null)
            {
                while (reader.Read())
                {
                    User _user = new User();
                    _user.Id = Convert.ToInt32(reader[0]);
                    _user.Name = String.Format("{0}", reader[1]);
                    _user.FacebookId = String.Format("{0}", reader[2]);
                    _user.Admin = Convert.ToBoolean(reader[3]);
                    if (reader.IsDBNull(4))
                        _user.CompleteDate = DateTime.MinValue;
                    else
                        _user.CompleteDate = Convert.ToDateTime(reader[4]);

                    groupNo = Convert.ToInt32(reader[5]);
                    _chatGroup.Id = groupNo;
                    _chatGroup.UserList.Add(_user);
                    var check = _chatGroupList.Any(item => item.Id == groupNo);                  
                    if (check == false)
                    {
                        _chatGroupList.Add(_chatGroup);                       
                        _chatGroup = new ChatGroup();
                    } else
                    {
                        foreach (ChatGroup item in _chatGroupList.Where(c => c.Id == groupNo))
                        {
                            item.UserList.Add(_user);
                            Debug.WriteLine("adding this user ="+_user.Name +" to group ="+groupNo);
                        }
                    }    
                }
            }
            connection.Close();
            return _chatGroupList;
        }
        private List<User> readSingleGroupUsers(SqlDataReader _reader)
        {
            User _user = new User();
            List<User> _userList = new List<User>();
            if (_reader != null)
            {
                while (reader.Read())
                {
                    _user.Id = Convert.ToInt32(reader[0]);
                    _user.Name = String.Format("{0}", reader[1]);
                    _user.FacebookId = String.Format("{0}", reader[2]);
                    _user.Admin = Convert.ToBoolean(reader[3]);
                    _userList.Add(_user);
                }
            }
            connection.Close();
            return _userList;
        }
        private List<ChatGroup> readChatGroups(SqlDataReader _reader)
        {
            
            List<ChatGroup> _chatGroupList = new List<ChatGroup>();
            if (_reader != null)
            {
                while (reader.Read())
                {
                    ChatGroup _group = new ChatGroup();
                    _group.Id = Convert.ToInt32(String.Format("{0}", reader[0]));
                    _group.GroupName = String.Format("{0}", reader[1]);                   
                    _chatGroupList.Add(_group);
                }
            }
            connection.Close();
            return _chatGroupList;
        }
        private static bool CanRetry(SqlError error)
        {
            // Use this switch if you want to handle only well-known errors,
            // remove it if you want to always retry. A "blacklist" approach may
            // also work: return false when you're sure you can't recover from one
            // error and rely on Class for anything else.
            switch (error.Number)
            {
                case 4060:
                    Debug.WriteLine("cannot open DB");
                    break;
                case 40197:
                    Debug.WriteLine("error processing request");
                    break;
                case 40501:
                    Debug.WriteLine("services busy - retry in 10 seconds");
                    break;
                case 40613:
                    Debug.WriteLine("database  currently unavailable");
                    break;
                case 49918:
                    Debug.WriteLine("cannot process request - not enough resources");
                    break;
                case 49919:
                    Debug.WriteLine("cannot process create or update request - too many operations");
                    break;
                case 49920:
                    Debug.WriteLine("cannot process request - too many operations");
                    break;

                    // Handle well-known error codes, 
            }

            // Handle unknown errors with severity 21 or less. 22 or more
            // indicates a serious error that need to be manually fixed.
            // 24 indicates media errors. They're serious errors (that should
            // be also notified) but we may retry...
            return RetriableClasses.Contains(error.Class); // LINQ...
        } 
    }
}
