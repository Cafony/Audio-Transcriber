using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WMPLib;
using System.IO;




// Canal Programacao Plena
//https://youtu.be/hYIHECkVmmg?si=45nf-YuaUmFzbFPe

namespace MP3_Player_Toze
{
    public partial class MP3 : Form  // Video 11,36
    {

        WindowsMediaPlayer myplayer; // inicializei objecto WindowsMediaPlayer
        List<string> listaMp3 = new List<string>();
        List<Musica> listaMusicas = new List<Musica>();  // Inicializa uma lista
        FolderBrowserDialog browser = new FolderBrowserDialog();  // Cria o browser

        string arquivoMp3 = @"c:\mp3\Amarteassim.mp3";



        public MP3() // o meu construtor chama-se MP3
        {
            // Aqui vou inicializar os componentes do programa
            InitializeComponent();
            myplayer = new WindowsMediaPlayer();  //aqui vou instanciar o objecto em musica
            trackBar1.Value = 50; // Volume inicial metade de 100 definido propriedades
        }


        private void MP3_Load(object sender, EventArgs e)
        {
            //System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
            //this.Size = new System.Drawing.Size(Convert.ToInt32(0.5*workingRectangle.Width), Convert.ToInt32(0.5* workingRectangle.Height));
            //this.Location = new System.Drawing.Point(10,10);    
            this.KeyPreview = true;
        }

        private void btnPlay_Click(object sender, EventArgs e) // botao play

        {
            Play(arquivoMp3);
        }


        private void Play(string arquivoMp3)//criar metodo Play
        {
            // verificar o estado do player (fazer este boatao play/pause
            if (myplayer.playState == WMPPlayState.wmppsUndefined || myplayer.playState == WMPPlayState.wmppsStopped)
            {
                myplayer.URL = arquivoMp3;
                myplayer.controls.play();
                //buttonPlay.ImageIndex = 0;
            }
            else if (myplayer.playState == WMPPlayState.wmppsPlaying) //Se a musica está a tocar
            {
                myplayer.controls.pause();
                //btnPlay.ImageIndex = 1;
                buttonPlay.Text = "Pause";
            }
            else if (myplayer.playState == WMPPlayState.wmppsPaused) // Se a musica está em PAUSE
            {
                myplayer.controls.play();
                //buttonPlay.ImageIndex = 1;
                buttonPlay.Text = "Play";

            }

        }

        private void button3_Click(object sender, EventArgs e)// botao STOP
        {
            myplayer.controls.stop();
            buttonPlay.ImageIndex = 0;
            trackBar1.Value = trackBar1.Minimum;
        }

        private void trackBar1_Scroll(object sender, EventArgs e) // Botao Volume
        {
            //myplayer.settings.volume = trackBar1.Value * 10;
            myplayer.settings.volume = trackBar1.Value;
        }


        WMPLib.IWMPPlaylist playlist;
        private void button1_Click(object sender, EventArgs e) // Botao Open Abrir 26min
        {
            OpenFileDialog listaArquivos = new OpenFileDialog(); // estanciar uma lista de arquivos
            listaArquivos.Filter = "arquivos de som MP3|*.mp3";
            listaArquivos.Multiselect = true;                   // selecionar varas musicas
            listaArquivos.ShowDialog();
            playlist = myplayer.playlistCollection.newPlaylist("MinhaListaMp3"); // Instancia do Playslist

            foreach (var arquivo in listaArquivos.FileNames)
            {
                listaMusicas.Add(new Musica(arquivo));
                playlist.appendItem(myplayer.newMedia(arquivo));  // addicinar itens playlist

            }

            listBoxMp3.Items.Clear(); // coloca o nome dos ficheiros no listbox
            foreach (var item in listaMusicas)
            {
                listBoxMp3.Items.Add(Path.GetFileName(item.Arquivo));
            }
        }


        private void loadPlaylist()
        {
            myplayer.currentPlaylist = myplayer.newPlaylist("Playlist", "");
            foreach (string files in listaMp3)
            {
                myplayer.currentPlaylist.appendItem(myplayer.newMedia(files));
                listBoxMp3.Items.Add((string)files);
            }


        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) // Tocar musica da listbox 29 minutos
        {
            arquivoMp3 = listaMusicas[listBoxMp3.SelectedIndex].Arquivo;
            Play2(arquivoMp3); // Funcao player envia o ficheiro escolhido na listbox
        }

        public void Play2(string arquivoMp3)
        {
            myplayer.URL = arquivoMp3;
            myplayer.controls.play();
        }



        private void timer1_Tick(object sender, EventArgs e) // Timer adicionado por cima do form design
        {



            if (myplayer.playState == WMPPlayState.wmppsPlaying)
            {

                label1.Text = myplayer.controls.currentPositionString;
                label2.Text = myplayer.controls.currentItem.durationString.ToString();
                trackBar2.Maximum = (int)myplayer.controls.currentItem.duration;
                trackBar2.Value = (int)myplayer.controls.currentPosition;
            }

            //label2.Text = v; 
        }

        private void button4_Click(object sender, EventArgs e) // Botao para frente
        {
            if (myplayer.playState == WMPPlayState.wmppsPlaying)
            {
                if (myplayer.controls.currentItem.duration >= myplayer.controls.currentPosition + 10)
                {
                    myplayer.controls.currentPosition += 10;
                }
                else
                {
                    myplayer.controls.currentPosition = myplayer.controls.currentItem.duration;
                    label1.Text = myplayer.controls.currentPositionString;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)  // Botao andar pra trás
        {
            if (myplayer.playState == WMPPlayState.wmppsPlaying)
            {
                if (myplayer.controls.currentPosition - 10 > 0)
                {
                    myplayer.controls.currentPosition -= 10;
                }
                else
                {
                    myplayer.controls.currentPosition = 0;
                }

                //label1.Text = musica.controls.currentPositionString;

            }
        }


        private void button4_KeyDown(object sender, KeyEventArgs e)  // Atalho MP3 Frente
        {


        }

        private void MP3_KeyDown(object sender, KeyEventArgs e)  // Atalhos KEYDOWN
        {

            if (e.KeyCode == Keys.Right)
            {
                //btnFrente.PerformClick();
                //musica.controls.currentPosition++;
            }

            if (e.KeyCode == Keys.Left)
            {
                //btnTras.PerformClick();
            }

            //https://www.youtube.com/watch?v=hYIHECkVmmg
            // Programação Plena
            if (e.KeyCode == Keys.Space)
            {

                if (myplayer.playState == WMPPlayState.wmppsUndefined || myplayer.playState == WMPPlayState.wmppsStopped)
                {
                    myplayer.URL = arquivoMp3;
                    myplayer.controls.play();
                    buttonPlay.ImageIndex = 0;
                }
                else if (myplayer.playState == WMPPlayState.wmppsPlaying)
                {
                    myplayer.controls.pause();
                    buttonPlay.ImageIndex = 1;
                }
                else if (myplayer.playState == WMPPlayState.wmppsPaused)
                {
                    myplayer.controls.play();
                    buttonPlay.ImageIndex = 1;

                }

            }
        }

        private void trackBar1_KeyDown(object sender, KeyEventArgs e) // Trackbar KeyDown
        {
            // e.SuppressKeyPress = true;   // desligar o teclado para o trackbar
        }

        private void trackBar2_Scroll(object sender, EventArgs e) // Trackbar  Scroll
        {
            // Video: https://www.youtube.com/watch?v=000lu-6Kcjs&t=229s
            // Ver 13 minutos

            myplayer.controls.currentPosition = trackBar2.Value;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Font = fd.Font;
            }

        }


        private void button4_Click_1(object sender, EventArgs e)  // SPEED MAIS
        {
            myplayer.settings.rate = 1.1f;
        }

        private void button5_Click_1(object sender, EventArgs e) // SPEED MENOR
        {
            myplayer.settings.rate = 0.9f;
        }



        private void button6_Click(object sender, EventArgs e)  // SPEED RESET
        {
            myplayer.settings.rate = 1.0;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e) // Richtext Caixa
        {
            this.KeyPreview = false;  // Desligar o atalhos ?
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e) // Ritchtext Atalho desligado
        {
            if (e.KeyCode == Keys.Space)
            {
                e.Handled = false;
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void textFontToolStripMenuItem_Click(object sender, EventArgs e) // MENU TOP FONTS
        {
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Font = fd.Font;
            }
        }


        private void sairToolStripMenuItem_Click(object sender, EventArgs e) // MENU TOP SIZE
        {
            // Get the current selection's font


        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e) // MENU COLOR
        {
            ColorDialog cores = new ColorDialog();
            cores.ShowDialog();
            //richTextBox1.SelectionColor = cores.Color; 
            richTextBox1.ForeColor = cores.Color;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) // MENU TOP open files
        {
            // Create an OpenFileDialog to request a file to open.
            OpenFileDialog openFile1 = new OpenFileDialog();
            openFile1.Title = "Open";
            openFile1.Filter = "Text Document |*.txt;*.doc;*.docx";
            if (openFile1.ShowDialog() == DialogResult.OK)
            {
                // Load the contents of the file into the RichTextBox.
                richTextBox1.LoadFile(openFile1.FileName, RichTextBoxStreamType.PlainText);
                this.Text = openFile1.FileName;
            }


        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) // MENU TOP About
        {
            About form2 = new About();

            form2.ShowDialog();
        }

        private void closeFileToolStripMenuItem_Click(object sender, EventArgs e) // MENUT TOP limpar texto  rtb
        {
            richTextBox1.Clear();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) // TOP MENU Save files
        {
            SaveFileDialog saveFile1 = new SaveFileDialog();
            saveFile1.Title = "Save";
            saveFile1.Filter = "Test Document(*.txt)|*.txt|All files(*.*)|*.*";
            if (saveFile1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SaveFile(saveFile1.FileName, RichTextBoxStreamType.PlainText);
                this.Text = saveFile1.FileName;
            }
        }

        private void SaveListBoxItems(ListBox listBox, string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    foreach (var item in listBox.Items)
                    {
                        writer.WriteLine(item.ToString());
                    }
                }

                MessageBox.Show("Items saved successfully.", "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving items: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MP3_FormClosed(object sender, FormClosedEventArgs e) // Evento Fechar Janela
        {
            //SaveListBoxItens()
        }

        private void savePlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        public class Musica  // classe criada por mim
        {
            public string Arquivo { get; set; }  // é a propiedade deste construtor
            public Musica() { }  // construtor vazio porque vou fazer outro

            public Musica(string arquivo)
            {
                Arquivo = arquivo;    // o Arquivo é o meu arquivo o "arquivo" é a propriedade
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Adjust the pitch by setting the rate
            double pitchFactor = 1.5; // Adjust this value for the desired pitch change

            // Calculate the new rate based on the pitch factor
            double newRate = pitchFactor * myplayer.settings.rate;

            myplayer.settings.rate = newRate;

        }
    }
}
