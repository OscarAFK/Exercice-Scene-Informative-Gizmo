using System.Collections;
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
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Label("Text");
            foreach (var g in data.Gizmos)
            {
                GUILayout.TextField(g.Name, GUILayout.MinWidth(200));
            }
            GUILayout.EndVertical();


            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Position");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            foreach (var g in data.Gizmos)
            {
                EditorGUILayout.Vector3Field("",g.Position);
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("");

            foreach (var g in data.Gizmos)
            {
                if (GUILayout.Button("Edit", GUILayout.MinWidth(75)))
                {
                    Debug.Log("Edit");
                }
            }
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

    }
}

