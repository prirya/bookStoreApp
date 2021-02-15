using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace bookStoreApp
{
    class DataAccess
    {

        public const string dbpath = "Database_BookStore.db";
        public const string admindbpath = "adminDatabase.db";
        //private static object createTable; ไม่ได้ใช่งาน

        public static void InitializeDatabase()
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String tableCommand =
                    //ตารางเก็บข้อมูลลูกค้า
                    "CREATE TABLE IF NOT EXISTS CustomersTable (`Customer ID` INTEGER PRIMARY KEY, `Name` NVARCHAR(50) NULL, `Address` NVARCHAR(500) NULL, `Email` NVARCHAR(50) NULL);" +
                    //สร้างตารางหนังสือ
                    "CREATE TABLE IF NOT EXISTS BookTable (`ISBN` CHAR(10) PRIMARY KEY, Title NVARCHAR(200) NULL,Description NVARCHAR(500) NULL,Price DOUBLE NULL);" +
                    //ข้อมูลการขาย
                    "CREATE TABLE IF NOT EXISTS Transactions (`ISBN` CHAR(10) PRIMARY KEY,`Customers ID` NVARCHAR(200) NULL,Quatity INTEGER NULL,`Total Price` DOUBLE NULL);";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);
                createTable.ExecuteReader();

            }
            using (SqliteConnection admindb = new SqliteConnection($"Filename={admindbpath}"))
            {
                admindb.Open();

                String tableCommand =
                    //ตารางเก็บข้อมูล User
                    "CREATE TABLE IF NOT EXISTS User (`Number` INTEGER PRIMARY KEY, `User ID` NVARCHAR(100) NULL, `Password` NVARCHAR(100) NULL, `Name` NVARCHAR(50) NULL, `Address` NVARCHAR(500) NULL, `Email` NVARCHAR(50) NULL, `Birth day` DATE NULL, `Sex (Male)` BOOL NULL, `TypeAdmin` BOOL NULL);";

                SqliteCommand createTable = new SqliteCommand(tableCommand, admindb);
                createTable.ExecuteReader();
            }

#if DEBUG
            AddDebugData();
#endif
        }

        public static void AddDebugData()
        {
            var usernames = GetUsernames();
            if (usernames.ContainsKey("root"))
                return;
            //Admin test
            AddUserTable("root", "7777", "Administrator", "", "", new DateTime(1990, 5, 12), true, true);
            //User test
            //    AddDataCustomerTable(GetRandomName(), "", "");
            //    AddDataCustomerTable(GetRandomName(), "", "");
            //    AddDataCustomerTable(GetRandomName(), "", "");
            //    AddDataCustomerTable(GetRandomName(), "", "");
            //    AddDataCustomerTable(GetRandomName(), "", "");
        }

        static string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static Random randomCore;
        public static string GetRandomName()
        {
            if (randomCore == null)
                randomCore = new Random();
            int length = randomCore.Next(10, 20);
            string randomName = "";
            while (length > 1)
            {
                randomName += chars[randomCore.Next(0, chars.Length - 1)];
                length--;
            }
            return randomName;
        }
        #region Administrator part
        public static List<UserModel> LoadAdmin()
        {
            List<UserModel> entries = new List<UserModel>();
            using (SqliteConnection admindb = new SqliteConnection($"Filename={admindbpath}"))
            {
                admindb.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT Number,`User Id`,Password,Name,Address,Email,`Birth day`,`Sex (Male)`,TypeAdmin from User", admindb);


                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(new UserModel()
                    {
                        Number = query.GetInt32(0),
                        userId = query.GetString(1),
                        password = query.GetString(2),
                        name = query.GetString(3),
                        address = query.GetString(4),
                        email = query.GetString(5),
                        birthday = query.GetDateTime(6),
                        sex = query.GetBoolean(7),
                        typeAdmin = query.GetBoolean(8)
                    });
                }

                admindb.Close();
                return entries.ToList();
            }
        }
        public static void AddUserTable(UserModel user)
        {
            using (SqliteConnection admindb = new SqliteConnection($"Filename={admindbpath}"))
            {
                admindb.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = admindb;

                insertCommand.CommandText = "INSERT INTO User VALUES (NULL,@userId,@Password,@Name,@Address,@Email,@Birthday,@Sex,@TypeAdmin);";
                insertCommand.Parameters.AddWithValue("@userId", user.userId);
                insertCommand.Parameters.AddWithValue("@Password", user.password);
                insertCommand.Parameters.AddWithValue("@Name", user.name);
                insertCommand.Parameters.AddWithValue("@Address", user.address);
                insertCommand.Parameters.AddWithValue("@Email", user.email);
                insertCommand.Parameters.AddWithValue("@Birthday", user.birthday);
                insertCommand.Parameters.AddWithValue("@Sex", user.sex);
                insertCommand.Parameters.AddWithValue("@TypeAdmin", user.typeAdmin);

                insertCommand.ExecuteReader();

                admindb.Close();
            }
        }
        public static void AddUserTable(string UserId, string Password, string Name, string Address, string Email, DateTime Birthday, bool Sex, bool TypeAdmin)
        {
            AddUserTable(new UserModel()
            {
                userId = UserId,
                password = Password,
                name = Name,
                address = Address,
                email = Email,
                birthday = Birthday,
                sex = Sex,
                typeAdmin = TypeAdmin
            });
        }
        public static void UpdateUser(UserModel user) //แก้ไขข้อมูลตาราง SQL
        {
            using (SqliteConnection admindb = new SqliteConnection($"Filename={admindbpath}"))
            {
                admindb.Open();
                SqliteCommand sqliteCommand = new SqliteCommand();
                sqliteCommand.Connection = admindb;
                //SQLite Command
                sqliteCommand.CommandText = $"UPDATE User SET " +
                    $"`User ID` = \"{user.userId}\" ," +
                    $"`Password` = \"{user.password}\" ," +
                    $"`Name` = \"{user.name}\" ," +
                    $"`Address` = \"{user.address}\" ," +
                    $"`Email` = \"{user.email}\" ," +
                    $"`Birth day` = \"{user.birthday}\" ," +
                    $"`Sex (Male)` = {user.sex} ," +
                    $"`TypeAdmin` = {user.typeAdmin} " +
                    $"WHERE Number = {user.Number};";


                sqliteCommand.ExecuteReader();
                admindb.Close();
            }
        }
        public static void UpdateUser(string UserId, string Password, string Name, string Address, string Email, DateTime Birthday, bool Sex, bool TypeAdmin)
        {
            UpdateUser(new UserModel()
            {
                userId = UserId,
                password = Password,
                name = Name,
                address = Address,
                email = Email,
                birthday = Birthday,
                sex = Sex,
                typeAdmin = TypeAdmin
            });
        }
        public static void RemoveUser(UserModel user)
        {
            using (SqliteConnection admindb = new SqliteConnection($"Filename={admindbpath}"))
            {
                admindb.Open();
                SqliteCommand sqliteCommand = new SqliteCommand();
                sqliteCommand.Connection = admindb;

                sqliteCommand.CommandText = $"DELETE FROM User WHERE Number = \"{user.Number}\";";
                sqliteCommand.ExecuteReader();

                admindb.Close();
            }
        }
        public static void RemoveUser(int number)
        {
            RemoveUser(new UserModel()
            {
                Number = number
            }) ;
        }
        public static List<UserModel>  SearchPeople(string search) //TODO : ทำระบบค้นหา
        {
            List<UserModel> entries = new List<UserModel>();
            using (SqliteConnection admindb = new SqliteConnection($"Filename={admindbpath}"))
            {
                admindb.Open();
                SqliteCommand sqliteCommand = new SqliteCommand();
                sqliteCommand.Connection = admindb;
                sqliteCommand.CommandText = $"SELECT Number,`User Id`,Password,Name,Address,Email,`Birth day`,`Sex (Male)`,TypeAdmin FROM User WHERE Name LIKE \"%{search.ToLower()}%\" OR `User ID` LIKE  \"%{search.ToLower()}%\";"; //TODO : แก้ code ตรงนี้ซะ
                SqliteDataReader query = sqliteCommand.ExecuteReader();
                while (query.Read()) 
                {
                    entries.Add(new UserModel()
                    {
                        Number = query.GetInt32(0),
                        userId = query.GetString(1),
                        password = query.GetString(2),
                        name = query.GetString(3),
                        address = query.GetString(4),
                        email = query.GetString(5),
                        birthday = query.GetDateTime(6),
                        sex = query.GetBoolean(7),
                        typeAdmin = query.GetBoolean(8)
                    });
                }
                admindb.Close();
                return entries;
            }
        }
        #endregion
        #region Client part
        public static void AddDataCustomerTable(CustomerModel customer)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "INSERT INTO CustomersTable VALUES (NULL,@Name,@Address,@Email,@Birthday,@Sex,@`Phone Number`);";
                insertCommand.Parameters.AddWithValue("@Name", customer.name);
                insertCommand.Parameters.AddWithValue("@Address", customer.address);
                insertCommand.Parameters.AddWithValue("@Email", customer.email);
                insertCommand.Parameters.AddWithValue("@Birthday", customer.birthday);
                insertCommand.Parameters.AddWithValue("@sex", customer.sex);
                insertCommand.Parameters.AddWithValue("@`Phone Number`", customer.phone);

                insertCommand.ExecuteReader();

                db.Close();
            }
        }
        public static void AddDataBookTable(string isbn, string title, string description, string price)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "INSERT INTO BookTable VALUES(@isbn,@title,@description,@price);";
                insertCommand.Parameters.AddWithValue("@isbn", isbn);
                insertCommand.Parameters.AddWithValue("@title", title);
                insertCommand.Parameters.AddWithValue("@description", description);
                insertCommand.Parameters.AddWithValue("@price", price);
                insertCommand.ExecuteReader();

                db.Close();
            }
        }
        public static void AddDataTransactions(string isbn, string customersID, string quatity, string totalPrice)
        {

            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "INSERT INTO BookTable VALUES(@isbn,`@Customers ID`,@quatity,`@Total Price`);";
                insertCommand.Parameters.AddWithValue("@isbn", isbn);
                insertCommand.Parameters.AddWithValue("@title", customersID);
                insertCommand.Parameters.AddWithValue("@description", quatity);
                insertCommand.Parameters.AddWithValue("@price", totalPrice);
                insertCommand.ExecuteReader();

                db.Close();
            }
        }
        public static List<String> GetData() //คือการแสดงผลข้อมูลที่อยู่ใน dataBase ออกมาทั้งหมด
        {
            List<String> entries = new List<string>();

            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT Name,Address,Email from CustomersTable", db); //หลัง SELECT คือใส่ชื่อ field หากมีหลาย field ข้องใส่ (,) เอาไว้ด้วย แต่ถ้าอยากได้ทั้งหมดเลยก็ใส่ (*)


                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add($"{query.GetString(0)} {query.GetString(1)} ({query.GetString(2)})"); //มันใช้งานเหมือน ArrayList คือกำหนดให้ส่งค่ากลับออกมาเป็นแต่ละ column (field) โดยเรียงตามที่เราเรียง Opject [0][1][2]
                }

                db.Close();
            }

            return entries;
        }
        public static Dictionary<string, string> GetUsernames() //คือการแสดงผลข้อมูลที่อยู่ใน dataBase ออกมาทั้งหมด
        {
            Dictionary<string, string> entries = new Dictionary<string, string>();

            using (SqliteConnection admindb = new SqliteConnection($"Filename={admindbpath}"))
            {
                admindb.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT `User Id`,Password from User", admindb); //หลัง SELECT คือใส่ชื่อ field หากมีหลาย field ข้องใส่ (,) เอาไว้ด้วย แต่ถ้าอยากได้ทั้งหมดเลยก็ใส่ (*)


                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(query.GetString(0), query.GetString(1));
                }

                admindb.Close();
            }

            return entries;
        }
        public static bool Typeuser(string userid, string password)
        {
            bool IsAdmin = false;

            using (SqliteConnection admindb = new SqliteConnection($"Filename={admindbpath}"))
            {
                admindb.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT `User Id`,Password,TypeAdmin from User " +
                    $"WHERE `User Id`=\"{userid}\"; WHERE Password=\"{password}\";", admindb); //หลัง SELECT คือใส่ชื่อ field หากมีหลาย field ข้องใส่ (,) เอาไว้ด้วย แต่ถ้าอยากได้ทั้งหมดเลยก็ใส่ (*)



                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    IsAdmin = query.GetBoolean(2);
                }

                admindb.Close();
            }

            return IsAdmin;
        }
        #endregion
    }
}
