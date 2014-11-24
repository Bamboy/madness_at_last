using UnityEngine;
using System.Collections;

namespace Excelsion.WeaponSystem
{
	public class WeaponAnimation : MonoBehaviour 
	{
		private Animator anim;
		private GunInventory gunInv;
		private bool reload;

		private void Awake()
		{
			anim = GetComponent< Animator >();
			gunInv = transform.root.GetComponentInChildren< GunInventory >();
		}
		public void Update()
		{
			if(!reload)
			{
				if(Input.GetKey(Utils.KeyManager.Get("forward")))
				{
					anim.Play("walking");
					//anim.SetBool("walking", true);
				} else {
					//anim.SetBool("walking", false);
					anim.Play("idle");
				}
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

		public void OnGunRaycastFired( BulletInfo[] bullets )
		{ PlayShotFired(); }
		public void OnGunPrefabFired()
		{ PlayShotFired(); }
		private void PlayShotFired()
		{
			if(!reload)
			{
				//float multiplier = VectorExtras.GetDurationMultiplier( animation["idle_oneshot"].length, gunInv.guns[ gunInv.ActiveWeapon ].FireRate );
				//anim.speed = multiplier; //'speed' acts as a multiplier!
				anim.Play("idle_oneshot");
			}
		}






	}
}