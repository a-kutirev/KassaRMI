using Microsoft.Win32;

namespace KassaLib
{
    public static class RegistryOptions
    {
        private static readonly RegistryKey currentUser = Registry.CurrentUser;
        private static readonly string path = @"SOFTWARE\HistoryParkDB_Kassa";

        #region Сеть

        private static string m_server = "";
        private static string m_port = "";
        private static string m_database = "";
        private static string m_user = "";
        private static string m_password = "";
        private static string m_showZero = "";

        public static string Server
        {
            get
            {
                if (m_server == "")
                {
                    RegistryKey rk_park;
                    if ((rk_park = currentUser.OpenSubKey(path, true)) == null)
                        rk_park = currentUser.CreateSubKey(path);
                    object v = rk_park.GetValue("Server");
                    if (v == null)
                    {
                        rk_park.SetValue("Server", "10.5.5.200");
                        m_server = "10.5.5.200";
                    }
                    else
                    {
                        m_server = v.ToString();
                    }
                }
                return m_server;
            }
            set
            {
                m_server = value;
                RegistryKey rk_park = currentUser.OpenSubKey(path, true);
                rk_park.SetValue("Server", m_server);
            }
        }
        public static string Database
        {
            get
            {
                if (m_database == "")
                {
                    RegistryKey rk_park;
                    if ((rk_park = currentUser.OpenSubKey(path, true)) == null)
                        rk_park = currentUser.CreateSubKey(path);
                    object v = rk_park.GetValue("Database");
                    if (v == null)
                    {
                        rk_park.SetValue("Database", "kassa");
                        m_database = "kassa";
                    }
                    else
                    {
                        m_database = v.ToString();
                    }
                }
                return m_database;
            }
            set
            {
                m_database = value;
                RegistryKey rk_park = currentUser.OpenSubKey(path, true);
                rk_park.SetValue("Database", m_database);
            }
        }
        public static string User
        {
            get
            {
                if (m_user == "")
                {
                    RegistryKey rk_park;
                    if ((rk_park = currentUser.OpenSubKey(path, true)) == null)
                        rk_park = currentUser.CreateSubKey(path);
                    object v = rk_park.GetValue("User");
                    if (v == null)
                    {
                        rk_park.SetValue("User", "administrator");
                        m_user = "administrator";
                    }
                    else
                    {
                        m_user = v.ToString();
                    }
                }
                return m_user;
            }
            set
            {
                m_user = value;
                RegistryKey rk_park = currentUser.OpenSubKey(path, true);
                rk_park.SetValue("User", m_user);
            }
        }
        public static string Password
        {
            get
            {
                if (m_password == "")
                {
                    RegistryKey rk_park;
                    if ((rk_park = currentUser.OpenSubKey(path, true)) == null)
                        rk_park = currentUser.CreateSubKey(path);
                    object v = rk_park.GetValue("Password");
                    if (v == null)
                    {
                        rk_park.SetValue("Password", "456Park()");
                        m_password = "456Park()";
                    }
                    else
                    {
                        m_password = v.ToString();
                    }
                }
                return m_password;
            }
            set
            {
                m_password = value;
                RegistryKey rk_park = currentUser.OpenSubKey(path, true);
                rk_park.SetValue("Password", m_password);
            }
        }
        public static string Port
        {
            get
            {
                if (m_port == "")
                {
                    RegistryKey rk_park;
                    if ((rk_park = currentUser.OpenSubKey(path, true)) == null)
                        rk_park = currentUser.CreateSubKey(path);
                    object v = rk_park.GetValue("Port");
                    if (v == null)
                    {
                        rk_park.SetValue("Port", "3306");
                        m_port = "3306";
                    }
                    else
                    {
                        m_port = v.ToString();
                    }
                }
                return m_port;
            }
            set
            {
                m_port = value;
                RegistryKey rk_park = currentUser.OpenSubKey(path, true);
                rk_park.SetValue("Password", m_port);
            }
        }
        public static string ShowZero
        {
            get
            {
                if (m_showZero == "")
                {
                    RegistryKey rk_park;
                    if ((rk_park = currentUser.OpenSubKey(path, true)) == null)
                        rk_park = currentUser.CreateSubKey(path);
                    object v = rk_park.GetValue("ShowZero");
                    if (v == null)
                    {
                        rk_park.SetValue("ShowZero", "true");
                        m_showZero = "true";
                    }
                    else
                    {
                        m_showZero = v.ToString();
                    }
                }
                return m_port;
            }
            set
            {
                m_showZero = value;
                RegistryKey rk_park = currentUser.OpenSubKey(path, true);
                rk_park.SetValue("ShowZero", m_showZero);
            }
        }

        #endregion

        #region Check & Create
        public static bool Check()
        {
            RegistryKey rk = currentUser.OpenSubKey(path);
            if (rk == null) return false;

            object rk5 = rk.GetValue("Server");
            if (rk5 == null) return false;
            object rk6 = rk.GetValue("Database");
            if (rk6 == null) return false;
            object rk7 = rk.GetValue("User");
            if (rk7 == null) return false;
            object rk8 = rk.GetValue("Password");
            if (rk8 == null) return false;
            object rk9 = rk.GetValue("Version");
            if (rk9 == null) return false;
            return true;
        }

        public static void CreateKey()
        {
            RegistryKey rk_park = currentUser.CreateSubKey(path);

            object rk5 = rk_park.GetValue("Server");
            if (rk5 == null) rk_park.SetValue("Server", "10.5.5.200");
            object rk6 = rk_park.GetValue("Database");
            if (rk6 == null) rk_park.SetValue("Database", "kassa");
            object rk7 = rk_park.GetValue("User");
            if (rk7 == null) rk_park.SetValue("User", "administrator");
            object rk8 = rk_park.GetValue("Password");
            if (rk8 == null) rk_park.SetValue("Password", "456Park()");
            object rk9 = rk_park.GetValue("Version");
            if (rk9 == null) rk_park.SetValue("Version", "1.0");
        }
        #endregion
    }
}
