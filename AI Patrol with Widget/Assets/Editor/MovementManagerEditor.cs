using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(MovementManager))]
public class MovementManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MovementManager myScript = (MovementManager) target;
    }

}
