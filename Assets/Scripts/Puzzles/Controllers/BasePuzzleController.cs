using GGJ;
using GGJ.Puzzles;
using UnityEngine;

namespace Puzzles.Controllers
{
    public class BasePuzzleController<TPuzzleData> : MonoBehaviour where TPuzzleData : IPuzzleData
    {
        protected TPuzzleData PuzzleData;

        protected virtual void Awake()
        {
            Game.Instance.PuzzleManager.OnPuzzleStarted += OnPuzzleStarted;
        }

        private void OnPuzzleStarted(IPuzzleData obj)
        {
            if (obj is TPuzzleData puzzleData)
            {
                PuzzleData = puzzleData;
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        
    }
}