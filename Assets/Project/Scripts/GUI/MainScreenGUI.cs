using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Stats;

// By Nick E.
namespace UI{
	public class MainScreenGUI : MonoBehaviour {
		[HideInInspector]
		public static MainScreenGUI instance;
		public GUISkin skin;
		public Texture2D mla;
		bool OptionToggle = false;
		bool StatToggle = false;
		bool CreditsToggle = false;
		bool AbilitiesToggle = false;
		bool[] KeyToggle;
		bool[] Abilities;
		bool[] CantToggle;
		bool[] Toggle;
		bool[] scroll;
		bool IsWindowed;
		bool DisplayCustom;
		string width;
		string height;
		Vector2 AbilitiesPosition = Vector2.zero;
		int GUISwitch = 0;
		float Width = 1024.0f;
		float Height = 768.0f;
		Vector3 Scale;
		void Start(){
			instance = this;
			IsWindowed = Screen.fullScreen;
			Abilities = new bool[40];
			CantToggle = new bool[40];
			scroll = new bool[10];
			Toggle = new bool[2];
			width = "";
			height = "";
			KeyToggle = new bool[Utils.KeyManager.keyCodes.Count];
		}
		void OnGUI(){
			GUI.skin = skin;
			Scale.x = Screen.width/Width;
			Scale.y = Screen.height/Height;
			Scale.z = 1;
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Scale);
			GUI.DrawTexture(new Rect(Width - 600, 50, 600, 100), mla);
			PlayButton();
			OptionsButton();
			StatisticsButton();
			CreditsButton();
			ExitButton(); 
		}
		void PlayButton(){
			if(GUI.Button(new Rect(0, 0, 200, 100), "Play")){
				Application.LoadLevel(1);
			}
		}
		public void OptionsButton(){
			if(GUI.Button(new Rect(0, 110, 200, 100), "Options")){
				OptionToggle = !OptionToggle;
				StatToggle = false;
				CreditsToggle = false;
				AbilitiesToggle = false;
			}
			if(OptionToggle){
				GUI.BeginGroup(new Rect(210, 50, 700, 500));
				GUI.Box(new Rect(0, 0, 700, 500), "Options");
				if(GUI.Button(new Rect(2, 20, 80, 30), "General"))
					GUISwitch = 1;
				if(GUI.Button(new Rect(2, 50, 80, 30), "Video"))
					GUISwitch = 2;
				if(GUI.Button(new Rect(2, 80, 80, 30), "Audio"))
					GUISwitch = 3;
				if(GUI.Button(new Rect(2, 110, 80, 30), "Controls"))
					GUISwitch = 4;
				switch(GUISwitch){
				default: break;
				case 1://general
					break;
				case 2://video
					IsWindowed = GUI.Toggle(new Rect(230, 30, 80, 20), IsWindowed, "Windowed");
					GUI.Label(new Rect(100, 30, 100, 30), "Resolution");
					if(GUI.Button(new Rect(100, 50, 100, 20), "800x600")){
						Screen.SetResolution(800, 600, IsWindowed);
					}
					if(GUI.Button(new Rect(100, 70, 100, 20), "1024x768")){
						Screen.SetResolution(1024, 768, IsWindowed);
					}
					if(GUI.Button(new Rect(100, 90, 100, 20), "1152x648")){
						Screen.SetResolution(1152, 648, IsWindowed);
					}
					if(GUI.Button(new Rect(100, 110, 100, 20), "1280x720")){
						Screen.SetResolution(1280, 720, IsWindowed);
					}
					if(GUI.Button(new Rect(100, 130, 100, 20), "1280x768")){
						Screen.SetResolution(1280, 768, IsWindowed);
					}
					if(GUI.Button(new Rect(100, 150, 100, 20), "1280x800")){
						Screen.SetResolution(1280, 800, IsWindowed);
					}
					if(GUI.Button(new Rect(100, 170, 100, 20), "1280x960")){
						Screen.SetResolution(1280, 960, IsWindowed);
					}
					if(GUI.Button(new Rect(100, 190, 100, 20), "1280x1024")){
						Screen.SetResolution(1280, 1024, IsWindowed);
					}
					if(GUI.Button(new Rect(100, 210, 100, 20), "1600x900")){
						Screen.SetResolution(1600, 900, IsWindowed);
					}
					if(GUI.Button(new Rect(100, 230, 100, 20), "1920x1080")){
						Screen.SetResolution(1920, 1080, IsWindowed);
					}
					if(GUI.Button(new Rect(100, 250, 100, 20), "Custom")){
						DisplayCustom = !DisplayCustom;
					}
					if(DisplayCustom){
						width = GUI.TextField(new Rect(100, 270, 40, 20), width, 4);
						GUI.Label(new Rect(142f, 270, 20, 20), "x");
						height = GUI.TextField(new Rect(150, 270, 40, 20), height, 4);
						if(GUI.Button(new Rect(190, 275, 10, 10), "")){
							int RWidth = int.Parse(width);
							int RHeight = int.Parse(height);
							Screen.SetResolution(RWidth, RHeight, IsWindowed);
							width = "";
							height = "";
						}
					}
					GUI.Label(new Rect(100, 290, 100, 30), "Graphics");
					if(GUI.Button(new Rect(100, 310, 100, 20), "Fastest")){
						QualitySettings.SetQualityLevel(0, true);
					}
					if(GUI.Button(new Rect(100, 330, 100, 20), "Fast")){
						QualitySettings.SetQualityLevel(1, true);
					}
					if(GUI.Button(new Rect(100, 350, 100, 20), "Simple")){
						QualitySettings.SetQualityLevel(2, true);
					}
					if(GUI.Button(new Rect(100, 370, 100, 20), "Good")){
						QualitySettings.SetQualityLevel(3, true);
					}
					if(GUI.Button(new Rect(100, 390, 100, 20), "Beautiful")){
						QualitySettings.SetQualityLevel(4, true);
					}
					if(GUI.Button(new Rect(100, 410, 100, 20), "Fantastic")){
						QualitySettings.SetQualityLevel(5, true);
					}
					GUI.Label(new Rect(230, 60, 100, 30), "Anti Aliasing");
					if(GUI.Button(new Rect(230, 80, 120, 20), "Disabled")){
						QualitySettings.antiAliasing = 0;
					}
					if(GUI.Button(new Rect(230, 100, 120, 20), "2x Multi Sampling")){
						QualitySettings.antiAliasing = 2;
					}
					if(GUI.Button(new Rect(230, 120, 120, 20), "4x Multi Sampling")){
						QualitySettings.antiAliasing = 4;
					}
					if(GUI.Button(new Rect(230, 140, 120, 20), "8x Multi Sampling")){
						QualitySettings.antiAliasing = 8;
					}
					break;
				case 3://audio
					GUI.Label(new Rect(100, 30, 100, 30), "Master Volume");
					AudioHelper.MasterVolume = GUI.HorizontalSlider(new Rect(210, 35, 300, 30), AudioHelper.MasterVolume, 0.0f, 1.0f);
					GUI.Label(new Rect(100, 60, 100, 30), "Music Volume");
					AudioHelper.MusicVolume = GUI.HorizontalSlider(new Rect(210, 65, 300, 30), AudioHelper.MusicVolume, 0.0f, 1.0f);
					GUI.Label(new Rect(100, 90, 100, 30), "Effect Volume");
					AudioHelper.EffectVolume = GUI.HorizontalSlider(new Rect(210, 95, 300, 30), AudioHelper.EffectVolume, 0.0f, 1.0f);
					GUI.Label(new Rect(100, 120, 100, 30), "Voice Volume");
					AudioHelper.VoiceVolume = GUI.HorizontalSlider(new Rect(210, 125, 300, 30), AudioHelper.VoiceVolume, 0.0f, 1.0f);
					break;
				case 4://controls
					if(GUI.Button(new Rect(250, 450, 200, 50), "Reset to Default")){
						Utils.KeyManager.ResetDefaultKeys();
					}
					GUI.Label(new Rect(100, 30, 100, 50), "Forward: " + Utils.KeyManager.Get("forward").ToString());
					GUI.Label(new Rect(100, 60, 100, 50), "Back: " + Utils.KeyManager.Get("back").ToString());
					GUI.Label(new Rect(100, 90, 100, 50), "Left: " + Utils.KeyManager.Get("left").ToString());
					GUI.Label(new Rect(100, 120, 100, 50), "Right: " + Utils.KeyManager.Get("right").ToString());
					GUI.Label(new Rect(100, 150, 100, 50), "Jump: " + Utils.KeyManager.Get("jump").ToString());
					GUI.Label(new Rect(100, 180, 100, 50), "Crouch: " + Utils.KeyManager.Get("crouch").ToString(""));
					for(int i = 0; i < Utils.KeyManager.keyCodes.Count; i++){
						KeyToggle[i] = GUI.Toggle(new Rect(180, 30 * (i + 1), 15, 15), KeyToggle[i], "");
						if(KeyToggle[i]){
							Event e = Event.current;
							if(e.isKey){
								if(KeyToggle[0]){
									Utils.KeyManager.Set("forward", e.keyCode);
									KeyToggle[0] = false;
								}
								if(KeyToggle[1]){
									Utils.KeyManager.Set("back", e.keyCode);
									KeyToggle[1] = false;
								}
								if(KeyToggle[2]){
									Utils.KeyManager.Set("left", e.keyCode);
									KeyToggle[2] = false;
								}
								if(KeyToggle[3]){
									Utils.KeyManager.Set("right", e.keyCode);
									KeyToggle[3] = false;
								}
								if(KeyToggle[4]){
									Utils.KeyManager.Set("jump", e.keyCode);
									KeyToggle[4] = false;
								}
								if(KeyToggle[5]){
									Utils.KeyManager.Set("crouch", e.keyCode);
									KeyToggle[5] = false;
								}
							}
						}
					}
					break;
				}
				GUI.EndGroup();
			}
		}
		void StatisticsButton(){
			if(GUI.Button(new Rect(0, 220, 200, 100), "Statistics")){
				StatToggle = !StatToggle;
				OptionToggle = false;
				CreditsToggle = false;
				AbilitiesToggle = false;
			}
			if(StatToggle){
				GUI.BeginGroup(new Rect(210, 50, 700, 500));
				GUI.Box(new Rect(0, 0, 700, 500), "Statistics");
				GUI.Label(new Rect(0, 20, 100, 50), "Played:");
				int seconds = (int)GlobalStats.saved.seconds;
				GUI.Label(new Rect(70, 20, 100, 50), (seconds/60/60).ToString("n0") + " hours,");
				GUI.Label(new Rect(135, 20, 100, 50), ((seconds/60)%60).ToString("n0") + " minutes,");
				GUI.Label(new Rect(210, 20, 100, 50), ((seconds%60)).ToString("n0") + " seconds.");
				GUI.EndGroup();
			}
		}
		void CreditsButton(){
			if(GUI.Button(new Rect(0, 330, 200, 100), "Credits")){
				CreditsToggle = !CreditsToggle;
				OptionToggle = false;
				StatToggle = false;
				AbilitiesToggle = false;
			}
			if(CreditsToggle){

			}
		}
		void ExitButton(){
			if(GUI.Button(new Rect(0, 440, 200, 100), "Quit")){
				LocalStorage.instance.Save();
				Utils.KeyManager.Save();
				Application.Quit();
			}
		}
	}
}