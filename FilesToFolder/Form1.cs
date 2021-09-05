using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilesToFolder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string _exeLocation = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            int _jpgFilesCount = Directory.EnumerateFiles(_exeLocation).Select(Path.GetFileName).Where(x => x.ToLower().EndsWith(".jpg")).Count();


            if (_jpgFilesCount > 0)
            {
                label1.Text = _jpgFilesCount.ToString() + " images found!";
                button1.Enabled = true;
            }
            else
            {
                label1.Text = "No images found in this folder!";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string _exeLocation = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

                List<string> _jpgFiles = Directory.EnumerateFiles(_exeLocation).Select(Path.GetFileName)
                    .Where(x => x.ToLower().EndsWith(".jpg") || x.ToLower().EndsWith(".jpeg") || x.ToLower().EndsWith(".png") || x.ToLower().EndsWith(".gif") || x.ToLower().EndsWith(".webp"))
                    .OrderBy(s => s).ToList();

                if (_jpgFiles.Count() == 0)
                {
                    throw new Exception("No image files found in this folder!");
                }

                DialogResult dialog = MessageBox.Show("Any files with same name in the folders will be overwritten. Are you sure to move?!", Text, MessageBoxButtons.YesNo);
                if (dialog == DialogResult.No)
                {
                    return;
                }

                progressBar1.Minimum = 0;
                progressBar1.Maximum = _jpgFiles.Count();
                label1.Visible = false;
                progressBar1.Visible = true;

                string fileInSameGroup = "";

                foreach (string pic in _jpgFiles)
                {
                    progressBar1.Value++;
                    string oneImageGroup = "";

                    if (pic.Contains("_", StringComparison.CurrentCulture))
                    { 
                        oneImageGroup = pic.Substring(0, pic.IndexOf("_") );
                    }
                    else
                    {
                        oneImageGroup = pic.Substring(0, pic.IndexOf("."));
                    }

                    if (fileInSameGroup == oneImageGroup)
                    {
                        File.Move(_exeLocation + "\\" + pic, _exeLocation + "\\" + oneImageGroup + "\\" + pic, true);
                    }
                    else
                    {
                        string folderPath = new(_exeLocation + "\\" + oneImageGroup);
                        if (!Directory.Exists(folderPath))
                        {
                            DirectoryInfo di = Directory.CreateDirectory(folderPath);
                        }

                        File.Move(_exeLocation + "\\" + pic, _exeLocation + "\\" + oneImageGroup + "\\"+ pic, true);
                    }

                    fileInSameGroup = oneImageGroup;

                    // Console.WriteLine(Path.GetFileName(f));
                }

                DialogResult dialog2 = MessageBox.Show("All images have moved to their folders!", Text, MessageBoxButtons.OK);
                if (dialog2 == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
            catch (Exception)
            {
                throw;
            }


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
