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
            htmlDoc hdoc = new htmlDoc(f, depth, h);
            depth--;
        }

        class htmlDoc
        {
            public struct Param
            {
                public string name;
                public string value;

                public Param(string n, string v)
                {
                    name = n;
                    value = v;
                }
            }

            string metaConst = @"<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">";
            string menu ="";

            public htmlDoc(Folder f, int depth, string workDir)
            {
                StreamWriter sw = new StreamWriter(workDir + ".htm");

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
                foreach (Folder fold in f.folderList)
                {
                    // не заходим в папки, которые заканчиваются на .филес
                    if (!fold.name.EndsWith(Global.foldersNotIncludeString))
                    {
                        menu += doTag("ul", doTag("li", doTag("a", Global.getFileOrFolderName(fold.name), new Param[] { new Param("href", Global.getFileOrFolderName(f.name) + @"\" + Global.getFileOrFolderName(fold.name) + ".htm"), new Param("target", target_string) })));
                    }
                }

                foreach (string s in f.fileList)
                {
                    menu += doTag("ul", doTag("li", doTag("a", Global.getFileOrFolderName(Global.deleteParts(Global.trimRootPath(s), new string[] { ".ppt", ".htm", ".doc" })), new Param[] { new Param("href", doDot(depth) + Global.trimRootPath(s)), new Param("target", target_string) })));
                }

                string body = doTag("html", doTag("head", doTag("title", "Учреждение образования «Гродненский государственный университет имени Янки Купалы». Общевойсковая кафедра.") + metaConst + linkToCss(depth)) + menu);

                sw.Write(body);
                sw.Close();
            }

            /// <summary>
            ///  лепит точки чтобы путь получился относительный
            /// </summary>
            /// <param name="count"></param>
            /// <returns></returns>
            public string doDot(int count)
            {
                string res="";
                for (int i=0; i<count; i++)
                {
                    res+=@"..\";
                }
                return res;
            }

            public string linkToCss(int depth)
            {
                string dot = "";
                for (int i = 0; i  < depth; i++)
                {
                    dot += @"..\";
                }
                return (string.Format(@"<link href=""{0}index.files/style_all.css"" rel=""stylesheet"" type=""text/css"" >",dot));
            }

            public string doTag(string tag, string text)
            {
                string res = string.Format("<{0}>{1}</{0}>", tag, text);
                return res;
            }

            public string doTag(string tag, string text, Param[] paramSet)
            {
                string res = string.Format("<{0} ",tag);
                foreach (Param p in paramSet)
                {
                    res += string.Format(@"{0}=""{1}"" ", p.name, p.value);
                }
                res += string.Format(">{0}</{1}>", text, tag);

                return res;
            }
        }
    }
}
