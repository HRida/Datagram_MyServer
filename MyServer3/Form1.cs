using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MyServer3
{
    public partial class Form1 : Form
    {
        private UdpClient client;
        private IPEndPoint receivePoint;
        public Form1()
        {
            InitializeComponent();
            client = new UdpClient(5000);
            receivePoint = new IPEndPoint(new IPAddress(0), 0);

            Thread readThread = new Thread(
               new ThreadStart(WaitForPackets));
  
            readThread.Start();
        }
        // shut down the server
        protected void Server_Closing(
           object sender, CancelEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }

        // wait for a packet to arrive
        public void WaitForPackets()
        {
            Action updateLabel;
            while (true)
            {
                
                // set up packet
                byte[] data = client.Receive(ref receivePoint);
                updateLabel = () => displayTextBox.Text += "\r\nPacket received:" +
                   "\r\nLength: " + data.Length +
                   "\r\nContaining: " +
                   System.Text.Encoding.ASCII.GetString(data);
                displayTextBox.Invoke(updateLabel);

                // echo information from packet back to client
                updateLabel = () => displayTextBox.Text += "\r\n\r\nEcho data back to client...";
                displayTextBox.Invoke(updateLabel);
                client.Send(data, data.Length, receivePoint);
                updateLabel = () => displayTextBox.Text += "\r\nPacket sent\r\n";
                displayTextBox.Invoke(updateLabel);
            }

        } // end method WaitForPackets

    }
}
