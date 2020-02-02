using System;
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

        public abstract String PuzzleId { get; }

        protected virtual void fail()
        {
            resetPuzzle();
            OnPuzzleCompleted?.Invoke(false);
        }

        protected void completed()
        {
            resetPuzzle();
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
        }

        protected abstract void generatePuzzleData();

        protected virtual void resetPuzzle()
        {
            generatePuzzleData();

            started = false;
            startText.gameObject.SetActive(true);
        }
    }
}