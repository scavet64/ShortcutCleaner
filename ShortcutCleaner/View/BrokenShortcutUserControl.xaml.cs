using ShortcutCleaner.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShortcutCleaner.View
{
    /// <summary>
    /// Interaction logic for BrokenShortcutUserControl.xaml
    /// </summary>
    public partial class BrokenShortcutUserControl : UserControl, INotifyPropertyChanged
    {
        public BrokenShortcut BrokenShortcutObject { get; set; }
        public string test { get; set; } = "test";

        public BrokenShortcutUserControl(BrokenShortcut brokenShortcutObject)
        {
            InitializeComponent();
            this.TargetPath.Content = brokenShortcutObject.TargetPath;
            this.ShortcutPath.Content = brokenShortcutObject.ShortcutPath;
            BrokenShortcutObject = brokenShortcutObject;
        }

        #region INotifiedProperty Block        
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed]. Used in data binding
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
