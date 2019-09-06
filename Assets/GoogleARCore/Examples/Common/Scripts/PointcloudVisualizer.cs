﻿// <copyright file="PointcloudVisualizer.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    /// <summary>
    /// Visualizes the feature points for spatial mapping, showing a pop animation when they appear.
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class PointcloudVisualizer : MonoBehaviour
    {
     /*   private static int port = 11111;
        //string HostIP = "172.20.10.2";
        //private static string Host = "172.20.10.2"; //HOTSPOT
        private static string Host = "192.168.8.100"; //HOME
        private static IPAddress HostIP = IPAddress.Parse(Host);
        //byte[] bytes = new byte[1024]; */

        /// <summary>
        /// The color of the feature points.
        /// </summary>
        [Tooltip("The color of the feature points.")]
        public Color PointColor;



        /// <summary>
        /// Whether to enable the pop animation for the feature points.
        /// </summary>
        [Tooltip("Whether to enable the pop animation for the feature points.")]
        public bool EnablePopAnimation = true;

        /// <summary>
        /// The maximum number of points to add per frame.
        /// </summary>
        [Tooltip("The maximum number of points to add per frame.")]
        public int MaxPointsToAddPerFrame = 100;

        /// <summary>
        /// The time interval that the pop animation lasts in seconds.
        /// </summary>
        [Tooltip("The time interval that the animation lasts in seconds.")]
        public float AnimationDuration = 0.3f;

        /// <summary>
        /// The maximum number of points to show on the screen.
        /// </summary>
        [Tooltip("The maximum number of points to show on the screen.")]
        [SerializeField] private int m_MaxPointCount = 1000;

        /// <summary>
        /// The default size of the points.
        /// </summary>
        [Tooltip("The default size of the points.")]
        [SerializeField] private int m_DefaultSize = 2;

        /// <summary>
        /// The maximum size that the points will have when they pop.
        /// </summary>
        [Tooltip("The maximum size that the points will have when they pop.")]
        [SerializeField] private int m_PopSize = 50;

        /// <summary>
        /// The mesh.
        /// </summary>
        private Mesh m_Mesh;
        private Mesh m_SaveMesh;

        /// <summary>
        /// The mesh renderer.
        /// </summary>
        private MeshRenderer m_MeshRenderer;

        /// <summary>
        /// The unique identifier for the shader _ScreenWidth property.
        /// </summary>
        private int m_ScreenWidthId;

        /// <summary>
        /// The unique identifier for the shader _ScreenHeight property.
        /// </summary>
        private int m_ScreenHeightId;

        /// <summary>
        /// The unique identifier for the shader _Color property.
        /// </summary>
        private int m_ColorId;

        /// <summary>
        /// The property block.
        /// </summary>
        private MaterialPropertyBlock m_PropertyBlock;

        /// <summary>
        /// The cached resolution of the screen.
        /// </summary>
        private Resolution m_CachedResolution;

        /// <summary>
        /// The cached color of the points.
        /// </summary>
        private Color m_CachedColor;

        /// <summary>
        /// The cached feature points.
        /// </summary>
        private LinkedList<PointInfo> m_CachedPoints;
        private LinkedList<PointInfo> m_SavedPoints;

        private int m_Track;
        private int m_Frames;
        private static int pCount;
        private Vector3 m_prevARPosePosition;
        public Pose initPose;
        public Pose curPose;
        public Pose poseTransform;
//        private int m_init;

        /// <summary>
        /// The Unity Start() method.
        /// </summary>
        public void Start()
        {
            m_MeshRenderer = GetComponent<MeshRenderer>();
            m_Mesh = GetComponent<MeshFilter>().mesh;
            if (m_Mesh == null)
            {
                m_Mesh = new Mesh();
            }


            m_Mesh.Clear();

            m_SaveMesh = GetComponent<MeshFilter>().mesh;
            if (m_SaveMesh == null)
            {
                m_SaveMesh = new Mesh();
            }

            m_SaveMesh.Clear();

            m_CachedColor = PointColor;

            m_ScreenWidthId = Shader.PropertyToID("_ScreenWidth");
            m_ScreenHeightId = Shader.PropertyToID("_ScreenHeight");
            m_ColorId = Shader.PropertyToID("_Color");

            m_PropertyBlock = new MaterialPropertyBlock();
            m_MeshRenderer.GetPropertyBlock(m_PropertyBlock);
            m_PropertyBlock.SetColor(m_ColorId, m_CachedColor);
            m_MeshRenderer.SetPropertyBlock(m_PropertyBlock);

            m_CachedPoints = new LinkedList<PointInfo>();
            m_SavedPoints = new LinkedList<PointInfo>();
            initPose = Frame.Pose;
//            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//            s.Connect(HostIP, port);
            //ExportSetUp();
            //HelloAR.Connection.Connect();

        }

        /// <summary>
        /// The Unity OnDisable() method.
        /// </summary>
        public void OnDisable()
        {
            _ClearCachedPoints();    
        }

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            // If ARCore is not tracking, clear the caches and don't update.
            if (Session.Status != SessionStatus.Tracking)
            {
                _ClearCachedPoints();
                _ClearSavedPoints();
                return;
            }

            if (Screen.currentResolution.height != m_CachedResolution.height
                || Screen.currentResolution.width != m_CachedResolution.width)
            {
                _UpdateResolution();
            }

            if (m_CachedColor != PointColor)
            {
                _UpdateColor();
            }

            _AddAllPointsToCache();
            //_AddAllPointsToSave();
            _UpdateMesh();
            //ExportMeshPoints();
            ExportPoints();
            //IncrementSend();
        }

        /// <summary>
        /// Clears all cached feature points.
        /// </summary>
        private void _ClearCachedPoints()
        {
            m_CachedPoints.Clear();
            m_Mesh.Clear();
        }

        public void ExportSetUp()
        {
           // Path of file
        string path = Application.persistentDataPath + @"/Points.txt";

        // Create Writing Object
        StreamWriter sr = new StreamWriter(path);
          
            // Create File if it doesnt exist
            if (!File.Exists(path)) {
                Debug.Log(path + " doesn't exist... Creating...");
                sr = File.CreateText(path);
                sr.WriteLine("Points \n");
                    }
            else
            {
                Debug.Log(path + " Does exist... Clearing...");
                sr = File.CreateText(path);
                sr.WriteLine("Points: \n");
            }
        sr.Close();
        }

        

        //public void ExportMeshPoints()
        //{
        //    m_Frames++;
        //    string path = Application.persistentDataPath + @"/PointsCache.txt";
        //    StreamWriter sr1 = new StreamWriter(path);
        //    int count = 1;
        //    //m_SaveMesh.vertices = m_SavedPoints.Select(p => p.Position).ToArray();
        //    var pts = m_CachedPoints.Select(p => p.Position).ToArray();
        //    foreach (Vector3 m_vec in pts) {
        //        string m_Print = m_vec.x + " " + m_vec.y + " " + m_vec.z;
        //        sr1.WriteLine(m_Print);
        //        //HelloAR.Connection.Write(m_vec);
        //        count++;
        //    }
        //  //  Debug.Log("Frame: " + m_Frames + " Wrote: " + count + " Points \n"); 
        //    sr1.Close();

        //}

        public void ExportPoints()
        {
            //// Path of file
            //string path = Application.persistentDataPath + @"/Points.txt";
           // StreamWriter sr = new StreamWriter(path, append: true);
			string buff = "";
            // Content of the file
            //m_Frames += 1;
            //sr.WriteLine("Frame: " + m_Frames + "Has " + Frame.PointCloud.PointCount + "Points");
            float minX = -0.9f;
            float minY = -0.8f;
            float minZ = 0.2f;

            float maxX = 0.3f;
            float maxY = 3.0f;
            float maxZ = 3.3f;



            if (Frame.PointCloud.PointCount > 0 && Frame.PointCloud.IsUpdatedThisFrame)
            {
                for (int i = 0; i < Frame.PointCloud.PointCount; i++)
                {
                    Vector3 point = Frame.PointCloud.GetPointAsStruct(i);
                    if (point.x < minX || point.y < minY || point.z < minZ ||
                        point.x < maxX || point.y < maxY || point.z < maxZ)
                    {
                        continue;
                    }
                    else
                    {
                        string content = point.x + " " + point.y + " " + point.z + "\n";
                        buff += content;
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
                //sr.WriteLine(buff);
                HelloAR.Connection.WriteString(Frame.PointCloud.PointCount, buff);
              
            }
            //sr.Close();
            }

        //public static void IncrementSend()
        //{
        //    //// Path of file
        //    string path = Application.persistentDataPath + @"/PointsIncrement.txt";
        //    StreamWriter sr1 = new StreamWriter(path, append: true);
        //    string buff = "";
        //    int pc = pCount;
        //    // Content of the file
        //    if (Frame.PointCloud.PointCount > 0 && Frame.PointCloud.IsUpdatedThisFrame)
        //    {
        //        for (int i = pc; i < Frame.PointCloud.PointCount; i++)
        //        {
        //            Vector3 point = Frame.PointCloud.GetPointAsStruct(i);
        //            string content = i + " " + point.x + " " + point.y + " " + point.z + "\n";
        //            buff += content;
        //            //HelloAR.Connection.WriteString(buff);
        //            //m_Track += 1;
        //            pCount++;
        //        }
        //        sr1.WriteLine(buff);

        //    }
        //}

        private void _ClearSavedPoints()
        {
            m_SavedPoints.Clear();
            m_Mesh.Clear();
        }
        /// <summary>
        /// Updates the screen resolution.
        /// </summary>
        private void _UpdateResolution()
        {
            m_CachedResolution = Screen.currentResolution;
            if (m_MeshRenderer != null)
            {
                m_MeshRenderer.GetPropertyBlock(m_PropertyBlock);
                m_PropertyBlock.SetFloat(m_ScreenWidthId, m_CachedResolution.width);
                m_PropertyBlock.SetFloat(m_ScreenHeightId, m_CachedResolution.height);
                m_MeshRenderer.SetPropertyBlock(m_PropertyBlock);
            }
        }

        /// <summary>
        /// Updates the color of the feature points.
        /// </summary>
        private void _UpdateColor()
        {
            m_CachedColor = PointColor;
            m_MeshRenderer.GetPropertyBlock(m_PropertyBlock);
            m_PropertyBlock.SetColor("_Color", m_CachedColor);
            m_MeshRenderer.SetPropertyBlock(m_PropertyBlock);
        }

        /// <summary>
        /// Adds points incrementally to the cache, by selecting points at random each frame.
        /// </summary>
        private void _AddPointsIncrementallyToCache()
        {
            if (Frame.PointCloud.PointCount > 0 && Frame.PointCloud.IsUpdatedThisFrame)
            {
                int iterations = Mathf.Min(MaxPointsToAddPerFrame, Frame.PointCloud.PointCount);
                for (int i = 0; i < iterations; i++)
                {
                    Vector3 point = Frame.PointCloud.GetPointAsStruct(
                        Random.Range(0, Frame.PointCloud.PointCount - 1));
                    _AddPointToCache(point);
                }
            }
        }
        /// <summary>
        /// Adds points incrementally to the cache, by selecting points at random each frame.
        /// </summary>
        private void _AddPointsIncrementallyToSave()
        {
            if (Frame.PointCloud.PointCount > 0 && Frame.PointCloud.IsUpdatedThisFrame)
            {
                int iterations = Mathf.Min(MaxPointsToAddPerFrame, Frame.PointCloud.PointCount);
                for (int i = 0; i < iterations; i++)
                {
                    Vector3 point = Frame.PointCloud.GetPointAsStruct(
                        Random.Range(0, Frame.PointCloud.PointCount - 1));

                    _AddPointToSave(point);
                }
            }
        }

        /// <summary>
        /// Adds all points from this frame's pointcloud to the cache.
        /// </summary>
        private void _AddAllPointsToCache()
        {
            if (Frame.PointCloud.IsUpdatedThisFrame)
            {
                for (int i = 0; i < Frame.PointCloud.PointCount; i++)
                {
                    _AddPointToCache(Frame.PointCloud.GetPointAsStruct(i));
                }
            }
        }

        /// <summary>
        /// Adds all points from this frame's pointcloud to the save.
        /// </summary>
        private void _AddAllPointsToSave()
        {
            if (Frame.PointCloud.IsUpdatedThisFrame)
            {
                for (int i = 0; i < Frame.PointCloud.PointCount; i++)
                {
                    _AddPointToSave(Frame.PointCloud.GetPointAsStruct(i));

                }
            }
        }

        /// <summary>
        /// Adds the specified point to cache.
        /// </summary>
        /// <param name="point">A feature point to be added.</param>
        private void _AddPointToCache(Vector3 point)
        {
            if (m_CachedPoints.Count >= m_MaxPointCount)
            {
                m_CachedPoints.RemoveFirst();
            }

            m_CachedPoints.AddLast(new PointInfo(point, new Vector2(m_DefaultSize, m_DefaultSize),
                                                 Time.time));
        }

        /// <summary>
        /// Adds the specified point to save.
        /// </summary>
        /// <param name="point">A feature point to be added.</param>
        private void _AddPointToSave(Vector3 point)
        {
            if (m_SavedPoints.Count >= m_MaxPointCount)
            {
                m_SavedPoints.RemoveFirst();
            
            }

            m_SavedPoints.AddLast(new PointInfo(point, new Vector2(m_DefaultSize, m_DefaultSize),
                                                 Time.time));
        }

        /// <summary>
        /// Updates the size of the feature points, producing a pop animation where the size
        /// increases to a maximum size and then goes back to the original size.
        /// </summary>
        private void _UpdatePointSize()
        {
            if (m_CachedPoints.Count <= 0 || !EnablePopAnimation)
            {
                return;
            }

            LinkedListNode<PointInfo> pointNode;

            for (pointNode = m_CachedPoints.First; pointNode != null; pointNode = pointNode.Next)
            {
                float timeSinceAdded = Time.time - pointNode.Value.CreationTime;
                if (timeSinceAdded >= AnimationDuration)
                {
                    continue;
                }

                float value = timeSinceAdded / AnimationDuration;
                float size = 0f;

                if (value < 0.5f)
                {
                    size = Mathf.Lerp(m_DefaultSize, m_PopSize, value * 2f);
                }
                else
                {
                    size = Mathf.Lerp(m_PopSize, m_DefaultSize, (value - 0.5f) * 2f);
                }

                pointNode.Value = new PointInfo(pointNode.Value.Position, new Vector2(size, size),
                                                pointNode.Value.CreationTime);
            }
        }

        /// <summary>
        /// Updates the mesh, adding the feature points.
        /// </summary>
        private void _UpdateMesh()
        {
            m_Mesh.Clear();
            m_Mesh.vertices = m_CachedPoints.Select(p => p.Position).ToArray();
            m_Mesh.uv = m_CachedPoints.Select(p => p.Size).ToArray();
            m_Mesh.SetIndices(Enumerable.Range(0, m_CachedPoints.Count).ToArray(),
                              MeshTopology.Points, 0);
        }

        private void _UpdateSavedMesh()
        {
            m_SaveMesh.Clear();
            m_SaveMesh.vertices = m_SavedPoints.Select(p => p.Position).ToArray();
            m_SaveMesh.uv = m_SavedPoints.Select(p => p.Size).ToArray();
            m_SaveMesh.SetIndices(Enumerable.Range(0, m_SavedPoints.Count).ToArray(),
                              MeshTopology.Points, 0);
        }

        /// <summary>
        /// Contains the information of a feature point.
        /// </summary>
        private struct PointInfo
        {
            /// <summary>
            /// The position of the point.
            /// </summary>
            public Vector3 Position;

            /// <summary>
            /// The size of the point.
            /// </summary>
            public Vector2 Size;

            /// <summary>
            /// The creation time of the point.
            /// </summary>
            public float CreationTime;

            public PointInfo(Vector3 position, Vector2 size, float creationTime)
            {
                Position = position;
                Size = size;
                CreationTime = creationTime;
            }
        }
    }
}
