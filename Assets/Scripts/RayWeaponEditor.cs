using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RayWeapon))]
public class RayWeaponEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var rayWeapon = (RayWeapon) target;
        
        if(GUILayout.Button("Set Map Reference"))
        {
            var map = FindObjectOfType<Converter>().GetMap();
            rayWeapon.SetMapReference(map);
        }
    }
}
