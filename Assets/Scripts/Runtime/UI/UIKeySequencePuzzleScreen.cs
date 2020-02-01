using GGJ.Puzzles;
using GGJ.Puzzles.Data;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ.Runtime.UI
{
    public class UIKeySequencePuzzleScreen : UIBasePuzzleController<KeySequencePuzzleData>
    {
        private const string KEY_DISPLAY_BASE_PATH = "Prefabs/Ui/";
        public HorizontalLayoutGroup KeyLayout { get; set; }


        protected override void Awake()
        {
            base.Awake();
            KeyLayout = transform.Find("KeyLayout").GetComponent<HorizontalLayoutGroup>();
        }

        private void ClearPuzzleDisplay()
        {
            for (var i = KeyLayout.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(KeyLayout.transform.GetChild(i).gameObject);
            }
        }


        private void Update()
        {
            if (PuzzleData == null)
            {
                gameObject.SetActive(false);
                return;
            }
            
            ShowSequencePuzzle();
        }

        private void ShowSequencePuzzle()
        {
            ClearPuzzleDisplay();
            foreach (var key in PuzzleData.KeySequence)
            {
                Instantiate(Resources.Load<UIKeyDisplay>(KEY_DISPLAY_BASE_PATH + getPrefabToDisplay(key)), KeyLayout.transform);
            }
        }

        private static string getPrefabToDisplay(KeyCode code)
        {
            if (code == KeyCode.UpArrow) return "UIUpArrowKeyDisplay";
            if (code == KeyCode.DownArrow) return "UIDownArrowKeyDisplay";
            if (code == KeyCode.LeftArrow) return "UILeftArrowKeyDisplay";
            return "UIRightArrowKeyDisplay";
        }
    }
}