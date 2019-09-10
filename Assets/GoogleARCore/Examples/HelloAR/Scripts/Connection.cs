namespace GoogleARCore.Examples.HelloAR
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Net;
	using System.Net.Sockets;
	using System.Text;
	using UnityEngine;
    using System.IO;

    /// <summary>
    /// Creates Connection...
    /// </summary>
    ///


    public class Connection : MonoBehaviour
	{
		private static int port = 11111;
		//string HostIP = "172.20.10.2";
		//private static string Host = "172.20.10.2"; //HOTSPOT
        private static string HostIP = EntryScript.HostSet;
                                                    //private static string Host = "192.168.8.100"; //HOME
                                                    //private static IPAddress HostIP = IPAddress.Parse(Host);
        public static int pCount;
		// IPEndPoint hostEndPoint;
		// hostEndPoint = new IPEndPoint(HostIP, port);
		public static Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


		public static void Connect(string Host)
		{
			//Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			Debug.Log("Establishing Connection to " + Host);
            //IPAddress HostIP = IPAddress.Parse(Host);
            s.Connect(HostIP, port);
			Debug.Log("Connection established \n");
			//s.Receive(bytes);
			//string Message = Encoding.ASCII.GetString(bytes);
			//Debug.Log("The time got from the server is " + Message);
		}

		public static void WriteFloats(Vector3 wPoint)
		{
			//Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			//Debug.Log("Establishing Connection to " + Host);
			s.Connect(HostIP, port);
			// Debug.Log("Connection established \n Writing... \n");
			string sWrite = wPoint.x + " " + wPoint.y + " " + wPoint.z + "\n";
			byte[] sBytes = Encoding.ASCII.GetBytes(sWrite);
			s.Send(sBytes);
			//Debug.Log("Sent: " + sWrite);
			s.Close();
		}

		public static void WriteString(int NPoints, string pointBuffer)
		{
            // Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (pCount == 0) {
                s.Connect(HostIP, port);
                pCount++;
            }
            
			//Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			//Debug.Log("Establishing Connection to " + Host);
			//s.Connect(HostIP, port);
            // Debug.Log("Connection established \n Writing... \n");
            // Convert to Strings
            string numPoints = NPoints.ToString() + " ENDN\n";
            string buffSend = /*"START\n" + numPoints + "\nPOINTS\n" + */pointBuffer + " ENDP\n";
            // Convert to Bytes
            byte[] nSend = Encoding.ASCII.GetBytes(numPoints);
            byte[] sBytes = Encoding.ASCII.GetBytes(buffSend);
            //int size = sBytes.Length;
            //string sizeSend = size.ToString();
            //byte[] sSendByte = Encoding.ASCII.GetBytes(sizeSend);
            //SEND
            //s.Send(sSendByte);
            s.Send(nSend);
			s.Send(sBytes);
			//Debug.Log("Sent: " + sBytes);
			//s.Close();
		}

      /*  public static void IncrementSend()
        {
            //// Path of file
            string path = Application.persistentDataPath + @"/PointsIncrement.txt";
            StreamWriter sr1 = new StreamWriter(path, append: true);
            string buff = "";
            //int pc = pCount;
            // Content of the file
            if (Frame.PointCloud.PointCount > 0 && Frame.PointCloud.IsUpdatedThisFrame)
            {
                for (int i = pc; i < Frame.PointCloud.PointCount; i++)
                {
                    Vector3 point = Frame.PointCloud.GetPointAsStruct(i);
                    string content = i + " " + point.x + " " + point.y + " " + point.z + "\n";
                    buff += content;
                    //HelloAR.Connection.WriteString(buff);
                    //m_Track += 1;
                    pCount++;
                }
                sr1.WriteLine(buff);

            }
        } */

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