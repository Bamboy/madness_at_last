using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	public Texture2D SecondsHand;
	public Texture2D MinutesHand;
	public bool CountDown = false;
	public float _time;
	public float _startingTime;
	public float _countFromTime;
	public float _countToTime;
	public bool CanCount = true;
	public int seconds = 0;
	public int minutes = 0;
	public float _SAngle = -180.0f;
	public float _MAngle = -180.0f;
	Rect SecondsRect = new Rect(200, 200, 5, 50);
	Rect MinuteRect = new Rect(200, 200, 5, 70);
	Vector2 pivot = new Vector2(202.5f, 200);

	void Start(){
		_startingTime = Time.time;
	}
	void Update(){
		if(CanCount){
			if(CountDown){
				_time = _countFromTime - Time.time - _startingTime;
				seconds = (int)_time % 60;
				minutes = (int)_time / 60;
				
				_SAngle = 6*seconds + 180;
				_MAngle = 6*minutes + 180;
			} else {
				_time = Time.time - _startingTime;
				seconds = (int)_time % 60;
				minutes = (int)_time / 60;
				
				_SAngle = 6*seconds - 180;
				_MAngle = 6*minutes - 180;
			}
		}
		if(_time <= 0){
			CanCount = false;
			_time = 0;
		} else if(_time >= _countToTime){
			CanCount = false;
		} else {
			CanCount = true;
		}
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