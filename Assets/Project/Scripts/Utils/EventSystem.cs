using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//By Cristian "vozochris" Vozoca
public class EventSystem
{
	public static Dictionary<string, ArrayList> events = new Dictionary<string, ArrayList>();

	public static bool CallEvent(string id, object value)
	{
		if (events.ContainsKey(id))
		{
			ArrayList callbacks = events[id];
			ArrayList removeCallbacks = new ArrayList();
			int le = callbacks.Count;
			for(int i = 0; i < le; i++)
			{
				Callback callback = (Callback)callbacks[i];
				callback.calledCount++;
				callback.action(value, callback.calledCount);
				if (callback.callCount == callback.calledCount)
					removeCallbacks.Add(callback);
			}

			le = removeCallbacks.Count;
			for(int i = 0; i < le; i++)
				callbacks.Remove(removeCallbacks[i]);

			if (callbacks.Count == 0)
				events.Remove(id);
			return true;
		}
		return false;
	}

	public static void AddEventListener(string id, Action<object, int> action, int callTimes = 0)
	{
		ArrayList callbacks = events.ContainsKey(id) ? events[id] : new ArrayList();
		callbacks.Add(new Callback(action, callTimes));
		events[id] = callbacks;
	}
}
public class Callback
{
	public Action<object, int> action;
	public int callCount = 0;

	internal int calledCount = 0;

	public Callback(Action<object, int> action, int callTimes)
	{
		this.action = action;
		this.callCount = callTimes;
	}
}