using UnityEngine;
using System;

public class Stat
{
	private string id;
	private float baseValue;
	private float current;
	private float min;
	private float max;
	
	private Action<Stat> onChangeBase;
	private Action<Stat> onChangeCurrent;
	private Action<Stat> onChangeMin;
	private Action<Stat> onChangeMax;
	
	public Stat(string id, float value, float min = float.MinValue, float max = float.MaxValue)
	{
		this.id = id;
		this.baseValue = value;
		this.min = min;
		this.max = max;
		
		Current = value;
	}
	
	public string Id
	{
		get { return id; }
	}
	
	public float Base
	{
		get { return baseValue; }
		set {
			baseValue = value;
			if (onChangeBase != null)
				onChangeBase(this);
		}
	}
	
	public float Current
	{
		get { return current; }
		set {
			current = value;
			if (current > max)
				current = max;
			else if (current < min)
				current = min;
			if (onChangeCurrent != null)
				onChangeCurrent(this);
		}
	}
	
	public float Min
	{
		get { return min; }
		set {
			min = value;
			if (onChangeMin != null)
				onChangeMin(this);
		}
	}
	
	public float Max
	{
		get { return max; }
		set {
			max = value;
			if (onChangeMax != null)
				onChangeMax(this);
		}
	}

	public bool Bool
	{
		get { return current != 0; }
		set {
			current = value ? 1 : 0;
		}
	}
	
	public Action<Stat> OnChangeBase
	{
		get { return onChangeBase; }
		set { onChangeBase = value; }
	}
	
	public Action<Stat> OnChangeCurrent
	{
		get { return onChangeCurrent; }
		set { onChangeCurrent = value; }
	}
	
	public Action<Stat> OnChangeMin
	{
		get { return onChangeMin; }
		set { onChangeMin = value; }
	}
	
	public Action<Stat> OnChangeMax
	{
		get { return onChangeMax; }
		set { onChangeMax = value; }
	}
	
	public override string ToString()
	{
		return string.Format("[Stat: id={0}, baseValue={1}, current={2}, min={3}, max={4}]", Id, Base, Current, Min, Max);
	}
	
	#region operators
	public static float operator +(Stat c1, Stat c2)
	{
		return c1.Current + c2.Current;
	}
	
	public static float operator +(Stat c1, float c2)
	{
		return c1.Current + c2;
	}
	
	public static float operator +(float c1, Stat c2)
	{
		return c1 + c2.Current;
	}
	
	public static float operator -(Stat c1, Stat c2)
	{
		return c1.Current - c2.Current;
	}
	
	public static float operator -(Stat c1, float c2)
	{
		return c1.Current - c2;
	}
	
	public static float operator -(float c1, Stat c2)
	{
		return c1 - c2.Current;
	}
	
	public static float operator *(Stat c1, Stat c2)
	{
		return c1.Current * c2.Current;
	}
	
	public static float operator *(Stat c1, float c2)
	{
		return c1.Current * c2;
	}
	
	public static float operator *(float c1, Stat c2)
	{
		return c1 * c2.Current;
	}
	
	public static float operator /(Stat c1, Stat c2)
	{
		return c1.Current / c2.Current;
	}
	
	public static float operator /(Stat c1, float c2)
	{
		return c1.Current / c2;
	}
	
	public static float operator /(float c1, Stat c2)
	{
		return c1 / c2.Current;
	}
	
	public static float operator ^(Stat c1, Stat c2)
	{
		return (int)c1.Current ^ (int)c2.Current;
	}
	
	public static float operator ^(Stat c1, int c2)
	{
		return (int)c1.Current ^ c2;
	}
	
	public static float operator ^(int c1, Stat c2)
	{
		return c1 ^ (int)c2.Current;
	}
	
	public static bool operator <(Stat c1, float c2)
	{
		return c1.Current < c2;
	}
	
	public static bool operator >(Stat c1, float c2)
	{
		return c1.Current > c2;
	}
	
	public static bool operator <(float c1, Stat c2)
	{
		return c1 < c2.Current;
	}
	
	public static bool operator >(float c1, Stat c2)
	{
		return c1 > c2.Current;
	}
	
	public static bool operator <(Stat c1, Stat c2)
	{
		return c1.Current < c2.Current;
	}
	
	public static bool operator >(Stat c1, Stat c2)
	{
		return c1.Current > c2.Current;
	}
	
	public static bool operator <=(Stat c1, float c2)
	{
		return c1.Current <= c2;
	}
	
	public static bool operator >=(Stat c1, float c2)
	{
		return c1.Current >= c2;
	}
	
	public static bool operator <=(float c1, Stat c2)
	{
		return c1 <= c2.Current;
	}
	
	public static bool operator >=(float c1, Stat c2)
	{
		return c1 >= c2.Current;
	}
	
	public static bool operator <=(Stat c1, Stat c2)
	{
		return c1.Current <= c2.Current;
	}
	
	public static bool operator >=(Stat c1, Stat c2)
	{
		return c1.Current >= c2.Current;
	}
	
	public static bool operator ==(Stat c1, float c2)
	{
		return c1.Current == c2;
	}
	
	public static bool operator !=(Stat c1, float c2)
	{
		return c1.Current != c2;
	}
	
	public static bool operator ==(Stat c1, Stat c2)
	{
		return c1.Current == c2.Current;
	}
	
	public static bool operator !=(Stat c1, Stat c2)
	{
		return c1.Current != c2.Current;
	}
	
	public static bool operator ==(float c1, Stat c2)
	{
		return c1 == c2.Current;
	}
	
	public static bool operator !=(float c1, Stat c2)
	{
		return c1 != c2.Current;
	}
	
	public static float operator -(Stat c1)
	{
		return -c1.Current;
	}
	
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}
	
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
	#endregion
}
