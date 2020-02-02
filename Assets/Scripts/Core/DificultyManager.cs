using UnityEngine;

namespace GGJ
{
	public class DificultyManager : BaseBehaviour
	{
		public int Dificulty { get; private set; }

		public void OnLevelCompleted()
		{

		}

		public int GetNumberOfConsoles()
		{
			if (Dificulty <= 2)
			{
				return Random.value > 0.3f ? 2 : 1;
			}
			else if (Dificulty <= 4)
			{
				return Random.value > 0.6f ? 2 : 1;
			}
			else if (Dificulty <= 7)
			{
				return Random.value > 0.4f ? 3 : 2;
			}
			else if (Dificulty <= 11)
			{
				return Random.value >= 0.8 ? 3 : 2;
			}
			else
			{
				return 4;
			}
		}
	}
}
