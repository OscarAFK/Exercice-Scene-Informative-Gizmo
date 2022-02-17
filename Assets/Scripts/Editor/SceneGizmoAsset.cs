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

        public void RemoveGizmoAt(int index)
        {
            if (index == 0) return;
            Gizmo[] temp = new Gizmo[_gizmos.Length - 1];
            for(int i = 0; i<_gizmos.Length; i++)
            {
                if (i == index) continue;
                temp[i] = _gizmos[i];
            }
            _gizmos = temp;
        }
    }

    [CustomEditor(typeof(SceneGizmoAsset))]
    public class SceneGizmoEditorUI : Editor        //This class allow us to personalize the SceneGizmoAsset in Inspector, and notably to add a button to open the editor window
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
                GizmoEditor.Data = sceneGizmoAsset;
            }
        }
    }
}