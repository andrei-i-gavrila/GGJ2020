using System;
using System.Collections.Generic;

namespace GGJ
{
	public class InteractablesManager : BaseBehaviour
	{
		private const float INTERACTION_DISTANCE_SQUARED = 4f;
		public BaseInteractable CurrentInteractable { get; private set; }
		public Action<BaseInteractable> OnCurrentInteractableChanged;

		private HashSet<BaseInteractable> interactables = new HashSet<BaseInteractable>();

		public void Subscribe(BaseInteractable interactable)
		{
			interactables.Add(interactable);
		}
		public void Unsubscribe(BaseInteractable interactable)
		{
			interactables.Remove(interactable);
		}

		private void Update()
		{
			if (!Game.Character.CanInteract)
				return;

			if (GetClosestInteractable(INTERACTION_DISTANCE_SQUARED) is var closestInteractable && closestInteractable != CurrentInteractable)
			{
				CurrentInteractable = closestInteractable;
				OnCurrentInteractableChanged?.Invoke(CurrentInteractable);
			}
		}

		private BaseInteractable GetClosestInteractable(float sqrDistanceTreshold)
		{
			var characterPos = Game.Character.transform.position;
			var minDistance = float.PositiveInfinity;
			BaseInteractable closestInteractable = null;

			foreach (var interactable in interactables)
			{
				var sqrDistance = (characterPos - interactable.transform.position).sqrMagnitude;
				if (sqrDistance <= sqrDistanceTreshold && sqrDistance <= minDistance)
				{
					closestInteractable = interactable;
					minDistance = sqrDistance;
				}
			}

			return closestInteractable;
		}
	}
}
