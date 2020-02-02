using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GGJ
{
    public abstract class BasePuzzleController : BaseBehaviour
    {
        protected bool opened;
        protected bool started;
        protected TextMeshProUGUI startText;
        protected abstract Component puzzleContainer { get; }

        public Action OnOpen { get; set; }

        public Action<bool> OnPuzzleCompleted { get; set; }

        public abstract string PuzzleId { get; }

        private void LateUpdate()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                completeWithState(false);
            }
        }

        protected void fail()
        {
            puzzleContainer.gameObject.SetActive(false);
            startText.gameObject.SetActive(true);
            startText.text = "Failed...";
            Invoke(() => completeWithState(false), 0.66f);
        }

        protected void completed()
        {
            puzzleContainer.gameObject.SetActive(false);
            startText.gameObject.SetActive(true);
            startText.text = "Success!";
            Invoke(() => completeWithState(true), 0.33f);
        }

        private void completeWithState(bool state)
        {
            resetPuzzle();
            opened = false;
            OnPuzzleCompleted?.Invoke(state);
            OnPuzzleCompleted = null;
            gameObject.SetActive(false);
        }

        public virtual void Open()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            opened = true;
            startText.gameObject.SetActive(true);
            startText.text = "Press space to start";


            resetPuzzle();
        }

        protected virtual void StartPuzzle()
        {
            puzzleContainer.gameObject.SetActive(true);
            startText.gameObject.SetActive(false);
            started = true;
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