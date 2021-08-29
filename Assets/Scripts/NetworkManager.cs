using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{

    public string ip = "192.168.4.1";
    public int port = 80;

    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    private bool canSendData = true;
    private bool connected = false;
    private string messageToSend = "";
    private void Update()
    {
        //Debug.Log(connected);
        if (!connected)
        {
            ConnectToTcpServer();
        }
    }
    public void UpdateMessage(string message)
    {
        messageToSend = message;
    }
    /// <summary>
    /// Setup socket connection.
    /// </summary>
    private void ConnectToTcpServer()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(HandleComunication));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
    }
    /// <summary>
    /// Runs in background clientReceiveThread; Listens for incomming data.
    /// </summary>
    private void HandleComunication()
    {
        try
        {
            socketConnection = new TcpClient(ip, port);
            //Debug.Log(socketConnection.GetStream());
            connected = true;
            var bytes = new Byte[1024];
            while (true)
            {
                // Get a stream object for reading
                using (NetworkStream stream = socketConnection.GetStream())
                {

                    var clientMessageAsByteArray = Encoding.ASCII.GetBytes(messageToSend);
                    // Write byte array to socketConnection stream.
                    stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);

                    var length = 0;
                    // Read incomming stream into byte arrary.
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message.
                        var serverMessage = Encoding.ASCII.GetString(incommingData);
                        //Debug.Log(serverMessage);
                        if (!serverMessage.ToLower().Equals("ok"))
                        {
                            return;
                        }
                    }
                }
            }
        }
        catch (Exception socketException)
        {
            //Debug.Log("Socket exception: " + socketException);
            connected = false;
            return;
        }
    }
    /// <summary>
    /// Send message to server using socket connection.
    /// </summary>



}
