using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GGJ.Rooms
{
	public class Room : BaseBehaviour
	{
		public string RoomId { get; set; }
		public HashSet<Direction> Directions { get; set; } = new HashSet<Direction>();
		public HashSet<Direction> CompatibleDirections { get; set; } = new HashSet<Direction>();

		private List<EntranceSpawnPoint> entrancePositions = new List<EntranceSpawnPoint>();

		public void SetupRoom()
		{
			var dificulty = Game.CurrentDificulty;
		}

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
		}

		private void Start()
		{
			Initialize();
		}

		private void Initialize()
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

		private void OnTriggerEnter(Collider other)
		{
			Game.RoomManager.CharacterEnteredInRoom(this);
		}
	}
}
