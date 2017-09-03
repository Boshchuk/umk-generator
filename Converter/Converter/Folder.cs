using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Converter
{
    internal class Folder
    {
        /// <summary>
        /// Файлы, находящиеся в данном каталоге
        /// </summary>
        public List<string> FileList { get; }

        /// <summary>
        /// Подкаталоги, находящиеся в данном каталоге
        /// </summary>
        public List<Folder> FolderList { get; }

        /// <summary>
        /// Имя этого каталога
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Глубина его "вложенности" относительно корневого каталога
        /// </summary>
        public int Depth { get; } = 0;

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

            string[] dirs;

            try // сканим на подкаталоги
            {
                dirs = Directory.GetDirectories(Name, Global.FoldersSearchTemplate);
            }
            catch (UnauthorizedAccessException) // если нельзя этого делать (например, SystemVolumeInformation)
            {
                MessageBox.Show("Попытка чтения каталога, доступ к которому запрещен.",
                                "Внимание!",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Warning);
                return;
            }

            foreach (var i in dirs) // Залазим в каждый из подкаталогов
            {
                FolderList.Add(new Folder(i, Depth+1));
            }
            foreach (var j in Directory.GetFiles(Name, Global.FileSearchPattern)) // Запоминаем все файлы в каталоге
            {   
                FileList.Add(j);
            }
        }

        /// <summary>
        /// Заполняет TreeNode полным вариантом ссылок на файлы и папки. То есть TreeNode содержит полные пути к папкам\файлам
        /// </summary>
        /// <param name="tn">Корневой узел TreeNode</param>
        public void InitFullTree(TreeNode tn)
        {
            foreach (var i in FolderList)
            {
                var nodeChild = new TreeNode(i.Name);
                tn.Nodes.Add(nodeChild);
                i.InitFullTree(nodeChild);
            }
            foreach (var j in FileList)
            {
                tn.Nodes.Add(j);
            }
            
            if (FileList.Count == 0 && FolderList.Count == 0)
            {
                tn.Nodes.Add(Global.EmptyFolder);
            }
        }
        /// <summary>
        /// Заполняет TreeNode сокращенным вариантом ссылок на файлы и папки.
        /// Каждый узел TreeNode содержит имя файла\каталога
        /// </summary>
        /// <param name="tn">Корневой узел TreeNode</param>
        public void initShortTree(TreeNode tn)
        {
            foreach (var i in FolderList)
            {
                var node_child = new TreeNode(Global.GetFileOrFolderName(i.Name));
                tn.Nodes.Add(node_child);
                i.initShortTree(node_child);
            }
            foreach (string j in FileList)
            {
                tn.Nodes.Add(Global.GetFileOrFolderName(j));
            }

            if (FileList.Count == 0 && FolderList.Count == 0)
            {
                tn.Nodes.Add(Global.EmptyFolder);
            }
        }

        /// <summary>
        /// Заполняет TreeNode сокращенным вариантом ссылок на файлы и папки.
        /// Каждый узел TreeNode содержит имя файла\каталога. Плюс к этому из имен файлов вырезается расширение, а папки, не входящие в структуру меню, не включаются в структуру TreeNode.
        /// </summary>
        /// <param name="tn">Корневой узел TreeNode</param>
        public void initShortTree(TreeNode tn, string template, string[] extensions)
        {
            foreach (Folder i in FolderList)
            {
                if (i.Name.LastIndexOf(template) == -1)
                {
                    TreeNode node_child = new TreeNode(Global.GetFileOrFolderName(i.Name));
                    tn.Nodes.Add(node_child);
                    i.initShortTree(node_child, template, extensions);
                }
            }
            foreach (string j in FileList)
            {
                tn.Nodes.Add(Global.GetFileOrFolderName(Global.DeleteParts(j, extensions)));
            }

            if ((FileList.Count == 0) && (FolderList.Count == 0))
            {
                tn.Nodes.Add(Global.EmptyFolder);
            }
        }

    }
}