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
    }
}
