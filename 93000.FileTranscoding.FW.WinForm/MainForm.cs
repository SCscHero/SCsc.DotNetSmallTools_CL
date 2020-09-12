using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using _93000.FileTranscoding.Base.FWCL.Help;


namespace _93000.FileTranscoding.FW.WinForm
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            _txtBox_BrowseFolders.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _listViewnfAllFile.View = View.Details;
            _listViewnfAllFile.Columns.Add("文件路径", 500);
            _listViewnfAllFile.Columns.Add("编码方式", 100);
            _listViewnfAllFile.Columns.Add("转换结果", 100);
            _listViewnfAllFile.Columns.Add("耗时", 100);
            _listViewnfAllFile.SmallImageList = imageList;
            _ddEncoding.SelectedIndex = 3;// "UTF-8";
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 浏览文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_browse_Click(object sender, EventArgs e)
        {
            //弹出资源管理器选择文件
            _folderDialog.ShowDialog(this);
            string selectedPath = _folderDialog.SelectedPath;
            if (string.IsNullOrEmpty(selectedPath))
                return;
            _txtBox_BrowseFolders.Text = selectedPath;
        }



        private void AddItem(int i, List<string> files, string path, string folder, string extension)
        {
            //开启线程
            Invoke(new MethodInvoker(() =>
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
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                //获取文件图标
                var item = new ListViewItem();
                //控件中如果不包含文件图标
                if (!imageList.Images.ContainsKey(extension))
                {
                    string description;
                    Icon largeIcon;
                    Icon smallIcon;
                    FileHelper.GetExtsIconAndDescription(extension, out largeIcon,
                                                         out smallIcon,
                                                         out description);
                    if (imageList != null)
                    {
                        imageList.Images.Add(extension, smallIcon); //指定键值对 
                    }
                }
                item.ImageKey = extension; //设定键值对

                item.Text = path.Replace(folder, "").TrimStart('\\');//获取文件名.拓展名
                item.SubItems.Add(encode.BodyName);//获取文件编码
                item.SubItems.Add("");
                item.SubItems.Add(elapsedTime);

                _listViewnfAllFile.BeginUpdate();//启动ListViewFiles更新
                _listViewnfAllFile.Items.Add(item);//添加ListViewItem
                _listViewnfAllFile.Items[_listViewnfAllFile.Items.Count - 1].EnsureVisible();//将其设为可见
                _listViewnfAllFile.EndUpdate();//结束更新
                _labelState.Text = "正在获取文件编码..." + i + "/" + files.Count;//状态更新
            }));
        }




        /// <summary>
        /// 检查编码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CheckCode_Click(object sender, EventArgs e)
        {
            string pattern = _txtBox_FileSelection.Text.Trim();
            string folder = _txtBox_BrowseFolders.Text.Trim();
            if (string.IsNullOrEmpty(pattern))
            {
                MessageBox.Show("请填写文件筛选条件。", "提示");
            }
            else if (string.IsNullOrEmpty(folder))
            {
                MessageBox.Show("请选择或填写文件夹。", "提示");
            }
            else
            {
                try
                {
                    var threadDelegate = new ThreadStart(delegate
                    {
                        //消息通知 
                        Invoke(new MethodInvoker(() =>
                        {
                            _btnCheckCode.Enabled = false;
                            _btnBrowse.Enabled = false;
                            _btnTo.Enabled = false;
                            _labelState.Text = "正在获取文件编码...";
                            _listViewnfAllFile.Items.Clear();
                        }));


                        SearchOption searchOption = !_chkIncludeFloder.Checked
                                 ? SearchOption.TopDirectoryOnly
                                 : SearchOption.AllDirectories;
                        //读取该目录下文件列表
                        _files = FileHelper.GetFiles(folder, pattern, searchOption);
                        //遍历文件
                        for (int i = 0; i < _files.Count; ++i)
                        {
                            string path = _files[i];
                            string except = _txtBoxRuleOutType.Text.Trim();
                            //"|.cer|.pfx|.lib|.bat|.chm|.dbmdl|.snk|.pdb|.gif|.jpg|.mdb|.dll|.exe|.doc|.docx|.suo|.Cache|.StyleCop|.accessor|.bmp|.png|.netmodule|.ico|.dat|.dct|.xls|.zip|.rar|.url|.db|.csv|.rdlc|.swf|.fla|";

                            string extension = Path.GetExtension(path);
                            if (!except.Contains("|" + extension + "|"))
                            {
                                Invoke(new MethodInvoker(() =>
                                {
                                    var encodeTime = EncodingHelper.GetEncodingByFile(path);
                                    //获取文件图标
                                    var item = new ListViewItem();
                                    //控件中如果不包含文件图标
                                    if (!imageList.Images.ContainsKey(extension))
                                    {
                                        string description;
                                        Icon largeIcon;
                                        Icon smallIcon;
                                        FileHelper.GetExtsIconAndDescription(extension, out largeIcon,
                                            out smallIcon,
                                            out description);
                                        if (imageList != null)
                                        {
                                            imageList.Images.Add(extension, smallIcon); //指定键值对 
                                        }
                                    }

                                    item.ImageKey = extension; //设定键值对

                                    item.Text = path.Replace(folder, "").TrimStart('\\'); //获取文件名.拓展名
                                    item.SubItems.Add(encodeTime.Item1.BodyName); //获取文件编码
                                    item.SubItems.Add("");
                                    item.SubItems.Add(encodeTime.Item2);

                                    _listViewnfAllFile.BeginUpdate(); //启动ListViewFiles更新
                                    _listViewnfAllFile.Items.Add(item); //添加ListViewItem
                                    _listViewnfAllFile.Items[_listViewnfAllFile.Items.Count - 1]
                                        .EnsureVisible(); //将其设为可见
                                    _listViewnfAllFile.EndUpdate(); //结束更新
                                    _labelState.Text = "正在获取文件编码..." + i + "/" + _files.Count; //状态更新

                                }));
                            }
                        }

                        //消息通知
                        Invoke(new MethodInvoker(() =>
                        {
                            _labelState.Text = "获取文件编码完成。";
                            _btnCheckCode.Enabled = true;
                            _btnBrowse.Enabled = true;
                            _btnTo.Enabled = true;
                        }));
                    });
                    //开启线程执行
                    new Thread(threadDelegate).Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "异常信息");
                }
            }
        }

        /// <summary>
        /// 转换编码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _btnTo_Click(object sender, EventArgs e)
        {
            Encoding encoding;
            //需要转换的编码
            switch (_ddEncoding.SelectedIndex)
            {
                case 0:
                    encoding = Encoding.Default;
                    break;
                case 1:
                    encoding = Encoding.Unicode;
                    break;
                case 2:
                    encoding = Encoding.BigEndianUnicode;
                    break;
                case 3:
                    encoding = Encoding.UTF8;
                    break;
                default:
                    MessageBox.Show("请选择要转换的编码。", "提示");
                    return;
            }

            var folder = _txtBox_BrowseFolders.Text.Trim();
            var start = new ThreadStart(delegate
            {
                Invoke(new MethodInvoker(() =>
                {
                    _btnCheckCode.Enabled = false;
                    _btnBrowse.Enabled = false;
                    _btnTo.Enabled = false;
                    _labelState.Text = "正在转换文件编码...";
                    _listViewnfAllFile.Items.Clear();
                }));

                if (_files != null)
                {
                    int count = _files.Count;
                    var sucress = 0;
                    for (int i = 0; i < count; ++i)
                    {
                        var sw = new Stopwatch();
                        sw.Start();

                        string path = _files[i];
                        string extension = Path.GetExtension(path);
                        FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.ReadWrite);
                        var buffer = new byte[fileStream.Length];
                        fileStream.Read(buffer, 0, buffer.Length);
                        fileStream.Close();
                        fileStream.Dispose();
                        var fileEncoding = EncodingHelper.GetEncode(buffer);
                        var result = "无需转换";
                        if (!Equals(fileEncoding, encoding))
                        {
                            try
                            {
                                string txt = EncodingHelper.GetTxtByByArrayAndEncode(buffer, EncodingHelper.GetEncode(buffer));
                                FileHelper.WriteTxt(path, txt, encoding);
                                sucress++;
                                result = "转换成功";
                            }
                            catch
                            {
                                result = "转换失败";
                            }
                        }

                        sw.Stop();
                        TimeSpan ts = sw.Elapsed;
                        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                        var item = new ListViewItem();
                        if (!imageList.Images.ContainsKey(extension))
                        {
                            string description;
                            Icon largeIcon;
                            Icon smallIcon;
                            FileHelper.GetExtsIconAndDescription(extension, out largeIcon,
                                                                 out smallIcon,
                                                                 out description);
                            if (imageList != null)
                            {
                                imageList.Images.Add(extension, smallIcon); //指定键值对 
                            }
                        }
                        item.ImageKey = extension; //设定键值对

                        item.Text = path.Replace(folder, "").TrimStart('\\');
                        item.SubItems.Add(fileEncoding.BodyName);
                        item.SubItems.Add(result);
                        item.SubItems.Add(elapsedTime);
                        Invoke(new MethodInvoker(() =>
                        {
                            _listViewnfAllFile.BeginUpdate();
                            _listViewnfAllFile.Items.Add(item);
                            _listViewnfAllFile.Items[_listViewnfAllFile.Items.Count - 1].EnsureVisible();
                            _listViewnfAllFile.EndUpdate();
                            _labelState.Text = "正在转换文件编码..." + sucress;

                        }));
                    }
                }

                Invoke(new MethodInvoker(() =>
                {
                    _labelState.Text = "转换文件编码完成。";
                    _btnCheckCode.Enabled = true;
                    _btnBrowse.Enabled = true;
                    _btnTo.Enabled = true;
                }));

            });
            new Thread(start).Start();
        }
    }
}
