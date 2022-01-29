using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoidController))]
public class BoidControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BoidController boidController = (BoidController)target;
        if (GUILayout.Button("Update Boid Values"))
            boidController.UpdateBoidValues();
        if (GUILayout.Button("Randomize Boid Values Separately"))
            boidController.RandomizeBoidValuesSeparately();
        if (GUILayout.Button("Randomize Boid Values Together"))
            boidController.RandomizeBoidValuesTogether();
        if (GUILayout.Button("Randomize Each Boid Value Separately"))
            boidController.RandomizeEachBoidValueSeparately();
        if (GUILayout.Button("Randomize Each Boid Value Together"))
            boidController.RandomizeEachBoidValueTogether();
    }
}
