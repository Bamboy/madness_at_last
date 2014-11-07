using UnityEngine;
using System.Collections;

public class GUIFunctions : MonoBehaviour {
	protected bool IsWindowed;
	protected bool OptionToggle = false;
	protected bool StatToggle = false;
	protected bool CreditsToggle = false;
	protected bool AbilitiesToggle = false;
	protected bool[] KeyToggle;
	protected bool[] Abilities;
	protected bool[] CantToggle;
	protected bool[] Toggle;
	protected bool[] scroll;
	protected bool DisplayCustom;
	protected string width;
	protected string height;
	protected Vector2 AbilitiesPosition = Vector2.zero;
	protected int GUISwitch = 0;
	protected float Width = 1024.0f;
	protected float Height = 768.0f;
	protected Vector3 Scale;

	protected void ResolutionButton(int x, int y, int width, int height){
		if(GUI.Button(new Rect(x, y, 100, 20), width + "x" + height)){
			Screen.SetResolution(width, height, IsWindowed);
		}
	}
}