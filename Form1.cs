using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ISAM_batch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.button2.BackColor = Color.AliceBlue;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "xml",
                Filter = "xml files (*.xml)|*.xml",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        //assincronous method
        private async void button2_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            this.button2.BackColor = Color.Red;
            this.button2.Text = "Waiting to Process End";

            ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;

            psi.FileName = @"C:\isam_files\pautas_output\executaPauta.bat";
            psi.WorkingDirectory = @"";

            await Task.Run(() => {

                var proc = Process.Start(psi);
                proc.WaitForExit();
                string log = proc.StandardOutput.ReadToEnd();
                textBox2.Text = log;
                

                /*UTILIZAR INVOKE E DELEGATE*/

            });

            textBox2.SelectionStart = textBox2.Text.Length;
            textBox2.ScrollToCaret();

            this.button2.BackColor = Color.AliceBlue;
            this.button2.Text = "Iniciar Processo de Integração";
            this.Enabled = true;
        }

        /// <summary>
        ///  verifica e copia o novo ficheiro para diretoria de script
        /// </summary>
      
        private void button3_Click(object sender, EventArgs e)
        {
            string f = textBox1.Text;
            if (System.IO.File.Exists(@"C:\isam_files\pautas_output\pauta.xml") == true)
            {
                System.IO.File.Delete(@"C:\isam_files\pautas_output\pauta.xml");
                System.IO.File.Copy(f, @"C:\isam_files\pautas_output\pauta.xml");
            }
            else
            {
                System.IO.File.Copy(f, @"C:\isam_files\pautas_output\pauta.xml");
            }
            
        }

    }
}
