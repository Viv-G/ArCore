namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using GoogleARCore;

    public class BoxController : MonoBehaviour
    {
     //   public Anchor box_Anchor;
        private Vector3 box_size = new Vector3(2f, 2f, 2f);
        private Vector3 p0 = new Vector3(-1f, -1.5f, 0.3f);
        private Vector3 p1 = new Vector3(-1f, -1.5f, 2.3f);
        private Vector3 p2 = new Vector3(-1f, 0.5f, 0.3f);
        private Vector3 p3 = new Vector3(-1f, 0.5f, 2.3f);
        private Vector3 p4 = new Vector3(1f, -1.5f, 0.3f);
        private Vector3 p5 = new Vector3(1f, -1.5f, 2.3f);
        private Vector3 p6 = new Vector3(1f, 0.5f, 0.3f);
        private Vector3 p7 = new Vector3(1f, 0.5f, 2.3f); 

    //    public Vector3[] positions = new Vector3[4];
    //    public Vector3[] positions1 = new Vector3[4];
     //   public Vector3[] positions2 = new Vector3[3];
    //    public Vector3[] positions3 = new Vector3[4];

     //   private LineRenderer lr;
     //   private LineRenderer lr1;
     //   private LineRenderer lr2;
     //   private LineRenderer lr3;

        // Start is called before the first frame update
        void Start()
        {
        //    box_Anchor = Session.CreateAnchor(
        //        new Pose(p0, Quaternion.identity));

            //    transform.position = p0;
            //    transform.SetParent(box_Anchor.transform);
        Vector3[] positions = new Vector3[4];
        Vector3[] positions1 = new Vector3[4];
        Vector3[] positions2 = new Vector3[4];
        Vector3[] positions3 = new Vector3[4];


        // Set some positions
            positions[0] = p0;
            positions[1] = p1;
            positions[2] = p3;
            positions[3] = p2;

            positions1[0] = p0;
            positions1[1] = p4;
            positions1[2] = p5;
            positions1[3] = p1;

            positions2[0] = p6;
            positions2[1] = p4;
            positions2[2] = p3;
            positions2[3] = p7;

            positions3[0] = p3;
            positions3[1] = p2;
            positions3[2] = p6;
            positions3[3] = p7;

            //LineRenderer lr = gameObject.AddComponent<LineRenderer>();

            LineRenderer lr = gameObject.AddComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = Color.red;
            lr.startWidth = 0.1f;
            lr.endWidth = 0.1f;
            lr.positionCount = positions.Length;
            lr.SetPositions(positions);

            LineRenderer lr1 = gameObject.AddComponent<LineRenderer>();
            lr1.material = new Material(Shader.Find("Sprites/Default"));
            // lr1 = GetComponent<LineRenderer>();
            lr1.startColor = Color.red;
            lr1.startWidth = 0.1f;
            lr1.endWidth = 0.1f;
 
            lr1.positionCount = positions1.Length;
            lr1.SetPositions(positions1);

            LineRenderer lr2 = gameObject.AddComponent<LineRenderer>();
            lr2.material = new Material(Shader.Find("Sprites/Default"));
            //lr2 = GetComponent<LineRenderer>();
            lr2.startColor = Color.red;
            lr2.startWidth = 0.1f;
            lr2.endWidth = 0.1f;
            lr2.positionCount = positions2.Length;
            lr2.SetPositions(positions2);

            LineRenderer lr3 = gameObject.AddComponent<LineRenderer>();
            lr3.material = new Material(Shader.Find("Sprites/Default"));
            //lr3 = GetComponent<LineRenderer>();
            lr3.startColor = Color.red;
            lr3.startWidth = 0.1f;
            lr3.endWidth = 0.1f;
           // lr3.material = new Material(Shader.Find("Sprites/Default"));
            lr3.positionCount = positions3.Length;
            lr3.SetPositions(positions3);



            // Set some positions

            // Record the y offset from the plane.
            //    yOffset = transform.position.y - detectedPlane.CenterPose.position.y;
        }

        // Update is called once per frame
        void Update()
        {
            if (Session.Status != SessionStatus.Tracking)
            {
                return;
            }

            //   transform.position = new Vector3(transform.position.x,
            //   transform.position.y, transform.position.z);

            /*  DrawLine(L1, p0, p1);
                DrawLine(L2, p0, p2);
                DrawLine(L3, p0, p4);

                DrawLine(L4, p1, p3);
                DrawLine(L5, p1, p5);

                DrawLine(L6, p2, p3);
                DrawLine(L7, p2, p6);

                DrawLine(L8, p3, p7);

                DrawLine(L9, p4, p6);
                DrawLine(L10, p4, p5);

                DrawLine(L11, p5, p7);

                DrawLine(L12, p6, p7); */
            /*
                        lr = GetComponent<LineRenderer>();
                       /* lr.startColor = Color.red;
                        lr.startWidth = 0.1f;
                        lr.endWidth = 0.1f;
                        lr.material = new Material(Shader.Find("Sprites/Default"));
                        lr.positionCount = positions.Length;
                        lr.SetPositions(positions);

                        lr1 = GetComponent<LineRenderer>();
                     /*   lr1.startColor = Color.red;
                        lr1.startWidth = 0.1f;
                        lr1.endWidth = 0.1f;
                        lr1.material = new Material(Shader.Find("Sprites/Default"));
                        lr1.positionCount = positions1.Length;
                        lr1.SetPositions(positions1);

                        lr2 = GetComponent<LineRenderer>();
                    /*    lr2.startColor = Color.red;
                        lr2.startWidth = 0.1f;
                        lr2.endWidth = 0.1f;
                        lr2.material = new Material(Shader.Find("Sprites/Default"));
                        lr2.positionCount = positions2.Length;
                        lr2.SetPositions(positions1);

                        lr3 = GetComponent<LineRenderer>();
                     //   lr3.startColor = Color.red;
                     //   lr3.startWidth = 0.1f;
                     //   lr3.endWidth = 0.1f;
                     //   lr3.material = new Material(Shader.Find("Sprites/Default"));
                        lr3.positionCount = positions3.Length;
                        lr3.SetPositions(positions1); */
        }

        /*   void DrawLine(GameObject myLine, Vector3 start, Vector3 end)
               {
               //GameObject myLine = new GameObject();
                   myLine.transform.position = start;
                  // myLine.GetComponent<LineRenderer>();
                   LineRenderer lr = myLine.GetComponent<LineRenderer>();
                   lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
                   lr.startColor = Color.red;
                   lr.startWidth = 0.1f;
                   lr.endWidth = 0.1f;
                   lr.SetPosition(0, start);
                   lr.SetPosition(1, end);
               }*/
    }
}