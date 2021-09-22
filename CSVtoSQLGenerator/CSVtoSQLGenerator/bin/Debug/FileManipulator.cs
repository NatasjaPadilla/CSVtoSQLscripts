using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CSVtoSQLGenerator
{
    /// <summary>
    /// The FileManipulator class is the class in charge of manipulating files
    /// this includes but is not limited to opening files, closing files and writing files
    /// </summary>
    static class FileManipulator
    {
        public static List<string> lines = new List<string>(); // all the text from the file, raw

        private static List<string> columnNames = new List<string>(); // just the list of column headers

        private static string filePath = "";

        public static List<string> plswork = new List<string>();

        public static void fileReader(string filePath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line = "";

                    while ((line = sr.ReadLine()) != null)
                    {
                        // Post Processing will happen here . . . 
                        lines.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine("Error Message: Please close the file and try again");
                lines = new List<string>();
            }
        }

        public static string[] fileStatus()
        {
            string[] status = new string[2];

            if(lines.Count > 2)
            {
                status[0] = "1";
                status[1] = "File read successful. " + lines.Count + " read.";
            }
            else
            {
                status[0] = "0";
                status[1] = "File reading error! Please double check file format.";
            }

            return status;
        }

        public static string fileGetTableName()
        {
            // the first index of lines is always the title

            // this line splits the string and then gets whatever is in the first index of the split string
            // which should be the table name
            return stringSplitter(lines[0], new char[] { ',' })[0];
        }

        public static string[] stringSplitter(string stringToSplit, char[] splitChars)
        {
            return stringToSplit.Split(splitChars);
        }

        public static void fileExtractColumnNames()
        {
            // this line will split the 2nd read line, which is the line of column names
            string[] temp = stringSplitter(lines[1], new char[] { ',' });
            foreach (string t in temp)
            {
                if (t.Length > 0)
                { // checks if the column name is blank or not
                    columnNames.Add(t);
                    //MessageBox.Show(t);
                }
            }
        }

        public static void thescriptthingy()
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (i > 1)
                    plswork.Add(lines[i]);
            }
        }

        /// <summary>
        /// Make sure that fileExtractColumnNames is run at least once
        /// </summary>
        /// <returns></returns>
        public static int getColumnCount()
        {
            // this just returns the count of columns
            return columnNames.Count;
        }

        /// <summary>
        /// This method will create the file where the exe file is located
        /// </summary>
        public static void fileCreator() 
        {
            filePath = fileGetTableName() + ".txt";
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
        }


        public static void fileWriter(bool appendFlag, string message)
        {
            using (StreamWriter sr = new StreamWriter(filePath, appendFlag))
            {
                sr.WriteLine(message);
            }
        }
    }
}
