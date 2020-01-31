using UnityEngine;

namespace GGJ
{
	public static class Utils
	{
		public static bool GetComponentInChild<T>(Transform transform, string name, out T component)
		{
			if (GetComponent(transform, name, out component))
				return true;

			foreach (Transform child in transform)
			{
				if (GetComponentInChild(child, name, out component))
					return true;
			}

			return false;
		}

		private static bool GetComponent<T>(Transform t, string name, out T component)
		{
			if (t.name == name && t.GetComponent<T>() is var comp && comp != null)
			{
				component = comp;
				return true;
			}

			component = default;
			return false;
		}
	}
}