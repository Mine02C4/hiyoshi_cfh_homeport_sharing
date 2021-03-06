﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Grabacr07.KanColleWrapper;

namespace HiyoshiCfhClient.Models
{
    [Serializable]
    public class Settings : Notifier
    {
        private static string path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "HiyoshiCfh",
            "HiyoshiCfhClient",
            Properties.Settings.Default.SettingFile);
        public static Settings Current { get; set; }

        public static void Init()
        {
            if (path == null || !File.Exists(path))
            {
                Current = new Settings();
            }
            else
            {
                StreamReader stream = null;
                try
                {
                    stream = new StreamReader(path, new UTF8Encoding(false));
                    var serializer = new XmlSerializer(typeof(Settings));
                    Current = (Settings)serializer.Deserialize(stream);
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                }
            }
        }

        #region AccessToken変更通知プロパティ
        private string _AccessToken;

        public string AccessToken
        {
            get
            { return _AccessToken; }
            set
            {
                if (_AccessToken == value)
                    return;
                _AccessToken = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region TokenType変更通知プロパティ
        private string _TokenType;

        public string TokenType
        {
            get
            { return _TokenType; }
            set
            {
                if (_TokenType == value)
                    return;
                _TokenType = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public void Save()
        {
            lock (Current)
            {
                StreamWriter stream = null;
                try
                {
                    var dir = Path.GetDirectoryName(Path.GetFullPath(path)) ?? "";
                    Directory.CreateDirectory(dir);
                    stream = new StreamWriter(path, false, new UTF8Encoding(false));
                    var serializer = new XmlSerializer(typeof(Settings));
                    serializer.Serialize(stream, this);
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                }
            }
        }
    }
}
