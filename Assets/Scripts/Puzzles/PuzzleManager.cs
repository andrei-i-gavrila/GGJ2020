using GGJ.Rooms;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Puzzles
{
	public class PuzzleManager : BaseBehaviour
	{
		public Action<BasePuzzleController> OnPuzzleStarted;
		public Action<Entrance> OnEntranceUnlocked;
		public Action<Console> OnConsoleUnlocked;
		public Action<bool> OnPuzzleCompleted;

		public float ChanceToUnlockConsole = 0.3f;

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
			basePuzzleController.gameObject.SetActive(true);
			basePuzzleController.Open();
			basePuzzleController.OnPuzzleCompleted += OnPuzzleCompletedHandler;
			basePuzzleController.OnPuzzleCompleted += OnPuzzleCompleted;
			OnPuzzleStarted?.Invoke(basePuzzleController);
		}

		private void HideAllPuzzles()
		{
			foreach (var puzzle in puzzleControllers.Values)
			{
				puzzle?.gameObject?.SetActive(false);
			}
		}

		private void OnPuzzleCompletedHandler(bool completeState)
		{
			if (completeState)
			{
				if (Game.ConsolesManager.GetNumberOfConsolesWithState(ConsoleState.Locked) == 0)
				{
					//Open a door
					UnlockRandomEntrance();
				}
				else
				{
					if (UnityEngine.Random.value < ChanceToUnlockConsole)
					{
						UnlockRandomConsole();
					}
					else
					{
						UnlockRandomEntrance();
					}
				}
			}
		}

		private void UnlockRandomEntrance()
		{
			var entrance = Game.RoomManager.GetRandomLockedEntrance();
			entrance.Locked = false;
			OnEntranceUnlocked?.Invoke(entrance);
		}

		private void UnlockRandomConsole()
		{
			var console = Game.ConsolesManager.GetRandomConsoleWithState(ConsoleState.Locked);
			console.SetConsoleState(ConsoleState.Interactable);
			OnConsoleUnlocked?.Invoke(console);
		}
	}
}