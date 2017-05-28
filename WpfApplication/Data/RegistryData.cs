﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication.Data
{
    public static class RegistryData
    {
        static private RegistryKey regKey = Registry.CurrentUser;

        static RegistryData()
        {
            regKey = regKey.CreateSubKey("Software\\PrintScreen Saver", RegistryKeyPermissionCheck.ReadWriteSubTree);
        }

        public static string SavePath
        {
            get { return Registry.GetValue(regKey.ToString(), "SavePath", null) != null? (string)regKey.GetValue("SavePath") : null; }
            set { regKey.SetValue("SavePath", value); }
        }

        public static Color FavColor
        {
            get { return Registry.GetValue(regKey.ToString(), "FavColor", null) != null ? (Color)ColorConverter.ConvertFromString(regKey.GetValue("FavColor").ToString()) : (Color)ColorConverter.ConvertFromString(SetValue("FavColor", Colors.RoyalBlue).ToString()); }
            set { regKey.SetValue("FavColor", value); }
        }

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

        private static object SetValue(string name, object value)
        {
            regKey.SetValue(name, value);
            return value;
        }
    }
}