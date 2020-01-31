using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
	public class BaseBehaviour : MonoBehaviour
	{
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