using System.Collections.Generic;

namespace GGJ
{
	public class BaseInteractable : BaseBehaviour
	{
		public virtual void Start()
		{
			Game.InteractablesManager?.Subscribe(this);
		}

		private List<string> conditions = new List<string>();
		public void AddCondition(string condition)
		{
			conditions.Add(condition);
		}

		public void SetConditions(List<string> conditions)
		{
			this.conditions = conditions;
		}

		public virtual bool CanBeInteractedWith()
		{
			return conditions.Count == 0 || Game.ConditionsManager.CheckMany(conditions);
		}

		public virtual void OnInteract()
		{

		}
	}
}
