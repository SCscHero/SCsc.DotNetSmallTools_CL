using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace _93000.FileTranscoding.FW.WinForm
{
    partial class MainForm : Form
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._txtBox_FileSelection = new System.Windows.Forms.TextBox();
            this._txtBox_BrowseFolders = new System.Windows.Forms.TextBox();
            this._chkIncludeFloder = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this._txtBoxRuleOutType = new System.Windows.Forms.TextBox();
            this._btnBrowse = new System.Windows.Forms.Button();
            this._btnCheckCode = new System.Windows.Forms.Button();
            this._labelState = new System.Windows.Forms.Label();
            this._btnTo = new System.Windows.Forms.Button();
            this._ddEncoding = new System.Windows.Forms.ComboBox();
            this._label3 = new System.Windows.Forms.Label();
            this._folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this._listViewnfAllFile = new _93000.FileTranscoding.Base.FWCL.UI.ListViewNF();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(13, 13);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(65, 12);
            this.label.TabIndex = 0;
            this.label.Text = "文件筛选：";
            this.label.Click += new System.EventHandler(this.label1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "文件夹：";
            // 
            // _txtBox_FileSelection
            // 
            this._txtBox_FileSelection.Location = new System.Drawing.Point(74, 10);
            this._txtBox_FileSelection.Name = "_txtBox_FileSelection";
            this._txtBox_FileSelection.Size = new System.Drawing.Size(505, 21);
            this._txtBox_FileSelection.TabIndex = 2;
            this._txtBox_FileSelection.Text = "*.*";
            // 
            // _txtBox_BrowseFolders
            // 
            this._txtBox_BrowseFolders.Location = new System.Drawing.Point(74, 40);
            this._txtBox_BrowseFolders.Name = "_txtBox_BrowseFolders";
            this._txtBox_BrowseFolders.Size = new System.Drawing.Size(424, 21);
            this._txtBox_BrowseFolders.TabIndex = 3;
            // 
            // _chkIncludeFloder
            // 
            this._chkIncludeFloder.AutoSize = true;
            this._chkIncludeFloder.Checked = true;
            this._chkIncludeFloder.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chkIncludeFloder.Location = new System.Drawing.Point(42, 77);
            this._chkIncludeFloder.Name = "_chkIncludeFloder";
            this._chkIncludeFloder.Size = new System.Drawing.Size(96, 16);
            this._chkIncludeFloder.TabIndex = 4;
            this._chkIncludeFloder.Text = "包含子文件夹";
            this._chkIncludeFloder.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(154, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "排除类型：";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // _txtBoxRuleOutType
            // 
            this._txtBoxRuleOutType.Location = new System.Drawing.Point(219, 73);
            this._txtBoxRuleOutType.Name = "_txtBoxRuleOutType";
            this._txtBoxRuleOutType.Size = new System.Drawing.Size(279, 21);
            this._txtBoxRuleOutType.TabIndex = 6;
            this._txtBoxRuleOutType.Text = "|.cer|.pfx|.lib|.bat|.chm|.dbmdl|.snk|.pdb|.gif|.jpg|.mdb|.dll|.exe|.doc|.docx|.s" +
    "uo|.Cache|.StyleCop|.accessor|.bmp|.png|.netmodule|.ico|.dat|.dct|.xls|.zip|.rar" +
    "|.url|.db|.csv|.rdlc|.swf|.fla|";
            // 
            // _btnBrowse
            // 
            this._btnBrowse.Location = new System.Drawing.Point(504, 39);
            this._btnBrowse.Name = "_btnBrowse";
            this._btnBrowse.Size = new System.Drawing.Size(75, 23);
            this._btnBrowse.TabIndex = 7;
            this._btnBrowse.Text = "浏览";
            this._btnBrowse.UseVisualStyleBackColor = true;
            this._btnBrowse.Click += new System.EventHandler(this.btn_browse_Click);
            // 
            // _btnCheckCode
            // 
            this._btnCheckCode.Location = new System.Drawing.Point(504, 72);
            this._btnCheckCode.Name = "_btnCheckCode";
            this._btnCheckCode.Size = new System.Drawing.Size(75, 23);
            this._btnCheckCode.TabIndex = 8;
            this._btnCheckCode.Text = "检测编码";
            this._btnCheckCode.UseVisualStyleBackColor = true;
            this._btnCheckCode.Click += new System.EventHandler(this.btn_CheckCode_Click);
            // 
            // _labelState
            // 
            this._labelState.AutoSize = true;
            this._labelState.Location = new System.Drawing.Point(40, 416);
            this._labelState.Name = "_labelState";
            this._labelState.Size = new System.Drawing.Size(29, 12);
            this._labelState.TabIndex = 10;
            this._labelState.Text = "状态";
            // 
            // _btnTo
            // 
            this._btnTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnTo.Location = new System.Drawing.Point(504, 411);
            this._btnTo.Name = "_btnTo";
            this._btnTo.Size = new System.Drawing.Size(75, 23);
            this._btnTo.TabIndex = 13;
            this._btnTo.Text = "转换";
            this._btnTo.UseVisualStyleBackColor = true;
            this._btnTo.Click += new System.EventHandler(this._btnTo_Click);
            // 
            // _ddEncoding
            // 
            this._ddEncoding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._ddEncoding.FormattingEnabled = true;
            this._ddEncoding.Items.AddRange(new object[] {
            "ANSI",
            "Unicode",
            "UnicodeBE",
            "UTF-8"});
            this._ddEncoding.Location = new System.Drawing.Point(380, 413);
            this._ddEncoding.Name = "_ddEncoding";
            this._ddEncoding.Size = new System.Drawing.Size(118, 20);
            this._ddEncoding.TabIndex = 12;
            // 
            // _label3
            // 
            this._label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._label3.AutoSize = true;
            this._label3.Location = new System.Drawing.Point(321, 416);
            this._label3.Name = "_label3";
            this._label3.Size = new System.Drawing.Size(53, 12);
            this._label3.TabIndex = 11;
            this._label3.Text = "转换为：";
            // 
            // _folderDialog
            // 
            this._folderDialog.SelectedPath = "Vri";
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // _listViewnfAllFile
            // 
            this._listViewnfAllFile.HideSelection = false;
            this._listViewnfAllFile.Location = new System.Drawing.Point(15, 109);
            this._listViewnfAllFile.Name = "_listViewnfAllFile";
            this._listViewnfAllFile.Size = new System.Drawing.Size(564, 296);
            this._listViewnfAllFile.TabIndex = 14;
            this._listViewnfAllFile.UseCompatibleStateImageBehavior = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 450);
            this.Controls.Add(this._listViewnfAllFile);
            this.Controls.Add(this._btnTo);
            this.Controls.Add(this._ddEncoding);
            this.Controls.Add(this._label3);
            this.Controls.Add(this._labelState);
            this.Controls.Add(this._btnCheckCode);
            this.Controls.Add(this._btnBrowse);
            this.Controls.Add(this._txtBoxRuleOutType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._chkIncludeFloder);
            this.Controls.Add(this._txtBox_BrowseFolders);
            this.Controls.Add(this._txtBox_FileSelection);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label);
            this.Name = "MainForm";
            this.Text = "文件转码小工具";
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _txtBox_FileSelection;
        private System.Windows.Forms.TextBox _txtBox_BrowseFolders;
        private System.Windows.Forms.CheckBox _chkIncludeFloder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _txtBoxRuleOutType;
        private System.Windows.Forms.Button _btnBrowse;
        private System.Windows.Forms.Button _btnCheckCode;
        private System.Windows.Forms.Label _labelState;
        private System.Windows.Forms.Button _btnTo;
        private System.Windows.Forms.ComboBox _ddEncoding;
        private System.Windows.Forms.Label _label3;
        private System.Windows.Forms.FolderBrowserDialog _folderDialog;
        private List<string> _files;
        private ImageList imageList;
        private Base.FWCL.UI.ListViewNF _listViewnfAllFile;
    }
}

