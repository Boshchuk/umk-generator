using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Converter
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Структура файлов и папок
        /// </summary>
        private Folder mfs;
        private TreeNode fullTree, shortTree;
        private MenuBuilder myMenu;

        public Form1()
        {
            InitializeComponent();
        }

        private void doStartup()
        {
            Global.pathRoot = "";
            RenderToLabel(label1, "Текущий каталог: \n" + Global.pathRoot);
        }

        private void RenderToLabel(Label screen, string source)
        {
            screen.Text = source;
        }

        private void doChooseRootFolder()
        {
            folderBrowserDialog1.SelectedPath = Global.pathRoot;
            folderBrowserDialog1.ShowDialog();

            Global.pathRoot = folderBrowserDialog1.SelectedPath;
            Global.pathToMenu = Global.pathRoot + "\\Меню";
            RenderToLabel(label1, Global.pathRoot);
        }

        private void doForest() // создаем деревья (все: полные, сокращенные)
        {
            if (Global.pathRoot.IndexOf("\\Содержимое") != -1)
            {
                Global.pathRoot = Global.deleteParts(Global.pathRoot, new string[] { "\\Содержимое" });
            }
            try
            {
                Directory.Delete(Global.pathToMenu, true);
            }
            catch (IOException)
            {
                MessageBox.Show("Не могу обновить меню. Некоторые файлы меню открыты и не могут быть обработаны. Закройте все файлы и папки из папки Меню и повторите попытку", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            fullTree = new TreeNode(Global.pathRoot);
            shortTree = new TreeNode(Global.pathRoot);
            
            mfs = new Folder(Global.pathRoot + "\\Содержимое", 0);
            mfs.initFullTree(fullTree);
            mfs.initShortTree(shortTree, Global.foldersNotIncludeString, new string[] { ".ppt", ".htm", ".doc" });

            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(shortTree);
            treeView2.Nodes.Add(fullTree);
        }

        private void doMenu()
        {
            myMenu = new MenuBuilder(mfs);
        }

        private void button1_Click(object sender, EventArgs e) // кнопка "Выбрать папку"
        {
            doChooseRootFolder();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            doStartup();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Global.pathRoot != "")
            {
                doForest();
                doMenu();
            }
            else
            {
                MessageBox.Show("не выбран корневой каталог!", "ошибка!");
            }
        }
    }
}
