using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WooDispatch.Common
{
    public class SqliteHelper
    {

        public SqliteHelper() { }

        public readonly static string dbname = "sqliteScheduler";
        public readonly static string dbpath = $"db/{dbname}.db";
        public readonly static string dataSource = $"Data Source={dbpath}";

        public void  Exist()
        { 

            
        }

        public  void Create()
        {
            //如果不存在sqlite数据库，则创建
            if (!File.Exists(dbpath))
            {
                if (!Directory.Exists(Path.GetDirectoryName(dbpath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(dbpath));
                using (var connection = new SQLiteConnection(dataSource))
                {
                    connection.OpenAsync().Wait();
                    string sql = IncludeEmbeddedFile();// File.ReadAllText("db\\tables_sqlite.sql");
                    var command = new SQLiteCommand(sql, connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                    command.Dispose();
                }
            }
        }

        private  string IncludeEmbeddedFile()
        {



            var stream = Assembly.GetAssembly(type: GetType()).GetManifestResourceStream($"WooDispatch.db.tables_sqlite.sql");
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();


        }

    }
}
