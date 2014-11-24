using UnityEngine;
using System.Collections;

///////////////////////////////////
/// By: Stephan "Bamboy" Ennen ////
/// Last Updated: 10/28/14     ////
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
	private static AudioHelper audHelper;
	public static AudioHelper Get() //This makes it so we can remotely access this script from anywhere in the scene.
	{
		if( !audHelper ) //Check if we already have an instance of this script in the scene, if so just return it.
		{
			audHelper = GameObject.FindObjectOfType< AudioHelper >(); //Find this script in the scene.
			if( !audHelper ) //Make sure we found something.
			{
				//Make a new object and attach this script to it. 
				GameObject newObj = new GameObject("=== Audio Helper ===");
				audHelper = newObj.AddComponent< AudioHelper >();
			}
		}
		return audHelper;
	}
	#endregion
	#region Volume Accessors
	private static float masterVolume;

	private static float musicVolume;
	private static float r_musicVolume; // r_ = volume relative to master volume
	private static float effectVolume;
	private static float r_effectVolume;
	private static float voiceVolume;
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
			case SoundType.Effect:
				return volume * r_effectVolume;
			case SoundType.Music:
				return volume * r_musicVolume;
			case SoundType.Voice:
				return volume * r_voiceVolume;
		}
		return 0.0f;
	}
	#endregion

	// A replacement for "AudioSource.PlayClipAtPoint". Returns the created AudioSource. Deletes the object after playing. 
	// Parent transform is for organization or if we want the sound to come from a specific object as it moves.
	#region Play Clip At Point
	public static AudioSource PlayClipAtPoint( AudioClip clip, Vector3 pos )
	{
		return PlayClipAtPoint( clip, pos, 1.0f );
	}
	public static AudioSource PlayClipAtPoint( AudioClip clip, Vector3 pos, float volume )
	{
		return PlayClipAtPoint( clip, pos, volume, SoundType.None );
	}
	public static AudioSource PlayClipAtPoint( AudioClip clip, Vector3 pos, float volume, SoundType type )
	{
		return PlayClipAtPoint( clip, pos, volume, type, null );
	}
	public static AudioSource PlayClipAtPoint( AudioClip clip, Vector3 pos, float volume, SoundType type, Transform parent )
	{
		if( clip != null )
		{
			GameObject obj = new GameObject("AudioClip (Temp)");
			obj.transform.position = pos;
			obj.transform.parent = parent;

			AudioSource audHelper = (AudioSource)obj.AddComponent<AudioSource>();
			audHelper.clip = clip;

			switch( type )
			{
				case SoundType.None:
					audHelper.volume = volume * masterVolume;
					break;
				case SoundType.Effect:
					audHelper.volume = volume * r_effectVolume;
					break;
				case SoundType.Music:
					audHelper.volume = volume * r_musicVolume;
					break;
				case SoundType.Voice:
					audHelper.volume = volume * r_voiceVolume;
					break;
			}

			audHelper.Play ();
			GameObject.Destroy( obj, clip.length ); //Destroy the object after the sound is done playing.

			return audHelper;
		}
		else
		{
			Debug.LogWarning("Given AudioClip was null!");
			return null;
		}
	}
	#endregion


	// Makes a new AudioSource component on the specified transform. Return the new AudioSource.
	// This makes it easy for a single script to handle multiple sounds at once, without having to recreate them every time.

	//THE INPUT VOLUME NEEDS TO BE SCALED BY "GetVolume()" ON THE OTHER SCRIPT!! 
	//(Otherwise, if the user changes volume, sounds from this component will not change volume!)
	#region Create Audio Source
	public static AudioSource CreateAudioSource( Transform obj, AudioClip clip )
	{
		return CreateAudioSource( obj, clip, 1.0f );
	}
	public static AudioSource CreateAudioSource( Transform obj, AudioClip clip, float volume )
	{
		return CreateAudioSource( obj, clip, volume, 1.0f );
	}
	public static AudioSource CreateAudioSource( Transform obj, AudioClip clip, float volume, float pitch )
	{
		AudioSource audHelper = obj.gameObject.AddComponent<AudioSource>();

		audHelper.clip = clip;
		audHelper.volume = Mathf.Clamp01( volume );
		audHelper.pitch = Mathf.Clamp( pitch, -3.0f, 3.0f );

		return audHelper;
	}
	#endregion






















}
