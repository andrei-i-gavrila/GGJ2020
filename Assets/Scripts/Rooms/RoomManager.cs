using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GGJ.Rooms
{
	public class RoomManager : MonoBehaviour
	{
		private static int roomNumber = 1;
		public Room CurrentRoom { get; private set; }
		public Room PreviouseRoom { get; private set; }
		public Action<Room, Room> OnRoomEntered { get; private set; }
		public Transform EntrancesParent { get; private set; }
		public object Utilits { get; private set; }

		private HashSet<Entrance> entrances = new HashSet<Entrance>();

		private List<Room> roomsPrefab = new List<Room>();
		private Entrance entrancePrefab;
		private Transform roomsParent;
		private Dictionary<string, List<int>> availableIds = new Dictionary<string, List<int>>();
		private Dictionary<Room, List<Direction>> roomDirections = new Dictionary<Room, List<Direction>>();
		private Dictionary<Room, List<Direction>> roomCompatibleDirections = new Dictionary<Room, List<Direction>>();

		public List<Entrance> GetEntrancesForRoom(Room room)
		{
			if (room == null)
				return default;

			return entrances.ToList().FindAll(entrance => room.Equals(entrance.ConnectedRooms.Item1) || room.Equals(entrance.ConnectedRooms.Item2));
		}

		public Room GenerateRoom(Vector3 entrancePosition, Entrance entrance, Direction direction)
		{
			var newRoom = Instantiate(GetCompatableRoomPrefabs(direction).GetRandomValue(), roomsParent);
			var entryTransform = newRoom.GetComponentsInChildren<EntranceSpawnPoint>().FirstOrDefault(comp => comp.Direction == (Utils.GetOppositeDirection(direction)))?.transform;
			if (entryTransform == null)
			{
				Debug.LogError("The entry for that direction was not found...");
			}
			else
			{
				newRoom.transform.position -= entryTransform.position - entrancePosition;
			}

			newRoom.name = "Room " + roomNumber++.ToString();
			newRoom.RoomId = GenerateRoomId();
			entrance.ConnectedRooms.Item2 = newRoom;
			return newRoom;
		}

		public Entrance GenerateEntrance(Vector3 position, Room room, Direction direction)
		{
			var quaternion = (direction == Direction.Nord || direction == Direction.South) ? Quaternion.identity : Quaternion.Euler(0f, 90f, 0f);
			var entrance = Instantiate(entrancePrefab, position, quaternion, Game.Instance.RoomManager.EntrancesParent);
			entrance.ConnectedRooms.Item1 = room;
			entrance.SetDirectionForRoom(room, direction);
			entrances.Add(entrance);
			return entrance;
		}

		public void CharacterEnteredInRoom(Room newRoom)
		{
			if (PreviouseRoom != newRoom)
			{
				PreviouseRoom?.gameObject.SetActive(false);
			}

			PreviouseRoom = CurrentRoom;
			CurrentRoom = newRoom;
			UpdateEntrancesState();
			OnRoomEntered?.Invoke(CurrentRoom, PreviouseRoom);
		}

		private List<Room> GetCompatableRoomPrefabs(Direction direction)
		{
			var result = new List<Room>();
			return roomsPrefab.Where(roomPrefab => roomCompatibleDirections[roomPrefab].Contains(direction)).ToList();
		}

		private void UpdateEntrancesState()
		{
			foreach (var entrance in entrances)
			{
				if (!entrance.gameObject.activeSelf && (entrance.IsRoomInThisEntrance(CurrentRoom, false) || entrance.IsRoomInThisEntrance(PreviouseRoom, false)))
					entrance.gameObject.SetActive(true);

				if (!entrance.IsRoomInThisEntrance(CurrentRoom, false) && !entrance.IsRoomInThisEntrance(PreviouseRoom, false))
					entrance.gameObject.SetActive(false);
			}
		}

		private string GenerateRoomId()
		{
			var key = availableIds.Keys.GetRandomValue();
			return key + availableIds[key].GetRandomValue();
		}

		private void Awake()
		{
			Initialize();
		}

		private void Initialize()
		{
			EntrancesParent = new GameObject("Entrances").transform;
			roomsPrefab = Resources.LoadAll<Room>(Paths.ROOMS_PREFABS).ToList();
			entrancePrefab = Resources.Load<Entrance>(Paths.PREFABS + "Entrance");
			roomsParent = GameObject.Find("Rooms")?.transform ?? new GameObject("Rooms").transform;
			roomsPrefab.ForEach(room =>
			{
				roomDirections.Add(room, new List<Direction>());
				roomCompatibleDirections.Add(room, new List<Direction>());

				var entrancePositions = room.GetComponentsInChildren<EntranceSpawnPoint>().ToList();
				foreach (var entrance in entrancePositions)
				{
					roomDirections[room].Add(entrance.Direction);
					roomCompatibleDirections[room].Add(Utils.GetOppositeDirection(entrance.Direction));
				}
			});
			InitializeRoomIds();
		}

		private void InitializeRoomIds()
		{
			availableIds.Add("Storage ", GetListWithNumbers(1, 200));
			availableIds.Add("Hall ", GetListWithNumbers(1, 200));
		}

		private List<int> GetListWithNumbers(int from, int to)
		{
			if (from > to)
			{
				var temp = to;
				to = from;
				from = temp;
			}

			var result = new List<int>();
			for (int i = from; i <= to; i++)
			{
				result.Add(i);
			}
			return result;
		}
	}
}
