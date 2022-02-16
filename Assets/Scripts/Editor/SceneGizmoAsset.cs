using UnityEditor;
using UnityEngine;

namespace technical.test.editor
{
    [CreateAssetMenu(fileName = "Scene Gizmo Asset", menuName = "Custom/Create Scene Gizmo Asset")]
    public class SceneGizmoAsset : ScriptableObject
    {
        [SerializeField] private Gizmo[] _gizmos = default;

        public Gizmo[] Gizmos
        {
            get{return _gizmos;}
        }

        public override string ToString()
        {
            return "Gizmo count : " + _gizmos.Length;
        }
    }

    [CustomEditor(typeof(SceneGizmoAsset))]
    public class SceneGizmoEditorUI : Editor
    {
        private SceneGizmoAsset sceneGizmoAsset;

        public void OnEnable()
        {
            if (sceneGizmoAsset == null)
            {
                sceneGizmoAsset = target as SceneGizmoAsset;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Open Gizmo Editor"))
            {
                GizmoEditor.ShowWindow();
                GizmoEditor.data = sceneGizmoAsset;
            }
        }
    }
}