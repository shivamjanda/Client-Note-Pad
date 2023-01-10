// Author:          Shivam Janda
// Created:         April, 2022
// Last Modified:   April 4, 2022
// Description:     The form works like a notepad with its basic features such as copying and pasting, cutting, saving, opening a file, creating a new file. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ClientNotePad
{
    public partial class FormClientPad : Form
    {
        // current file thats been open
        string openFile = String.Empty;
        public FormClientPad()
        {
            InitializeComponent();
        }

        #region Event Handlers
        
        /// <summary>
        /// Closes the form
        /// </summary>
        private void menuFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Save Dialog allows the user to pick a location and save the contents of the file
        /// </summary>
        private void menuFileSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Text FDiles (*.txt)| *.txt";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {

                openFile = saveDialog.FileName;

                SaveFile(openFile);
            }

            UpdateTitle();
        }

        /// <summary>
        /// If we know which file is open, save it . If we dont run, "save as"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileSave_Click(object sender, EventArgs e)
        {
            
            // If openFile( the file that is currently open) has no value, call save as instead
            if (openFile == string.Empty)
            {
                menuFileSaveAs_Click(sender, e);
            }
            // if openFile has a value, just call the savefile() function

            else
            {
                SaveFile(openFile);
            }
        }

        /// <summary>
        /// Empties the current file
        /// </summary>
        private void menuFileNew_Click(object sender, EventArgs e)
        {
            textBoxBody.Clear();
            openFile = String.Empty;
            UpdateTitle();
        }

        /// <summary>
        /// This will open (Open File Dialog) a file and read (using stream reader) its content at the indicated path filtering to only txt files 
        /// </summary>
        private void menuFileOpen_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            using (OpenFileDialog openFile = new OpenFileDialog())
            {
                // filters of the file that can be opened
                openFile.InitialDirectory = "c:\\";
                openFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFile.FilterIndex = 2;
                openFile.RestoreDirectory = true;

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    // Specifiecd file 
                    filePath = openFile.FileName;

                    // read the contents of the file
                    var fileStream = openFile.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();

                        // put its content into the text body
                        textBoxBody.Text = fileContent;

                        // close the reader to release the resource when it is read
                        reader.Close();

                    }
                }
            }
        }

        /// <summary>
        /// Copies the selected text contents into the clipboard
        /// </summary>
        private void menuEditCopy_Click(object sender, EventArgs e)
        {
            if (textBoxBody.SelectionLength > 0)
            {
                Clipboard.SetText(textBoxBody.SelectedText);
            }
        }

        /// <summary>
        /// Cuts the selected text contents and saves it to the clipboard
        /// </summary>
        private void menuEditCut_Click(object sender, EventArgs e)
        {
            if (textBoxBody.SelectedText != "")
            {
                Clipboard.SetText(textBoxBody.SelectedText);
                textBoxBody.Cut();
            }
        }

        /// <summary>
        /// Pastes the contents in the clipboard to the desired location
        /// </summary>
     
        private void menuFilePaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true)
            {
                if (textBoxBody.SelectionLength > 0)
                {
                    textBoxBody.SelectionStart = textBoxBody.SelectionStart + textBoxBody.SelectionLength;
                }

                textBoxBody.Paste();
            }
        }

        /// <summary>
        /// Displays a messagebox about the form
        /// </summary>
        private void menuHelpAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ClientPad\n\n Notpad Assignment for NETD 2202, April 2022");
        }
        #endregion

        /// <summary>
        /// updates the text of the windows
        /// </summary>
        private void UpdateTitle()
        {
            this.Text = "ClientPad";

            if (openFile != String.Empty)
            {
                this.Text += " - " + openFile;
            }
        }

        #region Functions 
        /// <summary>
        /// This will save the file at the path indicated by the file name passed in 
        /// </summary>
        /// <param name="fileName"></param>
        private void SaveFile(string fileName)
        {
          
            FileStream fileToAccess = new FileStream (fileName, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(fileToAccess);

            writer.Write(textBoxBody.Text);

            writer.Close();
        }
        #endregion
    }
}
