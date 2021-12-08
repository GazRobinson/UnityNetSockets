using System.Collections;
using System.Collections.Generic;
using System;
using System.Net.Sockets;
using UnityEngine;

/// <summary>
/// Example TCP client class that can connect to a server, send data and receive it back from the server
/// It is not robust and does not adequately clean up after itself
/// 
/// Gaz Robinson, Abertay University
/// CMP303
/// </summary>
public class ExampleClient : MonoBehaviour
{
    private TcpClient       client;
    private NetworkStream   stream;

    [HideInInspector]
    public Data            dataToSend = new Data();

    // Start is called before the first frame update
    void Start()
    {
        dataToSend.position = new Vector2( 5, 10 );
        dataToSend.health = 3;

        print( "Press space to connect" );
    }

    // Update is called once per frame
    void Update() {
        // Buffer for reading data
        Byte[] bytes = new Byte[256];

        if (Input.GetKeyDown(KeyCode.Space) & client == null)
        {
            Connect();
        }
        if (Input.GetKeyDown(KeyCode.S) & client != null) {
            SendMessage();
        }


        // Get a stream object for reading and writing
        if (client != null && stream.DataAvailable) {
           
            // Loop to receive all the data sent by the client.
            while (stream.DataAvailable && stream.Read( bytes, 0, bytes.Length ) != 0) {
                //Receive the data back from the server and increment the values
                Data rcvData = new Data( bytes );
                Debug.Log( "Received from server pos: " + rcvData.position + ", Health: " + rcvData.health );
                dataToSend.position = rcvData.position + Vector2.one;
                dataToSend.health = rcvData.health + 1;
            }
        }

    }

    public void Connect() {

        print( "Client connecting to server..." );
        client = new TcpClient( "127.0.0.1", 555 );
        stream = client.GetStream();
    }
    public void SendMessage() {
        print( "Sending message to server..." );
        stream.Write( dataToSend.Serialize(), 0, Data.Size );
    }
}
