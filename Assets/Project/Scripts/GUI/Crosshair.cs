using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {
	public Texture2D texture;
	void OnGUI(){
		GUI.DrawTexture(new Rect((Screen.width/2) - texture.width/2, (Screen.height/2) - texture.height/2, texture.width, texture.height), texture);
	}
}
