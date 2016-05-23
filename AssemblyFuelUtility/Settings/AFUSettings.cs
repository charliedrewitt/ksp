using CDKspUtil.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AssemblyFuelUtility.Settings
{
    [Serializable]
    public class AFUSettings
    {
        public int WindowX { get; set; }
        public int WindowY { get; set; }

        public static AFUSettings Load()
        {
            var settingsPath = GetSaveFilePath();

            if (File.Exists(settingsPath))
            {
                var serializer = new XmlSerializer(typeof(AFUSettings));

                try
                {
                    using (var readStream  = File.OpenRead(settingsPath))
                    {
                        var settings = serializer.Deserialize(readStream) as AFUSettings;

                        if (settings != null)
                        {
                            return settings;
                        }
                    }
                }
                catch { }
            }

            //Load Here
            return new AFUSettings
            {
                WindowX = 100,
                WindowY = 100
            };
        }

        public static void Save(AFUSettings settings)
        {
            var settingsPath = GetSaveFilePath();

            LogHelper.Info("AFUSettings File: {0}", settingsPath);

            using (var writeStream = File.OpenWrite(settingsPath))
            {
                var serializer = new XmlSerializer(typeof(AFUSettings));

                serializer.Serialize(writeStream, settings);
            }
        }

        private static string GetSaveFilePath()
        {
            //LogHelper.Info("AFUSettings PluginDataFolder: {0}", GameDatabase.Instance.PluginDataFolder);
            //LogHelper.Info("AFUSettings EnvironmentInfo: {0}", GameDatabase.EnvironmentInfo); 

            return Path.Combine(GameDatabase.Instance.PluginDataFolder.Replace("/", "\\"), "GameData\\AssemblyFuelUtility\\settings.xml");
        }
    }
}
