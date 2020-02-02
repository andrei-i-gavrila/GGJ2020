using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Puzzles
{
	public class PuzzleManager : BaseBehaviour
	{
		public Action<BasePuzzleController> OnPuzzleStarted;
		public Action<bool> OnPuzzleEnd;
		private Dictionary<string, BasePuzzleController> puzzleControllers = new Dictionary<string, BasePuzzleController>();

		private void Awake()
		{
			Initialize();
		}

		private void Initialize()
		{
			var puzzles = Resources.LoadAll<BasePuzzleController>(Paths.PREFABS);
			foreach (var puzzle in puzzles)
			{
				puzzleControllers.Add(puzzle.PuzzleId, Instantiate(puzzle, transform));
			}
			HideAllPuzzles();
		}

		public void StartPuzzle(string id)
		{
			if (!puzzleControllers.ContainsKey(id))
			{
				Debug.LogError("There was no puzzle with the given id in the dictionary");
			}
			else
			{
				StartPuzzle(puzzleControllers[id]);
			}
		}

		private void StartPuzzle(BasePuzzleController basePuzzleController)
		{
			basePuzzleController?.gameObject.SetActive(true);
			basePuzzleController?.Open();
			OnPuzzleStarted?.Invoke(basePuzzleController);
		}

		private void HideAllPuzzles()
		{
			foreach (var puzzle in puzzleControllers.Values)
			{
				puzzle?.gameObject?.SetActive(false);
			}
		}
	}
}