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
	protected bool displayResolution;
	protected bool displayGraphics;
	protected bool displayAA;
	protected string width;
	protected string height;
	protected Vector2 AbilitiesPosition = Vector2.zero;
	protected int GUISwitch = 0;
	protected float Width = 1024.0f;
	protected float Height = 768.0f;
	protected Vector3 Scale;

	protected void ResolutionButton(int x, int y, int width, int height, bool close){
		if(GUI.Button(new Rect(x, y, 100, 40), width + "x" + height)){
			Screen.SetResolution(width, height, IsWindowed);
			close = false;
		}
	}
	protected void ResolutionButton(int x, int y, int width, int height, GUIStyle style, bool close){
		if(GUI.Button(new Rect(x, y, 100, 40), width + "x" + height, style)){
			Screen.SetResolution(width, height, IsWindowed);
			close = false;
		}
	}
	protected void GraphicsButton(int x, int y, string text, int numberLevel, GUIStyle style, bool close){
		if(GUI.Button(new Rect(x, y, 100, 40), text, style)){
			QualitySettings.SetQualityLevel(numberLevel, true);
			close = false;
		}
	}
	protected void AAButton(int x, int y, string text, int numberLevel, GUIStyle style, bool close){
		if(GUI.Button(new Rect(x, y, 100, 40), text, style)){
			QualitySettings.antiAliasing = numberLevel;
			close = false;
		}
	}
	protected static string StringSplitter( string stringtosplit){
		string words = string.Empty;
		if(!string.IsNullOrEmpty(stringtosplit)){
			foreach(char ch in stringtosplit){
				if(char.IsLower(ch)){
					words += ch.ToString();
				} else {
					words += " " + ch.ToString();
				}
			}
			return words;
		} else {
			return string.Empty;
		}
	}
}