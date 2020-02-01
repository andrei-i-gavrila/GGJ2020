using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GGJ.Rooms
{
	public class RoomManager : MonoBehaviour
	{
		[SerializeField] private Room roomPrefab;
		[SerializeField] private Entrance entrancePrefab;

		public Room CurrentRoom { get; private set; }
		public Room PreviouseRoom { get; private set; }
		public Action<Room, Room> OnRoomEntered { get; private set; }

		public Transform EntrancesParent { get; private set; }

		private HashSet<Entrance> entrances = new HashSet<Entrance>();

		public List<Entrance> GetEntrancesForRoom(Room room)
		{
			if (room == null)
				return default;

			return entrances.ToList().FindAll(entrance => room.Equals(entrance.ConnectedRooms.Item1) || room.Equals(entrance.ConnectedRooms.Item2));
		}

		public Room GenerateRoom(Vector3 entrancePosition, Entrance entrance, Direction direction)
		{
			var position = entrancePosition + Utils.GetVectorDirection(direction) * 6f - Vector3.up * 0.5f; /*calculate this based on the size of the rooms*/
			var newRoom = Instantiate(roomPrefab, position, Quaternion.identity);
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

		private void Awake()
		{
			Initialize();
		}

		private void Initialize()
		{
			EntrancesParent = new GameObject("Entrances").transform;
			roomPrefab = Resources.Load<Room>(Paths.PREFABS + "Room");
			entrancePrefab = Resources.Load<Entrance>(Paths.PREFABS + "Entrance");
		}
	}
}
