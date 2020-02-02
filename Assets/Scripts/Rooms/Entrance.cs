using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Rooms
{
	public class Entrance : BaseBehaviour
	{
		[SerializeField] private Transform slidingDoor;
		private const float OPEN_DURATION = 0.5f;
		private const float OPEN_DISTANCE = 3f;

		public Pair<Room, Room> ConnectedRooms = new Pair<Room, Room>();
		public Dictionary<Room, Direction> EntranceDirections = new Dictionary<Room, Direction>();
		private List<string> unlockConditions = new List<string>();
		private Tween doorTween;

		public bool Locked { get; set; } = true;
		private bool runned = false;
		public void SetDirectionForRoom(Room room, Direction direction)
		{
			if (!IsRoomInThisEntrance(room))
				return;

			if (EntranceDirections.ContainsKey(room))
			{
				EntranceDirections[room] = direction;
			}
			else
			{
				EntranceDirections.Add(room, direction);
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

			if (!EntranceDirections.ContainsKey(room))
			{
				Debug.LogError("The entrance direction for this room was not set!");
				return false;
			}

			direction = EntranceDirections[room];
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
			if (Locked)
			{
				return;
			}

			OpenDoor();
			OnUnlock();
			ConnectedRooms.Item1?.gameObject.SetActive(true);
			ConnectedRooms.Item2?.gameObject.SetActive(true);
		}

		private void OnTriggerExit(Collider other)
		{
			if (Locked)
				return;

			CloseDoor();
		}

		private void OnUnlock()
		{
			if (runned)
				return;

			runned = true;
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

		private void OpenDoor()
		{
			if (doorTween != null && doorTween.active)
			{
				doorTween.Kill();
				doorTween = null;
			}

			var remainingDistance = OPEN_DISTANCE - transform.position.y;
			var remainingTime = OPEN_DURATION * (remainingDistance / OPEN_DISTANCE);
			slidingDoor.DOMoveY(OPEN_DISTANCE, remainingTime);
		}

		private void CloseDoor()
		{
			if (doorTween != null && doorTween.active)
			{
				doorTween.Kill();
				doorTween = null;
			}

			var remainingDistance = transform.position.y;
			var remainingTime = OPEN_DURATION * (remainingDistance / OPEN_DISTANCE);
			slidingDoor.DOMoveY(0, remainingTime);
		}
	}
}
