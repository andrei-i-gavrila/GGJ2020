namespace GGJ
{
	public class Console : BaseInteractable
	{
		public ConsoleState ConsoleState { get; private set; }
		private string puzzleId = "";
		public void SetPuzzleId(string id)
		{
			puzzleId = id;
		}

		public override void OnInteract()
		{
			Game.PuzzleManager.StartPuzzle(puzzleId);
		}

		public void SetConsoleState(ConsoleState newState)
		{
			ConsoleState = newState;
		}
	}
}
