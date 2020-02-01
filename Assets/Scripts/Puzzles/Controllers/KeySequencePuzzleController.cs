using GGJ;
using GGJ.Puzzles.Data;
using UnityEngine;

namespace Puzzles.Controllers
{
    public class KeySequencePuzzleController : BasePuzzleController<KeySequencePuzzleData>
    {
        private void Update()
        {
            if (PuzzleData == null)
            {
                gameObject.SetActive(false);
                return;
            }

            PuzzleData.TimePassed += Time.deltaTime;


            if (PuzzleData.TimePassed > PuzzleData.CompletionTime)
            {
                Game.Instance.PuzzleManager.PuzzleFailed();
            }
            
            if (Input.GetKeyDown(PuzzleData.KeySequence[PuzzleData.CorrectKeyCount]))
            {
                PuzzleData.CorrectKeyCount++;
            }
            else if (Input.anyKeyDown)
            {
                Game.Instance.PuzzleManager.PuzzleFailed();
            }
        }
    }
}