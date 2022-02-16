﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace technical.test.editor
{
    public class GizmoEditor : EditorWindow
    {

        public static SceneGizmoAsset data;

        [MenuItem("Window/Custom/Show Gizmos")]
        public static void ShowWindow()
        {
            GetWindow(typeof(GizmoEditor), false, "Gizmo Editor");
        }

        private void OnGUI()
        {
            GUILayout.Label("Gizmo Editor", EditorStyles.boldLabel);

            if (data)
            {
                GUILayout.BeginHorizontal();

                GUILayout.BeginVertical();
                GUILayout.Label("Text");
                for(int i = 0; i<data.Gizmos.Length; i++)
                {
                    data.Gizmos[i].Name = EditorGUILayout.TextField(data.Gizmos[i].Name, GUILayout.MinWidth(200));
                }
                GUILayout.EndVertical();


                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Position");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                for (int i = 0; i < data.Gizmos.Length; i++)
                {
                    data.Gizmos[i].Position = EditorGUILayout.Vector3Field("", data.Gizmos[i].Position);
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.Label("");

                for (int i = 0; i < data.Gizmos.Length; i++)
                {
                    if (GUILayout.Button("Edit", GUILayout.MinWidth(75)))
                    {
                        
                    }
                }
                GUILayout.EndVertical();

                GUILayout.EndHorizontal();

                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Choose another Scene Gizmo Asset"))
                {
                    data = null;
                }

            }
            else
            {
                data = EditorGUILayout.ObjectField("Scene Gizmo Asset: ", data, typeof(SceneGizmoAsset), true) as SceneGizmoAsset;
            }
        }

        // Window has been selected
        void OnFocus()
        {
            SceneView.duringSceneGui += this.OnSceneGUI;
        }

        private void OnLostFocus()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
        }

        void OnDestroy()
        {
            SceneView.duringSceneGui -= this.OnSceneGUI;
        }

        void OnSceneGUI(SceneView sceneView)
        {
            if (data)
            {
                Handles.color = Color.white;
                GUI.color = Color.black;
                foreach (var g in data.Gizmos)
                {
                    Handles.color = Color.white;
                    Handles.SphereHandleCap(0, g.Position, Quaternion.identity, 0.5f, EventType.Repaint);
                    Handles.color = Color.black;
                    Handles.DrawLine(g.Position, g.Position + Vector3.up);
                    Handles.Label(g.Position + Vector3.up * 1.25f, g.Name, EditorStyles.boldLabel);
                }
            }
        }

    }
}

