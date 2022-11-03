namespace Informacni_System_Pojistovny.Models.Dao
{
    using Oracle.ManagedDataAccess.Client;
    using System;

    namespace BlogPostApi
    {
        public class Db : IDisposable
        {

            string connString = " Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = fei-sql1.upceucebny.cz)(PORT = 1521))) " +
                "(CONNECT_DATA = (SERVER = DEDICATED)(SID = IDAS))); User Id = st64134;Password=jouklj;";
            public OracleConnection Connection { get; set; }
            public Db()
            {
                Connection = new OracleConnection(connString);
            }

            public void Dispose() => Connection.Dispose();
        }
    }
}
