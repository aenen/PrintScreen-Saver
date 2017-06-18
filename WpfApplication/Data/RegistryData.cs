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
    /// <summary>
    /// Данні з реєстру (налаштування/кастомізація)
    /// </summary>
    public class RegistryData
    {
        public static event EventHandler FavColorChanged;

        static private RegistryKey regKey = Registry.CurrentUser.CreateSubKey("Software\\PrintScreen Saver", RegistryKeyPermissionCheck.ReadWriteSubTree);
        static private RegistryKey regApp = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        
        /// <summary>
        /// Дефолтна папка для збереження скріншотів
        /// </summary>
        public static string SavePath
        {
            get { return Registry.GetValue(regKey.ToString(), "SavePath", null) != null? (string)regKey.GetValue("SavePath") : null; }
            set { regKey.SetValue("SavePath", value); }
        }

        /// <summary>
        /// Улюблений колір (використовуєтся в UI)
        /// </summary>
        public static Color FavColor
        {
            //вкласти всю логіку в 1 рядок коду - безцінно
            get { return Registry.GetValue(regKey.ToString(), "FavColor", null) != null ? (Color)ColorConverter.ConvertFromString(regKey.GetValue("FavColor").ToString()) : (Color)ColorConverter.ConvertFromString(SetValue("FavColor", Colors.RoyalBlue).ToString()); }
            set
            {
                regKey.SetValue("FavColor", value);
                FavColorChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        #region SolidColorBrush FavColor
        public static SolidColorBrush FavColorSB
        {
            get { return new SolidColorBrush(FavColor); }
            set { FavColor = value.Color; }
        }
        #endregion

        /// <summary>
        /// Мінімізація програми в трей при закритті чи згортуванні
        /// </summary>
        public static bool MinimizeToTray
        {
            get { return Registry.GetValue(regKey.ToString(), "MinimizeToTray", null) != null ? Convert.ToBoolean(regKey.GetValue("MinimizeToTray")) : (bool)SetValue("MinimizeToTray", true); }
            set { regKey.SetValue("MinimizeToTray", value); }
        }

        /// <summary>
        /// Папка, в якії був збережений скріншот автоматично стає стандартною
        /// </summary>
        public static bool LatestSavePathAsDefault
        {
            get { return Registry.GetValue(regKey.ToString(), "LatestSavePathAsDefault", null) != null ? Convert.ToBoolean(regKey.GetValue("LatestSavePathAsDefault")) : (bool)SetValue("LatestSavePathAsDefault", true); }
            set { regKey.SetValue("LatestSavePathAsDefault", value); }
        }

        /// <summary>
        /// Сповіщення при виявлені в буфері нового зображення
        /// </summary>
        public static bool TrayNotification
        {
            get { return Registry.GetValue(regKey.ToString(), "TrayNotification", null) != null ? Convert.ToBoolean(regKey.GetValue("TrayNotification")) : (bool)SetValue("TrayNotification", false); }
            set { regKey.SetValue("TrayNotification", value); }
        }

        /// <summary>
        /// Підтвердження для збереження зображення після кліку на сповіщення
        /// </summary>
        public static bool ConfirmSave
        {
            get { return Registry.GetValue(regKey.ToString(), "ConfirmSave", null) != null ? Convert.ToBoolean(regKey.GetValue("ConfirmSave")) : (bool)SetValue("ConfirmSave", true); }
            set { regKey.SetValue("ConfirmSave", value); }
        }

        /// <summary>
        /// Автоматично створювати ім'я файлу формату "дата-час"
        /// </summary>
        public static bool AutoGenerateName
        {
            get { return Registry.GetValue(regKey.ToString(), "AutoGenerateName", null) != null ? Convert.ToBoolean(regKey.GetValue("AutoGenerateName")) : (bool)SetValue("AutoGenerateName", true); }
            set { regKey.SetValue("AutoGenerateName", value); }
        }

        /// <summary>
        /// Запустакти програму при старті Windows
        /// </summary>
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

        /// <summary>
        /// Відкривати "Провідник" після збереження скріну
        /// </summary>
        public static bool ShowExplorer
        {
            get { return Registry.GetValue(regKey.ToString(), "ShowExplorer", null) != null ? Convert.ToBoolean(regKey.GetValue("ShowExplorer")) : (bool)SetValue("ShowExplorer", true); }
            set { regKey.SetValue("ShowExplorer", value); }
        }

        private static object SetValue(string name, object value)
        {
            regKey.SetValue(name, value);
            return value;
        }

        /// <summary>
        /// Скидання налаштувань
        /// </summary>
        public static void DefaultSettings()
        {
            SavePath = "";
            FavColorSB = Colors.RoyalBlue.ToSolidColorBrush();
            MinimizeToTray = true;
            TrayNotification = true;
            AutoGenerateName = true;
            LatestSavePathAsDefault = true;
            Autorun = false;
            ConfirmSave = true;
            ShowExplorer = true;
        }
    }
}
