namespace Informacni_System_Pojistovny.Models.Dao
{
    using Oracle.ManagedDataAccess.Client;
    using System;

    namespace BlogPostApi
    {
        public class Db : IDisposable
        {
            string connString = "DATA SOURCE=fei-sql1.upceucebny.cz:1521/IDAS;" +
            "PERSIST SECURITY INFO=True;" +
            "USER ID=xxx; " +
            "password=xxx; " +
            "Pooling = False;" +
            "Connection Timeout=120;";
            public OracleConnection Connection { get; }

            public Db()
            {
                OracleConnection conn = new OracleConnection(connString);
                conn.Open();
                Console.WriteLine(conn.State);
                conn.Dispose();
            }

            public void Dispose() => Connection.Dispose();
        }
    }
}
