using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GGJ
{
	public static class IEnumebrableExtension
	{
		public static T GetRandomValue<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable == null)
				return default;

			var elements = enumerable.ToList();
			if (elements.Count == 0)
				return default;

			return elements[Random.Range(0, elements.Count - 1)];
		}

		public static T GetRandomValue<T>(this List<T> elements)
		{
			if (elements == null || elements.Count == 0)
				return default;

			return elements[Random.Range(0, elements.Count)];
		}
	}
}
