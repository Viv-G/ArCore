namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Creates Connection...
    /// </summary>

    public class Connection : MonoBehaviour
    {
        private int port = 9999;
        //string HostIP = "172.20.10.2";
        static string Host = "172.20.10.2";
        private IPAddress HostIP = IPAddress.Parse(Host);
        byte[] bytes = new byte[1024];
        // IPEndPoint hostEndPoint;
        // hostEndPoint = new IPEndPoint(HostIP, port);
        // Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public void Connect()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Debug.Log("Establishing Connection to " + Host);
            s.Connect(HostIP, port);
            Debug.Log("Connection established \n Recieving...");
            s.Receive(bytes);
            string Message = Encoding.ASCII.GetString(bytes);
            Debug.Log("The time got from the server is " + Message);
            s.Close();
        }
        //// Start is called before the first frame update
        //void Start()
        //{

        //}

        //// Update is called once per frame
        //void Update()
        //{

        //}
    }
}