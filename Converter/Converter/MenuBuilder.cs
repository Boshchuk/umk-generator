using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Converter
{
    class MenuBuilder
    {
        private static int depth = 0;        

        public MenuBuilder(Folder f)
        {
            dirMenu(f);
        }

        public void dirMenu(Folder f)
        {
            depth++;
            string h = "";
            foreach (Folder item in f.folderList)
            {
                h = Global.pathToMenu + "\\" + Global.trimRootPath(f.name);
                Directory.CreateDirectory(h);
                dirMenu(item);
            }

            if (f.folderList.Count == 0)
            foreach (string item in f.fileList)
            {
                h = Global.pathToMenu + "\\" + Global.trimRootPath(f.name);
            }
            htmlDoc(f, depth, h);
            depth--;
        }
        public void htmlDoc(Folder f, int depth, string workDir)
        {
            StreamWriter sw = new StreamWriter(workDir + ".htm");

            Tag meta = new Tag("meta", false);
            meta.AddParam("http-equiv", "Content-Type");
            meta.AddParam("content", "text/html; charset=utf-8");

            Tag html = new Tag("html");
            Tag head = new Tag("head");
            Tag body = new Tag("body");
            html.AddChild(head);
            html.AddChild(body);
            head.AddChild(meta);

            Tag title = new Tag("title");
            title.AddText("Учреждение образования «Гродненский государственный университет имени Янки Купалы». Общевойсковая кафедра.");
            head.AddChild(title);

            Tag link = new Tag("link", false);
            link.AddParam("href", doDot(depth) + "index.files\style_all.css");
            link.AddParam("rel", "stylesheet");
            link.AddParam("type", "text/css");
            head.AddChild(link);

            string target_string;
            if (depth == 1)
            {
                target_string = "rightFrame";
            }
            else
            {
                target_string = "_self";
            }

            // пробегаем по всем папкам
            Tag ul = new Tag("ul");
            foreach (Folder fold in f.folderList)
            {
                // не заходим в папки, которые заканчиваются на .филес
                if (!fold.name.EndsWith(Global.foldersNotIncludeString))
                {
                    Tag li = new Tag("li");
                    Tag a = new Tag("a");
                    a.AddText(Global.getFileOrFolderName(fold.name));
                    a.AddParam("href", Global.getFileOrFolderName(f.name) + @"\" + Global.getFileOrFolderName(fold.name) + ".htm");
                    a.AddParam("target", target_string);

                    ul.AddChild(li);
                    li.AddChild(a);                        
                }
            }
            body.AddChild(ul);

            foreach (string s in f.fileList)
            {
                Tag li = new Tag("li");
                Tag a = new Tag("a");
                a.AddText(Global.getFileOrFolderName(Global.deleteParts(Global.trimRootPath(s), new string[] { ".ppt", ".htm", ".doc" })));
                a.AddParam("href", doDot(depth) + Global.trimRootPath(s));
                a.AddParam("target", target_string);

                ul.AddChild(li);
                li.AddChild(a);  
            }                
            sw.Write(html.ConvertToHTML());
            sw.Close();
        }

        /// <summary>
        ///  лепит точки чтобы путь получился относительный
        /// </summary>
        /// <param name="count">скока уровней вниз</param>
        /// <returns>строку со "скока нада" точек</returns>
        public string doDot(int count)
        {
            string res = "";
            for (int i = 0; i < count; i++)
            {
                res += @"..\";
            }
            return res;
        }
    }
}
