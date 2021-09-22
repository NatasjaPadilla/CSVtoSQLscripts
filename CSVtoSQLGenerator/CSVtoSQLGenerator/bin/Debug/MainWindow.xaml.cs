using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace CSVtoSQLGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        bool fileLocked = false; // this flag is false when there are no files open, it is true when there is a file open
        CheckBox dynamicCB = new CheckBox();
        CheckBox[] cblist = new CheckBox[] { };
        string[] words = new string[] { };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCSVSelector_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Comma Separated Values (*.csv;)|*.csv;";

            if (ofd.ShowDialog() == true) 
            {
                tbDisplayPath.Text = ofd.FileName;
            }
        }

        private void btnSelectionLock_Click(object sender, RoutedEventArgs e)
        {
            if(fileLocked) // file is about to be released
            {

            }
            else // file is about to be open
            {
                if(tbDisplayPath.Text.Length > 0) // file opening and reading should only occur if there is a file path on display
                {
                    FileManipulator.fileReader(tbDisplayPath.Text);
                    MessageBox.Show(FileManipulator.fileStatus()[1]);
                    if(FileManipulator.fileStatus()[0] == "1") // file reading success, proceed to next step
                    {
                        fileLocked = true;
                        btnCSVSelector.IsEnabled = false;
                        btnGenerateScript.IsEnabled = true;
                        lblTableName.Content = "Table Name : " + FileManipulator.fileGetTableName();
                        FileManipulator.fileExtractColumnNames();
                        lblColumnCount.Content = "Column Count : " + FileManipulator.getColumnCount();
                        FileManipulator.fileCreator();

                        string[] temp = FileManipulator.stringSplitter(FileManipulator.lines[1], new char[] { ',' });
                            
                        int num = 10;
                        cblist = new CheckBox[temp.Length];
                        for(int i = 0; i < temp.Length; i++)
                        {
                            dynamicCB = new CheckBox();
                            dynamicCB.FlowDirection = FlowDirection.LeftToRight;
                            dynamicCB.Content = temp[i];
                            dynamicCB.Margin = new System.Windows.Thickness { Left = 10, Top = 180 + num, Right = 0, Bottom = 0 };
                            dynamicCB.HorizontalAlignment = HorizontalAlignment.Left;

                            dynamicCB.IsChecked = true;

                            cblist[i] = dynamicCB;
                            DisplayGrid.Children.Add(dynamicCB);
                            
                            num += 20;
                        }

                    }
                }
            }
        }

        private void btnGenerateScript_Click(object sender, RoutedEventArgs e)
        {
            // Pre Diagnostic scripts
            if((bool)chkbDiagnostic.IsChecked)
            {
                FileManipulator.fileWriter(true, "-- PRE DIAGNOSTIC SCRIPTS");
                FileManipulator.fileWriter(true, "SELECT COUNT(*) FROM " + FileManipulator.fileGetTableName());
                FileManipulator.fileWriter(true, "");
                FileManipulator.fileWriter(true, "");
            }

            // Generated scripts

            //FileManipulator.fileWriter(true, "This is where the generated scripts should appear");

            FileManipulator.thescriptthingy();

            string var1 = "";
            string var2 = "";
            string beans = "";
            for (int i = 0; i < FileManipulator.plswork.Count; i++)
            {
                words = FileManipulator.stringSplitter(FileManipulator.plswork[i], new char[] { ',' });
                var2 = "";
                for (int j = 0; j < words.Length; j++)
                {
                    if ((bool)cblist[j].IsChecked)
                    {
                        //var2 += "," + "'" + words[j] + "'";
                        var2 += "'" + words[j] + "'" + ",";
                        beans = var2.TrimEnd(',');
                    }
                    else
                    {
                        //var2 += "," + words[j];
                        var2 += words[j] + ",";
                        beans = var2.TrimEnd(',');
                    }
                }
                var1 += "insert " + FileManipulator.fileGetTableName() + "\nvalues (" + beans + ")\n";
            }

            FileManipulator.fileWriter(true, var1);

            // Post Diagnostic scripts
            if ((bool)chkbDiagnostic.IsChecked)
            {
                FileManipulator.fileWriter(true, "-- POST DIAGNOSTIC SCRIPTS");
                FileManipulator.fileWriter(true, "SELECT COUNT(*) FROM " + FileManipulator.fileGetTableName());
            }

            MessageBox.Show("File Writing successfully done!");
        }

        private void chkbDiagnostic_Checked(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
