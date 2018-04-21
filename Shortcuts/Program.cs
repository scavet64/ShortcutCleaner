using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SymbolicLinkSupport;
using IWshRuntimeLibrary;

namespace Shortcuts
{
    class Program
    {
        private static string shortcutTest = @"C:\Users\Vincent\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Nidhogg.lnk";
        private static string urlTest = @"D:\Users Folders\Desktop\(69) The IBM 1401 compiles and runs FORTRAN II - YouTube.url";
        private static string realFileTest = @"D:\Users Folders\Desktop\monika.chr";
        private static string brokenTest = @"C:\Users\Vincent\AppData\Roaming\Microsoft\Windows\Start Menu\Programs";

        static void Main(string[] args)
        {
            Console.WriteLine(isBrokenShortcut(shortcutTest));
            Console.WriteLine(isBrokenShortcut(urlTest));
            Console.WriteLine(isBrokenShortcut(realFileTest));
            Console.ReadLine();

            Console.WriteLine(GetAllBrokenShortcuts(brokenTest).Count);
            Console.ReadLine();
        }

        private static HashSet<FileInfo> GetAllBrokenShortcuts(string directoryToCheck)
        {
            HashSet<FileInfo> brokenShortcuts = new HashSet<FileInfo>();

            string [] subdirectories = Directory.GetDirectories(directoryToCheck);
            foreach (string subdir in subdirectories)
            {
                brokenShortcuts.UnionWith(GetAllBrokenShortcuts(subdir));
            }

            string[] files = Directory.GetFiles(directoryToCheck, "*.lnk");
            foreach (string file in files)
            {
                if (isBrokenShortcut(file))
                {
                    brokenShortcuts.Add(new FileInfo(file));
                }
            }

            return brokenShortcuts;
        }

        private static bool isBrokenShortcut(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Extension.Equals(".lnk", StringComparison.CurrentCultureIgnoreCase) && fileInfo.Exists)
            {
                WshShell shell = new WshShell();
                dynamic dynamicLink = shell.CreateShortcut(path);
                if (dynamicLink is IWshShortcut link)
                {
                    

                    if (!System.IO.File.Exists(link.TargetPath) && !Directory.Exists(link.TargetPath))
                    {
                        Console.WriteLine(path + " is a broken shortcut");
                        Console.WriteLine(link.TargetPath + " Does not exist\n");
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine(path + " is not really a shortcut");
                    return false;
                }
            }
            return false;
        }

        private static bool isUrl(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            if (HasCorrectExtension(fileInfo) && fileInfo.Exists)
            {
                WshShell shell = new WshShell();
                dynamic dynamicLink = shell.CreateShortcut(path);
                if (dynamicLink is IWshURLShortcut)
                {
                    Console.WriteLine(dynamicLink.TargetPath);
                    return true;
                }
                else
                {
                    Console.WriteLine(path + " is not a shortcut");
                    return false;
                }
            }
            return false;
        }

        private static bool HasCorrectExtension(FileInfo fileInfo)
        {
            bool isCorrect = false;
            if(fileInfo.Extension.Equals(".lnk", StringComparison.CurrentCultureIgnoreCase) || 
                fileInfo.Extension.Equals(".url", StringComparison.CurrentCultureIgnoreCase))
            {
                isCorrect = true;
            }
            return isCorrect;
        }
    }
}
