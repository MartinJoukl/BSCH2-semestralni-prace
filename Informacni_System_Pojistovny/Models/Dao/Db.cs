namespace Informacni_System_Pojistovny.Models.Dao
{
    using Microsoft.AspNetCore.Http;
    using Oracle.ManagedDataAccess.Client;
    using System;

    namespace BlogPostApi
    {
        public class Db : IDisposable
        {

            string connString = " Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = fei-sql1.upceucebny.cz)(PORT = 1521))) " +
                "(CONNECT_DATA = (SERVER = DEDICATED)(SID = IDAS))); User Id = st64135;Password=xxx;";
            public OracleConnection Connection { get; }
            public Db()
            {
                Connection = new OracleConnection(connString);
            }

            public OracleDataReader executeRetrievingCommand(string command, Dictionary<string, object> parameters = null)
            {
                parameters = parameters ?? new Dictionary<string, object>();
                OracleCommand oracleCommand = new OracleCommand(command, Connection);
                foreach (var paramKey in parameters.Keys)
                {
                    oracleCommand.Parameters.Add(paramKey, parameters[paramKey]);
                }
                if (parameters.Count > 0)
                {
                    oracleCommand.Prepare();
                }

                Connection.Open();
                return oracleCommand.ExecuteReader();
            }

            public void Dispose() => Connection.Dispose();
        }
    }
}
