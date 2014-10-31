using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour {
	public Texture2D fadeToBlack;
	private Color fade;

	void Start(){
		fade = Color.clear;
	}
	void LateUpdate(){
		if(Stats.PlayerUnit.instance.isDead){
		}
	}
	void OnGUI(){
		if(Stats.PlayerUnit.instance.isDead){
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeToBlack);
			if(GUI.Button(new Rect(Screen.width/2 - 50, Screen.height/2 - 50, 200, 200), "Restart")){
				Application.LoadLevel(Application.loadedLevel);
			}
			if(GUI.Button(new Rect(Screen.width/2 + 50, Screen.height/2 + 50, 200, 200), "Quit")){
				Application.LoadLevel(0);
			}
		}
	}
}