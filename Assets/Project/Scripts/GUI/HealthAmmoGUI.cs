using UnityEngine;
using System.Collections;
using Stats;
using Excelsion.WeaponSystem;

public class HealthAmmoGUI : MonoBehaviour {
	private GunInventory wi;
	private PlayerUnit pu;

	public Texture2D crosshair;
	[Range(0.0f, 1.0f)] public float crosshairSize;
	public Vector2 minMaxSize; //X is min, Y is max. Both measured in pixels
	private float _lastAccuracy = 0.0f;
	private float ammoSeconds;
	private float snapTime;
	private bool isInCombat;
	//public GUISkin AmmoSkin;
	//public GUISkin HealthSkin;

	void Start(){
		wi = transform.root.GetComponentInChildren<GunInventory>(); //TODO - Remove the find!
		pu = PlayerUnit.instance;
	}
	void Update()
	{
		//See if our weapon's accuracy has changed.
		float accuracy = wi.ActiveWeapon > -1 ? wi.guns[wi.ActiveWeapon].Accuracy : 0.0f;
		if( accuracy != _lastAccuracy )
		{
			//Recalculate our crosshair size.
			if( accuracy == 0.0f )
				crosshairSize = 0.0f;
			else
				crosshairSize = accuracy / 10.0f;
		}
		_lastAccuracy = accuracy;

		if(Input.GetMouseButtonDown(0)){
			ammoSeconds = 30.0f;
			snapTime = Time.time;
			isInCombat = true;
		}
	}
	void OnGUI(){
		DrawAmmo();
		DrawHealth();
		DrawCrosshair();
	}
	void DrawCrosshair()
	{
		//Calculate our crosshair size based on the weapon's accuracy.
		int guiSize = Mathf.RoundToInt((crosshairSize * minMaxSize.y) + minMaxSize.x); //X is min, Y is max. Both measured in pixels
		
		
		Rect rect = new Rect(
			(Screen.width/2) - guiSize/2, 
			(Screen.height/2) - guiSize/2, 
			guiSize, 
			guiSize );
		GUI.DrawTexture(rect, crosshair, ScaleMode.ScaleToFit);
	}
	void DrawHealth(){
		GUI.Box(new Rect(0, 0, 300, 100), "");
		GUI.Box(new Rect(5, 5, 290 * (pu.health.Current/pu.health.Max), 90), "");
	}
	void DrawAmmo(){
		//GUI.skin = AmmoSkin;
		GUI.BeginGroup(new Rect(Screen.width - 150f, Screen.height - 50f, 150, 50), "");
		if(isInCombat){
			if(Time.time - snapTime >= ammoSeconds){
				GUI.Box(new Rect(0, 0, 150, 50), "");
			}
		}
		GUI.EndGroup();
	}
}
