using System;
using System.IO;
using System.Threading;

namespace _93000.FileTranscoding.Base.FWCL.Help
{
    /// <summary>
    ///     系统日志记录
    /// </summary>
    public class LogManager
    {
        /// <summary>
        ///     记录日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Log(string msg)
        {
            string dirpath = Path.Combine(Thread.GetDomain().BaseDirectory, "Log");
            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(dirpath);
            }
            string filePath = Path.Combine(dirpath, DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
            using (StreamWriter w = File.AppendText(filePath))
            {
                w.WriteLine("# " + DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss ") + msg);
                w.Close();
            }
        }

        /// <summary>
        ///     记录日志
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        public static void Log(string title, string msg)
        {
            string dirpath = Path.Combine(Thread.GetDomain().BaseDirectory, "Log");
            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(dirpath);
            }
            string filePath = Path.Combine(dirpath, DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
            using (StreamWriter w = File.AppendText(filePath))
            {
                w.WriteLine("# " + DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss ") + "[" + title + "]" + msg);
                w.Close();
            }
        }
    }
}