using UnityEngine;
using System.Collections;

public class WeaponAnimation : MonoBehaviour {
	private GunDefinitions gunDef;
	private WeaponInventory weapInv;
	private Animator anim;
	private bool reload;

	private void Awake(){
		anim = GetComponent<Animator>();
		gunDef = GunDefinitions.Get();
		weapInv = transform.root.GetComponentInChildren<WeaponInventory>();
	}
	public void Update(){
		if(!reload){
			if(Input.GetKey(Utils.KeyManager.Get("forward"))){
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
	public void OnReloadStart(float time){
		anim.Play("reload");
		reload = true;
	}
	public void OnReloadFinished(){
		reload = false;
	}
}