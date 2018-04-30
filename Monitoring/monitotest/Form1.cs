using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace monitotest
{
    public partial class Form1 : Form
    {
        delegate void StringArgReturningVoidDelegate(string text,TextBox textbox); // Delegate enable asynchronous call for setting txt property on the textBox9
        public TcpClient client;
        private TcpListener tcpListener;
        private Thread listenThread;

        StreamWriter sw = new StreamWriter(@"C:\Users\riche\Desktop\Log.txt"); // Get the file where the log will be written
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sw.WriteLine(textBox9.Text); // Copy the text in the log file when "Stop" button is pressed
            sw.Close(); // Close the file
            sw.Dispose(); // Release the memory used by the StreamWriter
            client.Close(); // Stop the local server
            this.Close();
            MessageBox.Show("Local Server Closed");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Monitoring started successfully");
            this.SetText("Starting...",this.textBox9); // method "SetText" is executed on the worker thread => thread-safe call on the textBox9
            tcpListener = new TcpListener(IPAddress.Any, 3000); 
            listenThread = new Thread(new ThreadStart(ListenForClients));
            listenThread.Start();



            void ListenForClients()
            {
                this.tcpListener.Start();
                this.SetText("Sucessfully started", this.textBox9);
                while (true)
                {
                    //blocks until a client has connected to the server
                    TcpClient client = this.tcpListener.AcceptTcpClient();

                    //create a thread to handle communication 
                    //with connected client
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                    clientThread.Start(client);
                }
            }
            void HandleClientComm(object client)
            {
                TcpClient tcpClient = (TcpClient)client;
                this.client = tcpClient;
                NetworkStream clientStream = tcpClient.GetStream();

                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] buffer = encoder.GetBytes("Hello Client!");
                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();

                byte[] message = new byte[4096];
                int bytesRead;

                while (true)
                {
                    bytesRead = 0;

                    try
                    {
                        //blocks until a client sends a message
                        bytesRead = clientStream.Read(message, 0, 4096);
                    }
                    catch
                    {
                        //a socket error has occured
                        break;
                    }

                    if (bytesRead == 0)
                    {
                        //the client has disconnected from the server
                        break;
                    }
                    this.TreatText(encoder.GetString(message,0,bytesRead));
                    //File.WriteAllText("Log.txt", textBox9.Text);
                    
                }

            }
        

        }
        private void TreatText(string message)
        {
            var split = message.Split(':');
            if(split[0] == "PAGE")
            {
                this.SetText(split[1], this.textBox2);

            }
            else if(split[0] == "CONNEXION")
            {
                this.SetText(split[1], this.textBox1);
                this.SetText(split[1] + " s'est connecté", this.textBox9);
            }
            else if(split[0] == "VOTEAPP")
            {
                this.SetText(split[1] + " a voté pour une application", textBox9);
            }
            else if(split[0] == "VOTEGAME")
            {
                this.SetText(split[1] + " a voté pour un jeu", textBox9);
            }
        }

        private void SetText(string text,TextBox textbox)  
        {
            // if the calling thread is different from the thread that created the textBox control, it will create a delegate 
            // and will call itself asynchronously using the Invoke method

            //if the calling thread is the same from the thread that created the textBox control, the text property is set directly
            string Ancien = textbox.Text;
            if (textbox.InvokeRequired) // InvokeRequired compares the calling thread ID to the creating thread ID
            {   // if these threads are different, it will returns true
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetText);
                this.Invoke(d, new object[] { text,textbox });
            }
            else
            {
                if (textbox == this.textBox9) textbox.Text = Ancien.TrimStart() + Environment.NewLine + text;
                else textbox.Text = text;
            }

        }
        private void Form1_Load(object sender,EventArgs e)
        {

        }
        private void label1_Click(object sender,EventArgs e)
        {

        }
        private void label3_Click(object sender,EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender,EventArgs e)
        {

        }
        private void textBox9_TextChanged_1(object sender,EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
