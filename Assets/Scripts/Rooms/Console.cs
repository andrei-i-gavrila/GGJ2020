using GGJ.Rooms;
using TMPro;
using UnityEngine;

namespace GGJ
{
	public class Console : BaseInteractable
	{
		[SerializeField] private TextMeshPro consoleName;
		public ConsoleState ConsoleState { get; private set; } = ConsoleState.Interactable;
		public string Id { get; private set; }
		public Room Room;

		private string puzzleId = "";

		public Light Light;
		public Renderer Monitor;

		public override void Start()
		{
			base.Start();
			SetConsoleState(ConsoleState);
		}

		public void SetConsoleId(string id)
		{
			Id = id;
			consoleName.text = "Console " + id;
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
			Game.PuzzleManager.StartPuzzle(puzzleId, this);
		}

		public override bool CanBeInteractedWith()
		{
			return ConsoleState == ConsoleState.Interactable;
		}

		public void SetConsoleState(ConsoleState newState)
		{
			ConsoleState = newState;
			var mats = Monitor.materials;
			switch (newState)
			{
				case ConsoleState.Locked:
					mats[1] = Resources.Load<Material>(Constants.StationMaterialPath);
					Light.intensity = 0;
					break;

				case ConsoleState.Interactable:
					mats[1] = Resources.Load<Material>(Constants.BlueEmissionMaterialPath);
					Light.color = new Color(Constants.BlueLightColorColor.X, Constants.BlueLightColorColor.Y, Constants.BlueLightColorColor.Z);
					Light.intensity = 1;
					break;

				case ConsoleState.Resolved:
					mats[1] = Resources.Load<Material>(Constants.GreenEmissionMaterialPath);
					Light.color = new Color(Constants.GreenLightColor.X, Constants.GreenLightColor.Y, Constants.GreenLightColor.Z);
					Light.intensity = 1;
					break;

				default:
					break;
			}
			Monitor.materials = mats;
		}
	}
}
