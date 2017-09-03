using System;

namespace Converter
{
    public static class Global
    {
        /// <summary>
        /// Шаблон поиска для файлов
        /// </summary>
        public static string FileSearchPattern = "*.*";
        /// <summary>
        /// Шаблон поиска для папок
        /// </summary>
        public static string FoldersSearchTemplate = "*.*";
        /// <summary>
        /// Текст, который обозначает что данная папка пуста
        /// </summary>
        public static string EmptyFolder = "<нет элементов>";
        /// <summary>
        /// Кусок строки в именах папок, которые не включаются в структуру меню. Пока что это тупо .files
        /// </summary>
        public static string FoldersNotIncludeString = ".files";

        /// <summary>
        /// Корневой каталог для данного проекта
        /// </summary>
        public static string PathRoot { get; set; }

        /// <summary>
        /// Путь к каталогу, хранящему файлы меню
        /// </summary>
        public static string PathToMenu;

        /// <summary>
        /// Отрезает от строки имя файла\папки
        /// </summary>
        /// <param name="source">Исходная строка</param>
        /// <returns>Имя файла\папки</returns>
        public static string GetFileOrFolderName(string source)
        {
            var p1 = source.LastIndexOf("\\", StringComparison.Ordinal) + 1;
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
        public static string DeleteParts(string source, string[] parts)
        {
            foreach (var s in parts)
            {
                var i = source.LastIndexOf(s, StringComparison.Ordinal);
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
        public static string TrimRootPath(string source)
        {
            var i = PathRoot.Length + 1;
            var j = source.Length;
            source = source.Substring(i, j - i);
            return source;
        }
    }
}
