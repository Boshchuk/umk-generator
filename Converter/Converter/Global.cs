using System;
using System.Collections.Generic;
using System.Text;

namespace Converter
{
    public static class Global
    {
        /// <summary>
        /// Шаблон поиска для файлов
        /// </summary>
        public static string fileSearchPattern = "*.*";
        /// <summary>
        /// Шаблон поиска для папок
        /// </summary>
        public static string foldersSearchTemplate = "*.*";
        /// <summary>
        /// Текст, который обозначает что данная папка пуста
        /// </summary>
        public static string emptyFolder = "<нет элементов>";
        /// <summary>
        /// Кусок строки в именах папок, которые не включаются в структуру меню. Пока что это тупо .files
        /// </summary>
        public static string foldersNotIncludeString = ".files";

        private static string PathRoot;
        /// <summary>
        /// Корневой каталог для данного проекта
        /// </summary>
        public static string pathRoot
        {
            get
            {
                return PathRoot;
            }
            set
            {
                PathRoot = value;
            }
        }
        /// <summary>
        /// Путь к каталогу, хранящему файлы меню
        /// </summary>
        public static string pathToMenu;

        /// <summary>
        /// Отрезает от строки имя файла\папки
        /// </summary>
        /// <param name="source">Исходная строка</param>
        /// <returns>Имя файла\папки</returns>
        public static string getFileOrFolderName(string source)
        {
            int p1 = source.LastIndexOf("\\") + 1;
            string t;
            if (p1 > 0)
            {
                t = source.Substring(p1);
            }
            else
            {
                t = source;
            }
            return t;
        }
        /// <summary>
        /// Убирает из строки куски указанные в параметре
        /// </summary>
        /// <param name="source">Исходная строка</param>
        /// <param name="parts">Массив кусков строк</param>
        /// <returns>Строка без расширений</returns>
        public static string deleteParts(string source, string[] parts)
        {
            int i = 0;
            foreach (string s in parts)
            {
                i = source.LastIndexOf(s);
                if (i > 0)
                {
                    return source.Substring(0, i);
                }
            }            
            return source;
        }
        /// <summary>
        /// Делает путь относительным (вырезает из него пусть к корневому каталогу)
        /// </summary>
        /// <param name="source">Исходная путь</param>
        /// <returns>Относительный путь</returns>
        public static string trimRootPath(string source)
        {
            int i = PathRoot.Length;
            int j = source.Length;
            source = source.Substring(i, j-i);
            return source;
        }
    }
}
