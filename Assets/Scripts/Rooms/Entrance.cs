using UnityEngine;

namespace GGJ.Rooms
{
	public class Entrance : BaseBehaviour
	{
		public Pair<Room, Room> ConnectedRooms = new Pair<Room, Room>();
		public bool IsLocked { get; private set; } = true;

		public Room GetNextRoom(Room room)
		{
			if (room == null)
			{
				Debug.LogError("The given room was null!!!");
				return null;
			}

			if (ConnectedRooms.Item1 != room && ConnectedRooms.Item2 != room)
			{
				Debug.LogError("The given room is not in this entrance!!!");
				return null;
			}

			return room == ConnectedRooms.Item1 ? ConnectedRooms.Item2 : ConnectedRooms.Item1;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (IsLocked)
				return;

			ConnectedRooms.Item1.gameObject.SetActive(true);
			ConnectedRooms.Item2.gameObject.SetActive(true);
		}

		private void OnUnlock()
		{
			if (ConnectedRooms.Item1 == null)
			{
				//TODO : generate room 1
			}

			if (ConnectedRooms.Item2 == null)
			{
				//TODO : generate room 2
			}
		}
	}
}
