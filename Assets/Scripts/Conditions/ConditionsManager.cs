using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
	public class ConditionsManager
	{
		private Dictionary<string, bool> conditions = new Dictionary<string, bool>();

		public void AddCondition(string id)
		{
			if (conditions.ContainsKey(id))
			{
				Debug.LogError("The id is already in the dictionary");
			}
			else
			{
				conditions.Add(id, false);
			}
		}

		public void ResolveCondition(string id)
		{
			if (!conditions.ContainsKey(id))
			{
				Debug.LogError("The given id does not exist in the dictionary");
			}
			else
			{
				conditions[id] = true;
			}
		}

		public bool CheckCondition(string id)
		{
			if (!conditions.ContainsKey(id))
			{
				Debug.LogError("The given id does not exist in the dictionary");
				return false;
			}

			return conditions[id];
		}

		public bool CheckMany(List<string> conditions)
		{
			return conditions.TrueForAll(cond => CheckCondition(cond));
		}
	}
}
