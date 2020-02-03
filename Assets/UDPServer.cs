using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class UDPServer : MonoBehaviour
{
    // prefs
    public string IP;  // define in init
    public int port;  // define in init
    public GameObject headset;
    public GameObject brain;
    public GameObject tool;
    private Dictionary<string, GameObject> objects;
   
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
        objects = new Dictionary<string, GameObject>();
        objects.Add("headset", headset);
        objects.Add("brain", brain);
        objects.Add("tool", tool);
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
        foreach (string name in objects.Keys)
        {
            GameObject obj = objects[name];
            var position = obj.transform.position;
            string posstr = position.x.ToString() + "," + position.y.ToString() + "," + position.z.ToString();
            var rotation = obj.transform.rotation;
            string rotstr = rotation.x.ToString() + "," + rotation.y.ToString() + "," + rotation.z.ToString() + "," + rotation.w.ToString();
            objstr += name + ":" + posstr + ":" + rotstr + "#";
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
