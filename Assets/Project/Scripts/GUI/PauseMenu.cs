using UnityEngine;
using System.Collections;

//By Nick Evans

public class PauseMenu : MonoBehaviour {
	private float fadeSpeed = 1.0f;
	private int GUISwitch;
	private bool pause;
	private bool OptionToggle;
	private bool IsWindowed;
	private bool DisplayCustom;
	private bool[] KeyToggle;
	private string width;
	private string height;

	void Start(){
		guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
		guiTexture.color = Color.clear;
		width = "";
		height = "";
		KeyToggle = new bool[Utils.KeyManager.keyCodes.Count];
	}
	void Update(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			pause = !pause;
		}
		if(pause){
			Screen.lockCursor = false;
			transform.root.GetComponentInChildren<MouseLook>().enabled = false;
			Camera.main.GetComponent<MouseLook>().enabled = false;
			transform.root.GetComponentInChildren<DrifterInput>().enabled = false;
			FadeToBlack();
			StartCoroutine(wait(0.0f));
		} else {
			Screen.lockCursor = true;
			transform.root.GetComponentInChildren<MouseLook>().enabled = true;
			Camera.main.GetComponent<MouseLook>().enabled = true;
			transform.root.GetComponentInChildren<DrifterInput>().enabled = true;
			Time.timeScale = 1.0f;
			FadeToNormal();
			OptionToggle = false;
		}
	}
	#region Fading
	void FadeToBlack(){
		guiTexture.color = Color.Lerp(guiTexture.color, Color.black, fadeSpeed * Time.deltaTime);
	}
	void FadeToNormal(){
		guiTexture.color = Color.Lerp(guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
	IEnumerator wait(float time){
		yield return new WaitForSeconds(3);
		Time.timeScale = time;
	}
	#endregion
	#region GUI
	void OnGUI(){
		if(pause){
			GUI.Box(new Rect(Screen.width - 50, Screen.height - 300, 100, 600), "");
			ResumeButton();
			OptionsButton();
			ExitButton();
		}
	}
	#endregion
	#region Buttons
	void ResumeButton(){
		if(GUI.Button(new Rect(Screen.width/2 - 50, Screen.height/2 - 300, 100, 100), "Resume")){
			pause = false;
			OptionToggle = false;
		}
	}
	void OptionsButton(){
		if(GUI.Button(new Rect(Screen.width/2 - 50, Screen.height/2 - 200, 100, 100), "Options")){
			OptionToggle = !OptionToggle;
		}
		if(OptionToggle){
			GUI.BeginGroup(new Rect(50, 50, 700, 500));
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
				GUI.Label(new Rect(100, 180, 100, 50), "Crouch: " + Utils.KeyManager.Get("crouch").ToString("G"));
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
	void ExitButton(){
		if(GUI.Button(new Rect(Screen.width/2 - 50, Screen.height/2 - 100, 100, 100), "Exit")){
			Application.LoadLevel(0);
		}
	}
	#endregion
}