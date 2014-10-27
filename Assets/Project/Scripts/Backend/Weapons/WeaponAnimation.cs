using UnityEngine;
using System.Collections;

public class WeaponAnimation : MonoBehaviour {
	private Animator anim;
	private bool reload;

	private void Awake(){
		anim = GetComponent<Animator>();
	}
	public void Update(){
		if(Input.GetKey(Utils.KeyManager.GetKeyCode("forward"))){
			anim.Play("walking");
		} else {
			anim.Play("idle");
		}
		Debug.Log(reload);
	}
	public void OnShotFired(){
		if(!reload){
			anim.Play("idle_oneshot");
		}
	}
	public void OnReloadStart(){
		anim.CrossFade("reload", 0.2f);
		reload = true;
	}
	public void OnReloadFinished(){
		reload = false;
	}
}