using MySql.Data.MySqlClient;
using System.Data;

namespace KassaLib
{
    public static class DBWrapper
    {
        private static MySqlConnection connection = null;
        public static string connectionString;

        private static MySqlConnection GetConnection()
        {
            if (connection == null)
            {
                string ip = RegistryOptions.Server;
                string port = RegistryOptions.Port;
                string db = RegistryOptions.Database;
                string user = RegistryOptions.User;
                string pass = RegistryOptions.Password;

                connectionString = $"SERVER={ip}; PORT={port}; DATABASE={db};UID={user};PASSWORD={pass};convert zero datetime=True";

                connection = new MySqlConnection(connectionString);
            }
            if (connection.State == ConnectionState.Closed) connection.Open();
            return connection;
        }

        public static int Execute(string sql)
        {
            MySqlConnection cc = GetConnection();
            MySqlCommand com = new MySqlCommand(sql, cc);
            com.ExecuteNonQuery();

            return (int)com.LastInsertedId;
        }

        public static DataTable Select(string sql)
        {
            DataTable dataTabe = new DataTable();
            try
            {
                MySqlConnection cc = GetConnection();
                MySqlCommand com = new MySqlCommand(sql, cc);
                com.ExecuteNonQuery();

                MySqlDataAdapter sqlAdapter = new MySqlDataAdapter(com);

                sqlAdapter.Fill(dataTabe);
                sqlAdapter.Update(dataTabe);
            }
            catch
            {
                return null;
            }

            return dataTabe;
        }
    }
}
