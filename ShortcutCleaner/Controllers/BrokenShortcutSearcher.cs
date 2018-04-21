using IWshRuntimeLibrary;
using ShortcutCleaner.Model;
using ShortcutCleaner.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ShortcutCleaner.Controllers
{
    public static class BrokenShortcutSearcher
    {
        public static HashSet<BrokenShortcut> GetAllBrokenShortcuts(string directoryToCheck, ObservableCollection<BrokenShortcutUserControl> observableCollection)
        {
            HashSet<BrokenShortcut> brokenShortcuts = new HashSet<BrokenShortcut>();

            string[] subdirectories = Directory.GetDirectories(directoryToCheck);
            foreach (string subdir in subdirectories)
            {
                brokenShortcuts.UnionWith(GetAllBrokenShortcuts(subdir, observableCollection));
            }

            string[] files = Directory.GetFiles(directoryToCheck, "*.lnk");
            foreach (string file in files)
            {
                BrokenShortcut brokenShortcut = IsBrokenShortcut(file);
                if (brokenShortcut != null)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                          DispatcherPriority.Background,
                          new Action(() => observableCollection.Add(new BrokenShortcutUserControl(brokenShortcut))));
                    brokenShortcuts.Add(brokenShortcut);
                }
            }

            return brokenShortcuts;
        }

        private static BrokenShortcut IsBrokenShortcut(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Extension.Equals(".lnk", StringComparison.CurrentCultureIgnoreCase) && fileInfo.Exists && !IsSpecialShortcut(fileInfo.Name))
            {
                WshShell shell = new WshShell();
                dynamic dynamicLink = shell.CreateShortcut(path);
                if (dynamicLink is IWshShortcut link)
                {


                    if (!System.IO.File.Exists(link.TargetPath) && !Directory.Exists(link.TargetPath))
                    {
                        Console.WriteLine(path + " is a broken shortcut");
                        Console.WriteLine(link.TargetPath + " Does not exist\n");
                        return new BrokenShortcut(path, link.TargetPath, fileInfo);
                    }
                }
                else
                {
                    Console.WriteLine(path + " is not really a shortcut");
                    return null;
                }
            }
            return null;
        }

        private static bool IsUrl(string path)
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

        private static bool IsSpecialShortcut(string path)
        {
            switch (path.ToLower())
            {
                case "control panel.lnk":
                    break;
                case "file explorer.lnk":
                    break;
                case "computer.lnk":
                    break;
                case "run.lnk":
                    break;
                case "this pc.lnk":
                    break;
                default:
                    return false;
            }

            return true;
        }

        private static bool HasCorrectExtension(FileInfo fileInfo)
        {
            bool isCorrect = false;
            if (fileInfo.Extension.Equals(".lnk", StringComparison.CurrentCultureIgnoreCase) ||
                fileInfo.Extension.Equals(".url", StringComparison.CurrentCultureIgnoreCase))
            {
                isCorrect = true;
            }
            return isCorrect;
        }
    }
}
