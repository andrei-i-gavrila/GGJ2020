using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Rooms
{
	public class Entrance : BaseBehaviour
	{
		public Pair<Room, Room> ConnectedRooms = new Pair<Room, Room>();
		private Dictionary<Room, Direction> entranceDirections = new Dictionary<Room, Direction>();
		public bool IsLocked { get; private set; } = true;
		public void SetDirectionForRoom(Room room, Direction direction)
		{
			if (!IsRoomInThisEntrance(room))
				return;

			if (entranceDirections.ContainsKey(room))
			{
				entranceDirections[room] = direction;
			}
			else
			{
				entranceDirections.Add(room, direction);
			}
		}

		public Room GetOtherRoom(Room room)
		{
			if (!IsRoomInThisEntrance(room))
			{
				return null;
			}

			return room == ConnectedRooms.Item1 ? ConnectedRooms.Item2 : ConnectedRooms.Item1;
		}

		public bool GetDirectioToRoomTroughEntrance(Room room, out Direction direction)
		{
			direction = default;

			if (!IsRoomInThisEntrance(room))
				return false;

			if (!entranceDirections.ContainsKey(room))
			{
				Debug.LogError("The entrance direction for this room was not set!");
				return false;
			}

			direction = entranceDirections[room];
			return true;
		}

		public bool IsRoomInThisEntrance(Room room, bool writeError = true)
		{
			if (room == null)
			{
				if (writeError)
				{
					Debug.LogError("The given room was null!!!");
				}
				return false;
			}

			if (ConnectedRooms.Item1 != room && ConnectedRooms.Item2 != room)
			{
				if (writeError)
				{
					Debug.LogError("The given room is not in this entrance!!!");
				}
				return false;
			}

			return true;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (IsLocked)
			{
				IsLocked = false;
				OnUnlock();                 //WARNING - REMOVE THIS AFTER TESTING
			}

			ConnectedRooms.Item1?.gameObject.SetActive(true);
			ConnectedRooms.Item2?.gameObject.SetActive(true);
		}

		private void OnUnlock()
		{
			if (ConnectedRooms.Item1 == null)
			{
				Debug.LogError("The first room should always be set, as there can not be an entrance between no rooms");
				return;
			}

			if (ConnectedRooms.Item2 == null)
			{
				if (GetDirectioToRoomTroughEntrance(ConnectedRooms.Item1, out var direction))
				{
					Game.RoomManager.GenerateRoom(transform.position, this, direction);
				}
			}
		}
	}
}
