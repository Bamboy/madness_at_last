using UnityEngine;
using System.Collections;

public class LevelMusic : MonoBehaviour 
{
	public AudioClip music;
	public bool loop = true;
	public float startDelay = 0.25f;
	[Range(0.0f,1.0f)]public float volume = 1.0f;

	private AudioSource player;
	// Use this for initialization
	void Awake () 
	{
		Invoke("PlayMusic", startDelay);
		player = AudioHelper.CreateAudioSource(this.transform, music, AudioHelper.GetVolume(volume, SoundType.Music));
		player.Stop ();
		player.loop = loop;
		player.PlayDelayed( startDelay );
	}

	void PlayMusic()
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		player.volume = AudioHelper.GetVolume(volume, SoundType.Music);
	}
}
