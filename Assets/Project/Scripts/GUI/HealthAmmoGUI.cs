using UnityEngine;
using System.Collections;
using Stats;

public class HealthAmmoGUI: MonoBehaviour {
	private WeaponInventory wi;
	private PlayerUnit pu;
	//public GUISkin AmmoSkin;
	//public GUISkin HealthSkin;
	void Start(){
		wi = GameObject.Find("Inventory").GetComponent<WeaponInventory>();
		pu = PlayerUnit.instance;
	}
	void OnGUI(){
		DrawAmmo();
		DrawHealth();
	}
	void DrawHealth(){
		GUI.Box(new Rect(0, 0, 300, 100), "");
		GUI.Box(new Rect(5, 5, 290 * (pu.health.Current/pu.health.Max), 90), "");
	}
	void DrawAmmo(){
		//GUI.skin = AmmoSkin;
		GUI.BeginGroup(new Rect(Screen.width - 150f, Screen.height - 150f, 150, 150), "");
		GUI.Box(new Rect(0, 0, 150, 150), "");
		GUI.Label(new Rect(5, 130, 50, 25), "Ammo:");
		GUI.Label(new Rect(55, 130, 70, 25), wi.GetClipAmmoFor(wi.slots[wi.activeSlot].Weapon).ToString("n0"));
		GUI.Label(new Rect(65, 130, 70, 25), "Clip:");
		GUI.Label(new Rect(95, 130, 70, 25), wi.GetAmmoFor(wi.slots[wi.activeSlot].Weapon).ToString("n0"));
		GUI.EndGroup();
	}
}
