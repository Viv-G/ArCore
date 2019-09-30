namespace GoogleARCore.Examples.Common
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using GoogleARCore;
    using System.IO;


    public class AnchorTrack : MonoBehaviour
    {
        private Anchor anchor_new;
        private Anchor anchor_old;
        private int m_Track;
        private int f_Track;
        private Vector3 minVals = new Vector3(-1.5f, -1.5f, 0.2f);
        private Vector3 maxVals = new Vector3(1.5f, 1.5f, 3f);
        public Pose curPose = Pose.identity;
        public Pose poseHome = Pose.identity;
        private Quaternion rotHome = Quaternion.identity;
        private Vector3 positionHome = Vector3.zero;
        private Vector3 OffsetPos;
        private Quaternion OffsetRot;
        private static string pathT = Application.persistentDataPath + @"/PointsTrans.txt";
        private static string path = Application.persistentDataPath + @"/Points.txt";
        private StreamWriter sr1 = new StreamWriter(pathT, append: true);
        private StreamWriter sr = new StreamWriter(path, append: true);
        private int idPoint;



        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            string buff = "";
            string buffT = "";

            if (Frame.PointCloud.PointCount > 0 && Frame.PointCloud.IsUpdatedThisFrame)
            {
                //CreateaAnchor();
                f_Track += 1;

                //anchor_old
                // Create the anchor at that point.
                if (f_Track == 1)
                {
                    anchor_new = Session.CreateAnchor(poseHome);

                   // anchor_new.transform.position

                    transform.position = poseHome.position;
                    transform.rotation = poseHome.rotation;
                    transform.parent = anchor_new.transform;
                    //anchor_new.
                }

                else
                {
                    curPose = new Pose(anchor_new.transform.position, anchor_new.transform.rotation);
                    OffsetPos = curPose.position - poseHome.position;
                    OffsetRot = Quaternion.Inverse(curPose.rotation);

                }
                
                //OffsetPos = anchor_new.transform.po
                //OffsetRot = anchor_new.transform.rotation - rotHome;

                for (int i = 0; i < Frame.PointCloud.PointCount; i++)
                {
                    Vector3 point = Frame.PointCloud.GetPointAsStruct(i);
                    idPoint = Frame.PointCloud.GetPointAsStruct(i).Id;
                    float conf = Frame.PointCloud.GetPointAsStruct(i).Confidence;
                    Vector3 Newpoint = transform.InverseTransformPoint(point);

              
                        //            newPoint = (transform.position.x,
                        // detectedPlane.CenterPose.position.y + yOffset, transform.position.z);

                    // sr.WriteLine(buff);
                    if (point.x < minVals.x || point.y < minVals.y || point.z < minVals.z ||
                        point.x > maxVals.x || point.y > maxVals.y || point.z > maxVals.z)
                    {
                        continue;
                    }
                    else
                    {
                        string content = point.x + " " + point.y + " " + point.z;
                        buff += content;
                        string contentT = Newpoint.x + " " + Newpoint.y + " " + Newpoint.z;
                        buffT += contentT;
                        //Vector3 pos = Frame.Pose.position;
                        //Quaternion rot = Frame.Pose.rotation;

                        //curPose = Frame.Pose;
                        //poseTransform = curPose.GetTransformedBy(initPose);
                        //Vector3 newPoint = transform.TransformPoint(point);
                        //Vector3 newpoint = point, poseTransform.rotation);
                        //Vector3 curPost = Frame.Pose.position;
                        //string content = m_Track + "," + newPoint.x + "," + newPoint.y + "," + newPoint.z + "," + point.x + "," + point.y + "," + point.z ;
                        //string content = m_Track + "," + point.x + "," + point.y + "," + point.z + "," + pos.x + "," + pos.y + "," + pos.z + "," + rot.w + "," + rot.x + "," + rot.y + "," + rot.z;
                        //HelloAR.Connection.WriteString(buff);
                        m_Track += 1;
                    }
                }
                sr.WriteLine(buff);
                sr.WriteLine(buffT);
                //HelloAR.Connection.WriteString(m_Track, buff);
            }


        }
        /// <summary>
        /// CREATE ANCHOR FOR EACH FRAME
        /// </summary>
        void CreateaAnchor()
        {
            // Create the position of the anchor by raycasting a point towards
            // the top of the screen.

        anchor_old = anchor_new;
        // Create the anchor at that point.
        anchor_new = Session.CreateAnchor(poseHome);

       
             // Attach the scoreboard to the anchor.
        //transform.position = poseHome.position;
        //transform.SetParent(anchor_new.transform);

            // Record the y offset from the plane.
        //    OffsetPos = transform.position - positionHome;
        //    OffsetRot = transform.rotation - rotHome;

        }
    }
}
