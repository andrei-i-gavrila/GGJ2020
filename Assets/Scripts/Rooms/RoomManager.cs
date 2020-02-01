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
		public Room PreviousRoom { get; private set; }
		public Action<Room, Room> OnRoomEntered { get; private set; }
		public Transform EntrancesParent { get; private set; }
		private HashSet<Entrance> entrances = new HashSet<Entrance>();

		private Room roomPrefab;
		private Entrance entrancePrefab;
		private Transform roomsParent;

		public List<Entrance> GetEntrancesForRoom(Room room)
		{
			if (room == null)
				return default;

			return entrances.ToList().FindAll(entrance => room.Equals(entrance.ConnectedRooms.Item1) || room.Equals(entrance.ConnectedRooms.Item2));
		}

		public Room GenerateRoom(Vector3 entrancePosition, Entrance entrance, Direction direction)
		{
			var position = entrancePosition + Utils.GetVectorDirection(direction) * 6f - Vector3.up * 0.5f; /*calculate this based on the size of the rooms*/
			var newRoom = Instantiate(roomPrefab, position, Quaternion.identity, roomsParent);
			newRoom.name = "Room " + roomNumber++.ToString();
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
			if (PreviousRoom != newRoom)
			{
				PreviousRoom.gameObject.SetActive(false);
			}

			PreviousRoom = CurrentRoom;
			CurrentRoom = newRoom;
			UpdateEntrancesState();
			OnRoomEntered?.Invoke(CurrentRoom, PreviousRoom);
		}

		private void UpdateEntrancesState()
		{
			foreach (var entrance in entrances)
			{
				if (!entrance.gameObject.activeSelf && (entrance.IsRoomInThisEntrance(CurrentRoom, false) || entrance.IsRoomInThisEntrance(PreviousRoom, false)))
					entrance.gameObject.SetActive(true);

				if (!entrance.IsRoomInThisEntrance(CurrentRoom, false) && !entrance.IsRoomInThisEntrance(PreviousRoom, false))
					entrance.gameObject.SetActive(false);
			}
		}

		private void Awake()
		{
			Initialize();
		}

		private void Initialize()
		{
			EntrancesParent = new GameObject("Entrances").transform;
			roomPrefab = Resources.Load<Room>(Paths.PREFABS + "Room");
			entrancePrefab = Resources.Load<Entrance>(Paths.PREFABS + "Entrance");
			roomsParent = GameObject.Find("Rooms")?.transform ?? new GameObject("Rooms").transform;
		}
	}
}
