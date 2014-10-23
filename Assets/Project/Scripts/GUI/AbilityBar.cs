using UnityEngine;
using System.Collections;

public class AbilityBar : MonoBehaviour {
	Vector3 Scale;
	float Width = 1024.0f;
	float Height = 768.0f;
	void OnGUI(){
		Scale.x = Screen.width/Width;
		Scale.y = Screen.height/Height;
		Scale.z = 1;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Scale);
	}
	void DrawAbilities(int skillNumber, Texture2D skillIcon){

	}
}