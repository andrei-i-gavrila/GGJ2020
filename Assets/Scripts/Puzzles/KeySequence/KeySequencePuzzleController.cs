using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ.Puzzles.KeySequence
{
    public class KeySequencePuzzleController : BasePuzzleController
    {
        protected override Component puzzleContainer => container;
        public override string PuzzleId => Constants.KEYSEQUENCE_ID;
        public List<KeyCode> KeySequence;
        public float CompletionTime;

        public float TimePassed;
        public int CorrectKeyCount;


        private const string KEY_DISPLAY_BASE_PATH = "Prefabs/Ui/";
        private HorizontalLayoutGroup keyLayout;
        private Image progressBar;

        private RectTransform container;
        
        
        private List<KeySequenceKeyDisplay> keyDisplays;
    
        protected void Awake()
        {
            Utils.GetComponentInChild(transform, "KeyLayout", out keyLayout);
            Utils.GetComponentInChild(transform, "ProgressBar", out progressBar);
            Utils.GetComponentInChild(transform, "StartText", out startText);
            Utils.GetComponentInChild(transform, "Container", out container);
        }

        private void createDisplayedKeys()
        {
            keyDisplays = KeySequence.Select(code => Instantiate(Resources.Load<GameObject>(KEY_DISPLAY_BASE_PATH + getPrefabToDisplay(code)), keyLayout.transform).AddComponent<KeySequenceKeyDisplay>()).ToList();
        }

        protected override void StartPuzzle()
        {
            keyLayout.gameObject.SetActive(true);
            base.StartPuzzle();
        }

        private void Update()
        {
            if (!opened) return;
            progressBar.fillAmount = Mathf.InverseLerp(CompletionTime, 0f, TimePassed);

            if (Input.GetKeyUp(KeyCode.Space) && !started) StartPuzzle();

            if (!started) return;

            TimePassed += Time.deltaTime;

            
            if (TimePassed > CompletionTime)
            {
                fail();
                return;
            }
            
            if (CorrectKeyCount >= KeySequence.Count) return;


            if (arrowKeys.Any(Input.GetKeyDown))
            {
                if (Input.GetKeyDown(KeySequence[CorrectKeyCount]))
                {
                    CorrectKeyCount++;
                    if (CorrectKeyCount == KeySequence.Count)
                    {
                        completed();
                        return;
                    }
                }
                else
                {
                    fail();
                }
            }

            showDisplayedKeys();
        }

        protected override void generatePuzzleData()
        {
            keyLayout.gameObject.SetActive(true);

            clearDisplayedKeys();

            var keyPossibilities = new[] {KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.LeftArrow};
            KeySequence = new List<KeyCode>();

            var keyAmount = Game.Instance.DificultyManager.GetNumberOfKeysInSequence();
            CompletionTime = keyAmount * Game.Instance.DificultyManager.GetTimePerKeyInKeySequence();

            for (var i = 0; i < keyAmount; i++)
            {
                KeySequence.Add(keyPossibilities[Random.Range(0, keyPossibilities.Length)]);
            }

            createDisplayedKeys();
            keyLayout.gameObject.SetActive(false);

        }

        protected override void resetPuzzle()
        {
            base.resetPuzzle();

            TimePassed = 0;
            CorrectKeyCount = 0;
            keyLayout.gameObject.SetActive(false);
        }


        private void clearDisplayedKeys()
        {
            keyDisplays?.ForEach(k => DestroyImmediate(k.gameObject));
        }


        private void showDisplayedKeys()
        {
            for (var i = 0; i < keyDisplays.Count; i++)
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