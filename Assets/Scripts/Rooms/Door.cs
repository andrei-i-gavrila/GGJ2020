using UnityEngine;

namespace GGJ.Rooms
{
	public class Door : BaseBehaviour
	{
		public bool IsLocked { get; private set; }

		/// <summary>
		/// The player entered the field of this door
		/// </summary>
		/// <param name="other"></param>
		private void OnTriggerEnter(Collider other)
		{

		}

		private void OnTriggerExit(Collider other)
		{

		}
	}
}
