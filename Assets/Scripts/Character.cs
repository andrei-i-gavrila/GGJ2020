using UnityEngine;

namespace GGJ
{
	public class Character : BaseBehaviour
	{
		public bool CanInteract { get; set; } = true;
		public bool CanMove { get; set; } = true;

		private void Awake()
		{
			Game.PuzzleManager.OnPuzzleStarted += OnPuzzleStarted;
			Game.PuzzleManager.OnPuzzleCompleted += OnPuzzleCompleted;
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

		private void OnPuzzleStarted(BasePuzzleController puzzle)
		{
			CanMove = false;
			CanInteract = false;
		}

		private void OnPuzzleCompleted(bool result)
		{
			CanMove = true;
			CanInteract = true;
		}
	}
}
