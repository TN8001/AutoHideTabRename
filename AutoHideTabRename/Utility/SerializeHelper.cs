using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using static System.IO.Path;

namespace AutoHideTabRename.Utility
{
    internal static class SerializeHelper
    {
        public static T Load<T>(string path = null) where T : new()
        {
            if(path.IsNullOrEmpty()) path = GetDefaultPath();

            using(var xr = XmlReader.Create(path))
                return (T)new DataContractSerializer(typeof(T)).ReadObject(xr);
        }

        public static T LoadOrDefault<T>(string path = null) where T : new()
        {
            try
            {
                return Load<T>(path);
            }
            catch
            {
                Debug.WriteLine("fail Deserialize");
                return new T();
            }
        }

        public static void Save<T>(T obj, string path = null) where T : new()
        {
            if(path.IsNullOrEmpty()) path = GetDefaultPath();

            Directory.CreateDirectory(GetDirectoryName(path));

            var settings = new XmlWriterSettings { Indent = true };
            using(var xw = XmlWriter.Create(path, settings))
                new DataContractSerializer(typeof(T)).WriteObject(xw, obj);
        }

        private static string GetDefaultPath()
            => Combine(GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.xml");
    }
}
