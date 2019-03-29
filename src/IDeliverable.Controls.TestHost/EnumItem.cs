using System;
using System.Linq;

namespace IDeliverable.Controls.TestHost
{
	public class EnumItem<T>
	{
		public static EnumItem<T>[] GetAll()
		{
			return ((T[])Enum.GetValues(typeof(T))).Select(value => FromValue(value)).ToArray();
		}

		public static EnumItem<T> FromValue(T value)
		{
			return new EnumItem<T>(Enum.GetName(typeof(T), value), value);
		}

		public EnumItem(string name, T value)
		{
			Name = name;
			Value = value;
		}

		public string Name { get; }
		public T Value { get; }
		public int ValueInt32 => Convert.ToInt32(Value);

		public override string ToString()
		{
			return Name;
		}
	}
}
