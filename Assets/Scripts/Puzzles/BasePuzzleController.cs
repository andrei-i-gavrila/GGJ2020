using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GGJ
{
    public abstract class BasePuzzleController : BaseBehaviour
    {
        public float difficulty = 1f;
        protected bool opened;
        protected bool started;

        protected TextMeshProUGUI startText;


        public Action OnOpen { get; set; }

        public Action<bool> OnPuzzleCompleted { get; set; }

        public abstract string PuzzleId { get; }


        protected virtual void fail()
        {
            resetPuzzle();
            // opened = false;
            OnPuzzleCompleted?.Invoke(false);
        }

        protected void completed()
        {
            resetPuzzle();
            // opened = false;
            OnPuzzleCompleted?.Invoke(true);
        }

        public virtual void Open()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            opened = true;
            resetPuzzle();
        }

        protected virtual void StartPuzzle()
        {
            started = true;
            startText.gameObject.SetActive(false);
        }

        protected abstract void generatePuzzleData();

        protected virtual void resetPuzzle()
        {
            generatePuzzleData();

            started = false;
            startText.gameObject.SetActive(true);
        }

        protected static readonly List<KeyCode> arrowKeys = new List<KeyCode> {KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.RightArrow};
    }
}