using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _93000.FileTranscoding.Base.FWCL.Help
{
    public class EncodingHelper
    {
        public static Encoding GetEncode(byte[] buffer)
        {
            if (buffer.Length <= 0 || buffer[0] < 239)
                return Encoding.Default;
            if (buffer[0] == 239 && buffer[1] == 187 && buffer[2] == 191)
                return Encoding.UTF8;
            if (buffer[0] == 254 && buffer[1] == byte.MaxValue)
                return Encoding.BigEndianUnicode;
            if (buffer[0] == byte.MaxValue && buffer[1] == 254)
                return Encoding.Unicode;
            return Encoding.Default;
        }

        public static string GetTxtByByArrayAndEncode(byte[] buffer, Encoding encoding)
        {
            if (Equals(encoding, Encoding.UTF8))
                return encoding.GetString(buffer, 3, buffer.Length - 3);
            if (Equals(encoding, Encoding.BigEndianUnicode) || Equals(encoding, Encoding.Unicode))
                return encoding.GetString(buffer, 2, buffer.Length - 2);
            return encoding.GetString(buffer);
        }

        /// <summary>
        /// 通过文件获取编码
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Tuple<Encoding, string> GetEncodingByFile(string path)
        {
            var sw = new Stopwatch();
            sw.Start();
            FileStream fileStream = File.OpenRead(path);
            var buffer = new byte[fileStream.Length];
            //从流中读取字节块并将该数据写入给定缓冲区中。
            fileStream.Read(buffer, 0, buffer.Length);
            fileStream.Close();
            fileStream.Dispose();
            Encoding encode = EncodingHelper.GetEncode(buffer);
            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            return Tuple.Create<Encoding, string>(encode, elapsedTime);
        }
    }
}
