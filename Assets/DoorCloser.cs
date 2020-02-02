using System;

namespace GGJ
{
	public class DoorCloser : BaseBehaviour
	{
		public Action OnExit;
		private void OnTriggerExit(UnityEngine.Collider other)
		{
			OnExit?.Invoke();
		}
	}
}
