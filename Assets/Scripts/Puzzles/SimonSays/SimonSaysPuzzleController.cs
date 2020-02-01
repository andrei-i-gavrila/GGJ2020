using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GGJ.Puzzles.SimonSays
{
    public class SimonSaysPuzzleController : BaseBehaviour
    {
        public List<KeyCode> KeySequence;
        public float MaxTime;

        public int CorrectKeyCount;
        public float TimePassed;
        private int stage;


        public float difficulty = 1f;

        private Image progressBar;
        private TextMeshProUGUI startText;
        private SimonSaysKeyDisplay up;
        private SimonSaysKeyDisplay down;
        private SimonSaysKeyDisplay left;
        private SimonSaysKeyDisplay right;

        private bool opened;
        private bool started;
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
                    }
                    else
                    {
                        TimePassed = 0;
                        StartCoroutine(showHints());
                    }
                }
            }
            else if (Input.anyKeyDown)
            {
                fail();
            }
        }

        private void fail()
        {
            resetPuzzle();
        }

        private void completed()
        {
            resetPuzzle();
        }

        public void Open()
        {
            generatePuzzleData();
            opened = true;
        }

        private void StartPuzzle()
        {
            startText.gameObject.SetActive(false);
            started = true;

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

        private void generatePuzzleData()
        {
            var keyPossibilities = new[] {KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.LeftArrow};
            KeySequence = new List<KeyCode>();

            var keyAmount = (int) Mathf.Lerp(5f, 8f, difficulty / 10f);

            MaxTime = Mathf.Lerp(10f, 5f, difficulty / 10f);

            for (var i = 0; i < keyAmount; i++)
            {
                KeySequence.Add(keyPossibilities[Random.Range(0, keyPossibilities.Length)]);
            }

            resetPuzzle();
        }

        private SimonSaysKeyDisplay getKeyDisplay(KeyCode code)
        {
            if (code == KeyCode.UpArrow) return up;
            if (code == KeyCode.LeftArrow) return left;
            if (code == KeyCode.RightArrow) return right;
            return down;
        }

        private void resetPuzzle()
        {
            TimePassed = 0;
            CorrectKeyCount = 0;
            started = false;
            stage = 1;
            startText.gameObject.SetActive(true);
        }
    }
}