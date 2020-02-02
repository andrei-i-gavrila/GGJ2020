using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GGJ.Puzzles.KeySequence
{
    public class KeySequencePuzzleController : BasePuzzleController
    {
        public override string PuzzleId => Constants.KEYSEQUENCE_ID;
        public List<KeyCode> KeySequence;
        public float CompletionTime;

        public float TimePassed;
        public int CorrectKeyCount;


        private const string KEY_DISPLAY_BASE_PATH = "Prefabs/Ui/";
        private HorizontalLayoutGroup keyLayout;
        private Image progressBar;

        private List<KeySequenceKeyDisplay> keyDisplays = new List<KeySequenceKeyDisplay>();

        protected void Awake()
        {
            Utils.GetComponentInChild(transform, "KeyLayout", out keyLayout);
            Utils.GetComponentInChild(transform, "ProgressBar", out progressBar);
            Utils.GetComponentInChild(transform, "StartText", out startText);
            createDisplayedKeys();
        }

        private void createDisplayedKeys()
        {
            keyDisplays = KeySequence.Select(code => Instantiate(Resources.Load<GameObject>(KEY_DISPLAY_BASE_PATH + getPrefabToDisplay(code)), keyLayout.transform).AddComponent<KeySequenceKeyDisplay>()).ToList();
        }

        private void Update()
        {
            if (!opened) return;
            progressBar.fillAmount = Mathf.InverseLerp(CompletionTime, 0f, TimePassed);
            clearDisplayedKeys();

            if (Input.GetKeyUp(KeyCode.Space) && !started) StartPuzzle();

            if (!started) return;

            TimePassed += Time.deltaTime;

            if (TimePassed > CompletionTime)
            {
                fail();
                return;
            }

            if (Input.GetKeyDown(KeySequence[CorrectKeyCount]))
            {
                CorrectKeyCount++;
                if (CorrectKeyCount == KeySequence.Count)
                {
                    completed();
                    return;
                }
            }
            else if (Input.anyKeyDown)
            {
                fail();
                return;
            }

            showDisplayedKeys();
        }

        protected override void generatePuzzleData()
        {
            var keyPossibilities = new[] {KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.LeftArrow};
            KeySequence = new List<KeyCode>();

            var keyAmount = (int) Mathf.Lerp(5f, 10f, difficulty / 10f);
            CompletionTime = keyAmount * Mathf.Lerp(1f, 0.3f, difficulty / 10f);

            for (var i = 0; i < keyAmount; i++)
            {
                KeySequence.Add(keyPossibilities[Random.Range(0, keyPossibilities.Length)]);
            }
        }

        protected override void resetPuzzle()
        {
            base.resetPuzzle();

            TimePassed = 0;
            CorrectKeyCount = 0;
        }


        private void clearDisplayedKeys()
        {
            for (var i = keyLayout.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(keyLayout.transform.GetChild(i).gameObject);
            }
        }


        private void showDisplayedKeys()
        {
            for (var i = 0; i < KeySequence.Count; i++)
            {
                if (i < CorrectKeyCount) keyDisplays[i].SetCorrect();
                else keyDisplays[i].SetNormal();
            }
        }

        private static string getPrefabToDisplay(KeyCode code)
        {
            switch (code)
            {
                case KeyCode.UpArrow: return "UIUpArrowKeyDisplay";
                case KeyCode.DownArrow: return "UIDownArrowKeyDisplay";
                case KeyCode.LeftArrow: return "UILeftArrowKeyDisplay";
                default: return "UIRightArrowKeyDisplay";
            }
        }
    }
}