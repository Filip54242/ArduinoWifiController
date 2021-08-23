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
    private bool canSendData = false;

    void Start()
    {
        ConnectToTcpServer();
    }
    /// <summary> 	
    /// Setup socket connection. 	
    /// </summary> 	
    private void ConnectToTcpServer()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
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
    private void ListenForData()
    {
        try
        {
            socketConnection = new TcpClient(ip, port);
            var bytes = new Byte[1024];
            while (true)
            {
                // Get a stream object for reading 				
                using (NetworkStream stream = socketConnection.GetStream())
                {
                    var length = 0;
                    // Read incomming stream into byte arrary. 					
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message. 						
                        var serverMessage = Encoding.ASCII.GetString(incommingData);
                        if (serverMessage.ToLower().Equals("ok"))
                        {
                            canSendData = true;
                        }
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
    /// <summary> 	
    /// Send message to server using socket connection. 	
    /// </summary> 	
    public void SendData(string message)
    {
        StartCoroutine(SendDataToServer(message));
    }
    private IEnumerator SendDataToServer(string message)
    {
        if (!canSendData)
        {
            yield return new WaitForSeconds(0.1f);
        }
        if (socketConnection == null)
        {
            yield return null;
        }
        try
        {
            // Get a stream object for writing. 			
            var stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                var clientMessageAsByteArray = Encoding.ASCII.GetBytes(message);
                // Write byte array to socketConnection stream.                 
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                canSendData = false;
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
        yield return null;
    }
}