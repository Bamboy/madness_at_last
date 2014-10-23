using UnityEngine;
using UnityEditor;
using Stats;

public class StatWindow : EditorWindow
{
	private StatObject statObject;
	private StatInfo removeStatInfo;

	string id;
	float value;
	float min = float.MinValue;
	float max = float.MaxValue;

	[MenuItem ("Window/Stat Window")]
	public static StatWindow ShowWindow()
	{
		return (StatWindow)EditorWindow.GetWindow(typeof(StatWindow));
	}

	public void Init(StatObject statObject)
	{
		this.statObject = statObject;
	}

	void OnGUI()
	{
		if (statObject == null)
		{
			EditorGUILayout.LabelField("No StatObject! Please reopen.", EditorStyles.boldLabel);
			return;
		}

		if (removeStatInfo != null)
		{
			statObject.statsInspector.Remove(removeStatInfo);
			removeStatInfo = null;
		}

		EditorGUILayout.LabelField(statObject.name + "' Stats (" + statObject.statsInspector.Count + ")", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		id = EditorGUILayout.TextField("ID", id);
		value = EditorGUILayout.FloatField("Value", value);
		min = EditorGUILayout.FloatField("Minimum", min);
		max = EditorGUILayout.FloatField("Maximum", max);
		if (GUILayout.Button("Add Stat"))
		{
			StatInfo statInfo = new StatInfo(id, value, min, max);
			statObject.statsInspector.Add(statInfo);
		}

		EditorGUILayout.Space();
		int le = statObject.statsInspector.Count;
		for(int i = 0; i < le; i++)
		{
			StatInfo statInfo = statObject.statsInspector[i];
			EditorGUILayout.LabelField(string.Format("{0} = {1}, Min: {2}, Max: {3}", statInfo.id, statInfo.baseValue, statInfo.min, statInfo.max));
			if (GUILayout.Button("Set"))
				statInfo.Init(id, value, min, max);
			if (GUILayout.Button("Remove"))
				removeStatInfo = statInfo;
		}
	}
}
