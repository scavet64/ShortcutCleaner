using ShortcutCleaner.Controllers;
using ShortcutCleaner.Model;
using ShortcutCleaner.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ShortcutCleaner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<BrokenShortcutUserControl> BrokenShortcuts { get; set; } = new ObservableCollection<BrokenShortcutUserControl>();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            //searchForBrokenShortcuts(@"C:\Users\Vincent\AppData\Roaming\Microsoft\Windows\Start Menu\Programs");
            Thread myNewThread = new Thread(() => searchForBrokenShortcuts(@"D:\My Shit"));
            myNewThread.SetApartmentState(ApartmentState.STA);
            myNewThread.Start();

        }

        private void searchForBrokenShortcuts(string rootDirectory)
        {
            HashSet<BrokenShortcut> brokenShortcutSet = BrokenShortcutSearcher.GetAllBrokenShortcuts(rootDirectory, BrokenShortcuts);
            foreach (BrokenShortcut bs in brokenShortcutSet)
            {
                BrokenShortcutUserControl bsuc = new BrokenShortcutUserControl(bs);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
