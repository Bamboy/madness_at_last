using UnityEngine;
using System.Collections;

public class CamNoClearEffect : MonoBehaviour 
{
	public float max = 100.0f;
	public float min = 5.0f;
	public float speed = 25.0f;

	public LayerMask[] layers;

	void Start () 
	{
		Camera.main.clearFlags = CameraClearFlags.Depth;
	}


	void Update () 
	{
		Camera.main.farClipPlane = Mathf.PingPong( Time.time * (speed * Random.value), max ) + min;
		Camera.main.fieldOfView = (Random.value * 100) + 20;

		layers = ArrayTools.Shuffle( layers );
		Camera.main.cullingMask = layers[0];
	}
}
