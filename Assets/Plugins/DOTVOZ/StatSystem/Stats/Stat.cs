using System;

// StatSystem version: 1.0
// By: Cristian "vozochris" Vozoca
namespace Stats
{
	/// <summary>
	/// Base Stat class.
	/// More info about writing the JSON data in Readme.txt file, Samples and Demo.
	/// </summary>
	public class Stat
	{
		#region Properties
		private string id;
		private float baseValue;
		private float current;
		private float min;
		private float max;
		#endregion

		#region Events
		private Action<Stat> onChangeBase;
		private Action<Stat> onChangeCurrent;
		private Action<Stat> onChangeMin;
		private Action<Stat> onChangeMax;
		#endregion

		/// <summary>
		/// Create a new <see cref="Stat"/> with given parameters.
		/// </summary>
		/// <param name="id">ID.</param>
		/// <param name="value">Base and Current value.</param>
		/// <param name="min">Minimum value, default: <see cref="float.MinValue"/>.</param>
		/// <param name="max">Maximum value, default: <see cref="float.MaxValue"/>e.</param>
		public Stat(string id, float value, float min = float.MinValue, float max = float.MaxValue)
		{
			this.id = id;
			this.baseValue = value;
			this.min = min;
			this.max = max;
			
			Current = value;// Checking for Min/Max
		}

		/// <summary>
		/// Gets the Stat ID
		/// </summary>
		/// <value>String id</value>
		public string Id
		{
			get { return id; }
		}

		/// <summary>
		/// Gets or sets the base value.
		/// </summary>
		/// <value>Base value</value>
		public float Base
		{
			get { return baseValue; }
			set {
				baseValue = value;
				if (onChangeBase != null)
					onChangeBase(this);
			}
		}

		/// <summary>
		/// Gets or sets the current value.
		/// </summary>
		/// <value>Current value.</value>
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

		/// <summary>
		/// Gets or sets the minimum value.
		/// </summary>
		/// <value>Minimum value.</value>
		public float Min
		{
			get { return min; }
			set {
				min = value;
				if (onChangeMin != null)
					onChangeMin(this);
			}
		}

		/// <summary>
		/// Gets or sets the maximum value.
		/// </summary>
		/// <value>Maximum value.</value>
		public float Max
		{
			get { return max; }
			set {
				max = value;
				if (onChangeMax != null)
					onChangeMax(this);
			}
		}

		/// <summary>
		/// Gets or sets the boolean value.
		/// This is equivalent to: Current value set to 0 for false otherwise true.
		/// </summary>
		/// <value>If Current is 0 return <c>false</c> otherwise <c>true</c>.</value>
		public bool Bool
		{
			get { return current != 0; }
			set {
				current = value ? 1 : 0;
			}
		}

		/// <summary>
		/// Calls this Action when Base value is changed.
		/// </summary>
		/// <value>Action with Stat parameter</value>
		public Action<Stat> OnChangeBase
		{
			get { return onChangeBase; }
			set { onChangeBase = value; }
		}

		/// <summary>
		/// Calls this Action when Current value is changed.
		/// </summary>
		/// <value>Action with Stat parameter</value>
		public Action<Stat> OnChangeCurrent
		{
			get { return onChangeCurrent; }
			set { onChangeCurrent = value; }
		}

		/// <summary>
		/// Calls this Action when Minimum value is changed.
		/// </summary>
		/// <value>Action with Stat parameter</value>
		public Action<Stat> OnChangeMin
		{
			get { return onChangeMin; }
			set { onChangeMin = value; }
		}

		/// <summary>
		/// Calls this Action when Maximum value is changed.
		/// </summary>
		/// <value>Action with Stat parameter</value>
		public Action<Stat> OnChangeMax
		{
			get { return onChangeMax; }
			set { onChangeMax = value; }
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Stat"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Stat"/>.</returns>
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
}