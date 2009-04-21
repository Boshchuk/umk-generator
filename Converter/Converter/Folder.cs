using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace Converter
{
    class Folder
    {
        private List<string> FileList;
        private List<Folder> FolderList;
        private string Name;
        private int Depth = 0;

        /// <summary>
        /// Файлы, находящиеся в данном каталоге
        /// </summary>
        public List<string> fileList
        {
            get
            {
                return FileList;
            }
        }
        /// <summary>
        /// Подкаталоги, находящиеся в данном каталоге
        /// </summary>
        public List<Folder> folderList
        {
            get
            {
                return FolderList;
            }
        }
        /// <summary>
        /// Имя этого каталога
        /// </summary>
        public string name
        {
            get
            {
                return Name;
            }
        }
        /// <summary>
        /// Глубина его "вложенности" относительно корневого каталога
        /// </summary>
        public int depth
        {
            get
            {
                return Depth;
            }
        }

        /// <summary>
        /// Конструктор объекта "каталог"
        /// </summary>
        /// <param name="path">Физический путь</param>
        /// <param name="d">Глубина "вложенности" относительно корневого каталога</param>
        public Folder(string path, int d) 
        {
            FolderList = new List<Folder>();
            FileList = new List<string>();

            Depth = d;
            Name = path;

            string[] Dirs;

            try // сканим на подкаталоги
            {
                Dirs = Directory.GetDirectories(Name, Global.foldersSearchTemplate);
            }
            catch (UnauthorizedAccessException) // если нельзя этого делать (например, SystemVolumeInformation)
            {
                MessageBox.Show("Попытка чтения каталога, доступ к которому запрещен.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (string i in Dirs) // Залазим в каждый из подкаталогов
            {
                FolderList.Add(new Folder(i, Depth+1));
            }
            foreach (string j in Directory.GetFiles(Name, Global.fileSearchPattern)) // Запоминаем все файлы в каталоге
            {   
                //FileList.Add(j.Replace('\\', '/'));
                FileList.Add(j);
            }
        }

        /// <summary>
        /// Заполняет TreeNode полным вариантом ссылок на файлы и папки. То есть TreeNode содержит полные пути к папкам\файлам
        /// </summary>
        /// <param name="tn">Корневой узел TreeNode</param>
        public void initFullTree(TreeNode tn)
        {
            foreach (Folder i in FolderList)
            {
                TreeNode node_child = new TreeNode(i.Name);
                tn.Nodes.Add(node_child);
                i.initFullTree(node_child);   
            }
            foreach (string j in FileList)
            {
                tn.Nodes.Add(j);
            }
            
            if ((FileList.Count == 0) || (FolderList.Count == 0))
            {
                tn.Nodes.Add(Global.emptyFolder);
            }
        }
        /// <summary>
        /// Заполняет TreeNode сокращенным вариантом ссылок на файлы и папки. Каждый узел TreeNode содержит имя файла\каталога
        /// </summary>
        /// <param name="tn">Корневой узел TreeNode</param>
        public void initShortTree(TreeNode tn)
        {
            foreach (Folder i in FolderList)
            {
                TreeNode node_child = new TreeNode(Global.getFileOrFolderName(i.Name));
                tn.Nodes.Add(node_child);
                i.initShortTree(node_child);
            }
            foreach (string j in FileList)
            {
                tn.Nodes.Add(Global.getFileOrFolderName(j));
            }

            if ((FileList.Count == 0) && (FolderList.Count == 0))
            {
                tn.Nodes.Add(Global.emptyFolder);
            }
        }

        /// <summary>
        /// Заполняет TreeNode сокращенным вариантом ссылок на файлы и папки. Каждый узел TreeNode содержит имя файла\каталога. Плюс к этому из имен файлов вырезается расширение, а папки, не входящие в структуру меню, не включаются в структуру TreeNode.
        /// </summary>
        /// <param name="tn">Корневой узел TreeNode</param>
        public void initShortTree(TreeNode tn, string template, string[] extensions)
        {
            foreach (Folder i in FolderList)
            {
                if (i.Name.LastIndexOf(template) == -1)
                {
                    TreeNode node_child = new TreeNode(Global.getFileOrFolderName(i.Name));
                    tn.Nodes.Add(node_child);
                    i.initShortTree(node_child, template, extensions);
                }
            }
            foreach (string j in FileList)
            {
                tn.Nodes.Add(Global.getFileOrFolderName(Global.trimExtension(j, extensions)));
            }

            if ((FileList.Count == 0) && (FolderList.Count == 0))
            {
                tn.Nodes.Add(Global.emptyFolder);
            }
        }

    }
}