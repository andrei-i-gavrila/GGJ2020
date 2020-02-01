using UnityEngine;

namespace GGJ
{
	public class PrefabsManager : BaseBehaviour
	{
		public Transform Console { get; private set; }

		private void Awake()
		{
			Console = Resources.Load<Transform>(Paths.PREFABS + "Console");
		}
	}
}