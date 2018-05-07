using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Foundation;
using SQLite;
using UIKit;
using Xamarin.Forms;
using Conapesca_Manager.iOS;

[assembly: Dependency(typeof(IOS_SQLite))]

namespace Conapesca_Manager.iOS
{
    public class IOS_SQLite : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var dbName = "Member.sqlite";
            string dbPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(dbPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, dbName);
            var conn = new SQLite.SQLiteConnection(path);
            return conn;
        }
    }
}