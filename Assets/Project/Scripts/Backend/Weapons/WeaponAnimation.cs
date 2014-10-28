using UnityEngine;
using System.Collections;

public class WeaponAnimation : MonoBehaviour {
	private Animator anim;
	private bool reload;

	private void Awake(){
		anim = GetComponent<Animator>();
	}
	public void Update(){
		if(!reload){
			if(Input.GetKey(Utils.KeyManager.GetKeyCode("forward"))){
				anim.Play("walking");
			} else {
				anim.Play("idle");
			}
		}
	}
	public void OnShotFired(){
		if(!reload){
			anim.Play("idle_oneshot");
		}
	}
	public void OnReloadStart(){
		anim.Play("reload");
		reload = true;
	}
	public void OnReloadFinished(){
		reload = false;
	}
}