using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Net;

namespace CHATCLIENTE
{
    public partial class Form1 : Form
    {
        static private NetworkStream Fdatos;
        static private StreamWriter Descritos;
        static private StreamReader Dleidos;
        static private TcpClient cliente = new TcpClient();
        static private string usuario = "unknown";

        private delegate void DAddItem(string s);

        private void AddItem (string s)
        {
            listBox1.Items.Add(s);
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Descritos.WriteLine(textBox1.Text);
            Descritos.Flush();
            textBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            textBox2.Visible = false;
            button2.Visible = false;
            listBox1.Visible = true;
            textBox1.Visible = true;
            button1.Visible = true;

            usuario = textBox2.Text;

            Conectar();

        }

        void Listen ()
        {
            while (cliente.Connected)
            {
                try
                {
                    this.Invoke(new DAddItem(AddItem), Dleidos.ReadLine());
                }
                catch
                {
                    MessageBox.Show("No se a podido conectar al servidor");
                    Application.Exit();
                }
            }
        }

        void Conectar()
        {
            try
            {
                cliente.Connect("127.0.0.1", 8000);
                if (cliente.Connected)
                {
                    Thread t = new Thread(Listen);
                    Fdatos = cliente.GetStream();
                    Descritos = new StreamWriter(Fdatos);
                    Dleidos = new StreamReader(Fdatos);

                    Descritos.WriteLine(usuario);
                    Descritos.Flush();
                    t.Start();
                }
                else
                {
                    MessageBox.Show("Servidor no Disponible");
                    Application.Exit();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Servidor no Disponible");
                Application.Exit();
            }
        }
    }
}
