using System.IO;

namespace Converter
{
    internal class MenuBuilder
    {
        private static int _depth;

        public MenuBuilder(Folder f)
        {
            DirMenu(f);
        }

        public void DirMenu(Folder f)
        {
            _depth++;
            string h = "";
            foreach (Folder item in f.FolderList)
            {
                h = Global.PathToMenu + "\\" + Global.TrimRootPath(f.Name);
                Directory.CreateDirectory(h);
                DirMenu(item);
            }

            if (f.FolderList.Count == 0)
            foreach (string item in f.FileList)
            {
                h = Global.PathToMenu + "\\" + Global.TrimRootPath(f.Name);
            }
            HtmlDoc(f, _depth, h);
            _depth--;
        }

        public void HtmlDoc(Folder f, int depth, string workDir)
        {
            var streamWriter = new StreamWriter(workDir + ".htm");

            var meta = new Tag("meta", false);
            meta.AddParam("http-equiv", "Content-Type");
            meta.AddParam("content", "text/html; charset=utf-8");

            var html = new Tag("html");
            var head = new Tag("head");
            var body = new Tag("body");
            html.AddChild(head);
            html.AddChild(body);
            head.AddChild(meta);

            var title = new Tag("title");
            title.AddInnerText("Учреждение образования «Гродненский государственный университет имени Янки Купалы». Общевойсковая кафедра.");
            head.AddChild(title);

            Tag link = new Tag("link", false);
            link.AddParam("href", DoRelativePathDots(depth) + "index.files\\style_all.css");
            link.AddParam("rel", "stylesheet");
            link.AddParam("type", "text/css");
            head.AddChild(link);

            var targetString = depth == 1 ? "rightFrame" : "_self";

            // пробегаем по всем папкам
            var ul = new Tag("ul");
            foreach (Folder fold in f.FolderList)
            {
                // не заходим в папки, которые заканчиваются на .филес
                if (!fold.Name.EndsWith(Global.FoldersNotIncludeString))
                {
                    Tag li = new Tag("li");
                    Tag a = new Tag("a");
                    a.AddInnerText(Global.GetFileOrFolderName(fold.Name));
                    a.AddParam("href", 
                              $@"{Global.GetFileOrFolderName(f.Name)}\{Global.GetFileOrFolderName(fold.Name)}.htm");
                    a.AddParam("target", targetString);

                    ul.AddChild(li);
                    li.AddChild(a);
                }
            }
            body.AddChild(ul);

            foreach (var s in f.FileList)
            {
                var li = new Tag("li");
                var a = new Tag("a");
                a.AddInnerText(Global.GetFileOrFolderName(Global.DeleteParts(Global.TrimRootPath(s), new string[] { ".ppt", ".htm", ".doc" })));
                a.AddParam("href", DoRelativePathDots(depth) + Global.TrimRootPath(s));
                a.AddParam("target", targetString);

                ul.AddChild(li);
                li.AddChild(a);
            }
            streamWriter.Write(html.ConvertToHtml());
            streamWriter.Close();
        }

        /// <summary>
        ///  лепит точки чтобы путь получился относительный
        /// </summary>
        /// <param name="count">скока уровней вниз</param>
        /// <returns>строку со "скока нада" точек</returns>
        public string DoRelativePathDots(int count)
        {
            var res = string.Empty;
            for (var i = 0; i < count; i++)
            {
                res += @"..\";
            }
            return res;
        }
    }
}
