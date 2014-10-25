using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	public Texture2D SecondsHand;
	public Texture2D MinutesHand;
	public bool CanCount = true;
	public bool CountDown = false;
	public float _time;
	public float _startingTime;
	public float _countFromTime;
	public float _countToTime;
	public bool SmoothGUI = true;
	public float seconds = 0;
	public float minutes = 0;
	public float _SAngle = -180.0f;
	public float _MAngle = -180.0f;

	//public Vector2 Position = new Vector2(200, 200);
	//public Vector2 PivotOffset = new Vector2(2.5f, 0.0f);
	//public Vector2 SecondsSize = new Vector2(5, 70);
	//public Vector2 MinutesSize = new Vector2(5, 50);

	Rect SecondsRect = new Rect(200, 200, 5, 70);
	Rect MinuteRect = new Rect(200, 200, 5, 50);
	Vector2 pivot = new Vector2(2.5f, 0.0f); //Vector2(202.5f, 200);

	void Start(){
		_startingTime = Time.time;

		SecondsRect = new Rect( Screen.width - 80, 80, 5, 70 );
		MinuteRect = new Rect( Screen.width - 80, 80, 5, 50 );
		pivot = new Vector2( pivot.x + Screen.width - 80, pivot.y + 80 );
	}
	void Update()
	{
		if(CanCount){
			if(CountDown){
				_time = _countFromTime - Time.time - _startingTime;
				seconds = SmoothGUI ? _time % 60.0f : Mathf.Ceil(_time % 60);
				minutes = SmoothGUI ? _time / 60.0f : Mathf.Ceil(_time / 60.0f);
				
				_SAngle = (6.0f * seconds) + 180.0f;
				_MAngle = (6.0f * minutes) + 180.0f;
			} else {
				_time = Time.time - _startingTime;
				seconds = SmoothGUI ? _time % 60.0f : Mathf.Ceil(_time % 60);
				minutes = SmoothGUI ? _time / 60.0f : Mathf.Ceil(_time / 60.0f);
				
				_SAngle = (6.0f * seconds) - 180.0f;
				_MAngle = (6.0f * minutes) - 180.0f;
			}
		}
		/*
		if(_time <= 0.0f){
			CanCount = false;
			_time = 0.0f;
			Debug.Log("1 _Time: "+ _time, this);
		} else if(_time >= _countToTime){
			CanCount = false;
			Debug.Log("2; _Time: "+ _time +", _ToTime: "+ _countToTime, this);
		} else {
			CanCount = true;
		} */

	}
	void OnGUI(){
		Seconds();
		Minutes();
	}
	void Seconds() {
		Matrix4x4 matrixBackup = GUI.matrix;
		GUIUtility.RotateAroundPivot(_SAngle, pivot);
		GUI.DrawTexture(SecondsRect, SecondsHand);
		GUI.matrix = matrixBackup;
	}
	void Minutes(){
		Matrix4x4 matrixBackup = GUI.matrix;
		GUIUtility.RotateAroundPivot(_MAngle, pivot);
		GUI.DrawTexture(MinuteRect, MinutesHand);
		GUI.matrix = matrixBackup;
	}
}