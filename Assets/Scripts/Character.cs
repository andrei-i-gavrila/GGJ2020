using UnityEngine;

namespace GGJ
{
	public class Character : BaseBehaviour
	{
		public bool CanInteract { get; set; } = true;

		private void Update()
		{
			if (!CanInteract)
				return;

			if (Input.GetKeyDown(KeyCode.E))
			{
				if (Game.InteractablesManager.CurrentInteractable != null)
				{
					Game.InteractablesManager.CurrentInteractable.OnInteract();
				}
			}
		}
	}
}
