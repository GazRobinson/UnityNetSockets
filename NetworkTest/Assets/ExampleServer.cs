using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

/// <summary>
/// Example data class to send across a network
/// Uses simple types and contains its own function for serializing the data
/// into a byte array and deserializing it back into a class
/// 
/// Gaz Robinson, Abertay University
/// CMP303
/// </summary>
public class Data {
    public Vector2 position;
    public Int32 health;
    public Data() {
        position = Vector2.zero;
        health = -1;
    }
    public Data( byte[] bytes ) {
        Deserialize( bytes );
    }
    public byte[] Serialize() {
        List<byte> bList = new List<byte>();
        bList.AddRange( BitConverter.GetBytes( position.x ) );
        bList.AddRange( BitConverter.GetBytes( position.y ) );
        bList.AddRange( BitConverter.GetBytes( health ) );

        return bList.ToArray();
    }
    public void Deserialize( byte[] bytes) {
        position.x = BitConverter.ToSingle( bytes, 0 );
        position.y = BitConverter.ToSingle( bytes, sizeof(float) );
        health = BitConverter.ToInt32( bytes, sizeof( float ) * 2 );
    }
    public static int Size {
        get { return sizeof( float ) + sizeof( float ) + sizeof( Int32 ); }
    }
}
/// <summary>
/// Basic .NET socket TCP server.
/// Doesn't gracefully close anything, but it can accept clients, receive bytes and send bytes
/// 
/// Gaz Robinson, Abertay University
/// CMP303
/// </summary>
public class ExampleServer : MonoBehaviour
{
    TcpListener server;
    TcpClient client;
    NetworkStream clientStream;

    void Start()
    {
        InitialiseServer();
    }

    void InitialiseServer() {
        Int32 port = 555;
        IPAddress localAddr = IPAddress.Parse( "127.0.0.1" );
        server = new TcpListener( localAddr, port );
        server.Start();

        print( "Waiting for a connection... " );
    }
    // Update is called once per frame
    void Update()
    {
        // Buffer for reading data
        Byte[] bytes = new Byte[256];

        //Poll to see if there's someone waiting
        // Pending returns false if there are no waiting connections - similar to select
        if (server.Pending()) {            
            client = server.AcceptTcpClient();  //If something is pending, we will not block, so accept the client
            clientStream = client.GetStream();  //Build the stream for sending/receiving
            print( "Client connected" );
        }
        //If we have a client and there is data waiting to be read
        if (client != null && clientStream.DataAvailable )
        {
            print("Data available");
            // Loop to receive all the data sent by the client.
            while (clientStream.DataAvailable && clientStream.Read(bytes, 0, Data.Size ) != 0)
            {
                Data rcvData = new Data( bytes );
                Debug.Log( "Received from client. Pos: " + rcvData.position + ", Health: " + rcvData.health );
                
                // Send back a response.
                clientStream.Write( rcvData.Serialize(), 0, Data.Size );
            }
        }
    }
}
