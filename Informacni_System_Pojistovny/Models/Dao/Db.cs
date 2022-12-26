namespace Informacni_System_Pojistovny.Models.Dao
{
    using Microsoft.AspNetCore.Http;
    using Oracle.ManagedDataAccess.Client;


    public class Db : IDisposable
    {

        string connString = "Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = fei-sql3.upceucebny.cz)(PORT = 1521))) " +
            "(CONNECT_DATA = (SERVER = DEDICATED)(SID = BDAS))); User Id = st64134;Password=jouklj;";
        public OracleConnection Connection { get; private set; }
        public Db()
        {
            Connection = new OracleConnection(connString);
        }

        public OracleDataReader ExecuteRetrievingCommand(string command, Dictionary<string, object> parameters = null)
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

            //reset connection
            if (Connection != null && Connection.State != System.Data.ConnectionState.Open)
            {
                if (Connection.State == System.Data.ConnectionState.Closed)
                {
                    Connection = new OracleConnection(connString);
                    oracleCommand.Connection = Connection;
                }
                Connection.Open();
            }
            return oracleCommand.ExecuteReader();
        }

        public int ExecuteNonQuery(string command, Dictionary<string, object> parameters = null, bool returnsId = true, bool isProcedure = false)
        {
            parameters = parameters ?? new Dictionary<string, object>();
            OracleCommand oracleCommand = new OracleCommand(command, Connection);
            if (isProcedure)
            {
                oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
            }
            foreach (var paramKey in parameters.Keys)
            {
                oracleCommand.Parameters.Add(paramKey, parameters[paramKey].ToString());
            }
            if (parameters.Count > 0)
            {
                oracleCommand.Prepare();
            }

            if (Connection != null && Connection.State != System.Data.ConnectionState.Open)
            {
                if (Connection.State == System.Data.ConnectionState.Closed)
                {
                    Connection = new OracleConnection(connString);
                    oracleCommand.Connection = Connection;
                }
                Connection.Open();
            }

            if (returnsId)
            {
                if (isProcedure)
                {
                    oracleCommand.Parameters.Add("id", OracleDbType.Decimal, System.Data.ParameterDirection.Output);
                }
                else { 
                    oracleCommand.Parameters.Add("id", OracleDbType.Decimal, System.Data.ParameterDirection.ReturnValue); 
                }
            }
            oracleCommand.ExecuteNonQuery();
            if (returnsId)
            {
                return int.Parse(oracleCommand.Parameters["id"].Value.ToString());
            }
            else
            {
                return -1;
            }
        }

        public void Dispose() => Connection.Dispose();
    }
}