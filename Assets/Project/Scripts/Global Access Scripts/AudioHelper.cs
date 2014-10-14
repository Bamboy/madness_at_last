﻿using UnityEngine;
using System.Collections;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 09/16/14     ////
///////////////////////////////////


// This class acts as an override for AudioSource.PlayClipAtPoint.
// We need this because we need to edit the properties of the created clip.
// This class also will also handle various volume controls.

public enum SoundType
{
	None,  // No type. Sound volume is determined by the Master volume
	Effect,// Volume is determined by the effect volume * master volume
	Music, // Volume is determined by the music volume * master volume
	Voice  // Volume is determined by the voice volume * master volume
}

public class AudioHelper : MonoBehaviour 
{
	#region Global Access
	private static AudioHelper audio;
	public static AudioHelper Get() //This makes it so we can remotely access this script from anywhere in the scene.
	{
		if( !audio ) //Check if we already have an instance of this script in the scene, if so just return it.
		{
			audio = GameObject.FindObjectOfType< AudioHelper >(); //Find this script in the scene.
			if( !audio ) //Make sure we found something.
			{
				//Make a new object and attach this script to it. 
				GameObject newObj = new GameObject("=== Audio Helper ===");
				audio = newObj.AddComponent< AudioHelper >();
			}
		}
		return audio;
	}
	#endregion
	#region Volume Accessors
	private static float masterVolume  = 1.00f;

	private static float musicVolume   = 0.80f;
	private static float r_musicVolume; // r_ = volume relative to master volume
	private static float effectVolume  = 0.85f;
	private static float r_effectVolume;
	private static float voiceVolume   = 1.00f;
	private static float r_voiceVolume;
	public static float MasterVolume
	{
		get{ return masterVolume; } 
		set{
			masterVolume = Mathf.Clamp01( value );
			//Set all of the relative volumes. Because we are staying within the 0.0 - 1.0 range, its easier to multiply.
			r_musicVolume = musicVolume * masterVolume;
			r_effectVolume = effectVolume * masterVolume;
			r_voiceVolume = voiceVolume * masterVolume;
		}
	}
	public static float MusicVolume
	{
		get{ return musicVolume; } 
		set{ 
			musicVolume = Mathf.Clamp01( value );
			r_musicVolume = musicVolume * masterVolume;
		}
	}
	public static float EffectVolume
	{
		get{ return effectVolume; } 
		set{ 
			effectVolume = Mathf.Clamp01( value );
			r_effectVolume = effectVolume * masterVolume;
		}
	}
	public static float VoiceVolume
	{
		get{ return voiceVolume; } 
		set{ 
			voiceVolume = Mathf.Clamp01( value );
			r_voiceVolume = voiceVolume * masterVolume;
		}
	}

	//Gets the volume of the sound after being scaled by the SoundType volume and MasterVolume settings.
	public static float GetVolume( float baseVolume, SoundType type )
	{
		float volume = Mathf.Clamp01( baseVolume );
		switch( type )
		{
			case SoundType.None:
				return volume * masterVolume;
				break;
			case SoundType.Effect:
				return volume * r_effectVolume;
				break;
			case SoundType.Music:
				return volume * r_musicVolume;
				break;
			case SoundType.Voice:
				return volume * r_voiceVolume;
				break;
		}
		return 0.0f;
	}
	#endregion

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	// A replacement for "AudioSource.PlayClipAtPoint". Returns the created AudioSource. Deletes the object after playing. 
	// Parent transform is for organization or if we want the sound to come from a specific object as it moves.
	#region Play Clip At Point
	public AudioSource PlayClipAtPoint( AudioClip clip, Vector3 pos )
	{
		return PlayClipAtPoint( clip, pos, 1.0f );
	}
	public AudioSource PlayClipAtPoint( AudioClip clip, Vector3 pos, float volume )
	{
		return PlayClipAtPoint( clip, pos, volume, SoundType.None );
	}
	public AudioSource PlayClipAtPoint( AudioClip clip, Vector3 pos, float volume, SoundType type )
	{
		return PlayClipAtPoint( clip, pos, volume, type, this.transform );
	}
	public AudioSource PlayClipAtPoint( AudioClip clip, Vector3 pos, float volume, SoundType type, Transform parent )
	{
		GameObject obj = new GameObject("AudioClip (Temp)");
		obj.transform.position = pos;
		obj.transform.parent = parent;

		AudioSource audio = (AudioSource)obj.AddComponent<AudioSource>();
		audio.clip = clip;

		switch( type )
		{
			case SoundType.None:
				audio.volume = volume * masterVolume;
				break;
			case SoundType.Effect:
				audio.volume = volume * r_effectVolume;
				break;
			case SoundType.Music:
				audio.volume = volume * r_musicVolume;
				break;
			case SoundType.Voice:
				audio.volume = volume * r_voiceVolume;
				break;
		}

		audio.Play ();
		GameObject.Destroy( obj, clip.length ); //Destroy the object after the sound is done playing.

		return audio;
	}
	#endregion


	// Makes a new AudioSource component on the specified transform. Return the new AudioSource.
	// This makes it easy for a single script to handle multiple sounds at once, without having to recreate them every time.

	//THE INPUT VOLUME NEEDS TO BE SCALED BY "GetVolume()" ON THE OTHER SCRIPT!! 
	//(Otherwise, if the user changes volume, sounds from this component will not change volume!)
	#region Create Audio Source
	public AudioSource CreateAudioSource( Transform obj, AudioClip clip )
	{
		return CreateAudioSource( obj, clip, 1.0f );
	}
	public AudioSource CreateAudioSource( Transform obj, AudioClip clip, float volume )
	{
		return CreateAudioSource( obj, clip, volume, 1.0f );
	}
	public AudioSource CreateAudioSource( Transform obj, AudioClip clip, float volume, float pitch )
	{
		AudioSource audio = obj.gameObject.AddComponent<AudioSource>();

		audio.clip = clip;
		audio.volume = Mathf.Clamp01( volume );
		audio.pitch = Mathf.Clamp( pitch, -3.0f, 3.0f );

		return audio;
	}
	#endregion






















}
