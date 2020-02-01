using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Rooms
{
	public class Room : BaseBehaviour
	{
		[SerializeField] private List<EntranceSpawnPoint> entrancePositions = new List<EntranceSpawnPoint>();
		[SerializeField] private Entrance spawningPrefab;

		public List<Entrance> Entrances { get; private set; } = new List<Entrance>();

		private void Awake()
		{

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
