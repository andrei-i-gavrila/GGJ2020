using System;
using UnityEngine;

namespace GGJ
{
	public abstract class BasePuzzleController : BaseBehaviour
	{
		public abstract void Open();
		public Action<bool> OnPuzzleCompleted { get; set; }
		public abstract String PuzzleId { get; }
	}
}