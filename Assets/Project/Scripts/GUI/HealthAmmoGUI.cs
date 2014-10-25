using UnityEngine;
using System.Collections;
using Stats;

public class HealthAmmoGUI: MonoBehaviour {
	private WeaponInventory wi;
	private PlayerUnit pu;
	private GunDefinitions wepDic;

	public Texture2D crosshair;
	[Range(0.0f, 1.0f)] public float crosshairSize;
	public Vector2 minMaxSize; //X is min, Y is max. Both measured in pixels
	private float _lastAccuracy = 0.0f;
	//public GUISkin AmmoSkin;
	//public GUISkin HealthSkin;
	void Start(){
		wi = GameObject.Find("Inventory").GetComponent<WeaponInventory>(); //TODO - Remove the find!
		pu = PlayerUnit.instance;

		wepDic = GunDefinitions.Get();
	}
	void Update()
	{
		//See if our weapon's accuracy has changed.
		float accuracy = wepDic.GetAccuracy( wi.slots[wi.activeSlot].Weapon );
		if( accuracy != _lastAccuracy )
		{
			//Recalculate our crosshair size.
			if( accuracy == 0.0f )
				crosshairSize = 0.0f;
			else
				crosshairSize = accuracy / 10.0f;
		}
		_lastAccuracy = accuracy;
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
		GUI.BeginGroup(new Rect(Screen.width - 150f, Screen.height - 150f, 150, 150), "");
			GUI.Box(new Rect(0, 0, 150, 150), "");
			GUI.Label(new Rect(5, 115, 100, 75), wi.slots[wi.activeSlot].Weapon);
			GUI.Label(new Rect(5, 130, 50, 25), "Ammo:");
			GUI.Label(new Rect(55, 130, 70, 25), wi.GetClipAmmoFor(wi.slots[wi.activeSlot].Weapon).ToString("n0"));
			GUI.Label(new Rect(65, 130, 70, 25), "Clip:");
			GUI.Label(new Rect(95, 130, 70, 25), wi.GetAmmoFor(wi.slots[wi.activeSlot].Weapon).ToString("n0"));
		GUI.EndGroup();
	}


}
