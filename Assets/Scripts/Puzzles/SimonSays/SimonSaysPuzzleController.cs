using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GGJ.Puzzles.SimonSays
{
    public class SimonSaysPuzzleController : BasePuzzleController
    {
        public override string PuzzleId => Constants.SIMON_ID;


        public List<KeyCode> KeySequence;
        public float MaxTime;

        public int CorrectKeyCount;
        public float TimePassed;
        private int stage;


        private Image progressBar;
        private SimonSaysKeyDisplay up;
        private SimonSaysKeyDisplay down;
        private SimonSaysKeyDisplay left;
        private SimonSaysKeyDisplay right;

        private bool waitingInput;

        protected void Awake()
        {
            Utils.GetComponentInChild(transform, "Up", out up);
            Utils.GetComponentInChild(transform, "Right", out right);
            Utils.GetComponentInChild(transform, "Down", out down);
            Utils.GetComponentInChild(transform, "Left", out left);
            Utils.GetComponentInChild(transform, "ProgressBar", out progressBar);
            Utils.GetComponentInChild(transform, "StartText", out startText);
        }

        private void Update()
        {
            if (!opened) return;
            progressBar.fillAmount = Mathf.InverseLerp(MaxTime, 0f, TimePassed);

            if (Input.GetKeyUp(KeyCode.Space) && !started) StartPuzzle();

            if (!started) return;

            if (!waitingInput) return;

            TimePassed += Time.deltaTime;

            if (TimePassed > MaxTime)
            {
                fail();
                return;
            }

            if (Input.GetKeyDown(KeySequence[CorrectKeyCount]))
            {
                var keyDisplay = getKeyDisplay(KeySequence[CorrectKeyCount]);
                keyDisplay.SetPressed();
                Invoke(keyDisplay.SetNormal, .5f);

                CorrectKeyCount++;
                if (CorrectKeyCount == stage)
                {
                    stage++;
                    if (stage > KeySequence.Count)
                    {
                        completed();
                        return;
                    }

                    TimePassed = 0;
                    StartCoroutine(showHints());
                }
            }
            else if (Input.anyKeyDown)
            {
                fail();
            }
        }


        protected override void StartPuzzle()
        {
            base.StartPuzzle();
            StartCoroutine(showHints());
        }


        private IEnumerator showHints()
        {
            waitingInput = false;
            yield return new WaitForSeconds(1f);

            for (var i = 0; i < stage; i++)
            {
                var keyDisplay = getKeyDisplay(KeySequence[i]);

                keyDisplay.SetHint();
                yield return new WaitForSeconds(.5f);
                keyDisplay.SetNormal();
                yield return new WaitForSeconds(.5f);
            }

            waitingInput = true;
            TimePassed = 0;
            CorrectKeyCount = 0;
        }

        protected override void generatePuzzleData()
        {
            var keyPossibilities = new[] {KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.LeftArrow};
            KeySequence = new List<KeyCode>();

            var keyAmount = (int) Mathf.Lerp(5f, 8f, difficulty / 10f);

            MaxTime = Mathf.Lerp(10f, 5f, difficulty / 10f);

            for (var i = 0; i < keyAmount; i++)
            {
                KeySequence.Add(keyPossibilities[Random.Range(0, keyPossibilities.Length)]);
            }
        }

        private SimonSaysKeyDisplay getKeyDisplay(KeyCode code)
        {
            switch (code)
            {
                case KeyCode.UpArrow: return up;
                case KeyCode.LeftArrow: return left;
                case KeyCode.RightArrow: return right;
                default: return down;
            }
        }

        protected override void resetPuzzle()
        {
            base.resetPuzzle();
            TimePassed = 0;
            CorrectKeyCount = 0;
            stage = 1;
        }
    }
}