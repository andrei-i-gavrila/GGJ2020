using GGJ.Puzzles;
using GGJ.Puzzles.ReactionSpeed;
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

			Initialize();
		}

		private void Initialize()
		{
			RoomManager = FindObjectOfType<RoomManager>() ?? new GameObject("RoomManager").AddComponent<RoomManager>();
			PrefabsManager = FindObjectOfType<PrefabsManager>() ?? gameObject.AddComponent<PrefabsManager>();
			Character = FindObjectOfType<Character>();
			Invoke(() =>
			{
				Utils.GetComponentInChild<ReactionSpeedPuzzleController>(transform, "ReactionSpeedPuzzle", out var puzzle);
				puzzle?.gameObject.SetActive(true);
				puzzle?.Open();
			}, 1);
		}

		public readonly PuzzleManager PuzzleManager = new PuzzleManager();
		public DificultyManager DificultyManager { get; private set; } = new DificultyManager();
		public PrefabsManager PrefabsManager { get; private set; }
		public RoomManager RoomManager;
		public Character Character { get; private set; }
		public int CurrentDificulty { get; private set; } = 0;
	}
}