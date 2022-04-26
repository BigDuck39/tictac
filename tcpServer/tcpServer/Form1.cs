using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tcpServer {
    public partial class Form1 : Form {
        TcpListener server = null;
        public void startServer() {

            try {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (server_started) {
                    listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("Waiting for a connection".ToString())));
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("Connected".ToString())));
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0) {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add(("Recived: " +data).ToString())));
                        Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("Sent: " + data).ToString()));
                        Console.WriteLine("Sent: {0}", data);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            } catch (SocketException e) {
                listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add(e.ToString())));
                Console.WriteLine("SocketException: {0}", e);
            } finally {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
        public Form1() {
            InitializeComponent();
        }
        Thread th1;
        bool server_started = false;
        private void button1_Click(object sender, EventArgs e) {
            server_started = true;
            
            th1 = new Thread(startServer);

            th1.Start();

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            if (server_started) {
                server.Stop();
                server_started = false;
                th1.Join();
            }
        }
    }
}
