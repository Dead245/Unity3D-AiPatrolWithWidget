using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(PatrolManager))]
public class PatrolManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PatrolManager myScript = (PatrolManager) target;

        if (GUILayout.Button("Add Point")) {
            myScript.GeneratePoint();
        }
    }

}
