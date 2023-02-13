using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace _93000.FileTranscoding.Base.FWCL.Help
{
    /// <summary>
    ///     序列化助手
    /// </summary>
    public static class SerializeHelper
    {
        #region XML序列化

        /// <summary>
        ///     把对象序列化为xml
        /// </summary>
        /// <param name="item">对象</param>
        public static string ToXmlString(this object item)
        {
            var serializer = new XmlSerializer(item.GetType());
            var sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb))
            {
                serializer.Serialize(writer, item);
                return sb.ToString();
            }
        }

        /// <summary>
        ///     把XML序列化后的字符串反序列化为对象
        /// </summary>
        /// <param name="str">字符串序列</param>
        public static T ToObjectFromXml<T>(this string str)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new XmlTextReader(new StringReader(str)))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        ///     将对象序列化为xml并保存到文件
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static void ToXmlFile(this object obj, string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                var serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        /// <summary>
        ///     文件化XML反序列化
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        public static object Load(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        #endregion

        #region Json序列化

        /// <summary>
        ///     JsonSerializer序列化
        /// </summary>
        /// <param name="item">对象</param>
        public static string ToJsonString(this object item)
        {
            return Newtonsoft.Json.JavaScriptConvert.SerializeObject(item);
        }

        /// <summary>
        ///     JsonSerializer反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public static T ToObjectFromJson<T>(this string str) where T : class
        {
            return Newtonsoft.Json.JavaScriptConvert.DeserializeObject<T>(str);
        }

        #endregion

        #region SoapFormatter序列化

        /// <summary>
        ///     SoapFormatter序列化
        /// </summary>
        /// <param name="item">对象</param>
        public static string ToSoapString<T>(this T item)
        {
            var formatter = new SoapFormatter();
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, item);
                ms.Position = 0;
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(ms);
                return xmlDoc.InnerXml;
            }
        }

        /// <summary>
        ///     SoapFormatter反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public static T ToObjectFromSoap<T>(this string str)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);
            var formatter = new SoapFormatter();
            using (var ms = new MemoryStream())
            {
                xmlDoc.Save(ms);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        #endregion

        #region BinaryFormatter序列化

        /// <summary>
        ///     BinaryFormatter序列化
        /// </summary>
        /// <param name="item">对象</param>
        public static string ToBinaryString<T>(this T item)
        {
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, item);
                ms.Position = 0;
                byte[] bytes = ms.ToArray();
                var sb = new StringBuilder();
                foreach (byte bt in bytes)
                {
                    sb.Append(string.Format("{0:X2}", bt));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        ///     BinaryFormatter反序列化
        /// </summary>
        /// <param name="str">字符串序列</param>
        public static T ToObjectFromBinary<T>(this string str)
        {
            int intLen = str.Length / 2;
            var bytes = new byte[intLen];
            for (int i = 0; i < intLen; i++)
            {
                int ibyte = Convert.ToInt32(str.Substring(i * 2, 2), 16);
                bytes[i] = (byte)ibyte;
            }
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream(bytes))
            {
                return (T)formatter.Deserialize(ms);
            }
        }

        #endregion
    }
}