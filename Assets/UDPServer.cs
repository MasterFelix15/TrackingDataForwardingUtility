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
    // prefs
    public string IP;  // define in init
    public int port;  // define in init
    public GameObject headset;
    public GameObject brain;
    public GameObject tool;
    public GameObject headsetdud;
    public GameObject braindud;
    public GameObject tooldud;
    private Dictionary<string, GameObject> objects;
    private Vector3 target = new Vector3(0.0f, 0.0f, 0.0f);

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
        objects.Add("brain", brain);
        objects.Add("tool", tool);
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();


        // status
        print("Sending to " + IP + " : " + port);
    }

    // Start is called before the first frame update
    void Start()
    {
        String strHostName = System.Net.Dns.GetHostName();
        IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
        var addr = ipEntry.AddressList;
        print(addr[addr.Length - 1].ToString());
        init();
    }

    string encodeObject(GameObject obj, GameObject dud)
    {
        var position = obj.transform.position;
        var rotation = obj.transform.rotation;
        //var relativePosition = position - headset.transform.position;
        //var relativeRotation = rotation * Quaternion.Inverse(headset.transform.rotation);
        //dud.transform.rotation = relativeRotation;
        //dud.transform.position = relativePosition;
        //dud.transform.RotateAround(target, Vector3.up, 90);
        //var offsetPosition = dud.transform.position;
        //var offsetRotation = dud.transform.rotation;

        dud.transform.position = position;
        dud.transform.rotation = rotation;
        var localPosition = dud.transform.localPosition;
        var localRotation = dud.transform.localRotation;
        string posstr = localPosition.x.ToString() + "," + localPosition.y.ToString() + "," + localPosition.z.ToString();
        string rotstr = localRotation.x.ToString() + "," + localRotation.y.ToString() + "," + localRotation.z.ToString() + "," + localRotation.w.ToString();
        return posstr + ":" + rotstr + "#";
    }

    // Update is called once per frame
    void Update()
    {
        encodeObject(headset, headsetdud);
        string objstr = encodeObject(tool, tooldud) + encodeObject(brain, braindud);
        byte[] data = Encoding.UTF8.GetBytes(objstr);
        client.Send(data, data.Length, remoteEndPoint);
        // print("packet sent:" + objstr);
    }
}
