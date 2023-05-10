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
        private TcpListener listener;
        private readonly Task listenTask;
        private readonly StringBuilder logBuilder = new StringBuilder();
        private int countClients = 0;

        public Form1()
        {
            InitializeComponent();

            // Inicia el servidor en un hilo separado
            IPAddress ip = IPAddress.Parse("192.168.100.89");
            int port = 8080;
            listener = new TcpListener(ip, port);
            listenTask = Task.Run(() => ListenForClients());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ListenForClients()
        {
            listener.Start();
            
            // Iniciar escucha de server
            Invoke((MethodInvoker)delegate
            {
                clientsListBox.Items.Add("Servidor TCP iniciado y esperando conexiones de clientes...");
            });

            while (true)
            {
                // Espera a que se conecte un cliente
                TcpClient client = listener.AcceptTcpClient();

                // Agrega el cliente a la lista de clientes conectados
                Invoke((MethodInvoker)delegate
                {
                    clientsListBox.Items.Add("Cliente conectado: " + client.Client.RemoteEndPoint);
                    countClients++;
                    lblClientes.Text = countClients.ToString();
                });

                // Inicia un nuevo hilo para manejar las comunicaciones con el cliente
                Task.Run(() => HandleClient(client));
            }
        }

        private void HandleClient(TcpClient client)
        {
            try
            {
                // Obtiene el stream de red del cliente
                NetworkStream stream = client.GetStream();

                // Buffer para almacenar los datos recibidos del cliente
                byte[] buffer = new byte[1024];

                while (true)
                {
                    // Lee los datos del cliente y los almacena en el buffer
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    // Convierte los datos recibidos en una cadena de texto
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Invoke((MethodInvoker)delegate
                    {
                        clientsListBox.Items.Add("Mensaje recibido del cliente " + client.Client.RemoteEndPoint + ": " + dataReceived);
                    });

                    // Envía una respuesta al cliente
                    string response = "Mensaje recibido por el servidor";
                    byte[] responseData = Encoding.ASCII.GetBytes(response);
                    stream.Write(responseData, 0, responseData.Length);
                    logBuilder.AppendLine("Mensaje enviado al cliente " + client.Client.RemoteEndPoint + ": " + response);
                    //Invoke((MethodInvoker)delegate
                    //{
                    //    clientsListBox.Items.Add(logBuilder.ToString());
                    //});
                }
            }
            catch (Exception ex)
            {
                Invoke((MethodInvoker)delegate
                {
                    clientsListBox.Items.Add("Error de comunicación con el cliente " + client.Client.RemoteEndPoint + ": " + ex.Message);
                });
            }
            finally
            {
                // Elimina al cliente de la lista de clientes conectados y cierra la conexión
                Invoke((MethodInvoker)delegate
                {
                    clientsListBox.Items.Remove(client);
                    countClients--;
                    lblClientes.Text = countClients.ToString();
                });
                client.Close();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Detiene el servidor y espera a que se cierren todas las conexiones
            listener.Stop();
            Task.WaitAll(listenTask);
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            // Actualiza el registro de actividad en el TextBox
            logTextBox.Text = logBuilder.ToString();
        }

    }
}
