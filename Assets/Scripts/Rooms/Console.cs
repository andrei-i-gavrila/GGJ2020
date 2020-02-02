using GGJ.Rooms;

namespace GGJ
{
	public class Console : BaseInteractable
	{
		public ConsoleState ConsoleState { get; private set; } = ConsoleState.Interactable;
		public string Id { get; private set; }
		public Room Room;

		private string puzzleId = "";

		public void SetConsoleId(string id)
		{
			Id = id;
		}

		public void SetRoom(Room room)
		{
			Room = room;
		}

		public void SetPuzzleId(string id)
		{
			puzzleId = id;
		}

		public override void OnInteract()
		{
			Game.PuzzleManager.StartPuzzle(puzzleId);
		}

		public override bool CanBeInteractedWith()
		{
			return ConsoleState == ConsoleState.Interactable;
		}

		public void SetConsoleState(ConsoleState newState)
		{
			ConsoleState = newState;
		}
	}
}
