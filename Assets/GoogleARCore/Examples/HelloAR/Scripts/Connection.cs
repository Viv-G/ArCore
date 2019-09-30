﻿namespace GoogleARCore.Examples.HelloAR
{
/*    using System.Collections;
    using System.Collections.Generic;
    using System.Net; */
    using System.Net.Sockets;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Creates Connection to Server and sends data as string
    /// </summary>
    ///
    public class Connection : MonoBehaviour
    {
        private static int port = 11111;
        private static string HostIP = EntryScript.HostSet;
        public static int pCount;
        public static Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        public static void Connect(string Host)
        {
            Debug.Log("Establishing Connection to " + Host);
            s.Connect(Host, port);
            Debug.Log("Connection established \n");
            if (s == null)
                Debug.LogError("Connection failed");
        }

        public static void WriteString(int NPoints, string pointBuffer)
        {
        /*    if (pCount == 0)
            {
                s.Connect(HostIP, port);
                pCount++;
            } */
            // Convert to Strings
            string numPoints = NPoints.ToString() + " ENDN\n";
            string buffSend = pointBuffer + " ENDP\n";
            // Convert to Bytes
            byte[] nSend = Encoding.ASCII.GetBytes(numPoints);
            byte[] sBytes = Encoding.ASCII.GetBytes(buffSend);
            int size = sBytes.Length;
            string sizeSend = size.ToString();
//            Debug.LogError(sizeSend);
            //SEND
            s.Send(nSend);
            s.Send(sBytes);
        }
    }
}