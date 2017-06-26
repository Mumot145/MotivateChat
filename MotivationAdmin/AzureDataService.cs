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
using System.Net.Http;
using Newtonsoft.Json;

namespace MotivationAdmin
{
    public class AzureDataService
    {

        SqlConnection connection = new SqlConnection();
        FacebookUser facebookUser = new FacebookUser();
        private static User User = new User();
        private Week week = new Week();
        static AzureDataService defaultInstance = new AzureDataService();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader reader;
        
        private AzureDataService()
        {
            
        }
        private void Init()
        {
            this.connection = new SqlConnection(Constants.AzureSQLConnection);
        }
        public User GetUser()
        {
            return User;
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
         
        public void SetUser(string Method)
        {
           // User _user = new User();
            string query="";
            if (Method == "fbId")
            {
                query = "SELECT Id, Name, FacebookId, AdminBool, Email, Phone, Location, Gender, Age FROM Users WHERE FacebookId = '" + facebookUser.Id + "'";
             
            } else if(Method == "rId")
            {
                query = "SELECT Id, Name, FacebookId, AdminBool, Email, Phone, Location, Gender, Age FROM Users WHERE Id = '" + facebookUser.Id + "'";
            } else if(Method == "Name")
            {
                query = "SELECT Id, Name, FacebookId, AdminBool, Email, Phone, Location, Gender, Age FROM Users WHERE Name = '" + facebookUser.Name + "'";
            }
                         
            User = (User) AzureConnect(query, "User");
           // return _user;
        }

        
        public void AddDaysToSchedule(int scheduleId, List<Day> dayList)
        {
            string fullBuilder = "";
            string builder = "";

            foreach (var day in dayList)
            {
                builder = "('" + scheduleId + "', '"+ day.Id + "'), ";
                fullBuilder = fullBuilder + builder;
            }
            fullBuilder = fullBuilder.Remove(fullBuilder.Length - 2);

            string query = "INSERT INTO Schedule (TodoId, TodoWeekday) VALUES " + fullBuilder;
            AzureConnect(query, "Edit");
        }
        public void RegisterUser()
        {
            string query = "INSERT INTO Users (Name, FacebookId, AdminBool) VALUES ('" + facebookUser.Name + "', '" + facebookUser.Id + "', 1)";
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
   
        public int[] AddScheduleToMessage(List<TodoFullItem> toDoList)
        {
            string fullBuilder = "";
            string builder="";
            int[] allIds = new int[7];
            int addedItems = 0;
            foreach(var todo in toDoList)
            {
                builder = "('" + todo.AttachedToDo.Id + "', '"+todo.getTime+"'), ";
                fullBuilder = fullBuilder + builder;
                builder = "";
                addedItems++;
            }
            fullBuilder = fullBuilder.Remove(fullBuilder.Length - 2);

            string query = "INSERT INTO ToDoSchedule (Id, SendTime) VALUES " + fullBuilder+ "; SELECT SCOPE_IDENTITY();";
            var passedback = (int)AzureConnect(query, "ToDoSchedule");
            if (passedback > 0 && addedItems > 1)
            {
                for (var i = 0; i < addedItems; i++)
                {
                    allIds[i] = passedback - (addedItems -(i+1));
                }
                //AddUserToGroup(_user, lastId);
                //AddScheduleToGroup(lastId);

                return allIds;
            } else
            {
                allIds[0] = passedback;
                return allIds;
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

        public void ResetCompletion(string _userId)
        {
            string query = "UPDATE Users SET Complete = null WHERE Id = '" + _userId + "'";
            AzureConnect(query, "Edit");
        }
        public List<ChatGroup> GetGroups(int _userId)
        {
            //List<String> _groupIdList = new List<String>();
            List<ChatGroup> _groupList = new List<ChatGroup>();


            string query = "SELECT cg.Id Id, cg.Name Name, cg.SoloGroup SoloGroup FROM ChatGroups cg INNER JOIN UserChatGroups ucg"
                                + " ON ucg.ChatGroupId = cg.Id"
                                + " WHERE ucg.UserId =" + _userId;

            _groupList = (List<ChatGroup>)AzureConnect(query, "ChatGroups");

            if (_groupList != null)
            {

                _groupList = GetGroupUsers(_groupList);
                _groupList = GetGroupToDos(_groupList);
               // if(_groupList.Where(gl=>gl.))
                _groupList = GetGroupDays(_groupList);
                // _groupList = GetToDoSchedule(_groupList);
            }


            return _groupList;
        }
        public List<ChatGroup> GetGroupDays(List<ChatGroup> groupList)
        {
            string daySched = "";
            string addToSched;
            //ChatGroup cg = new ChatGroup();
          //  List<TodoItem> toDoList = new List<TodoItem>();
            if(groupList != null)
            {
                foreach (var g in groupList)
                {
                    if (g.ToDoList != null)
                    {
                        foreach (var td in g.ToDoList)
                        {
                            if (td.ScheduleId > 0)
                            {                               
                                addToSched = String.Format("{0}", td.ScheduleId);
                                daySched = daySched + addToSched + ",";
                            }
                        }
                    }
                }
                if(daySched != "")
                {
                    daySched = daySched.Remove(daySched.Length - 1);
                    string query = "SELECT TodoId, TodoWeekday FROM Schedule WHERE TodoId IN (" + daySched + ") AND deleted != 'True'";
                    List<TodoFullItem> todoList = (List<TodoFullItem>)AzureConnect(query, "ToDoDaysList");
                    foreach (var gl in groupList)
                    {
                        if (gl != null && gl.ToDoList != null)
                        {
                            foreach (var g in gl.ToDoList)
                            {
                                //TodoItem todo = new TodoItem();
                                if (g.ScheduleId > 0)
                                {
                                    Console.WriteLine("----------------------" + g.ScheduleId);
                                    List<TodoFullItem> getScheduleDays = todoList.Where(td => td.ScheduleId == g.ScheduleId).ToList();
                                    if (getScheduleDays != null)
                                    {
                                        foreach (var s in getScheduleDays)
                                        {
                                            g.toDoDays = s.toDoDays;
                                            Console.WriteLine("g========>" + g.DayStr);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    return groupList;
                }
               
            }

            return groupList;
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
            string query = "SELECT u.Id, u.Name, u.FacebookId, u.AdminBool, u.Complete, ucg.ChatGroupId, u.Email, u.Phone, u.Location, u.Gender, u.Age FROM Users u INNER JOIN UserChatGroups ucg"
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
            List<TodoFullItem> toDoList = new List<TodoFullItem>();

            foreach (var g in groupList)
            {
                if (groups == "")
                    groups = "'" + g.Id + "', '";

                groups = groups + g.Id + "', '";
            }
            groups = groups.Remove(groups.Length - 3);
            string query = "SELECT td.id, td.text, td.groupId, td.complete, tds.ScheduleDay, tds.SendTime FROM ToDoItem td LEFT JOIN ToDoSchedule tds ON td.id = tds.Id WHERE td.groupId IN (" + groups + ") AND td.deleted != 'True'";
            List<ChatGroup> chatGroupList = (List<ChatGroup>)AzureConnect(query, "ToDoList");
            foreach (var gl in groupList)
            {
                List<TodoFullItem> td = chatGroupList.Where(cg => cg.Id == gl.Id).Select(x => x.ToDoList).FirstOrDefault();
                gl.ToDoList = td;

            }
            return groupList;
            
        }
        public List<ChatGroup> GetToDoSchedule(List<ChatGroup> groupList)
        {
            string groups = "";
            //ChatGroup cg = new ChatGroup();
            List<TodoFullItem> toDoList = new List<TodoFullItem>();

            foreach (var g in groupList)
            {
                if (groups == "")
                    groups = "'" + g.Id + "', '";

                groups = groups + g.Id + "', '";
            }
            groups = groups.Remove(groups.Length - 3);
            string query = "SELECT id, text, groupId, complete FROM toDoSchedule WHERE id IN (" + groups + ") AND deleted != 'True'";
            List<ChatGroup> chatGroupList = (List<ChatGroup>)AzureConnect(query, "ToDoList");
            foreach (var gl in groupList)
            {
                List<TodoFullItem> td = chatGroupList.Where(cg => cg.Id == gl.Id).Select(x => x.ToDoList).FirstOrDefault();
                gl.ToDoList = td;

            }
            return groupList;

        }
        public void DeleteFromGroup(int id, ChatGroup chatGroup)
        {
            string query = "DELETE FROM UserChatGroups WHERE UserId = '" + id.ToString() + "' AND ChatGroupId = '" + chatGroup.Id + "'";
            AzureConnect(query, "Edit");
        }
        public void AddToSchedule(int id, ChatGroup chatGroup)
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

                        Init();
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
                        }
                        else if (taskType == "ToDoSchedule")
                        {
                            returnValue = readLastScheduleIds(reader);
                        }
                        else if (taskType == "ToDoDaysList")
                        {
                            returnValue = readAllDays(reader);
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
            List<TodoFullItem> todoList = new List<TodoFullItem>();
            ChatGroup _chatGroup = new ChatGroup();
            List<ChatGroup> _chatGroupList = new List<ChatGroup>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    TodoFullItem toDo = new TodoFullItem();
                    toDo.AttachedToDo = new TodoItem();
                    toDo.AttachedToDo.Id = String.Format("{0}", reader[0]);
                    toDo.AttachedToDo.ToDo = String.Format("{0}", reader[1]);
                    toDo.AttachedToDo.GroupId = String.Format("{0}", reader[2]);
                    toDo.AttachedToDo.Done = Convert.ToBoolean(reader[3]);
                    if (!reader.IsDBNull(4))
                    {
                        toDo.ScheduleId = Convert.ToInt32(reader[4]);
                        toDo.SendTimeSpan = TimeSpan.Parse(String.Format("{0}", reader[5]));
                    }
                    todoList.Add(toDo);

                    _chatGroup.Id = Convert.ToInt32(toDo.AttachedToDo.GroupId);
                    
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
        private List<TodoFullItem> readAllDays(SqlDataReader _reader)
        {
            List<TodoFullItem> todoList = new List<TodoFullItem>();
            TodoFullItem todo;
            TodoFullItem lastTodo = new TodoFullItem();
            int lastId = 0;
           // Week week = new Week();
            if (reader != null)
            {
                while (reader.Read())
                {
                    
                    var id = Convert.ToInt32(reader[0]);
                    var other = Convert.ToInt32(reader[1]);
                    Console.WriteLine("id" + id);
                    Console.WriteLine("other" + other);
                    //Day addDay = new Day();
                    //int schedId = Convert.ToInt32(reader[0]);
                    //if (schedId == lastId)
                   // {
                    if (id == lastId)
                    {
                        lastTodo.toDoDays.Add(week.getDay(other));
                    }
                    else
                    {                        
                        todo = new TodoFullItem();
                        todo.AttachedToDo = new TodoItem();
                        todo.ScheduleId = id;
                        Day d = new Day();
                        d = week.getDay(other);
                        todo.toDoDays = new List<Day>();
                        todo.toDoDays.Add(d);
                        //if (lastId > 0)
                       // {
                       //     Console.WriteLine("added > 0+" + lastTodo.ScheduleId);
                       //     todoList.Add(lastTodo);
                       // } else
                      //  {
                            Console.WriteLine("added = 0 +" + todo.ScheduleId);
                            todoList.Add(todo);
                       // }
                        lastTodo = todo;

                    }
                    lastId = id;
                }
                foreach (var td in todoList)
                {
                    Console.WriteLine("td--" + td.AttachedToDo.ToDo);
                }
            }
            connection.Close();
            return todoList;
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
        private int readLastScheduleIds(SqlDataReader _reader)
        {

            int _lastId = 0;
            if (_reader != null)
            {
                while (reader.Read())
                {

                    _lastId = Convert.ToInt32(String.Format("{0}", reader[0]));                 
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
                    //SELECT Id, Name, FacebookId, AdminBool, Email, Phone, Location, Gender, Age
                    _user.Id = Convert.ToInt32(reader[0]);
                    _user.Name = String.Format("{0}", reader[1]);
                    _user.FacebookId = String.Format("{0}", reader[2]);
                    _user.Admin = Convert.ToBoolean(reader[3]);
                    _user.Email = String.Format("{0}", reader[4]);
                    _user.Phone = String.Format("{0}", reader[5]);
                    _user.Location = Convert.ToInt32(reader[6]);
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
                    //Id, Name, FacebookId, AdminBool, Email, Phone, Location, Gender, Age
                    groupNo = Convert.ToInt32(reader[5]);
                    _user.Email = Convert.ToString(reader[6]);
                    _user.Phone = Convert.ToString(reader[7]);
                    _user.Location = Convert.ToInt32(reader[8]);
                    _user.Gender = Convert.ToBoolean(reader[9]);
                    _user.Age = Convert.ToInt32(reader[10]);
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
                    _group.SoloGroup = bool.Parse(String.Format("{0}", reader[2]));
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
        public async Task GetFacebookProfileAsync(string accessToken)
        {
            var requestUrl = "https://graph.facebook.com/v2.8/me/"
                             + "?fields=name,picture,cover,age_range,devices,email,gender,is_verified"
                             + "&access_token=" + accessToken;
            var httpClient = new HttpClient();
            var userJson = await httpClient.GetStringAsync(requestUrl);
            facebookUser = JsonConvert.DeserializeObject<FacebookUser>(userJson);
        }

    }
}
