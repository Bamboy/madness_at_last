using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Stats;
using SimpleJSON;
using Utils.Loaders;

// By Nick E.
namespace UI{
	public class MainScreenGUI : GUIFunctions {
		[HideInInspector]
		public static MainScreenGUI instance;
		public GUISkin skin;
		public Texture2D mlaBackground;
		public Texture2D mlaTitle;
		public Texture2D whiteLine;
		public Texture2D sliderBackground;
		public Texture2D sliderWhiteBackground;
		public AudioClip mlaAudio;
		public ParticleSystem particle;
		private GUIStyle buttonStyle;
		private GUIStyle labelStyle;
		private float scrollTime = 0.0f;
		private static JSONNode creditData;
		private AudioSource music;
		
		void Start(){
			Instantiate(particle);
			instance = this;
			IsWindowed = Screen.fullScreen;
			width = "";
			height = "";
			KeyToggle = new bool[Utils.KeyManager.keyCodes.Count];
			creditData = JSONLoader.Load("Credits");
			music = AudioHelper.CreateAudioSource(this.transform, mlaAudio, 1.0f);
		}
		void Update(){
			if(CreditsToggle){
				scrollTime -= Time.deltaTime * 10.0f;
			} else {
				scrollTime = 0.0f;
			}
			music.volume = AudioHelper.GetVolume(1.0f, SoundType.Music);
		}
		void OnGUI(){
			GUI.skin = skin;
			buttonStyle = new GUIStyle(GUI.skin.button);
			labelStyle = new GUIStyle(GUI.skin.label);
			Scale.x = Screen.width/Width;
			Scale.y = Screen.height/Height;
			Scale.z = 1;
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Scale);
			GUI.DrawTexture(new Rect(Width - 600, 50, 600, 100), mlaBackground);
			GUI.DrawTexture(new Rect(Width - 600, 50, 400, 100), mlaTitle);
			PlayButton();
			OptionsButton();
			StatisticsButton();
			CreditsButton();
			ExitButton(); 
		}
		void PlayButton(){
			if(GUI.Button(new Rect(Width - 315, 150, 200, 100), "Play")){
				Application.LoadLevel(1);
			}
		}
		public void OptionsButton(){
			if(GUI.Button(new Rect(Width - 315, 250, 200, 100), "Options")){
				OptionToggle = !OptionToggle;
				StatToggle = false;
				CreditsToggle = false;
				AbilitiesToggle = false;
			}
			if(OptionToggle){
				buttonStyle.fontSize = 30;
				GUI.BeginGroup(new Rect(60, 170, 600, 500));
				GUI.DrawTexture(new Rect(0, 0, 600, 60), mlaBackground);
				GUI.Label(new Rect(10, 0, 100, 60), "Settings");
				GUI.DrawTexture(new Rect(10, 50, 580, 1), whiteLine);
				if(GUI.Button(new Rect(300, 20, 70, 30), "General", buttonStyle))
					GUISwitch = 1;
				if(GUI.Button(new Rect(370, 20, 55, 30), "Video", buttonStyle))
					GUISwitch = 2;
				if(GUI.Button(new Rect(425, 20, 55, 30), "Audio", buttonStyle))
					GUISwitch = 3;
				if(GUI.Button(new Rect(480, 20, 75, 30), "Controls", buttonStyle))
					GUISwitch = 4;
				switch(GUISwitch){
				default: break;
				case 1://general
					break;
				case 2://video
					GUI.DrawTexture(new Rect(0, 70, 600, 500), mlaBackground);
					string[] windowed = new string[]{"Fullscreen", "Windowed"};
					int windowNum = IsWindowed ? 0 : 1;
					GUI.Label(new Rect(5, 280, 180, 50), windowed[windowNum]);
					if(GUI.Button(new Rect(5, 280, 180, 50), "", buttonStyle)){
						IsWindowed = !IsWindowed;
					}
					GUI.Label(new Rect(5, 75, 140, 60), "Resolution:");
					buttonStyle.fontSize = 48;
					if(GUI.Button(new Rect(140, 80, 150, 50), Screen.width + "x" + Screen.height, buttonStyle)){
						displayResolution = !displayResolution;
						displayAA = false;
						displayGraphics = false;
					}
					if(displayResolution){
						string[] resolutions = new string[] { "800x600", "1024x768", "1152x648", "1280x720", "1280x768", "1280x800", "1280x960", "1280x1024", "1600x900", "1920x1080" };
						int le = resolutions.Length;
						buttonStyle.fontSize = 24;
						for(int i = 0; i < le; i++)
						{
							string resolution = resolutions[i];
							int indexOfX = resolution.IndexOf("x");
							int width = int.Parse(resolution.Substring(0, indexOfX));
							int height = int.Parse(resolution.Substring(indexOfX + 1, resolution.Length - indexOfX - 1));
							ResolutionButton(100, 50 + i * 20, width, height, buttonStyle, displayResolution);
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
								displayResolution = false;
							}
						}
					}
					GUI.Label(new Rect(5, 140, 130, 60), "Graphics:");
					string[] graphicSettings = new string[] {"Fastest", "Fast", "Simple", "Good", "Beautiful", "Fantastic"};
					if(GUI.Button(new Rect(115, 140, 160, 50), graphicSettings[QualitySettings.GetQualityLevel()])){
						displayGraphics = !displayGraphics;
						displayAA = false;
						displayResolution = false;
					}
					if(displayGraphics){
						for(int i = 0; i < graphicSettings.Length; i++){
							GraphicsButton(115, 200 + (i * 20), graphicSettings[i], i, buttonStyle, displayGraphics);
						}
					}
					GUI.Label(new Rect(5, 210, 160, 60), "Anti Aliasing:");
					int[] aaInt = new int[] {0, 2, 4, 8};
					string[] aaString = new string[] {"Disabled", "2x Multi Sampling", "4x Multi Sampling", "8x Multi Sampling"};
					string currentAA = QualitySettings.antiAliasing == 0 ? aaString[0] : QualitySettings.antiAliasing == 2 ? aaString[1] : QualitySettings.antiAliasing == 4 ? aaString[2] : aaString[3];
					if(GUI.Button(new Rect(150, 200, 300, 70), currentAA)){
						displayAA = !displayAA;
						displayGraphics = false;
						displayResolution = false;
					}
					if(displayAA){
						for(int i = 0; i < aaInt.Length; i++){
							buttonStyle.fontSize = 30;
							AAButton(150, 270 + (i * 40), aaString[i], aaInt[i], buttonStyle, displayAA);
						}
					}
					break;
				case 3://audio
					GUI.DrawTexture(new Rect(0, 70, 600, 500), mlaBackground);
					labelStyle.fontSize = 35;
					GUI.Label(new Rect(5, 80, 300, 60), "Master Volume", labelStyle);
					AudioHelper.MasterVolume = GUI.HorizontalSlider(new Rect(150, 85, 230, 20), AudioHelper.MasterVolume, 0.0f, 1.0f, GUIStyle.none, GUIStyle.none);
					GUI.Label(new Rect(400, 80, 80, 50), ((int)(AudioHelper.MasterVolume * 100)).ToString() + "%", labelStyle);
					for(int i = 0; i < 20; i++){
						GUI.DrawTexture(new Rect(150 + (i * 12), 91, 5, 20), sliderBackground);
					}
					for(int i = 0; i < (int)(AudioHelper.MasterVolume * 20); i++){
						GUI.DrawTexture(new Rect(150 + (i * 12), 91, 5, 20), sliderWhiteBackground);
					}
					GUI.Label(new Rect(5, 110, 300, 60), "Music Volume", labelStyle);
					AudioHelper.MusicVolume = GUI.HorizontalSlider(new Rect(115, 115, 300, 30), AudioHelper.MusicVolume, 0.0f, 1.0f, GUIStyle.none, GUIStyle.none);
					GUI.Label(new Rect(400, 110, 80, 50), ((int)(AudioHelper.MusicVolume * 100)).ToString() + "%", labelStyle);
					for(int i = 0; i < 20; i++){
						GUI.DrawTexture(new Rect(150 + (i * 12), 121, 5, 20), sliderBackground);
					}
					for(int i = 0; i < (int)(AudioHelper.MusicVolume * 20); i++){
						GUI.DrawTexture(new Rect(150 + (i * 12), 121, 5, 20), sliderWhiteBackground);
					}
					GUI.Label(new Rect(5, 140, 300, 60), "Effect Volume", labelStyle);
					AudioHelper.EffectVolume = GUI.HorizontalSlider(new Rect(115, 145, 300, 30), AudioHelper.EffectVolume, 0.0f, 1.0f, GUIStyle.none, GUIStyle.none);
					GUI.Label(new Rect(400, 140, 80, 50), ((int)(AudioHelper.EffectVolume * 100)).ToString() + "%", labelStyle);
					for(int i = 0; i < 20; i++){
						GUI.DrawTexture(new Rect(150 + (i * 12), 151, 5, 20), sliderBackground);
					}
					for(int i = 0; i < (int)(AudioHelper.EffectVolume * 20); i++){
						GUI.DrawTexture(new Rect(150 + (i * 12), 151, 5, 20), sliderWhiteBackground);
					}
					GUI.Label(new Rect(5, 170, 300, 60), "Voice Volume", labelStyle);
					AudioHelper.VoiceVolume = GUI.HorizontalSlider(new Rect(115, 175, 300, 30), AudioHelper.VoiceVolume, 0.0f, 1.0f, GUIStyle.none, GUIStyle.none);
					GUI.Label(new Rect(400, 170, 80, 50), ((int)(AudioHelper.VoiceVolume * 100)).ToString() + "%", labelStyle);
					for(int i = 0; i < 20; i++){
						GUI.DrawTexture(new Rect(150 + (i * 12), 181, 5, 20), sliderBackground);
					}
					for(int i = 0; i < (int)(AudioHelper.VoiceVolume * 20); i++){
						GUI.DrawTexture(new Rect(150 + (i * 12), 181, 5, 20), sliderWhiteBackground);
					}
					break;
				case 4://controls
					GUI.DrawTexture(new Rect(0, 70, 600, 500), mlaBackground);
					if(GUI.Button(new Rect(250, 450, 300, 50), "Reset to Default")){
						Utils.KeyManager.ResetDefaultKeys();
					}
					labelStyle.fontSize = 30;
					GUI.Label(new Rect(5, 80, 200, 50), "Forward: " + Utils.KeyManager.Get("forward").ToString(), labelStyle);
					GUI.Label(new Rect(5, 110, 200, 50), "Back: " + Utils.KeyManager.Get("back").ToString(), labelStyle);
					GUI.Label(new Rect(5, 140, 200, 50), "Left: " + Utils.KeyManager.Get("left").ToString(), labelStyle);
					GUI.Label(new Rect(5, 170, 200, 50), "Right: " + Utils.KeyManager.Get("right").ToString(), labelStyle);
					GUI.Label(new Rect(5, 200, 200, 50), "Jump: " + Utils.KeyManager.Get("jump").ToString(), labelStyle);
					GUI.Label(new Rect(5, 230, 200, 50), "Crouch: " + StringSplitter(Utils.KeyManager.Get("crouch").ToString()), labelStyle);
					for(int i = 0; i < Utils.KeyManager.keyCodes.Count; i++){
						KeyToggle[i] = GUI.Toggle(new Rect(200, 85 + (i * 30), 15, 15), KeyToggle[i], "");
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
			if(GUI.Button(new Rect(Width - 315, 350, 200, 100), "Statistics")){
				StatToggle = !StatToggle;
				OptionToggle = false;
				CreditsToggle = false;
				AbilitiesToggle = false;
			}
			if(StatToggle){
				GUI.BeginGroup(new Rect(60, 170, 600, 500));
				GUI.DrawTexture(new Rect(0, 0, 600, 60), mlaBackground);
				GUI.Label(new Rect(10, 0, 150, 60), "Statistics");
				GUI.DrawTexture(new Rect(10, 50, 580, 1), whiteLine);
				GUI.DrawTexture(new Rect(0, 70, 600, 500), mlaBackground);
				GUI.Label(new Rect(5, 80, 200, 50), "Time Played:");
				int seconds = (int)GlobalStats.saved.seconds;
				GUI.Label(new Rect(200, 80, 150, 50), (seconds/60/60).ToString("n0") + " hours,");
				GUI.Label(new Rect(305, 80, 150, 50), ((seconds/60)%60).ToString("n0") + " minutes,");
				GUI.Label(new Rect(435, 80, 150, 50), ((seconds%60)).ToString("n0") + " seconds.");
				GUI.Label(new Rect(5, 140, 200, 50), "Total Kills:");
				int kills = GlobalStats.saved.kills;
				GUI.Label(new Rect(200, 140, 100, 50), kills.ToString() + ".");
				GUI.EndGroup();
			}
		}
		void CreditsButton(){
			if(GUI.Button(new Rect(Width - 315, 450, 200, 100), "Credits")){
				CreditsToggle = !CreditsToggle;
				OptionToggle = false;
				StatToggle = false;
				AbilitiesToggle = false;
			}
			if(CreditsToggle){
				GUI.BeginGroup(new Rect(60, 170, 600, 479));
				GUI.DrawTexture(new Rect(0, 0, 600, 60), mlaBackground);
				GUI.Label(new Rect(10, 0, 150, 60), "Credits");
				GUI.DrawTexture(new Rect(10, 50, 580, 1), whiteLine);
				GUI.BeginGroup(new Rect(0, 70, 600, 450));
				GUI.DrawTexture(new Rect(0, 0, 600, 480), mlaBackground);
				int keyAdd = 50;
				foreach(KeyValuePair<string, JSONNode> kvp in creditData.AsObject){
					keyAdd += 50;
					GUI.Label(new Rect(5, 300 + keyAdd + scrollTime, 300, 50), kvp.Key);
					GUI.Label(new Rect(290, 300 + keyAdd + scrollTime, 300, 50), kvp.Value);
				}
				GUI.EndGroup();
				GUI.EndGroup();
			}
		}
		void ExitButton(){
			if(GUI.Button(new Rect(Width - 315, 550, 200, 100), "Quit")){
				LocalStorage.instance.Save();
				Utils.KeyManager.Save();
				Application.Quit();
			}
		}
		void OnLevelWasLoaded(){
			particle.Play();
			Time.timeScale = 1.0f;
		}
	}
}