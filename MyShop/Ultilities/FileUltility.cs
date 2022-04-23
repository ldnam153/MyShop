using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyShop.Ultilities
{
    static public class FileUltility
    {
        static public string CreateNewImagePath(string sourceFile)
        {
            string relativePath = "Resources\\Assets\\" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + Path.GetExtension(sourceFile);
            return relativePath;
        }
        static public bool CopyImage(string source, string relativePath)
        {
            try
            {
                string folder = AppDomain.CurrentDomain.BaseDirectory;
                string absolutePath = $"{folder}{relativePath}";
                File.Copy(source, absolutePath);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        static public bool IsExists(string path)
        {
            return File.Exists(path);
        }
    }
}
