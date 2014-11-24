using UnityEngine;
using System.Collections;

namespace Excelsion.WeaponSystem
{
	public class WeaponAnimation : MonoBehaviour 
	{
		private Animator anim;
		private bool reload;

		private void Awake()
		{
			anim = GetComponent<Animator>();
		}
		public void Update()
		{
			if(!reload){
				if(Input.GetKey(Utils.KeyManager.Get("forward")))
				{
					anim.Play("walking");
				} else {
					anim.Play("idle");
				}
			}
		}
		public void OnShotFired()
		{
			if(!reload){
				anim.Play("idle_oneshot");
			}
		}
		public void OnReloadStart(float time)
		{
			anim.Play("reload");
			reload = true;
		}
		public void OnReloadFinished()
		{
			reload = false;
		}
	}
}