using System;
using System.Windows.Forms;
using System.IO;

namespace Converter
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Структура файлов и папок
        /// </summary>
        private Folder _mfs;
        private TreeNode _fullTree;
        private TreeNode _shortTree;
        private MenuBuilder _myMenu;

        public Form1()
        {
            InitializeComponent();
        }

        private void DoStartup()
        {
            Global.PathRoot = "";
            RenderToLabel(label1, "Текущий каталог: \n" + Global.PathRoot);
        }

        private void RenderToLabel(Label screen, string source)
        {
            screen.Text = source;
        }

        private void doChooseRootFolder()
        {
            folderBrowserDialog1.SelectedPath = Global.PathRoot;
            folderBrowserDialog1.ShowDialog();

            Global.PathRoot = folderBrowserDialog1.SelectedPath;
            Global.PathToMenu = Global.PathRoot + "\\Меню";
            RenderToLabel(label1, Global.PathRoot);
        }
        // в корневой папке должна быть папка 'Содержимое'
        // в ней и проводится анализ
        private void DoForest() // создаем деревья (все: полные, сокращенные)
        {
            if (Global.PathRoot.IndexOf("\\Содержимое", StringComparison.Ordinal) != -1)
            {
                Global.PathRoot = Global.DeleteParts(Global.PathRoot, new string[] { "\\Содержимое" });
            }
            try
            {
                Directory.Delete(Global.PathToMenu, true);
            }
            catch (IOException exception)
            {
                MessageBox.Show(exception.Message + "Не могу обновить меню. Некоторые файлы меню открыты и не могут быть обработаны. Закройте все файлы и папки из папки Меню и повторите попытку", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            _fullTree = new TreeNode(Global.PathRoot);
            _shortTree = new TreeNode(Global.PathRoot);
            
            _mfs = new Folder(Global.PathRoot + "\\Содержимое", 0);
            _mfs.InitFullTree(_fullTree);
            _mfs.initShortTree(_shortTree, Global.FoldersNotIncludeString, new string[] { ".ppt", ".htm", ".doc",".html" });

            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(_shortTree);
            treeView2.Nodes.Add(_fullTree);
        }

        private void DoMenu()
        {
            _myMenu = new MenuBuilder(_mfs);
        }

        private void button1_Click(object sender, EventArgs e) // кнопка "Выбрать папку"
        {
            doChooseRootFolder();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DoStartup();
        }

        private void ButtonBuild_Click(object sender, EventArgs e)
        {
            if (Global.PathRoot != string.Empty)
            {
                DoForest();
                DoMenu();
            }
            else
            {
                MessageBox.Show("не выбран корневой каталог!", "ошибка!");
            }
        }
    }
}
