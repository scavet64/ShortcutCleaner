using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortcutCleaner.Model
{
    public class BrokenShortcut
    {
        public string ShortcutPath { get; set; }
        public string TargetPath { get; set; }

        private FileInfo fileInfo;


        public BrokenShortcut(string shortcutPath, string targetPath, FileInfo fileInfo)
        {
            ShortcutPath = shortcutPath;
            TargetPath = targetPath;
            this.fileInfo = fileInfo;
        }
    }
}
