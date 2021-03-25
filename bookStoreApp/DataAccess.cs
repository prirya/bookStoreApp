using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace bookStoreApp
{
    public class DataAccess
    {
        public const string timeformat = "dd-MM-yyyy";
        public const string dbpath = "Database_BookStore.db";
        public const string admindbpath = "adminDatabase.db";
        public static void Command(string filename, string command)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={filename}"))
            {
                db.Open();
                SqliteCommand sqliteCommand = new SqliteCommand();
                sqliteCommand.Connection = db;
                sqliteCommand.CommandText = command;
                sqliteCommand.ExecuteReader();
                db.Close();
            }
        }
        public static void InitializeDatabase()
        {
            Command(dbpath,
                    //ตารางเก็บข้อมูลลูกค้า
                    "CREATE TABLE IF NOT EXISTS CustomersTable (`Customer ID` INTEGER PRIMARY KEY, `Name` NVARCHAR(50) NULL, `Address` NVARCHAR(500) NULL, `Email` NVARCHAR(50) NULL, `Sex(Male)` BOOL NULL, Birthday NVARCHAR(50) NULL, `Phone Number` NVARCHAR(50) NULL);" +
                    //สร้างตารางหนังสือ
                    "CREATE TABLE IF NOT EXISTS BookTable (Number INTEGER PRIMARY KEY ,`ISBN` CHAR(10) NULL, Title NVARCHAR(200) NULL,Type NVARCHAR(500) NULL,Description NVARCHAR(500) NULL,Price DECIMAL NULL,Quantity INT NULL);" +
                    //ข้อมูลการขาย
                    "CREATE TABLE IF NOT EXISTS Transactions (BillNumber INTEGER PRIMARY KEY ,CustomersID INTEGER NULL,TimeSold DATETIME NULL,UserIDNumber INT NULL);" +
                    "CREATE TABLE IF NOT EXISTS Bill (BillNumber INTEGER NULL,ISBN CHAR(500) NULL,Quatity NVARCHAR(500) NULL);");
            Command(admindbpath, "CREATE TABLE IF NOT EXISTS User (`Number` INTEGER PRIMARY KEY, `User ID` NVARCHAR(100) NULL, `Password` NVARCHAR(100) NULL, `Name` NVARCHAR(50) NULL, `Address` NVARCHAR(500) NULL, `Email` NVARCHAR(50) NULL, `Birth day` NVARCHAR(50) NULL, `Sex (Male)` BOOL NULL, `TypeAdmin` BOOL NULL);");

#if DEBUG
            AddDebugData();
#endif
        }

        public static SqliteDataReader GetSqliterQuery(string filename, string command) //TODO : ยังไม่เวิร์ค ต้องมี List ลองรับ
        {
            using (SqliteConnection dp = new SqliteConnection($"Filename={filename}"))
            {
                dp.Open();
                SqliteCommand sqliteCommand = new SqliteCommand()
                {
                    Connection = dp,
                    CommandText = command
                };
                var query = sqliteCommand.ExecuteReader();
                dp.Close();

                return query;
            }
        }
        public static void AddDebugData()
        {
            var usernames = GetUsernames();
            if (usernames.ContainsKey("root"))
                return;
            //Admin test
            AddUserTable("root", "7777", "Administrator", "", "", new DateTime(1990, 5, 12), true, true);
            //User test
            //AddDataCustomerTable(GetRandomName(), "", "");
            //AddDataCustomerTable(GetRandomName(), "", "");
            //AddDataCustomerTable(GetRandomName(), "", "");
            //AddDataCustomerTable(GetRandomName(), "", "");
            //AddDataCustomerTable(GetRandomName(), "", "");
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
        public static DateTime Time(string time)
        {
            var split = time.Split('-');
            DateTime dday = new DateTime(int.Parse(split[2]), int.Parse(split[1]), int.Parse(split[0]));
            return dday;
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
                        birthday = Time(query.GetString(6)),
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
                insertCommand.Parameters.AddWithValue("@Birthday", user.birthday.ToString(timeformat));
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
            Command(admindbpath, $"UPDATE User SET " +
                    $"`User ID` = \"{user.userId}\" ," +
                    $"`Password` = \"{user.password}\" ," +
                    $"`Name` = \"{user.name}\" ," +
                    $"`Address` = \"{user.address}\" ," +
                    $"`Email` = \"{user.email}\" ," +
                    $"`Birth day` = \"{user.birthday.ToString(timeformat)}\" ," +
                    $"`Sex (Male)` = {user.sex} ," +
                    $"`TypeAdmin` = {user.typeAdmin} " +
                    $"WHERE Number = {user.Number};");
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
        public static List<UserModel> SearchPeople(string search)
        {
            List<UserModel> entries = new List<UserModel>();
            using (SqliteConnection admindb = new SqliteConnection($"Filename={admindbpath}"))
            {
                admindb.Open();
                SqliteCommand sqliteCommand = new SqliteCommand();
                sqliteCommand.Connection = admindb;
                sqliteCommand.CommandText = $"SELECT Number,`User Id`,Password,Name,Address,Email,`Birth day`,`Sex (Male)`,TypeAdmin FROM User WHERE Name LIKE \"%{search.ToLower()}%\" OR `User ID` LIKE  \"%{search.ToLower()}%\";";
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
                        birthday = Time(query.GetString(6)),
                        sex = query.GetBoolean(7),
                        typeAdmin = query.GetBoolean(8)
                    });
                }
                admindb.Close();
                return entries;
            }
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
        #endregion
        #region Customers part
        public static void AddDataCustomerTable(CustomerModel customer)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "INSERT INTO CustomersTable VALUES (NULL,@Name,@Address,@Email,@Sex,@Birthday,@PhoneNumber);";
                insertCommand.Parameters.AddWithValue("@Name", customer.Name);
                insertCommand.Parameters.AddWithValue("@Address", customer.Address);
                insertCommand.Parameters.AddWithValue("@Email", customer.Email);
                insertCommand.Parameters.AddWithValue("@Sex", customer.Sex);
                insertCommand.Parameters.AddWithValue("@Birthday", customer.Birthday.ToString(timeformat));
                insertCommand.Parameters.AddWithValue("@PhoneNumber", customer.Phone);

                insertCommand.ExecuteReader();

                db.Close();
            }
        }
        public static void UpdateCustomer(CustomerModel customer)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand sqliteCommand = new SqliteCommand();
                sqliteCommand.Connection = db;
                //SQLite Command
                sqliteCommand.CommandText = $"UPDATE CustomersTable SET " +
                    $"`Name` = \"{customer.Name}\" ," +
                    $"`Address` = \"{customer.Address}\" ," +
                    $"`Email` = \"{customer.Email}\" ," +
                    $"`Birthday` = \"{customer.Birthday.ToString(timeformat)}\" ," +
                    $"`Sex(Male)` = {customer.Sex} ," +
                    $"`Phone Number` = {customer.Phone} " +
                    $"WHERE `Customer ID` = {customer.CustomerId};";


                sqliteCommand.ExecuteReader();
                db.Close();
            }
        }
        public static void RemoveCustomers(CustomerModel customer)
        {
            Command(dbpath, $"DELETE FROM CustomersTable WHERE `Customer Id` = \"{customer.CustomerId}\";");
        }
        public static List<CustomerModel> GetDataUser() //คือการแสดงผลข้อมูลที่อยู่ใน dataBase ออกมาทั้งหมด
        {
            List<CustomerModel> entries = new List<CustomerModel>();
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT `Customer ID`,Name,Address,Email,`Sex(Male)`,Birthday,`Phone Number` from CustomersTable", db);


                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(new CustomerModel()
                    {
                        CustomerId = query.GetInt16(0),
                        Name = query.GetString(1),
                        Address = query.GetString(2),
                        Email = query.GetString(3),
                        Sex = query.GetBoolean(4),
                        Birthday = Time(query.GetString(5)),
                        Phone = query.GetString(6),
                    });
                }

                db.Close();
                return entries;
            }
        }
        public static List<CustomerModel> SearchCustomers(string search)
        {
            List<CustomerModel> entries = new List<CustomerModel>();
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand sqliteCommand = new SqliteCommand();
                sqliteCommand.Connection = db;
                sqliteCommand.CommandText = $"SELECT `Customer ID`,Name,Address,Email,`Sex(Male)`,Birthday,`Phone Number` FROM CustomersTable WHERE Name LIKE \"%{search.ToLower()}%\" OR `Phone Number` LIKE  \"%{search.ToLower()}%\";";
                SqliteDataReader query = sqliteCommand.ExecuteReader();
                while (query.Read())
                {
                    entries.Add(new CustomerModel()
                    {
                        CustomerId = query.GetInt16(0),
                        Name = query.GetString(1),
                        Address = query.GetString(2),
                        Email = query.GetString(3),
                        Sex = query.GetBoolean(4),
                        Birthday = Time(query.GetString(5)),
                        Phone = query.GetString(6),
                    });
                }
                db.Close();
                return entries;
            }
        }
        #endregion
        #region Book Data Base Zone
        public static List<BookModel> GetBookData()
        {
            List<BookModel> entries = new List<BookModel>();
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT Number,ISBN,Title,Type,Description,Price,Quantity FROM BookTable", db);
                SqliteDataReader query = selectCommand.ExecuteReader();
                while (query.Read())
                {
                    entries.Add(new BookModel()
                    {
                        Number = query.GetInt32(0),
                        ISBN = query.GetString(1),
                        Title = query.GetString(2),
                        Type = query.GetString(3),
                        Description = query.GetString(4),
                        Price = query.GetDecimal(5),
                        Quantity = query.GetInt32(6)
                    });
                }
                db.Close();
                return entries;
            }
        }
        public static void AddDataBookTable(string isbn, string title, string type, string description, decimal price, int quantity)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;
                insertCommand.CommandText = "INSERT INTO BookTable VALUES(NULL,@isbn,@title,@type,@description,@price,@quantity);";
                insertCommand.Parameters.AddWithValue("@isbn", isbn);
                insertCommand.Parameters.AddWithValue("@title", title);
                insertCommand.Parameters.AddWithValue("@type", type);
                insertCommand.Parameters.AddWithValue("@description", description);
                insertCommand.Parameters.AddWithValue("@price", price);
                insertCommand.Parameters.AddWithValue("@quantity", quantity);
                insertCommand.ExecuteReader();
                db.Close();
            }
        }
        public static void RemoveBookTable(BookModel book)
        {
            Command(dbpath, $"DELETE FROM BookTable WHERE Number = \"{book.Number}\";");
        }
        public static void SaveBook(BookModel book) 
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand sqliteCommand = new SqliteCommand();
                sqliteCommand.Connection = db;
                //SQLite Command
                sqliteCommand.CommandText = $"UPDATE BookTable SET " +
                    $"ISBN = \"{book.ISBN}\" ," +
                    $"Title = \"{book.Title}\" ," +
                    $"Type = \"{book.Type}\" ," +
                    $"Description = \"{book.Description}\" ," +
                    $"Price = {book.Price} ," +
                    $"Quantity = {book.Quantity} " +
                    $"WHERE Number = {book.Number};";
                sqliteCommand.ExecuteReader();
                db.Close();
            }
        }
        public static List<BookModel> SearchBooks(string search)
        {
            List<BookModel> entries = new List<BookModel>();
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand sqliteCommand = new SqliteCommand();
                sqliteCommand.Connection = db;
                sqliteCommand.CommandText = $"SELECT Number,ISBN,Type,Title,Description,Price,Quantity FROM BookTable WHERE Title LIKE \"%{search.ToLower()}%\" OR ISBN LIKE  \"%{search.ToLower()}%\";";
                SqliteDataReader query = sqliteCommand.ExecuteReader();
                while (query.Read())
                {
                    entries.Add(new BookModel()
                    {
                        Number = query.GetInt32(0),
                        ISBN = query.GetString(1),
                        Title = query.GetString(2),
                        Type = query.GetString(3),
                        Description = query.GetString(4),
                        Price = query.GetDecimal(5),
                        Quantity = query.GetInt32(6)
                    });
                }
                db.Close();
                return entries;
            }
        }
        #endregion
        #region Transactions Zone

        public static int AddDataTransactions(Bill Order)
        {
            
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;
                insertCommand.CommandText = "INSERT INTO Transactions VALUES(NULL,@CustomersID,@timeSold,@UserIDNumber);";
                insertCommand.Parameters.AddWithValue("@CustomersID", Order.CustomerID);
                insertCommand.Parameters.AddWithValue("@timeSold", Order.TimeSold);
                insertCommand.Parameters.AddWithValue("@UserIDNumber", Order.User);
                insertCommand.ExecuteReader();

                SqliteCommand selectCommand = new SqliteCommand();
                selectCommand.Connection = db;
                selectCommand.CommandText = "SELECT BillNumber FROM Transactions";
                SqliteDataReader query = selectCommand.ExecuteReader();
                int billNumber = 0;

                while (query.Read())
                {
                    billNumber = query.GetInt32(0);
                }
                db.Close();
                return billNumber;
            }
        }
        public static void AddBill(int billNumber ,List<BillDetail> bill)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                foreach (var book in bill)
                {
                    SqliteCommand insertBill = new SqliteCommand();
                    insertBill.Connection = db;
                    insertBill.CommandText = $"INSERT INTO Bill VALUES({billNumber},@ISBN,@Quatity);";
                    insertBill.Parameters.AddWithValue("@ISBN", book.ISBN);
                    insertBill.Parameters.AddWithValue("@Quatity", book.Quantity);
                    insertBill.ExecuteReader();
                }
                db.Close();
            }
            
        }
        public static List<BookModel> GetTransactions()
        {
            List<BookModel> entries = new List<BookModel>();
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT Number,ISBN,Title,Type,Description,Price,Quantity FROM BookTable", db);
                SqliteDataReader query = selectCommand.ExecuteReader();
                while (query.Read())
                {
                    entries.Add(new BookModel()
                    {
                        Number = query.GetInt32(0),
                        ISBN = query.GetString(1),
                        Title = query.GetString(2),
                        Type = query.GetString(3),
                        Description = query.GetString(4),
                        Price = query.GetDecimal(5),
                        Quantity = query.GetInt32(6)
                    });
                }
                db.Close();
                return entries;
            }
        }
        public static List<GetdataTransactions> GetDataBook(string isbn,int quantitySold)
        {
            List<GetdataTransactions> entries = new List<GetdataTransactions>();
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand sqliteCommand = new SqliteCommand();
                sqliteCommand.Connection = db;
                sqliteCommand.CommandText = $"SELECT ISBN,Title,Type,Price,Quantity,Number FROM BookTable WHERE ISBN LIKE  \"%{isbn}%\";";
                SqliteDataReader query = sqliteCommand.ExecuteReader();
                while (query.Read())
                {
                    entries.Add(new GetdataTransactions()
                    {
                        ISBN = query.GetString(0),
                        TitleBook = query.GetString(1),
                        Type = query.GetString(2),
                        Price = query.GetDecimal(3),
                        QuantitySold = quantitySold,
                        Quantity = query.GetInt32(4),
                        Number = query.GetInt32(5)
                    });
                }
                db.Close();
                return entries;
            }
        }
        public static void SellBook(List<GetdataTransactions> bookList)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                foreach (var book in bookList)
                {
                    var book2 = GetDataBook(book.ISBN, book.QuantitySold);
                    foreach (var book3 in book2)
                    {
                        SqliteCommand sqliteCommand = new SqliteCommand();
                        sqliteCommand.Connection = db;
                        sqliteCommand.CommandText = $"UPDATE BookTable SET " +
                        $"Quantity = {book3.Quantity - book.QuantitySold} " +
                        $"WHERE Number = {book3.Number};";
                        sqliteCommand.ExecuteReader();
                    }
                }
                db.Close();
            }
        }
        //TODO : GetLog Bill
        #endregion


    }
}
