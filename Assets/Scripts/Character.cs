using UnityEngine;

namespace GGJ
{
	public class Character : BaseBehaviour
	{
		public bool CanInteract { get; set; } = true;
		public bool CanMove { get; set; } = true;

		private void Awake()
		{
			Game.PuzzleManager.OnPuzzleStarted += (_) => CanMove = false;
			Game.PuzzleManager.OnPuzzleCompleted += (_) => CanMove = true;
		}
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
