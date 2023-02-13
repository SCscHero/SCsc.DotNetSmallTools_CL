using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace _93000.FileTranscoding.Base.FWCL.Help
{



    /// <summary>
    ///     文件辅助类
    /// </summary>
    public class FileHelper
    {
        private const double KBCount = 1024;
        private const double MBCount = KBCount * 1024;
        private const double GBCount = MBCount * 1024;
        private const double TBCount = GBCount * 1024;

        /// <summary>
        ///     编码方式
        /// </summary>
        private static readonly Encoding Encoding = Encoding.UTF8;


        /// <summary>
        /// 写入文本
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="body"></param>
        /// <param name="encoding"></param>
        public static void WriteTxt(string filepath, string body, Encoding encoding)
        {
            if (File.Exists(filepath))
                File.Delete(filepath);
            byte[] bytes = encoding.GetBytes(body);
            FileStream fileStream = File.Open(filepath, FileMode.CreateNew, FileAccess.Write);
            if (Equals(encoding, Encoding.UTF8))
            {
                fileStream.WriteByte(239);
                fileStream.WriteByte(187);
                fileStream.WriteByte(191);
            }
            else if (Equals(encoding, Encoding.BigEndianUnicode))
            {
                fileStream.WriteByte(254);
                fileStream.WriteByte(byte.MaxValue);
            }
            else if (Equals(encoding, Encoding.Unicode))
            {
                fileStream.WriteByte(byte.MaxValue);
                fileStream.WriteByte(254);
            }
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Flush();
            fileStream.Close();
            fileStream.Dispose();
        }



        /// <summary>
        /// 根据路径、筛选方案、是否包含文件夹，返回该路径下文件列表
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="pattern"></param>
        /// <param name="searchOption"></param>
        /// <returns></returns>
        public static List<string> GetFiles(string folder, string pattern, SearchOption searchOption)
        {
            string[] strArray = null;
            try
            {
                strArray = Directory.GetFiles(folder, pattern, searchOption);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("没有打开文件夹的权限。", "提示");
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show("指定的路径无效。", "提示");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("指定的路径无效。", "提示");
            }
            catch (PathTooLongException ex)
            {
                MessageBox.Show("路径超出了系统定义的最大长度。", "提示");
            }
            catch
            {
                MessageBox.Show("无法打开文件夹。", "提示");
            }
            if (strArray != null)
                return new List<string>(strArray);
            return new List<string>();
        }



        /// <summary>
        ///     递归取得文件夹下文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="list"></param>
        public static void GetFiles(string dir, List<string> list)
        {
            GetFiles(dir, list, new List<string>());
        }

        /// <summary>
        ///     递归取得文件夹下文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="list"></param>
        /// <param name="fileExtsions"></param>
        public static void GetFiles(string dir, List<string> list, List<string> fileExtsions)
        {
            //添加文件 
            string[] files = Directory.GetFiles(dir);
            if (fileExtsions.Count > 0)
            {
                foreach (string file in files)
                {
                    string extension = Path.GetExtension(file);
                    if (extension != null && fileExtsions.Contains(extension))
                    {
                        list.Add(file);
                    }
                }
            }
            else
            {
                list.AddRange(files);
            }
            //如果是目录，则递归
            DirectoryInfo[] directories = new DirectoryInfo(dir).GetDirectories();
            foreach (DirectoryInfo item in directories)
            {
                GetFiles(item.FullName, list, fileExtsions);
            }
        }

        /// <summary>
        ///     写入文件
        /// </summary>
        /// <param name="filePath">文件名</param>
        /// <param name="content">文件内容</param>
        public static void WriteFile(string filePath, string content)
        {
            try
            {
                var fs = new FileStream(filePath, FileMode.Create);
                Encoding encode = Encoding;
                //获得字节数组
                byte[] data = encode.GetBytes(content);
                //开始写入
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                fs.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        ///     读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath)
        {
            return ReadFile(filePath, Encoding);
        }

        /// <summary>
        ///     读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath, Encoding encoding)
        {
            using (var sr = new StreamReader(filePath, encoding))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        ///     读取文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<string> ReadFileLines(string filePath)
        {
            var str = new List<string>();
            using (var sr = new StreamReader(filePath, Encoding))
            {
                String input;
                while ((input = sr.ReadLine()) != null)
                {
                    str.Add(input);
                }
            }
            return str;
        }

        /// <summary>
        ///     复制文件夹（及文件夹下所有子文件夹和文件）
        /// </summary>
        /// <param name="sourcePath">待复制的文件夹路径</param>
        /// <param name="destinationPath">目标路径</param>
        public static void CopyDirectory(String sourcePath, String destinationPath)
        {
            var info = new DirectoryInfo(sourcePath);
            Directory.CreateDirectory(destinationPath);
            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                String destName = Path.Combine(destinationPath, fsi.Name);

                if (fsi is FileInfo) //如果是文件，复制文件
                    File.Copy(fsi.FullName, destName);
                else //如果是文件夹，新建文件夹，递归
                {
                    Directory.CreateDirectory(destName);
                    CopyDirectory(fsi.FullName, destName);
                }
            }
        }

        /// <summary>
        ///     删除文件夹（及文件夹下所有子文件夹和文件）
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void DeleteFolder(string directoryPath)
        {
            try
            {
                foreach (string d in Directory.GetFileSystemEntries(directoryPath))
                {
                    if (File.Exists(d))
                    {
                        var fi = new FileInfo(d);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly", StringComparison.Ordinal) != -1)
                            fi.Attributes = FileAttributes.Normal;
                        File.Delete(d); //删除文件   
                    }
                    else
                        DeleteFolder(d); //删除文件夹
                }
                Directory.Delete(directoryPath); //删除空文件夹
            }
            catch (Exception ex) { }
        }

        /// <summary>
        ///     清空文件夹（及文件夹下所有子文件夹和文件）
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void ClearFolder(string directoryPath)
        {
            foreach (string d in Directory.GetFileSystemEntries(directoryPath))
            {
                if (File.Exists(d))
                {
                    var fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly", StringComparison.Ordinal) != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d); //删除文件   
                }
                else
                    DeleteFolder(d); //删除文件夹
            }
        }

        /// <summary>
        /// 得到适应的大小
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string GetFileSize(string filepath)
        {
            return GetFileSize(filepath, 2);
        }

        /// <summary>
        /// 得到适应的大小
        /// </summary>
        /// <param name="filepath">文件路径 </param>
        /// <param name="round">小数位 </param>
        /// <returns>string</returns>
        public static string GetFileSize(string filepath, int round)
        {
            if (File.Exists(filepath))
            {
                var file = new FileInfo(filepath);
                double size = file.Length;
                if (KBCount > size)
                {
                    return Math.Round(size, round) + "B";
                }
                if (MBCount > size)
                {
                    return Math.Round(size / KBCount, round) + "KB";
                }
                if (GBCount > size)
                {
                    return Math.Round(size / MBCount, round) + "MB";
                }
                if (TBCount > size)
                {
                    return Math.Round(size / GBCount, round) + "GB";
                }
                return Math.Round(size / TBCount, round) + "TB";
            }
            return string.Empty;
        }

        /// <summary>
        ///     通过扩展名得到图标和描述
        /// </summary>
        /// <param name="ext">扩展名(如“.txt”)</param>
        /// <param name="largeIcon">得到大图标</param>
        /// <param name="smallIcon">得到小图标</param>
        /// <param name="description">得到类型描述或者空字符串</param>
        public static void GetExtsIconAndDescription(string ext, out Icon largeIcon, out Icon smallIcon,
                                                     out string description)
        {
            GetDefaultIcon(out largeIcon, out smallIcon); //得到缺省图标  
            description = ""; //缺省类型描述  
            RegistryKey extsubkey = Registry.ClassesRoot.OpenSubKey(ext); //从注册表中读取扩展名相应的子键  
            if (extsubkey == null) return;

            var extdefaultvalue = extsubkey.GetValue(null) as string; //取出扩展名对应的文件类型名称  
            if (extdefaultvalue == null) return;

            if (extdefaultvalue.Equals("exefile", StringComparison.OrdinalIgnoreCase)) //扩展名类型是可执行文件  
            {
                RegistryKey exefilesubkey = Registry.ClassesRoot.OpenSubKey(extdefaultvalue); //从注册表中读取文件类型名称的相应子键  
                if (exefilesubkey != null)
                {
                    var exefiledescription = exefilesubkey.GetValue(null) as string; //得到exefile描述字符串  
                    if (exefiledescription != null) description = exefiledescription;
                }
                var exefilePhiconLarge = new IntPtr();
                var exefilePhiconSmall = new IntPtr();
                NativeMethods.ExtractIconExW(Path.Combine(Environment.SystemDirectory, "shell32.dll"), 2,
                                             ref exefilePhiconLarge, ref exefilePhiconSmall, 1);
                if (exefilePhiconLarge.ToInt32() > 0) largeIcon = Icon.FromHandle(exefilePhiconLarge);
                if (exefilePhiconSmall.ToInt32() > 0) smallIcon = Icon.FromHandle(exefilePhiconSmall);
                return;
            }

            RegistryKey typesubkey = Registry.ClassesRoot.OpenSubKey(extdefaultvalue); //从注册表中读取文件类型名称的相应子键  
            if (typesubkey == null) return;

            var typedescription = typesubkey.GetValue(null) as string; //得到类型描述字符串  
            if (typedescription != null) description = typedescription;

            RegistryKey defaulticonsubkey = typesubkey.OpenSubKey("DefaultIcon"); //取默认图标子键  
            if (defaulticonsubkey == null) return;

            //得到图标来源字符串  
            var defaulticon = defaulticonsubkey.GetValue(null) as string; //取出默认图标来源字符串  
            if (defaulticon == null) return;
            string[] iconstringArray = defaulticon.Split(',');
            int nIconIndex = 0; //声明并初始化图标索引  
            if (iconstringArray.Length > 1)
                if (!int.TryParse(iconstringArray[1], out nIconIndex))
                    nIconIndex = 0; //int.TryParse转换失败，返回0  

            //得到图标  
            var phiconLarge = new IntPtr();
            var phiconSmall = new IntPtr();
            NativeMethods.ExtractIconExW(iconstringArray[0].Trim('"'), nIconIndex, ref phiconLarge, ref phiconSmall, 1);
            if (phiconLarge.ToInt32() > 0) largeIcon = Icon.FromHandle(phiconLarge);
            if (phiconSmall.ToInt32() > 0) smallIcon = Icon.FromHandle(phiconSmall);
        }

        /// <summary>
        ///     获取缺省图标
        /// </summary>
        /// <param name="largeIcon"></param>
        /// <param name="smallIcon"></param>
        private static void GetDefaultIcon(out Icon largeIcon, out Icon smallIcon)
        {
            largeIcon = smallIcon = null;
            var phiconLarge = new IntPtr();
            var phiconSmall = new IntPtr();
            NativeMethods.ExtractIconExW(Path.Combine(Environment.SystemDirectory, "shell32.dll"), 0, ref phiconLarge,
                                         ref phiconSmall, 1);
            if (phiconLarge.ToInt32() > 0) largeIcon = Icon.FromHandle(phiconLarge);
            if (phiconSmall.ToInt32() > 0) smallIcon = Icon.FromHandle(phiconSmall);
        }
    }

    public class NativeMethods
    {
        /// Return Type: UINT->unsigned int
        /// lpszFile: LPCWSTR->WCHAR*
        /// nIconIndex: int
        /// phiconLarge: HICON*
        /// phiconSmall: HICON*
        /// nIcons: UINT->unsigned int
        [DllImport("shell32.dll", EntryPoint = "ExtractIconExW", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ExtractIconExW([In] [MarshalAs(UnmanagedType.LPWStr)] string lpszFile, int nIconIndex,
                                                 ref IntPtr phiconLarge, ref IntPtr phiconSmall, uint nIcons);
    }
}