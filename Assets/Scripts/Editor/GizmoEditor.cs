using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace technical.test.editor
{
    public class GizmoEditor : EditorWindow
    {

        private static SceneGizmoAsset data;
        private static SceneGizmoAsset backupData;
        private static int indexEdit = -1;
        private static float gizmoSphereRadius = .5f;

        public static SceneGizmoAsset Data
        {
            set
            {
                data = value;
                backupData = Instantiate(data);
            }
        }

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

                //This code is a bit ugly, and some parts are redundant. It could have been refactored to take only a few lines
                //(by ordering by row rather than column), but I couldn't get the "Position" label to be well-centered.

                GUILayout.BeginHorizontal();

                //Text
                GUILayout.BeginVertical();
                GUILayout.Label("Text");
                for(int i = 0; i<data.Gizmos.Length; i++)
                {
                    if (indexEdit == i) GUI.backgroundColor = Color.red;
                    else GUI.backgroundColor = Color.white;
                    Undo.RecordObject(data, "Changed Name Of Gizmo");
                    data.Gizmos[i].Name = EditorGUILayout.TextField(data.Gizmos[i].Name, GUILayout.MinWidth(200));
                }
                GUILayout.EndVertical();


                //Position
                GUILayout.BeginVertical();

                GUILayout.BeginHorizontal();    //This part is used to center the "Position" label.
                GUILayout.FlexibleSpace();
                GUILayout.Label("Position");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                for (int i = 0; i < data.Gizmos.Length; i++)
                {
                    if (indexEdit == i) GUI.backgroundColor = Color.red;
                    else GUI.backgroundColor = Color.white;
                    Undo.RecordObject(data, "Changed Position Of Gizmo");
                    data.Gizmos[i].Position = EditorGUILayout.Vector3Field("", data.Gizmos[i].Position);
                }
                GUILayout.EndVertical();


                //Button
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

                //Other
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Choose another Scene Gizmo Asset"))
                {
                    data = null;
                    backupData = null;
                    indexEdit = -1;
                }

            }
            else
            {
                Data = EditorGUILayout.ObjectField("Scene Gizmo Asset: ", data, typeof(SceneGizmoAsset), true) as SceneGizmoAsset;
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
            indexEdit = -1;
            SceneView.duringSceneGui -= this.OnSceneGUI;
        }

        void OnSceneGUI(SceneView sceneView)
        {
            if (data)
            {
                Ray ray = new Ray();
                if(Event.current.type == EventType.MouseDown) ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                Handles.color = Color.white;
                GUI.color = Color.black;
                for (int i = 0; i < data.Gizmos.Length; i++)
                {
                    Handles.color = Color.white;
                    Handles.SphereHandleCap(i, data.Gizmos[i].Position, Quaternion.identity, gizmoSphereRadius, EventType.Repaint);               //We draw the sphere
                    Handles.color = Color.black;
                    Handles.DrawLine(data.Gizmos[i].Position, data.Gizmos[i].Position + Vector3.up);                                 //We draw a line between the sphere and the label
                    Handles.Label(data.Gizmos[i].Position + Vector3.up * 1.25f, data.Gizmos[i].Name, EditorStyles.boldLabel);         //We draw the label

                    if (indexEdit == i)
                    {
                        EditorGUI.BeginChangeCheck();
                        Vector3 newTargetPosition = Handles.PositionHandle(data.Gizmos[i].Position, Quaternion.identity);
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(data, "Changed Position Of Gizmo");
                            data.Gizmos[i].Position = newTargetPosition;
                        }
                    }
                    
                    if (Event.current.type == EventType.MouseDown)
                    {
                        Bounds bound = new Bounds(data.Gizmos[i].Position, Vector3.one * gizmoSphereRadius);
                        if (bound.IntersectRay(ray))
                        {
                            GenericMenu menu = new GenericMenu();
                            menu.AddItem(new GUIContent("Reset Position"), false, RestPosition, i);
                            menu.AddItem(new GUIContent("Delete Gizmo"), false, DeleteGizmo, i);
                            menu.ShowAsContext();
                        }
                    }
                }
            }
        }

        void RestPosition(object obj)
        {
            data.Gizmos[(int)obj].Position = backupData.Gizmos[(int)obj].Position;
        }

        void DeleteGizmo(object obj)
        {
            data.RemoveGizmoAt((int)obj);
        }
    }
}

