namespace GGJ
{
	public class Console : BaseInteractable
	{
		private string puzzleId = "";
		public void SetPuzzleId(string id)
		{
			puzzleId = id;
		}

		public override void OnInteract()
		{
			Game.PuzzleManager.StartPuzzle(puzzleId);
		}
	}
}
