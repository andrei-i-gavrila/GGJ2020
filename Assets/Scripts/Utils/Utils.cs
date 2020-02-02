using System.Collections.Generic;
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

		public static Direction GetOppositeDirection(Direction direction)
		{
			if (direction == Direction.East)
			{
				return Direction.West;
			}
			else if (direction == Direction.West)
			{
				return Direction.East;
			}
			else if (direction == Direction.North)
			{
				return Direction.South;
			}
			else
			{
				return Direction.North;
			}
		}

		public static Vector3 GetVectorDirection(Direction direction)
		{
			if (direction == Direction.North)
			{
				return Vector3.forward;
			}
			else if (direction == Direction.South)
			{
				return Vector3.back;
			}
			else if (direction == Direction.East)
			{
				return Vector3.left;
			}
			else
			{
				return Vector3.right;
			}
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