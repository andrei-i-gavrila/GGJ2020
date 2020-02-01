using GGJ.Puzzles;
using GGJ.Puzzles.KeySequence;
using GGJ.Puzzles.SimonSays;
using UnityEngine;

namespace GGJ
{
    public class Game : BaseBehaviour
    {
        public static Game Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("Game").AddComponent<Game>();
                }

                return instance;
            }
        }

        private static Game instance;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
            }
            else if (instance == null)
            {
                instance = this;
            }

            Invoke(() =>
            {
                Utils.GetComponentInChild<SimonSaysPuzzleController>(transform, "SimonSaysPuzzle", out var puzzle);
                puzzle.gameObject.SetActive(true);
                puzzle.Open();
            }, 1);
        }

        public readonly PuzzleManager PuzzleManager = new PuzzleManager();
    }
}