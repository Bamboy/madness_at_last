/*
Simon Lager
2014-08-19
00:25
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]
public class InGameUi : MonoBehaviour {

	// health value store the current health of player
	public float healthValue;
	//store the max health of the player
	public float maxHealthValue;
	// store the ammo of the player
	public int ammoValue;
	//display all the ammo
	public int totalAmmo;


	//Position Of ui on the screen
	[Range(-1024,1024)] public float uiPosX;
	[Range(-768,768)]public float uiPosY;
	[Range(0,1024)]public float posXHealth;
	[Range(0,768)]public float posYHealth;
	[Range(0,1024)]public float posXAmmo;
	[Range(0,768)]public float posYAmmo;
	[Range(0,1024)]public float posXTimer;
	[Range(0,768)]public float posYTimer;
	
	// w and h of the screen
	private float _uiWidth = Screen.width;
	private float _uiHeight = Screen.height;
	private float _uiWidthHealth;
	private float _uiHieghtHealth;

	//background for health
	private float _healthBg;

	//precent meater
	private float _precent;

	//puts the width of the health background
	public void Awake(){
		_healthBg = CalcHealthPrecent(healthValue)  * 4 ;
	}


	public void Update(){
		//Calculating the width of the health bar
		_uiHieghtHealth = _uiHeight / 30;

		// so health dosent go below 0
		 if (healthValue < 0) {
			healthValue = 0;
				}
		//so health dosent go over max health and dosent go over the bar
		else if(healthValue > maxHealthValue){
			healthValue = maxHealthValue;
			_uiWidthHealth = healthValue ;
		}
		//display the same width as the bg health bar
		else {
			_uiWidthHealth = CalcHealthPrecent(healthValue) * 4;		
				}
				
				}



		public void OnGUI(){

		//displays the ui on the screen
		GUI.BeginGroup(new Rect(uiPosX,uiPosY , _uiWidth, _uiHeight));
		healthBar(posXHealth,posYHealth,_uiWidthHealth,_uiHieghtHealth);
		AmmoBoxBar(posXAmmo,posYAmmo,totalAmmo);
		AmmoBoxBarClip(posXAmmo,posYAmmo,ammoValue);
		TimerBox (posXTimer,posYTimer,200);
		GUI.EndGroup();

		}

	// display a health bar whit a bg
	public void healthBar(float screenX,float screenY,float width,float height){
		GUI.Box(new Rect(screenX,screenY,width,height),"" + CalcHealthPrecent(healthValue) + "%");
		GUI.Box (new Rect(screenX,screenY,_healthBg,height),"");
	}
	
	//display a ammo bar
	public void AmmoBoxBar(float screenX,float screenY,int ammo){
		GUI.Box (new Rect(screenX,screenY,100,25),"Ammo: " + ammo);
	}
	//display a clip bar
	public void AmmoBoxBarClip(float screenX,float screenY,int ammo){
		GUI.Box (new Rect(screenX,screenY +24,100,25),"Clip: " + ammo);
	}
	//display a Timer box prototype atm
	public void TimerBox(float screenX,float screenY,int amountTime){
		GUI.Box (new Rect(screenX,screenY,100,100),"Time: " + amountTime + "s");
	}

	//Calculates the health toa a precent
	public float CalcHealthPrecent(float currentHealth){
		_precent = Mathf.Round(currentHealth / maxHealthValue * 100);
		return _precent;
	}


}
