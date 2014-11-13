using UnityEngine;
using System.Collections;
using Stats;

public class RPGRocket : MonoBehaviour 
{
	public DamageType damageType;
	public float maxDamage; //Damage will function as a 'falloff', so this is the maximum damage possible
	public float forceRequired = 1.0f; //The collision force needed before an explosion will happen.
	public float explosionRadius = 7.5f;
	public float armTime = 0.25f; //RPG will NEVER explode before this time.
	public float thrust = 5.0f; //Accelleration

	public ParticleSystem particles;

	private bool armed;
	// Use this for initialization
	void Awake () 
	{
		rigidbody.useGravity = true;
		Invoke("AutoCleanup", 30.0f);
		Invoke("Arm", armTime);
	}

	void Arm()
	{
		rigidbody.useGravity = false;
		armed = true;
		particles.Play( true );
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		if( armed )
		{
			rigidbody.AddRelativeForce( 0,0,thrust, ForceMode.Acceleration );
		}
	}

	void OnCollisionEnter( Collision collision )
	{
		if( collision.relativeVelocity.magnitude > forceRequired )
		{
			Debug.Log( collision.relativeVelocity );
			Explode ();
		}
	}

	void Explode()
	{
		particles.enableEmission = false;
		particles.GetComponentInChildren< ParticleSystem >().enableEmission = false;
		particles.transform.parent = null;
		DestroyObject( particles.gameObject, 6.0f );


		AutoCleanup();
	}

	void AutoCleanup()
	{ 
		DestroyObject( this.gameObject ); 
	}
}
