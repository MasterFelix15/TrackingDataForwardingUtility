using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPServer : MonoBehaviour
{
    private static int localPort;

    // prefs
    public string IP;  // define in init
    public int port;  // define in init
    public List<GameObject> objects;
   
    // "connection" things
    IPEndPoint remoteEndPoint;
    UdpClient client;

    void init()
    {
        if (String.IsNullOrEmpty(IP))
        {
            IP = "127.0.0.1";
        }
        if (port == 0)
        {
            port = 8051;
        }

        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();
       
        // status
        print("Sending to "+IP+" : "+port);
    }

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    string encodeObjects()
    {
        string objstr = "";
        foreach (GameObject obj in objects)
        {
            var position = obj.transform.position;
            string posstr = position.x.ToString() + "," + position.y.ToString() + "," + position.z.ToString();
            var rotation = obj.transform.rotation;
            string rotstr = rotation.w.ToString() + "," + rotation.x.ToString() + "," + rotation.y.ToString() + "," + rotation.z.ToString();
            objstr += obj.name + ":" + posstr + ":" + rotstr + "#";
        }
        return objstr;
    }

    // Update is called once per frame
    void Update()
    {
        string objstr = encodeObjects();
        byte[] data = Encoding.UTF8.GetBytes(objstr);
        client.Send(data, data.Length, remoteEndPoint);
        print("packet sent:" + objstr);
    }
}
