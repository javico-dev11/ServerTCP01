using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientTCP01
{

    public partial class Form1 : Form
    {
        private UdpClient udpClient;
        private string multicastAddress;
        private int multicastPort;
        private string myIp;

        public Form1()
        {
            InitializeComponent();
            GetLocalIPAddress();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void GetLocalIPAddress()
        {
            try
            {
                string hostname = Dns.GetHostName();
                IPHostEntry hostEntry = Dns.GetHostEntry(hostname);
                IPAddress[] ipAddressList = hostEntry.AddressList;
                IPAddress localIPAddress = null;

                foreach (IPAddress ipAddress in ipAddressList)
                {
                    if (!IPAddress.IsLoopback(ipAddress) && ipAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localIPAddress = ipAddress;
                        break;
                    }
                }

                if (localIPAddress != null)
                {
                    myIp = localIPAddress.ToString();
                    Console.WriteLine("La dirección IP local es: " + localIPAddress.ToString());
                    logTextBox.AppendText($"La dirección IP local es: {localIPAddress.ToString()}" + Environment.NewLine);
                }
                else
                {
                    Console.WriteLine("No se pudo encontrar una dirección IP local");
                    myIp = "xx.xx.xx.xx";
                    logTextBox.AppendText($"No se pudo encontrar una dirección IP local: {myIp}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error al obtener la dirección IP local");
                logTextBox.AppendText($"Ocurrió un error al obtener la dirección IP local: {ex.Message}" + Environment.NewLine);
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Conecta con el servidor multicast
                multicastAddress = txtServer.Text;
                multicastPort = 8080;
                udpClient = new UdpClient();
                udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, multicastPort));
                udpClient.JoinMulticastGroup(IPAddress.Parse(multicastAddress));

                // Inicia un hilo separado para recibir mensajes del servidor multicast
                Task.Run(() => ReceiveMessages());

                logTextBox.AppendText("Conectado al servidor multicast" + Environment.NewLine);
                connectButton.Enabled = false;
                txtServer.Enabled = false;
            }
            catch (Exception ex)
            {
                logTextBox.AppendText("Error al conectar con el servidor multicast: " + ex.Message + Environment.NewLine);
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Envía un mensaje al servidor multicast
                string message = messageTextBox.Text;
                byte[] data = Encoding.ASCII.GetBytes(message);
                udpClient.Send(data, data.Length, multicastAddress, multicastPort);

                // Borra el contenido del cuadro de texto del mensaje
                messageTextBox.Text = "";

                //logTextBox.AppendText("Mensaje enviado al servidor multicast: " + message + Environment.NewLine);
            }
            catch (Exception ex)
            {
                logTextBox.AppendText("Error al enviar el mensaje al servidor multicast: " + ex.Message + Environment.NewLine);
            }
        }

        private void ReceiveMessages()
        {
            try
            {
                while (true)
                {
                    IPEndPoint remoteEndPoint = null;
                    byte[] receivedData = udpClient.Receive(ref remoteEndPoint);
                    string message = Encoding.ASCII.GetString(receivedData);

                    // Muestra el mensaje recibido en el cuadro de texto de registro
                    Invoke((MethodInvoker)delegate
                    {
                        // Verifica si el mensaje es del mismo cliente
                        if (remoteEndPoint.Address.ToString() == myIp && remoteEndPoint.Port == ((IPEndPoint)udpClient.Client.LocalEndPoint).Port)
                        {
                            logTextBox.AppendText($"Yo => " + message + Environment.NewLine);
                        }
                        else
                        {
                            logTextBox.AppendText($"Mensaje recibido de: {remoteEndPoint.Address} => " + message + Environment.NewLine);
                        }

                        
                    });
                }
            }
            catch (Exception ex)
            {
                logTextBox.AppendText("Error al recibir el mensaje del servidor multicast: " + ex.Message + Environment.NewLine);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cierra la conexión con el servidor multicast
            udpClient?.DropMulticastGroup(IPAddress.Parse(multicastAddress));
            udpClient?.Close();
        }

        private void pingButton_Click(object sender, EventArgs e)
        {
            // No se puede realizar un ping a un servidor multicast, por lo que esta función no es aplicable en este caso
            logTextBox.AppendText("Esta función no es aplicable para un servidor multicast." + Environment.NewLine);
        }
    }


}
