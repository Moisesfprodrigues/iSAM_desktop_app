/*
 Instituto Politécnico do Cávado e do Ave
 Licenciatura em Engenharia de Sistemas Informáticos
 Autor: Moisés Rodrigues nº6412
 Email: a6412@alunos.ipca.pt
 Sobre: Aplicação para otimizar o procedimento de integração da aplicação ISAM
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
            this.button2.BackColor = Color.AliceBlue; // Ao iniciar altera a cor de fundo do botão
            button2.Enabled = false;
            button3.Enabled = false;
        }

        /// <summary>
        /// Abre uma caixa de dialogo para selecionar apenas ficheiros do tipo XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            // Armazena na textBox o caminho do ficheiro selecionado
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                button3.Enabled = true;
            }
        }

        /// <summary>
        /// Método assincrono para executar um processo, enquanto o script não terminar não mostra os resultados 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button2_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            this.button2.BackColor = Color.Red;
            this.button2.Text = "Aguarde que o processo termine!";

            // Instancia uma entrada de processo e configura parametros de um processo
            ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;

            psi.FileName = @"C:\isam\batch\isam_batch.bat"; // Localização do script por defeito
            psi.WorkingDirectory = @"";
            
            // Executa o processo de forma assincrona
            await Task.Run(() => {

                var proc = Process.Start(psi);
                proc.WaitForExit();
                string log = proc.StandardOutput.ReadToEnd();
                textBox2.Text = log; // Carrega resultado do procedimento numa textBox
            });

            // Auto scroll para o final da textBox
            textBox2.SelectionStart = textBox2.Text.Length;
            textBox2.ScrollToCaret();

            // Repõe a mensagem de origem no botão
            this.button2.BackColor = Color.AliceBlue;
            this.button2.Text = "Iniciar Envio de Dados";
            this.Enabled = true;
            button3.Enabled = false;

            ProcessStartInfo txtlog = new System.Diagnostics.ProcessStartInfo();
            
            // Procura todos os ficheiros contidos na diretoria "Folder"
            string Folder = @"C:\isam\logs\logs_api";
            var files = new DirectoryInfo(Folder).GetFiles("*.*");
            
            string latestfile = "";
            DateTime lastModified = DateTime.MinValue;

            // Na diretoria indicada verifica o ficheiro com data de alteração mais recente
            foreach (FileInfo file in files)
            {
                if (file.LastWriteTime > lastModified)
                {
                    lastModified = file.LastWriteTime;
                    latestfile = file.Name;
                }
            }
            // Monta o caminho + nome do ficheiro + extensão numa unica string
            txtlog.FileName = @"C:\isam\logs\logs_api\" + latestfile;

            // Invoca o ficheiro de log mais recente
            Process.Start(txtlog);

        }

        /// <summary>

        ///  Verifica e copia o novo ficheiro para diretoria de script.
        ///  Caso o ficheiro exista substitui o mesmo pelo novo selecionado.

        ///  verifica e copia o novo ficheiro para diretoria de script

        /// </summary>
      
        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                string f = textBox1.Text;
                if (System.IO.File.Exists(@"C:\isam\pm_folder\pauta.xml") == true)
                {
                    System.IO.File.Delete(@"C:\isam\pm_folder\pauta.xml");
                    System.IO.File.Copy(f, @"C:\isam\pm_folder\pauta.xml");
                    MessageBox.Show("Pauta Modular importada com sucesso!", "Resultado da Operação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button2.Enabled = true;
                }
                else
                {
                    System.IO.File.Copy(f, @"C:\isam\pm_folder\pauta.xml");
                    MessageBox.Show("Pauta Modular importada com sucesso!" , "Resultado da Operação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button2.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERRO: " + ex, "Resultado da Operação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button2.Enabled = false;
            }
        }

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
