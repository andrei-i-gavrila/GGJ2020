using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
	public class BaseBehaviour : MonoBehaviour
	{
		public static Game Game => Game.Instance;
		
		public Coroutine Invoke(Action action, float time)
		{
			return StartCoroutine(DelayedInvoke(action, time));
		}

		private IEnumerator DelayedInvoke(Action action, float time)
		{
			yield return new WaitForSeconds(time);
			action?.Invoke();
		}
	}
}