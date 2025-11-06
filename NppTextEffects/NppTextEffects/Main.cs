using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using NppPluginNET;
using System.Runtime.InteropServices;
using TextEffects.Utilities;
using TextEffects.Forms;
using static TextEffects.Utilities.KanaConverter;

namespace TextEffects
{
    class Main
    {
        #region " Fields "
        internal const string PluginName = "TextEffects";

        #endregion

        #region " StartUp/CleanUp "
        internal static void CommandMenuInit()
        {
            // 注册新的菜单项：菜单名 | 函数名 | 快捷键 | 是否检查状态
            // UTF-16_デコード

            PluginBase.SetCommand(0, "Escape Json", EscapeLiteral, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(1, "Unescape Json", UnescapeLiteral, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(2, "To Full-width", ConvertToFullWidth, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(3, "To Half-width", ConvertToHalfWidth, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(4, "To Furigana", ConvertKanjiToKana, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(5, "Remove Duplicate Lines", Selection, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(6, "Text Statistics", GetByteCharCount, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(7, "About", About, new ShortcutKey(false, false, false, Keys.None));
        }
        internal static void SetToolBarIcon()
        {

        }
        internal static void PluginCleanUp()
        {

        }
        #endregion

        #region " Menu functions About"
        internal static void About()
        {
            StringBuilder about = new StringBuilder();
            about.AppendLine();
            //about.AppendLine("TextEffects");
            about.AppendLine("Author:   Ladder Chin");
            about.AppendLine();
            about.AppendLine("Version:  1.0");
            about.AppendLine();
            about.AppendLine("Licence:  GPL");


            // 2. 实例化自定义对话框，并传递要显示的内容
            // 关键词 'using' 确保 customBox 资源在退出块时被释放
            using (var customBox = new AboutForm())
            {
                customBox.SetContentLabel(about.ToString());

                // 3. 使用 ShowDialog() 模态显示自定义窗体
                DialogResult result = customBox.ShowDialog();
            }
        }
        #endregion

        #region " Menu functions GetByteCount"


        /// <summary>
        /// 获取字符长度和字节长度
        /// </summary>
        /// <returns>字符长度和字节长度</returns>
        internal static void GetByteCharCount()
        {
            /*
                // 获取当前活动编辑器的句柄
                IntPtr scintillaHandle = PluginBase.GetCurrentScintilla();

                // 获取内部默认编码 (CodePage)   默认编码页为  65001  UTF-8 
                // 注意：SCI_GETCODEPAGE 消息返回的是 Scintilla 内部用于读取和写入文本的 CodePage ID
                int codePageID = (int)Win32.SendMessage(scintillaHandle, SciMsg.SCI_GETCODEPAGE, 0, 0);
            */

            StringBuilder message = new StringBuilder();
            // 1. ファイルのバイト列をすべて読み込む
            byte[] fileBytes;
            int fl;
            try
            {
                fileBytes = File.ReadAllBytes(GetCurrentFilePath());
                fl = fileBytes.Length;
                message.Append("File Size(Bytes)       : " + fl + "\r\n");
            }
            catch (Exception)
            {
            }
                       

            // 获取选中内容的字节长度
            byte[] inputBytes = GetSelectionBytes();
            int bl = inputBytes.Length;
            message.Append("Selection Bytes(UTF-8) : " + bl + "\r\n");

            // 获取选中内容的字符长度（包含终止符 \0）
            string inputText = Encoding.UTF8.GetString(inputBytes);
            int cl = inputText.Length;
            message.Append("Selection Characters   : " + cl);

            MessageBox.Show(message.ToString());
        }

        #endregion

        #region " Menu functions Selection"
        internal static void Selection()
        {
            // 获取当前活动编辑器的句柄
            IntPtr scintillaHandle = PluginBase.GetCurrentScintilla();

            // 步骤 3: 根据检测到的编码从字节序列中获取正确的字符串
            byte[] inputBytes = GetSelectionBytes();
            string inputText = Encoding.UTF8.GetString(inputBytes);

            if (string.IsNullOrEmpty(inputText))
            {
                MessageBox.Show("please select lines first");
                return;
            }

            // 现在 decodedText 变量将包含正确的文本
            //MessageBox.Show(decodedText);

            try
            {
                string original = inputText;
                string outputString;
                using (StringReader reader = new StringReader(original)) // code from https://stackoverflow.com/questions/2865863/removing-all-whitespace-lines-from-a-multi-line-string-efficiently
                using (StringWriter writer = new StringWriter())
                {
                    string line;
                    int x = 1;
                    while ((line = reader.ReadLine()) != null)
                    {
                        x += 1;
                        if (string.IsNullOrEmpty(line.Trim()))
                        {
                            line = Regex.Replace(line, "$", "3f5456cfsd661lld33Guid9CA0F324-3E3A-4C43-8AGu989CAACD-3BD6-499D-8664-41CE47622FE25E1E7C8FF3Guid9CA0F324-3E3A-4C43-8A22-308B5E1E7C8FA-4C43-8A22-308B5E1E7CGuid9CA0F324-3E3A-4C43-8A22-308B5E1E7C8F" + x);
                        }
                        writer.WriteLine(line);
                    }
                    outputString = writer.ToString();
                }

                string[] distinctLines = outputString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Distinct().ToArray();
                string outputStrin;
                string s = string.Join("\r\n", distinctLines);
                using (StringReader readerr = new StringReader(s)) // code from https://stackoverflow.com/questions/2865863/removing-all-whitespace-lines-from-a-multi-line-string-efficiently
                using (StringWriter writerr = new StringWriter())
                {
                    string linek;
                    while ((linek = readerr.ReadLine()) != null)
                    {
                        string uu = Regex.Replace(linek, "3f5456cfsd661lld33Guid9CA0F324-3E3A-4C43-8AGu989CAACD-3BD6-499D-8664-41CE47622FE25E1E7C8FF3Guid9CA0F324-3E3A-4C43-8A22-308B5E1E7C8FA-4C43-8A22-308B5E1E7CGuid9CA0F324-3E3A-4C43-8A22-308B5E1E7C8F.*$", "");
                        writerr.WriteLine(uu);
                    }
                    outputStrin = writerr.ToString();
                    outputStrin = outputStrin.Substring(0, outputStrin.LastIndexOf(Environment.NewLine));
                }
                if (inputText == outputStrin)
                {

                    Win32.SendMessage(scintillaHandle, SciMsg.SCI_CLEARSELECTIONS, 0, 0);
                    return;
                }


                // 步骤 3: 字符串被编码为字节流
                byte[] outputBytes = Encoding.UTF8.GetBytes(outputStrin);

                // 为字节流分配非托管内存 (+1 是为了终止符 \0)
                IntPtr outputPtr = Marshal.AllocHGlobal(outputBytes.Length + 1);

                // 复制字节流到非托管内存
                Marshal.Copy(outputBytes, 0, outputPtr, outputBytes.Length);

                // 添加终止符 \0
                Marshal.WriteByte(outputPtr, outputBytes.Length, 0);

                // 步骤 4: 发送 SCI_REPLACESEL 消息
                // Scintilla 此时知道 utf8Ptr 指向的是 UTF-8 字节
                Win32.SendMessage(scintillaHandle, SciMsg.SCI_REPLACESEL, 0, outputPtr);

                // 步骤 5: 释放非托管内存
                Marshal.FreeHGlobal(outputPtr);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Error. " + exp.Message);
            }
        }

        #endregion


        #region " Menu functions Escape Literal "
        //public static string EscapeForCSharpLiteral(string input)
        internal static void EscapeLiteral()
        {
            // 获取当前活动编辑器的句柄
            IntPtr scintillaHandle = PluginBase.GetCurrentScintilla();

          /*  // 获取当前文档的编码 (CodePage)
            // 注意：SCI_GETCODEPAGE 消息返回的是 Scintilla 内部用于读取和写入文本的 CodePage ID
            int codePageID = (int)Win32.SendMessage(scintillaHandle, SciMsg.SCI_GETCODEPAGE, 0, 0);

            // 设置 Scintilla 的 CodePage 为 (CodePage ID ) , 告诉 Scintilla 接下来接收到的字符串编码
            Win32.SendMessage(scintillaHandle, SciMsg.SCI_SETCODEPAGE, codePageID, 0);*/

            byte[] inputBytes = GetDocBytes(scintillaHandle);
            string inputText = Encoding.UTF8.GetString(inputBytes);

            // ----------------------------------------------------
            // 现在 documentText 变量中存储了当前文档的全部内容，编码已正确处理
            // 接下来您可以在此基础上执行转义操作：
            // string escapedString = EscapeForCSharpLiteral(documentText);

            // 1. 将所有的双引号 (") 替换为转义的双引号 (\")
            string escaped = inputText.Replace("\"", "\\\"");

            // 2. 将所有的换行符和回车符替换为转义的序列
            // 注意顺序：先处理 \r\n，再单独处理 \r 和 \n，以防只存在其中之一
            escaped = escaped.Replace("\r\n", "\\r\\n");
            escaped = escaped.Replace("\n", "\\n"); // Unix/Linux 换行
            escaped = escaped.Replace("\r", "\\r"); // 旧 Mac / 单独回车

            // 3. 将制表符 (\t) 替换为转义的制表符 (\\t)
            escaped = escaped.Replace("\t", "\\t");

            // 4. (可选但推荐) 将反斜杠本身 (\) 替换为双反斜杠 (\\)
            // 这一步必须在处理完其他转义字符后进行，以避免二次转义（如 \\r\\n 变成 \\\\r\\\\n）
            // 但由于我们上面已经手动插入了 \\"，\\r\\n 等，所以需要精确处理：
            // 在替换之前，先将我们手动插入的转义序列保护起来，或者确保只替换原本就存在的未转义的反斜杠。
            // 对于您的 JSON 例子，这一步可以简化或省略，因为 JSON 中通常不包含未转义的反斜杠。

            // 5. 在字符串的头尾加上双引号，以完成 C# 字面量的格式
            escaped = "\"" + escaped + "\"";

            //MessageBox.Show(escaped);

            // 替换文件内容
            ReplaceDocumentText(escaped);
        }

        #endregion


        #region " Menu functions Unescape Literal " 
        /// <summary>
        /// 将 C# 字符串字面量（包含转义序列 \r\n, \t, \" 等）反转义为原始字符串。
        /// </summary>
        /// <returns>还原后的原始字符串</returns>
        internal static void UnescapeLiteral()
        {
            // 获取当前活动编辑器的句柄
            IntPtr scintillaHandle = PluginBase.GetCurrentScintilla();

            byte[] inputBytes = GetDocBytes(scintillaHandle);
            string inputText = Encoding.UTF8.GetString(inputBytes);

            if (string.IsNullOrEmpty(inputText))
            {
                return;
            }

            string result = inputText;

            // 1. 移除字符串字面量首尾的双引号（如果存在）
            if (result.Length >= 2 && result.StartsWith("\"") && result.EndsWith("\""))
            {
                // 移除首尾的第一个和最后一个双引号
                result = result.Substring(1, result.Length - 2);
            }

            // 2. 将转义的控制字符 (\r, \n, \t) 还原
            // 注意：C# 字符串在内存中已经自动处理了 \r, \n, \t，
            // 但如果它们是以字面量形式（即双反斜杠 \\r, \\n）存在，我们需要替换。
            // 但对于您给出的示例 "{\r\n...", 这些是单个反斜杠的转义序列。

            // 我们可以使用一个中间字符串来让 C# 编译器处理这些标准的转义序列。

            // 步骤 2.1: 将转义的双引号替换为单个双引号
            // 必须先处理 \\"，因为它不会被 C# 编译器自动还原
            // 注意：替换的字符串需要使用 C# 的逐字字符串 (@"")，以避免在代码中进行双重转义
            result = result.Replace("\\\"", "\"");

            // 步骤 2.2: 将转义的反斜杠替换为单个反斜杠
            // 这一步对于 JSON 路径或正则表达式中的反斜杠很重要
            result = result.Replace("\\\\", "\\");

            // 步骤 2.3: 替换标准转义字符，但由于这些在 C# 字符串中是标准的，
            // 我们可以利用 Regex.Unescape 或手动替换，为了简化且针对您的情况：

            // 注意：如果输入字面量如 "Hello\\nWorld"，则它在 C# 中表示 "Hello\nWorld"
            // 如果输入字面量如 "Hello\nWorld"，则它在 C# 中表示一个带换行的字符串

            // 假设您的输入是通过某个文本框/剪贴板获取的，并且其中的序列是双字符形式（\r, \n, \t）

            result = result.Replace("\\r\\n", "\r\n"); // Windows 换行
            result = result.Replace("\\n", "\n");     // Unix/Linux 换行
            result = result.Replace("\\r", "\r");     // 回车
            result = result.Replace("\\t", "\t");     // 制表符

            // 最终，使用 Regex.Unescape 确保处理所有标准的 C# 转义序列（如 \uXXXX 等）
            // 这一步是最可靠的通用方法。
            result = Regex.Unescape(result);

            //MessageBox.Show(result);

            // 替换文件内容
            ReplaceDocumentText(result);
        }

        #endregion


        #region " Menu functions JPReverse"

        [STAThread]
        internal static void ConvertKanjiToKana()
        {
            // 1. 获取当前选中的文本
            string kanzi = GetSelectionText();

            // **注意：在标准的NppPlugin.NET中，您可能需要使用 Npp.Editor.GetSelectedText()**
            // 假设 GetSelectionText() 已经正确获取了选中文本

            // 确定要替换的新文本 (这里以 IFELanguage 的结果为例)
            string katakana = string.Empty;

            IFELanguage ife = null;
            try
            {
                // === 读音获取逻辑 (保持不变) ===
                // Raw IFE Ver
                ife = Activator.CreateInstance(Type.GetTypeFromProgID("MSIME.Japan")) as IFELanguage;
                ife.Open();

                // 假设 GetJMorphResult 成功，并将片假名读音赋值给 katakana 变量
                // 这一步的 Marshal 操作非常底层且复杂，需要确保 result 指针的有效性
                // 简单起见，我们直接使用 'katakana' 变量来承载结果
                ife.GetJMorphResult(0x00030000, 0x00000008, kanzi.Length, kanzi, IntPtr.Zero, out IntPtr result);
                katakana = Marshal.PtrToStringUni(Marshal.ReadIntPtr(result, 4), Marshal.ReadInt16(result, 8));

                ReplaceSelectionText(katakana);
                // （您原代码中 MessageBox.Show(katakana); 这一行可以用于调试，实际发布时应移除）
                // 也可以选择使用更简单的 FELanguage Ver的结果
                // FELanguage fe = new FELanguage();
                // katakana = fe.GetKatakana(kanzi);

                // 提示用户操作成功 (可选)
                //MessageBox.Show($"元のテキスト: {kanzi}\n置換後: {katakana}", "日本語の読み仮名置換", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"読み仮名の取得または置換に失敗しました。\nエラー: {ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ife?.Close();
            }
        }

        #endregion


        #region " Menu functions Half-width to Full-width Conversion"
        //  全角文字（英数字、スペース、カタカナ）を半角に変換します。
        internal static void ConvertToFullWidth()
        {
            // 获取当前选中的文本
            string kanzi = GetSelectionText();

            string katakana = TextConverter.ToFullWidth(kanzi);

            ReplaceSelectionText(katakana);
        }

        #endregion


        #region " Menu functions Full-width to half-width conversion"
        //  全角文字（英数字、スペース、カタカナ）を半角に変換します。
        internal static void ConvertToHalfWidth()
        {
            // 获取当前选中的文本
            string kanzi = GetSelectionText();

            string katakana = TextConverter.ToHalfWidth(kanzi);

            ReplaceSelectionText(katakana);
        }

        #endregion

        

        #region " Private functions "


        // 假设 NppMsg 和 PluginBase 已定义在您的插件项目中
        private static string GetCurrentFilePath()
        {
            // 1. 获取 Notepad++ 主窗口的句柄
            // PluginBase.nppData 应该包含 Notepad++ 的主要数据结构
            IntPtr nppHandle = PluginBase.nppData._nppHandle;

            // 2. 为文件路径分配缓冲区 (Win32 API 通常使用 MAX_PATH，即 260)
            const int MAX_PATH = 260;
            StringBuilder pathBuilder = new StringBuilder(MAX_PATH);

            // 3. 发送消息 NPPM_GETFULLCURRENTPATH
            // wParam: 总是 0
            // lParam: 指向用于接收路径字符串的 StringBuilder 缓冲区
            Win32.SendMessage(
                nppHandle,
                NppMsg.NPPM_GETFULLCURRENTPATH,
                MAX_PATH,     // 缓冲区的大小作为 wParam (有些模板可能使用不同的参数顺序，但通常是缓冲区大小或 0)
                pathBuilder   // 缓冲区的引用
            );

            // 4. 返回获取到的字符串
            string filePath = pathBuilder.ToString();

           /* // 如果文件是新建但未保存的（例如 "new 1"），filePath 将返回空字符串或特殊名称。
            if (string.IsNullOrEmpty(filePath) || filePath.EndsWith("new 1"))
            {
                // 实际应用中，您需要处理文件未保存的情况
                return null; // 或者返回一个表示未保存文件的特殊值
            }*/

            return filePath;
        }

        /// <summary>
        /// 获取文档内容
        /// </summary>
        /// <returns>字节数组</returns>
        private static byte[] GetDocBytes(IntPtr scintillaHandle)
        {
            // 1. 获取整个文档的文本长度（不包含终止符 \0）
            int textLength = (int)Win32.SendMessage(scintillaHandle, SciMsg.SCI_GETTEXTLENGTH, 0, 0);

            if (textLength <= 0)
            {
                // 文档为空，直接返回空
                return null;
            }

            // 2. 分配非托管内存：长度 = 实际文本长度 + 1 (用于容纳终止符 \0)
            // Scintilla 在使用 SCI_GETTEXT 时，总是会在末尾添加 \0
            int bufferSize = textLength + 1;
            IntPtr textPtr = Marshal.AllocHGlobal(bufferSize);

            // 3. 发送 SCI_GETTEXT 消息，将内容复制到非托管内存
            // wparam 是 buffer size，lparam 是 buffer pointer
            Win32.SendMessage(scintillaHandle, SciMsg.SCI_GETTEXT, bufferSize, textPtr);

            // 将指针指向的内存块复制到 C# 字节数组中
            byte[] buffer = new byte[textLength];
            Marshal.Copy(textPtr, buffer, 0, textLength);

            // 5. 释放非托管内存
            Marshal.FreeHGlobal(textPtr);

            return buffer;
        }


        /// <summary>
        /// 获取选中内容
        /// </summary>
        /// <returns>字节数组</returns>
        private static byte[] GetSelectionBytes()
        { 
            // 1. 获取选中内容的长度（包含终止符 \0）
            int selectionLength = (int)Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_GETSELTEXT, 0, 0);

            /* 
             StringBuilder inputText = new StringBuilder(selectionLength);
             Win32.SendMessage(scintillaHandle, SciMsg.SCI_GETSELTEXT, 0, inputText);
             */

            // 2. 创建一个足够大的非托管内存块来接收数据
            IntPtr selectionPtr = Marshal.AllocHGlobal(selectionLength);

            // 3. 将选中的文本复制到非托管内存中
            Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_GETSELTEXT, 0, selectionPtr);

            // 4. 从非托管内存指针 (IntPtr) 中读取字节数组
            // 注意：selectionLength 包含了终止符 \0，读取时需要减去 1
            int byteCount = selectionLength > 0 ? selectionLength - 1 : 0;
            byte[] buffer = new byte[byteCount];

            if (byteCount > 0)
            {
                // 将指针指向的内存块复制到 C# 字节数组中
                Marshal.Copy(selectionPtr, buffer, 0, byteCount);
            }

            // 5. 释放非托管内存
            Marshal.FreeHGlobal(selectionPtr);

            return buffer;
        }


        /// <summary>
        /// 获取选中内容
        /// </summary>
        /// <returns>字节数组</returns>
        private static string GetSelectionText()
        {
            // 注意：SCI_GETCODEPAGE 消息返回的是 Scintilla 内部用于读取和写入文本的 CodePage ID
            int codePageID = (int)Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_GETCODEPAGE, 0, 0);

            // 获取选中内容的字节长度
            byte[] inputBytes = GetSelectionBytes();

            // 步骤 3: 根据检测到的编码从字节序列中获取正确的字符串
            string inputText = Encoding.UTF8.GetString(inputBytes);

            return inputText;
        }

        /// <summary>
        /// 获取编码
        /// </summary>
        /// <returns></returns>
        private static Encoding GetEncoding(int codePageID)
        {
            // 步骤 2: 获取正确的编码
            Encoding targetEncoding = Encoding.Default; // 默认编码

            // 4. 根据 CodePage ID 选择正确的解码方式

            if (codePageID == 0) // ANSI编码
            {
                targetEncoding = Encoding.ASCII;
            }
            else if (codePageID == 65001) // UTF-8 编码页
            {
                targetEncoding = Encoding.UTF8;
            }
            else // 其他编码页
            {
                try
                {
                    // 获取目标编码（如 GB2312, Shift-JIS 等）
                    targetEncoding = Encoding.GetEncoding(codePageID);
                }
                catch (ArgumentException)
                {
                    // 如果编码 ID 无效，回退到 UTF-8 或抛出错误
                    targetEncoding = Encoding.UTF8;
                }
            }

            return targetEncoding;
        }

        /// <summary>
        /// 替换选中文件内容
        /// </summary>
        private static void ReplaceSelectionText(string result)
        {
            // 字符串被编码为字节流
            byte[] outputBytes = Encoding.UTF8.GetBytes(result);
            WriteBytes(outputBytes, SciMsg.SCI_REPLACESEL);
        }

        /// <summary>
        /// 替换整个文件内容
        /// </summary>
        private static void ReplaceDocumentText(string result)
        {
            // 字符串被编码为字节流
            byte[] outputBytes = Encoding.UTF8.GetBytes(result);
            WriteBytes(outputBytes, SciMsg.SCI_SETTEXT);
        }

        private static void WriteBytes(byte[] outputBytes, SciMsg writeType)
        {
            // 使用 Notepad++ 插件框架提供的替换方法
            // 假设您已经能够访问到主编辑器对象（如 Npp.Editor 或 Scintilla 接口）
            // 方法A: 使用 Editor 对象的 ReplaceSelection 方法 (如果框架提供)
            // Npp.Editor.ReplaceSelection(katakana); 

            // 方法B: 直接发送 Scintilla 消息

            // 3. 为字节流分配非托管内存
            // 分配的长度 = 字节流长度 + 1 (用于 C 风格的终止符 '\0')
            IntPtr outputPtr = IntPtr.Zero; // 初始化指针

            try
            {
                outputPtr = Marshal.AllocHGlobal(outputBytes.Length + 1);

                // 4. 复制字节流到非托管内存
                // 将 outputBytes 数组从索引 0 开始，复制到 outputPtr 指针
                Marshal.Copy(outputBytes, 0, outputPtr, outputBytes.Length);

                // 5. 在字节流末尾添加 C 风格的终止符 \0
                // 终止符的位置是：字节流的长度（即最后一个字节的后一位）
                Marshal.WriteByte(outputPtr, outputBytes.Length, 0);

                // 6. 发送 替换 消息到 Scintilla
                // 替换当前选中区域（插入）  SciMsg.SCI_REPLACESEL
                // 替换整个文件内容          SciMsg.SCI_SETTEXT
                // Win32.SendMessage(scintillaHandle, SciMsg.SCI_REPLACESEL, 0, outputPtr);
                Win32.SendMessage(PluginBase.GetCurrentScintilla(), writeType, 0, outputPtr);
            }
            finally
            {
                // 7. 释放非托管内存 (非常重要，防止内存泄漏)
                if (outputPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(outputPtr);
                }
            }
        }


        #endregion
    }




}