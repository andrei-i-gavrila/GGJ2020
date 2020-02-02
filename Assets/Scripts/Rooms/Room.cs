using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GGJ.Rooms
{
	public class Room : BaseBehaviour
	{
		[SerializeField] private string roomId = "";
		public string RoomId
		{
			get
			{
				return roomId;
			}
			set
			{
				roomId = value;
			}
		}
		public HashSet<Direction> Directions { get; set; } = new HashSet<Direction>();
		public HashSet<Direction> CompatibleDirections { get; set; } = new HashSet<Direction>();
		private List<EntranceSpawnPoint> entrancePositions = new List<EntranceSpawnPoint>();
		private List<ConsoleAvailablePosition> possibleConsolePositions = new List<ConsoleAvailablePosition>();
		private List<string> puzzleIds = new List<string>();

		public Transform GetEntranceForDirection(Direction direction)
		{
			if (!Directions.Contains(direction))
				return null;

			return entrancePositions.FirstOrDefault(entrance => entrance.Direction == direction)?.transform ?? null;
		}

		private void InitializeEntrances()
		{
			entrancePositions = gameObject.GetComponentsInChildren<EntranceSpawnPoint>().ToList();
			entrancePositions.ForEach(entrance =>
			{
				Directions.Add(entrance.Direction);
				CompatibleDirections.Add(Utils.GetOppositeDirection(entrance.Direction));
			});
		}

		private void Awake()
		{
			InitializeEntrances();
			possibleConsolePositions = gameObject.GetComponentsInChildren<ConsoleAvailablePosition>().ToList();
		}

		private void Start()
		{
			Initialize();
		}

		private void Initialize()
		{
			SetPuzzlesIds();
			ManageEntrances();
			CreateConsoles();
		}

		private void SetPuzzlesIds()
		{
		    // puzzleIds.Add(Constants.SIMON_ID);
		    // puzzleIds.Add(Constants.JIGSAW_ID);
		    // puzzleIds.Add(Constants.MEMORY_ID);
		    puzzleIds.Add(Constants.KEYSEQUENCE_ID);
			// puzzleIds.Add(Constants.REACTION_SPEED_ID);
		}

		private void ManageEntrances()
		{
			//Get the entrance for this Room
			var entrances = Game.Instance.RoomManager.GetEntrancesForRoom(this);
			if (entrances.Count > 1)
			{
				Debug.LogError("There should not be more than one entrance for this room when is created!!!");
				return;
			}

			if (entrances.Count == 1)
			{
				if (entrances[0].GetDirectioToRoomTroughEntrance(entrances[0].ConnectedRooms.Item1, out var direction))
				{
					var restrictedDirection = Utils.GetOppositeDirection(direction);
					entrancePositions.RemoveAll(point => point.Direction == restrictedDirection);
				}
			}

			foreach (var entranceSpawner in entrancePositions)
			{
				Game.RoomManager.GenerateEntrance(entranceSpawner.transform.position, this, entranceSpawner.Direction);
			}
		}

		private void CreateConsoles()
		{
			if (possibleConsolePositions == null || possibleConsolePositions.Count == 0)
			{
				Debug.LogError("There are no possible console positions for room " + RoomId);
				return;
			}
			var numberOfConsoles = Mathf.Clamp(Game.DificultyManager.GetNumberOfConsoles(), 0, possibleConsolePositions.Count);
			var consolePositions = possibleConsolePositions.GetRandomValues(numberOfConsoles);

			foreach (var position in consolePositions)
			{
				CreateConsole(position);
			}
		}

		private void CreateConsole(ConsoleAvailablePosition consolePosition)
		{
			var console = Instantiate(Game.PrefabsManager.Console, consolePosition.transform.position, consolePosition.GetRotation(), transform);
			console.SetPuzzleId(puzzleIds.GetRandomValue());
			//Set the console state
			if (Game.ConsolesManager.GetNumberOfConsolesWithState(ConsoleState.Interactable) == 0)
			{
				console.SetConsoleState(ConsoleState.Interactable);
			}
			else
			{
				console.SetConsoleState(Random.value < 0.25f ? ConsoleState.Locked : ConsoleState.Interactable);
			}
			console.SetRoom(this);
			Game.ConsolesManager.AddConsole(console);
		}

		private void OnTriggerEnter(Collider other)
		{
			Game.RoomManager.CharacterEnteredInRoom(this);
		}
	}
}
