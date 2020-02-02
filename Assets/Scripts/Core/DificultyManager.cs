using System;
using Random = UnityEngine.Random;

namespace GGJ
{
	public class DificultyManager : BaseBehaviour
	{
		public int Dificulty { get; private set; }

		private void Start()
		{
			Game.PuzzleManager.OnPuzzleCompleted += OnLevelCompleted;
		}

		public void OnLevelCompleted(bool completed)
		{
			if (!completed)
				return;
			Dificulty++;
		}

		public int GetNumberOfConsoles()
		{
			if (Dificulty <= 2)
			{
				return Random.value > 0.3f ? 2 : 1;
			}

			if (Dificulty <= 4)
			{
				return Random.value > 0.6f ? 2 : 1;
			}

			if (Dificulty <= 7)
			{
				return Random.value > 0.4f ? 3 : 2;
			}

			if (Dificulty <= 11)
			{
				return Random.value >= 0.8 ? 3 : 2;
			}

			return 4;
		}


		public int GetNumberOfJigsawPieces()
		{
			return 5 + Math.Max(Dificulty - 5, 0);
		}

		public int GetNumberOfSimonCommands()
		{
			return 3 + Dificulty / 3;
		}

		public float GetSimonMaxTime()
		{
			return 2 + Dificulty / 6;
		}

		public int GetNumberOfKeysInSequence()
		{
			return 4 + Dificulty / 2;
		}

		public float GetTimePerKeyInKeySequence()
		{
			return Math.Max(0.3f, 0.5f - Dificulty * 0.01f);
		}

		public int GetNumberOfMemoryPairs()
		{
			return 2 + Dificulty / 2;
		}

		public int GetNumberOfReactionTimeChallenges()
		{
			return 1 + Dificulty / 3;
		}

		public float GetReactTimeSpeed()
		{
			return Math.Min(0.8f, 0.33f + Dificulty * 0.0235f);
		}

		public float GetMaxReactionSpeedTimeError()
		{
			return Math.Max(0.01f, 0.1f - Dificulty * 0.0045f);
		}
	}
}