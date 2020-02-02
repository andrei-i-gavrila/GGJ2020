using GGJ.Rooms;
using UnityEngine;

namespace GGJ
{
	public class Console : BaseInteractable
	{
		public ConsoleState ConsoleState { get; private set; }
		public string Id { get; private set; }
		public Room Room;

		private string puzzleId = "";

		public Light Light;
		public Renderer Monitor;

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

		public void SetConsoleState(ConsoleState newState)
		{
			ConsoleState = newState;
			switch (newState)
			{
				case ConsoleState.Locked:
					Monitor.materials[1] = Resources.Load<Material>(Constants.StationMaterialPath);
					Light.intensity = 0;
					break;

				case ConsoleState.Interactable:
					Monitor.materials[1] = Monitor.materials[1] = Resources.Load<Material>(Constants.BlueEmissionMaterialPath);
					Light.color = new Color(Constants.BlueLightColorColor.X, Constants.BlueLightColorColor.Y, Constants.BlueLightColorColor.Z);
					Light.intensity = 1;
					break;

				case ConsoleState.Resolved:
					Monitor.materials[1] = Monitor.materials[1] = Resources.Load<Material>(Constants.GreenEmissionMaterialPath);
					Light.color = new Color(Constants.GreenLightColor.X, Constants.GreenLightColor.Y, Constants.GreenLightColor.Z);
					Light.intensity = 1;
					break;

				default:
					break;
			}
		}
	}
}
