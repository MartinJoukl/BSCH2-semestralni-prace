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
            "USER ID=st64134;" +
            "password=jouklj;" +
            "Pooling = False;" +
            "Connection Timeout=5;";

            string connString2 = " Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = fei-sql1.upceucebny.cz)(PORT = 1521))) " +
                "(CONNECT_DATA = (SERVER = DEDICATED)(SID = IDAS))); User Id = st64134;Password=jouklj;";
            public OracleConnection Connection { get; }
            public Db()
            {
                OracleConnection conn = new OracleConnection(connString2);
                conn.Open();
                Console.WriteLine(conn.State);
                conn.Dispose();
            }

            public void Dispose() => Connection.Dispose();
        }
    }
}
