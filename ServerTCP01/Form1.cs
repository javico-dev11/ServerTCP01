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

namespace ServerTCP01
{

    public partial class Form1 : Form
    {
        private UdpClient udpClient;
        private List<IPEndPoint> connectedClients;
        private StringBuilder logBuilder = new StringBuilder();

        public Form1()
        {
            InitializeComponent();
            connectedClients = new List<IPEndPoint>();
        }

        private void ListenForClients()
        {
            try
            {
                IPAddress multicastAddress = IPAddress.Parse("239.0.0.1");
                int multicastPort = 8080;

                udpClient = new UdpClient();
                udpClient.ExclusiveAddressUse = false;
                udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, multicastPort));
                udpClient.JoinMulticastGroup(multicastAddress);

                while (true)
                {
                    IPEndPoint remoteEndPoint = null;
                    byte[] data = udpClient.Receive(ref remoteEndPoint);
                    string message = Encoding.ASCII.GetString(data);

                    // Verifica si el cliente ya está conectado
                    if (!connectedClients.Contains(remoteEndPoint))
                    {
                        // Nuevo cliente conectado
                        connectedClients.Add(remoteEndPoint);
                        Invoke((MethodInvoker)delegate
                        {
                            clientsListBox.Items.Add($"Cliente conectado: {remoteEndPoint.Address}:{remoteEndPoint.Port}");
                            lblClientes.Text = connectedClients.Count.ToString();
                        });
                    }

                    // Muestra la información del cliente y el mensaje recibido
                    Invoke((MethodInvoker)delegate
                    {
                        clientsListBox.Items.Add($"Mensaje recibido del cliente {remoteEndPoint.Address}:{remoteEndPoint.Port}: {message}");
                    });

                    // Envía el mensaje a todos los clientes conectados
                    byte[] responseData = Encoding.ASCII.GetBytes(message);
                    //foreach (IPEndPoint client in connectedClients)
                    //{
                    //    udpClient.Send(responseData, responseData.Length, client);
                    //}
                    logBuilder.AppendLine($"Mensaje enviado a todos los clientes: {message}");
                }
            }
            catch (Exception ex)
            {
                Invoke((MethodInvoker)delegate
                {
                    clientsListBox.Items.Add("Error al recibir mensajes: " + ex.Message);
                });
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Empezar")
            {
                button1.Text = "Detener";

                // Inicia el servidor en un hilo separado
                Task.Run(() => ListenForClients());
                Invoke((MethodInvoker)delegate
                {
                    clientsListBox.Items.Add($"Escuchando a clientes desde: {serverText.Text}");
                    lblClientes.Text = connectedClients.Count.ToString();
                });

            }
            else if (button1.Text == "Detener")
            {
                button1.Text = "Empezar";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Cierra el UdpClient y libera los recursos
            udpClient?.Close();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            // Actualiza el registro de actividad en el TextBox
            logTextBox.Text = logBuilder.ToString();
        }
    }



}

