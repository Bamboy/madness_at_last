using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Stats;

[CustomEditor(typeof(StatObject), true)]
public class StatObjectEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		StatObject statObject = (StatObject)target;

		if (statObject.statsInspector == null)
			statObject.statsInspector = new List<StatInfo>();

		EditorGUILayout.LabelField("Stats: " + statObject.statsInspector.Count);

		if (GUILayout.Button("Open stat editor"))
		{
			StatWindow statWindow = StatWindow.ShowWindow();
			statWindow.Init(statObject);
		}

		if (GUI.changed)
			EditorUtility.SetDirty(statObject);
	}
}