using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Configuration;
using System.Data;
using System.Windows;


namespace bookStoreApp
{
    public class SqliteDataAccess
    {
        public const string dbpath = "Database_BookStore.db";
        public const string admindbpath = "adminDatabase.db";
        public static List<UserModel>  LoadPeople()
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
                    }) ; 
                }

                admindb.Close();
                return entries.ToList();
            }

            

        }
        //public static void SavePerson(UserModel person) //เพิ่มข้อมูลตาราง SQL
        //{
        //    using (IDbConnection cnn = new SqliteConnection(LoadConnectionString()))
        //    {
        //        cnn.Execute("INSERT into Person (FirstName , LastName) VALUES(@FirstName , @LastName)", person);
        //    }
        //}
        private static string LoadConnectionString(string id = "admindbpath") //โหลด SQL ไฟล์ไหนๆมาใช้ ในที่นี้เลือกโหลดไฟล์ที่ชื่อว่า Default (ลองไปหาความหมายของคำว่า Default ได้ที่หน้า App.config
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
        //public static void UpdatePerson(UserModel person) //แก้ไขข้อมูลตาราง SQL
        //{
        //    using (IDbConnection cnn = new SqliteConnection(LoadConnectionString()))
        //    {
        //        cnn.Execute($"UPDATE Person SET FirstName = \"{person.FirstName}\",LastName = \"{person.LastName}\" WHERE Id = {person.Id} ", person);
        //    }
        //}
        //public static void DeletePerson(UserModel person) //ลบออกจาก SQL
        //{
        //    using (IDbConnection cnn = new SqliteConnection(LoadConnectionString()))
        //    {
        //        cnn.Execute($"DELETE FROM Person WHERE Id = {person.Id} ", person);
        //    }
        //}
        //public static List<UserModel> SearchPeople(string search) //เอาไว้ค้นหา
        //{
        //    using (IDbConnection cnn = new SqliteConnection(LoadConnectionString()))
        //    {
        //        var output = cnn.query<UserModel>($"SELECT Id , FirstName , LastName FROM Person WHERE FirstName LIKE \"%{search.ToLower()}%\" OR LastName LIKE  \"%{search.ToLower()}%\"", new DynamicParameters()); // ตัว % ก็เหมือนกับ * เวลาเราไม่รู้ว่าต้องค้นหาคำนำหน้านั้นว่าอะไร *.exe บนค้นหาวินโด้ ก็มาเป็น %.exe แทนใน SQL // LIKE เป็นคำสั่งค้นหาในสิ่งที่ดูเหมือนกัน // .ToLower() คือแปลงตัวอักษรให้เป็นตัวพิมพ์เล็กเพื่อให้การค้นหาไม่ใส่ใจกับพิมพ์เล็กพิมพ์ใหญ่
        //        return output.ToList();
        //    }
        //}
    }
}
