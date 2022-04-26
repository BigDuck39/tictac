using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tcpClient {
    public partial class Form1 : Form {
        Int32 port = 13000;
        TcpClient client = new TcpClient("127.0.0.1", 13000);
        public void Connect(String server, String message) {
            try {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer
                // connected to the same address as specified by the server, port
                // combination.



                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);


                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            } catch (ArgumentNullException e) {
                Console.WriteLine("ArgumentNullException: {0}", e);
            } catch (SocketException e) {
                Console.WriteLine("SocketException: {0}", e);
            }
            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }
        public Form1() {
            InitializeComponent();
        }
        bool server_started = false;
        private void button1_Click(object sender, EventArgs e) {
            server_started = true;
            Connect("127.0.0.1", textBox1.Text);

        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            if (server_started) {
                client.Close();
                server_started = false;
            }
        }
    }
}
