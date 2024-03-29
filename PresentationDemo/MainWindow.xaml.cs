﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PresentationDemo {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            var items = new ObservableCollection<TreeItem>();

            var vm = new { TreeItems =  items};
            
            this.DataContext = vm;

            var treeItem = new TreeItem { IsChecked = true, Text = "Hello TreeView" };
            treeItem.Children.Add(new TreeItem { IsChecked = false, Text = "Hello Child" });
            items.Add(treeItem);
            items.Add(new TreeItem { IsChecked = false, Text = "Hello TreeView2" });
        }
    }
}
