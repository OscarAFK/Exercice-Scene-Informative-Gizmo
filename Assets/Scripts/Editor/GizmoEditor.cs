using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace technical.test.editor
{
    public class GizmoEditor : EditorWindow
    {

        public static SceneGizmoAsset data;
        public static int indexEdit = -1;

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
                    if (indexEdit == i) GUI.backgroundColor = Color.red;
                    else GUI.backgroundColor = Color.white;
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
                    if (indexEdit == i) GUI.backgroundColor = Color.red;
                    else GUI.backgroundColor = Color.white;
                    data.Gizmos[i].Position = EditorGUILayout.Vector3Field("", data.Gizmos[i].Position);
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                GUILayout.Label("");

                for (int i = 0; i < data.Gizmos.Length; i++)
                {
                    if (indexEdit == i) GUI.backgroundColor = Color.red;
                    else GUI.backgroundColor = Color.white;

                    if (GUILayout.Button("Edit", GUILayout.MinWidth(75)))
                    {
                        if (indexEdit == i) indexEdit = -1;
                        else indexEdit = i;
                    }
                }
                GUI.backgroundColor = Color.white;
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
                for (int i = 0; i < data.Gizmos.Length; i++)
                {
                    Handles.color = Color.white;
                    Handles.SphereHandleCap(0, data.Gizmos[i].Position, Quaternion.identity, 0.5f, EventType.Repaint);
                    Handles.color = Color.black;
                    Handles.DrawLine(data.Gizmos[i].Position, data.Gizmos[i].Position + Vector3.up);
                    Handles.Label(data.Gizmos[i].Position + Vector3.up * 1.25f, data.Gizmos[i].Name, EditorStyles.boldLabel);

                    if (indexEdit == i)
                    {
                        EditorGUI.BeginChangeCheck();
                        Vector3 newTargetPosition = Handles.PositionHandle(data.Gizmos[i].Position, Quaternion.identity);
                        if (EditorGUI.EndChangeCheck())
                        {
                            data.Gizmos[i].Position = newTargetPosition;
                        }
                    }
                }
            }
        }

    }
}

