using UnityEngine;

namespace GGJ
{
	public class PrefabsManager : BaseBehaviour
	{
		public Console Console { get; private set; }

		private void Awake()
		{
			Console = Resources.Load<Console>(Paths.PREFABS + "Console");
		}
	}
}