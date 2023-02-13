using System;
using System.IO;
using System.Net;

namespace _93000.FileTranscoding.Base.FWCL.Help.Software.Framework
{
    /// <summary>
    ///     Ftp辅助类
    /// </summary>
    public class FtpHelper
    {
        private const int BufferSize = 2048;
        private readonly string _host;
        private readonly string _pass;
        private readonly string _user;
        private FtpWebRequest _ftpRequest;
        private FtpWebResponse _ftpResponse;
        private Stream _ftpStream;

        public FtpHelper(string hostIp, string userName, string password)
        {
            _host = hostIp;
            _user = userName;
            _pass = password;
        }

        /// <summary>
        ///     下载文件
        /// </summary>
        /// <param name="localFile"></param>
        /// <param name="remoteFile"></param>
        /// <returns></returns>
        public FtpResult Download(string localFile, string remoteFile)
        {
            FtpResult result;
            _ftpRequest = (FtpWebRequest)WebRequest.Create(_host + "/" + remoteFile);
            try
            {
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = false;
                _ftpRequest.Timeout = 1000 * 30;
                _ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                _ftpStream = _ftpResponse.GetResponseStream();
                var localFileStream = new FileStream(localFile, FileMode.Create);
                var byteBuffer = new byte[BufferSize];
                if (_ftpStream != null)
                {
                    int bytesRead = _ftpStream.Read(byteBuffer, 0, BufferSize);
                    try
                    {
                        while (bytesRead > 0)
                        {
                            localFileStream.Write(byteBuffer, 0, bytesRead);
                            bytesRead = _ftpStream.Read(byteBuffer, 0, BufferSize);
                        }
                    }
                    catch (Exception ex)
                    {
                        result = new FtpResult(false, ex.Message);
                        return result;
                    }
                }
                localFileStream.Close();
                Dispose();
                result = new FtpResult(true, "ok");
            }
            catch (Exception ex)
            {
                Dispose();
                result = new FtpResult(false, ex.Message);
            }
            return result;
        }
        public void Dispose()
        {
            try
            {
                _ftpStream.Close();
                _ftpStream.Dispose();
                _ftpResponse.Close();
                _ftpRequest = null;
            }
            catch (Exception ex)
            {
                //LogManger.Instance.Log("FTP文件下载释放资源异常", ex.Message,LogCategory.Exception);
            }
        }

        /// <summary>
        ///     上传文件
        /// </summary>
        /// <param name="localFile"></param>
        /// <param name="remoteFile"></param>
        /// <returns></returns>
        public FtpResult Upload(string localFile, string remoteFile)
        {
            FtpResult result;
            try
            {
                _ftpRequest = (FtpWebRequest)WebRequest.Create(_host + "/" + remoteFile);
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = false;
                _ftpRequest.Timeout = 1000 * 30;
                _ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                _ftpStream = _ftpRequest.GetRequestStream();
                var localFileStream = new FileStream(localFile, FileMode.Open);
                var byteBuffer = new byte[BufferSize];
                int bytesSent = localFileStream.Read(byteBuffer, 0, BufferSize);
                try
                {
                    while (bytesSent != 0)
                    {
                        _ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, BufferSize);
                    }
                }
                catch (Exception ex)
                {
                    result = new FtpResult(false, ex.Message);
                    return result;
                }
                localFileStream.Close();

                Dispose();
                result = new FtpResult(true, "ok");
            }
            catch (Exception ex)
            {
                Dispose();
                result = new FtpResult(false, ex.Message);
            }
            return result;
        }

        /// <summary>
        ///     删除文件
        /// </summary>
        /// <param name="deleteFile"></param>
        public FtpResult Delete(string deleteFile)
        {
            FtpResult result;
            try
            {
                _ftpRequest = (FtpWebRequest)WebRequest.Create(_host + "/" + deleteFile);
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = false;
                _ftpRequest.Timeout = 1000 * 30;
                _ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                Dispose();
                result = new FtpResult(true, "ok");
            }
            catch (Exception ex)
            {
                Dispose();
                result = new FtpResult(false, ex.Message);
            }
            return result;
        }

        /// <summary>
        ///     文件重命名
        /// </summary>
        /// <param name="currentFileNameAndPath"></param>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        public FtpResult Rename(string currentFileNameAndPath, string newFileName)
        {
            FtpResult result;
            try
            {
                _ftpRequest = (FtpWebRequest)WebRequest.Create(_host + "/" + currentFileNameAndPath);
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = false;
                _ftpRequest.Timeout = 1000 * 30;
                _ftpRequest.Method = WebRequestMethods.Ftp.Rename;
                _ftpRequest.RenameTo = newFileName;
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                Dispose();
                result = new FtpResult(true, "ok");
            }
            catch (Exception ex)
            {
                Dispose();
                result = new FtpResult(false, ex.Message);
            }
            return result;
        }

        /// <summary>
        ///     创建目录
        /// </summary>
        /// <param name="newDirectory"></param>
        /// <returns></returns>
        public FtpResult CreateDirectory(string newDirectory)
        {
            FtpResult result;
            try
            {
                _ftpRequest = (FtpWebRequest)WebRequest.Create(_host + "/" + newDirectory);
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = false;
                _ftpRequest.Timeout = 1000 * 30;
                _ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();

                Dispose();
                result = new FtpResult(true, "ok");
            }
            catch (Exception ex)
            {
                Dispose();
                result = new FtpResult(false, ex.Message);
            }
            return result;
        }

        /// <summary>
        ///     取得文件创建时间
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FtpResult GetFileCreatedDateTime(string fileName)
        {
            FtpResult result;
            try
            {
                _ftpRequest = (FtpWebRequest)WebRequest.Create(_host + "/" + fileName);
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = false;
                _ftpRequest.Timeout = 1000 * 30;
                _ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                _ftpStream = _ftpResponse.GetResponseStream();
                if (_ftpStream != null)
                {
                    var ftpReader = new StreamReader(_ftpStream);
                    string fileInfo;
                    try
                    {
                        fileInfo = ftpReader.ReadToEnd();
                    }
                    catch (Exception ex)
                    {
                        result = new FtpResult(false, ex.Message);
                        ftpReader.Close();
                        Dispose();
                        return result;
                    }
                    ftpReader.Close();
                    Dispose();
                    return new FtpResult(true, fileInfo);
                }
                return new FtpResult(false, "响应流为空");
            }
            catch (Exception ex)
            {
                Dispose();
                result = new FtpResult(false, ex.Message);
            }
            return result;
        }

        /// <summary>
        ///     取得文件大小
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FtpResult GetFileSize(string fileName)
        {
            FtpResult result;
            try
            {
                _ftpRequest = (FtpWebRequest)WebRequest.Create(_host + "/" + fileName);
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = false;
                _ftpRequest.Timeout = 1000 * 30;
                _ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                _ftpStream = _ftpResponse.GetResponseStream();
                if (_ftpStream != null)
                {
                    var ftpReader = new StreamReader(_ftpStream);
                    string fileInfo = null;
                    try
                    {
                        while (ftpReader.Peek() != -1)
                        {
                            fileInfo = ftpReader.ReadToEnd();
                        }
                    }
                    catch (Exception ex)
                    {
                        result = new FtpResult(false, ex.Message);
                        ftpReader.Close();
                        Dispose();
                        return result;
                    }
                    Dispose();
                    return new FtpResult(true, fileInfo);
                }
                result = new FtpResult(false, "响应流为空");
            }
            catch (Exception ex)
            {
                Dispose();
                result = new FtpResult(false, ex.Message);
            }
            return result;
        }

        /// <summary>
        ///     显示远程目录结构 
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public FtpResult DirectoryListSimple(string directory)
        {
            var result = new FtpResult();
            try
            {
                _ftpRequest = (FtpWebRequest)WebRequest.Create(_host + "/" + directory);
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = false;
                _ftpRequest.Timeout = 1000 * 30;
                _ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                _ftpStream = _ftpResponse.GetResponseStream();
                if (_ftpStream != null)
                {
                    var ftpReader = new StreamReader(_ftpStream);
                    string directoryRaw = null;
                    try
                    {
                        while (ftpReader.Peek() != -1)
                        {
                            directoryRaw += ftpReader.ReadLine() + "|";
                        }
                    }
                    catch (Exception ex)
                    {
                        result = new FtpResult(false, ex.Message);
                    }
                    ftpReader.Close();
                    Dispose();
                    /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */
                    try
                    {
                        if (directoryRaw != null)
                        {
                            string[] directoryList = directoryRaw.Split("|".ToCharArray());
                            result = new FtpResult(true, directoryList);
                        }
                    }
                    catch (Exception ex)
                    {

                        Dispose();
                        result = new FtpResult(false, ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Dispose();
                result = new FtpResult(false, ex.Message);
            }
            return result;
        }

        /// <summary>
        ///     远程文件列表
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public string[] DirectoryListDetailed(string directory)
        {
            try
            {
                _ftpRequest = (FtpWebRequest)WebRequest.Create(_host + "/" + directory);
                _ftpRequest.Credentials = new NetworkCredential(_user, _pass);
                _ftpRequest.UseBinary = true;
                _ftpRequest.UsePassive = true;
                _ftpRequest.KeepAlive = false;
                _ftpRequest.Timeout = 1000 * 30;
                _ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                _ftpStream = _ftpResponse.GetResponseStream();
                if (_ftpStream != null)
                {
                    var ftpReader = new StreamReader(_ftpStream);
                    string directoryRaw = null;
                    try
                    {
                        while (ftpReader.Peek() != -1)
                        {
                            directoryRaw += ftpReader.ReadLine() + "|";
                        }
                    }
                    catch (Exception ex)
                    {
                        Dispose();
                        Console.WriteLine(ex.ToString());
                    }
                    ftpReader.Close();
                    Dispose();
                    try
                    {
                        if (directoryRaw != null)
                        {
                            string[] directoryList = directoryRaw.Split("|".ToCharArray());
                            return directoryList;
                        }
                    }
                    catch (Exception ex)
                    {
                        Dispose();
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Dispose();
                Console.WriteLine(ex.ToString());
            }
            /* Return an Empty string Array if an Exception Occurs */
            return new[] { "" };
        }
    }

    public class FtpResult
    {
        public FtpResult()
        {
        }

        public FtpResult(bool isCusecess, object message)
        {
            IsSucess = isCusecess;
            Message = message;
        }

        public bool IsSucess { get; set; }
        public object Message { get; set; }
    }
}