using GGJ.Rooms;

namespace GGJ
{
	public class Console : BaseInteractable
	{
		public ConsoleState ConsoleState { get; private set; }
		public string Id { get; private set; }
		public Room Room;

		private string puzzleId = "";

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
			Game.Character.CanInteract = false;
			Game.PuzzleManager.StartPuzzle(puzzleId);
		}

		public void SetConsoleState(ConsoleState newState)
		{
			ConsoleState = newState;
		}
	}
}
