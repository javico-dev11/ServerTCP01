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
        private TcpClient client;
        private NetworkStream stream;
        private string myIp;

        public Form1()
        {

            InitializeComponent();
            getIpLocal();
        }

        private void getIpLocal()
        {
            try
            {
                // Obtener la dirección IP local
                string hostname = Dns.GetHostName();
                IPHostEntry hostEntry = Dns.GetHostEntry(hostname);
                IPAddress[] ipAddressList = hostEntry.AddressList;
                IPAddress localIPAddress = null;

                // Buscar la primera dirección IP de la lista que no sea una dirección IP de loopback
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

        private bool pingToServer()
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(txtServer.Text);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException ex)
            {
                Console.WriteLine($"Ocurrió un error al intentar hacer un ping al servidor: {ex.Message}");
                logTextBox.AppendText("Ocurrió un error al intentar hacer un ping al servidor!." + Environment.NewLine);
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Conecta con el servidor
                client = new TcpClient(txtServer.Text, 8080);
                stream = client.GetStream();
                
                // Agrega un controlador de eventos para recibir mensajes del servidor
                byte[] buffer = new byte[1024];
                stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(ReceiveCallback), buffer);

                logTextBox.AppendText("Conectado al servidor TCP" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                logTextBox.AppendText("Error al conectar con el servidor: " + ex.Message + Environment.NewLine);
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Envía un mensaje al servidor
                string message = messageTextBox.Text;
                byte[] data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);

                // Borra el contenido del cuadro de texto del mensaje
                messageTextBox.Text = "";

                logTextBox.AppendText("Mensaje enviado al servidor: " + message + Environment.NewLine);
            }
            catch (Exception ex)
            {
                logTextBox.AppendText("Error al enviar el mensaje al servidor: " + ex.Message + Environment.NewLine);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Obtiene los datos del servidor
                byte[] buffer = (byte[])ar.AsyncState;
                int bytesRead = stream.EndRead(ar);
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                // Muestra el mensaje recibido en el cuadro de texto de registro
                Invoke((MethodInvoker)delegate
                {
                    logTextBox.AppendText("Mensaje recibido del servidor: " + message + Environment.NewLine);
                });

                // Inicia una nueva lectura de datos del servidor
                stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(ReceiveCallback), buffer);
            }
            catch (Exception ex)
            {
                logTextBox.AppendText("Error al recibir el mensaje del servidor: " + ex.Message + Environment.NewLine);
            }
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cierra la conexión con el servidor
            if (client != null)
            {
                client.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pingButton_Click(object sender, EventArgs e)
        {
            if (pingToServer())
            {
                logTextBox.AppendText("PING al servidor exitoso!." + Environment.NewLine);
            }
            else
            {
                logTextBox.AppendText("PING al servidor no exitoso!." + Environment.NewLine);
            }
        }
    }
}
