using Microsoft.Win32;
using RingingBloom.WWiseTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RingingBloom.Windows
{
    /// <summary>
    /// Interaction logic for EPVSPEditor.xaml
    /// </summary>
    public partial class EPVSPEditor : Window
    {
        private string ImportPath;
        private string ExportPath;
        public EPVSPFile epvsp { get; set; }
        public EPVSPEditor(Options options)
        {
            InitializeComponent();
            if (options.defaultImport != null)
            {
                ImportPath = options.defaultImport;
            }
            if (options.defaultImport != null)
            {
                ExportPath = options.defaultExport;
            }
        }

        public void ImportEPVSP(object sender, RoutedEventArgs e)
        {
            OpenFileDialog importFile = new OpenFileDialog();
            if (ImportPath != null)
            {
                importFile.InitialDirectory = ImportPath;
            }
            importFile.Multiselect = false;
            importFile.Filter = "MHW EPV Sound Param (*.epvsp)|*.epvsp";
            if (importFile.ShowDialog() == true)
            {
                BinaryReader readFile = HelperFunctions.OpenFile(importFile.FileName);
                epvsp = new EPVSPFile(readFile);
                readFile.Close();
            }
            PopulateTreeView();
        }

        public void CreateEPVSP(object sender, RoutedEventArgs e)
        {
            epvsp = new EPVSPFile();
            PopulateTreeView();
        }

        public void ExportEPVSP(object sender, RoutedEventArgs e)
        {
            if(epvsp == null)
            {
                MessageBox.Show("EPVSP file not currently loaded");
                return;
            }
            SaveFileDialog exportFile = new SaveFileDialog();
            if(ExportPath != null)
            {
                exportFile.InitialDirectory = ExportPath;
            }
            exportFile.Filter = "MHW EPV Sound Param (*.epvsp)|*.epvsp";
            if (exportFile.ShowDialog() == true)
            {
                epvsp.Export(new BinaryWriter(new FileStream(exportFile.FileName,FileMode.OpenOrCreate)));
            }
        }
        public void PopulateTreeView()
        {
            WWCT.Text = epvsp.WWCTPath;//this is the second half to the pseudo-binding of the WWCT path, because fuck actual binding
            bool requestExtend = false;
            bool triggerExtend = false;
            if (treeView1.Items.Count > 0)
            {
                //grab current status of header items
                for(int i = 0; i < treeView1.Items.Count; i++)
                {
                    TreeViewItem current = (TreeViewItem)treeView1.Items[i];
                    List<string> tag = (List<string>)current.Tag;
                    if (tag[1] == "-1")
                    {
                        if(tag[0] == "RD")
                        {
                            requestExtend = current.IsExpanded;
                        }
                        if(tag[0] == "TD")
                        {
                            triggerExtend = current.IsExpanded;
                        }
                    }
                }
                treeView1.Items.Clear();
            }
            TreeViewItem requestDatas = new TreeViewItem();
            requestDatas.Header = "Request Datas";
            requestDatas.Foreground = HelperFunctions.GetBrushFromHex("#AAAAAA");
            requestDatas.IsExpanded = requestExtend;
            List<string> rdTag = new List<string>();
            rdTag.Add("RD");
            rdTag.Add("-1");
            requestDatas.Tag = rdTag;
            for(int i = 0; i < epvsp.requestDatas.Count; i++)
            {
                TreeViewItem rd = new TreeViewItem();
                rd.Header = "Request Data " + i.ToString();//TODO: make this better with information based on the data itself, make a new TreeViewItem equivalent that inherits INotifyChanged?
                List<string> tag = new List<string>();
                tag.Add("RD");
                tag.Add(i.ToString());
                rd.Tag = tag;
                rd.Foreground = HelperFunctions.GetBrushFromHex("#AAAAAA");
                requestDatas.Items.Add(rd);

            }
            treeView1.Items.Add(requestDatas);
            TreeViewItem triggerDatas = new TreeViewItem();
            triggerDatas.Header = "Trigger Datas";
            triggerDatas.Foreground = HelperFunctions.GetBrushFromHex("#AAAAAA");
            triggerDatas.IsExpanded = triggerExtend;
            List<string> tdTag = new List<string>();
            tdTag.Add("TD");
            tdTag.Add("-1");
            triggerDatas.Tag = tdTag;
            for (int i = 0; i < epvsp.triggerDatas.Count; i++)
            {
                TreeViewItem td = new TreeViewItem();
                td.Header = "Trigger Data " + i.ToString();
                List<string> tag = new List<string>();
                tag.Add("TD");
                tag.Add(i.ToString());
                td.Tag = tag;
                td.Foreground = HelperFunctions.GetBrushFromHex("#AAAAAA");
                triggerDatas.Items.Add(td);

            }
            treeView1.Items.Add(triggerDatas);
        }
        private void treeView1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Make sure this is the right button.
            if (e.RightButton != MouseButtonState.Pressed) return;

            TreeViewItem currentItem = treeView1.SelectedItem as TreeViewItem;

            if (currentItem == null) return;
            if (currentItem.Tag != null)
            {
                List<string> tag = currentItem.Tag as List<string>;
                switch (tag[0])
                {
                    case "RD":
                        if (tag[1] != "-1")
                        {
                            treeView1.ContextMenu = FindResource("ResourceRightClick") as ContextMenu;
                            treeView1.ContextMenu.Tag = tag;
                        }
                        else
                        {
                            treeView1.ContextMenu = FindResource("RDRightClick") as ContextMenu;
                            treeView1.ContextMenu.Tag = tag;
                        }
                        break;
                    case "TD":
                        if (tag[1] != "-1")
                        {
                            treeView1.ContextMenu = FindResource("ResourceRightClick") as ContextMenu;
                            treeView1.ContextMenu.Tag = tag;
                        }
                        else
                        {
                            treeView1.ContextMenu = FindResource("TDRightClick") as ContextMenu;
                            treeView1.ContextMenu.Tag = tag;
                        }
                        break;
                    default:
                        break;
                }
            }

        }
        public void treeView1_SelectedItemChanged(object sender, RoutedEventArgs e)
        {
            if (treeView1.SelectedItem != null)
            {
                TreeViewItem ti = (TreeViewItem)treeView1.SelectedItem;
                List<string> tag = (List<string>)ti.Tag;
                if (tag[0] == "RD")
                {
                    if (tag[1] != "-1")//block it from trying to look at the "Data Index" tree item
                    {
                        ContentController.Content = epvsp.requestDatas[Convert.ToInt32(tag[1])];
                    }
                }
                else if (tag[0] == "TD")
                {
                    if (tag[1] != "-1")//block it from trying to look at the "Data Index" tree item
                    {
                        ContentController.Content = epvsp.triggerDatas[Convert.ToInt32(tag[1])];
                    }
                }
                else
                {
                    ContentController.Content = null;
                }
            }
            else
            {
                ContentController.Content = null;
            }

        }
        public void AddTrigger(object sender, RoutedEventArgs e)
        {
            epvsp.triggerDatas.Add(new TriggerData());
            PopulateTreeView();
        }
        public void AddRequest(object sender, RoutedEventArgs e)
        {
            epvsp.requestDatas.Add(new RequestData());
            PopulateTreeView();
        }
        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem item = (MenuItem)sender;
                ContextMenu cm = (ContextMenu)item.Parent;
                List<string> tag = (List<string>)cm.Tag;
                if (tag[0] == "RD"&&tag[1]!="-1")
                {
                    epvsp.requestDatas.RemoveAt(Convert.ToInt32(tag[1]));
                }
                if (tag[0] == "TD" && tag[1] != "-1")
                {
                    epvsp.triggerDatas.RemoveAt(Convert.ToInt32(tag[1]));
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("File not Loaded!");
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("No entry selected!");
            }
            PopulateTreeView();
        }
        private void ChangeWWCT(object sender, RoutedEventArgs e)
        {
            //fuck binding I'll just do it myself
            epvsp.WWCTPath = WWCT.Text;
        }
    }
    public class TypeSelector : DataTemplateSelector
    {
        public DataTemplate requestData { get; set; }
        public DataTemplate triggerData { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                if (item is RequestData)
                {
                    return requestData;
                }
                else if (item is TriggerData)
                {
                    return triggerData;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
