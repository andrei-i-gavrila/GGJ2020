using GGJ.Puzzles;
using GGJ.Rooms;
using UnityEngine;

namespace GGJ
{
	public class Game : BaseBehaviour
	{
		public static Game Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new GameObject("Game").AddComponent<Game>();
				}

				return instance;
			}
		}

		private static Game instance;

		private void Awake()
		{
			if (instance != null && instance != this)
			{
				Destroy(this);
			}
			else if (instance == null)
			{
				instance = this;
			}

			Invoke(() => PuzzleManager.StartPuzzle(PuzzleType.KeySequence, 5), 2);
		}

		private void Initialize()
		{
			RoomManager = FindObjectOfType<RoomManager>() ?? new GameObject("RoomManager").AddComponent<RoomManager>();
		}

		public readonly PuzzleManager PuzzleManager = new PuzzleManager();
		public RoomManager RoomManager;
	}
}