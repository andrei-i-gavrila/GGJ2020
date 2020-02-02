﻿using GGJ.Rooms;
using TMPro;
using UnityEngine;

namespace GGJ.UI
{
	public class UIMessagesManager : BaseBehaviour
	{
		private TextMeshProUGUI message;
		private Coroutine hideMessageCoroutine;

		private void Awake()
		{
			Initialize();
		}

		private void Initialize()
		{
			if (!Utils.GetComponentInChild(transform, "message", out message))
			{
				Debug.LogError("Component not found!!!");
			}

			message.text = "";
			Game.PuzzleManager.OnEntranceUnlocked += OnEntranceUnlocked;
			Game.PuzzleManager.OnConsoleUnlocked += OnConsoleUnlocked;
		}

		private void OnConsoleUnlocked(Console console)
		{
			var text = "Console " + console.Id + " in room " + console.Room.RoomId + " has been repaired";
			ShowText(text);
		}
		private void OnEntranceUnlocked(Entrance entrance)
		{
			var text = "The " + entrance.EntranceDirections[entrance.ConnectedRooms.Item1] + " door from " + entrance.ConnectedRooms.Item1.RoomId + " has been repaired";
			ShowText(text);
		}

		private void ShowText(string text)
		{
			if (hideMessageCoroutine != null)
			{
				StopCoroutine(hideMessageCoroutine);
			}

			message.text = text;
			hideMessageCoroutine = Invoke(() => message.text = "", 5f);
		}
	}
}
