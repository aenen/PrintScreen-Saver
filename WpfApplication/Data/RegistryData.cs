using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace WpfApplication.Data
{
    public class RegistryData
    {
        public static event EventHandler FavColorChanged;

        static private RegistryKey regKey = Registry.CurrentUser.CreateSubKey("Software\\PrintScreen Saver", RegistryKeyPermissionCheck.ReadWriteSubTree);
        static private RegistryKey regApp = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        
        public static string SavePath
        {
            get { return Registry.GetValue(regKey.ToString(), "SavePath", null) != null? (string)regKey.GetValue("SavePath") : null; }
            set { regKey.SetValue("SavePath", value); }
        }

        public static Color FavColor
        {
            //вкласти всю логіку в 1 рядок коду - безцінно
            get { return Registry.GetValue(regKey.ToString(), "FavColor", null) != null ? (Color)ColorConverter.ConvertFromString(regKey.GetValue("FavColor").ToString()) : (Color)ColorConverter.ConvertFromString(SetValue("FavColor", Colors.RoyalBlue).ToString()); }
            set
            {
                regKey.SetValue("FavColor", value);
                OnPropertyChanged();
            }
        }

        #region SolidColorBrush FavColor
        public static SolidColorBrush FavColorSB
        {
            get { return new SolidColorBrush(FavColor); }
            set { FavColor = value.Color; }
        }
        #endregion

        public static bool MinimizeToTray
        {
            get { return Registry.GetValue(regKey.ToString(), "MinimizeToTray", null) != null ? Convert.ToBoolean(regKey.GetValue("MinimizeToTray")) : (bool)SetValue("MinimizeToTray", true); }
            set { regKey.SetValue("MinimizeToTray", value); }
        }

        public static bool LatestSavePathAsDefault
        {
            get { return Registry.GetValue(regKey.ToString(), "LatestSavePathAsDefault", null) != null ? Convert.ToBoolean(regKey.GetValue("LatestSavePathAsDefault")) : (bool)SetValue("LatestSavePathAsDefault", true); }
            set { regKey.SetValue("LatestSavePathAsDefault", value); }
        }

        public static bool TrayNotification
        {
            get { return Registry.GetValue(regKey.ToString(), "TrayNotification", null) != null ? Convert.ToBoolean(regKey.GetValue("TrayNotification")) : (bool)SetValue("TrayNotification", false); }
            set { regKey.SetValue("TrayNotification", value); }
        }

        public static bool AutoGenerateName
        {
            get { return Registry.GetValue(regKey.ToString(), "AutoGenerateName", null) != null ? Convert.ToBoolean(regKey.GetValue("AutoGenerateName")) : (bool)SetValue("AutoGenerateName", true); }
            set { regKey.SetValue("AutoGenerateName", value); }
        }

        public static bool Autorun
        {
            get { return Registry.GetValue(regApp.ToString(), "PrintScreenSaver", null) != null ? true : false; }
            set
            {
                if (value)
                    regApp.SetValue("PrintScreenSaver", Assembly.GetExecutingAssembly().Location);
                else
                    regApp.DeleteValue("PrintScreenSaver", false);
            }
        }

        private static object SetValue(string name, object value)
        {
            regKey.SetValue(name, value);
            return value;
        }
        private static void OnPropertyChanged()
        {
            if (FavColorChanged != null)
            {
                FavColorChanged(null, EventArgs.Empty);
            }
        }

        public static void DefaultSettings()
        {
            SavePath = "";
            FavColor = Colors.RoyalBlue;
            MinimizeToTray = true;
            TrayNotification = false;
            AutoGenerateName = true;
            LatestSavePathAsDefault = true;
            Autorun = false;
        }
    }
}
