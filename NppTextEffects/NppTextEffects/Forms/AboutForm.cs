using System;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace TextEffects.Forms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }


        private void CustomAboutBox_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        public void SetContentLabel(string content)
        {
            this.contentLabel.Text = content;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Donate_Click(object sender, EventArgs e)
        {
            string PluginWebsiteUrl = "https://www.paypal.com/paypalme/RegExtractor";
            try
            {
                // 使用 Process.Start() 打开默认浏览器并访问指定的 URL
                Process.Start(PluginWebsiteUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开网站失败: {PluginWebsiteUrl}.\n错误: {ex.Message}",
                                "网站命令错误",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }

            this.Close();
        }



        /*
                public CustomAboutBox(string messageContent)
                {
                    // 初始化窗体设置
                    this.Text = "About TextEffects";
                    this.Size = new Size(500, 300);
                    this.StartPosition = FormStartPosition.CenterScreen;
                    this.FormBorderStyle = FormBorderStyle.FixedDialog; // 固定大小，不可调整
                    this.MaximizeBox = false;
                    this.MinimizeBox = false;

                    // --- 1. 文本内容标签 ---
                    Label contentLabel = new Label
                    {
                        Text = messageContent,
                        Location = new Point(20, 20),
                        Size = new Size(440, 160),
                        Font = new Font("Segoe UI", 9),
                        TextAlign = ContentAlignment.TopLeft,
                        AutoEllipsis = false,
                        // 设置为多行显示
                        BorderStyle = BorderStyle.FixedSingle
                    };
                    this.Controls.Add(contentLabel);

                    // --- 2. 自定义按钮 1: "确定" ---
                    Button confirmButton = new Button
                    {
                        Text = "Donate", // 自定义按钮名称 1
                        Location = new Point(280, 200),
                        Size = new Size(80, 30),
                        DialogResult = DialogResult.OK // 设置对话框结果
                    };
                    confirmButton.Click += (sender, e) => { this.Close(); };
                    this.Controls.Add(confirmButton);
                    this.AcceptButton = confirmButton; // 将其设为默认按钮 (回车键)

                    // --- 3. 自定义按钮 2: "关闭" ---
                    Button closeButton = new Button
                    {
                        Text = "Close", // 自定义按钮名称 2
                        Location = new Point(370, 200),
                        Size = new Size(80, 30),
                        DialogResult = DialogResult.Cancel // 设置对话框结果
                    };
                    closeButton.Click += (sender, e) => { this.Close(); };
                    this.Controls.Add(closeButton);
                    this.CancelButton = closeButton; // 将其设为取消按钮 (Esc 键)
                }*/

        private void LoadRegExTractorUI()
        {
            var currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var regextractorassembly = Assembly.LoadFrom(currentDirectory + "\\RegExtractor\\RegExTractorWinForm.dll");
            //MessageBox.Show("Registerd Assembly");

            Type type = regextractorassembly.GetType("RegExTractorWinForm.RegExTractorMainUI");
            //MessageBox.Show("Registerd type");

            //var method = type.GetMethod("Process");
            //MessageBox.Show("Registerd Method");


            object activator = Activator.CreateInstance(type);
            //MessageBox.Show("Registerd Activator");

            this.Controls.Add(activator as Control);
            (activator as Control).Dock = DockStyle.Fill;


            //method.Invoke(activator, new object[] { directory, recursive, filter, searchTermFile, outputFile });
            //var workflow = new RegExTractorSimpleWorkflow();
            //workflow.Process(directory, recursive, filter, searchTermFile, outputFile);
        }
    }
}