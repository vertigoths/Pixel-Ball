using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Converter))]
public class ConverterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var converter = (Converter) target;
        
        if(GUILayout.Button("Create 3D Model"))
        {
            converter.CreateThreeDimensionalModel();
        }
    }
}
