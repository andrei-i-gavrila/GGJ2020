using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIKeySequencePuzzleScreen : MonoBehaviour
{
    private const string KEY_DISPLAY_PATH = "";
    public HorizontalLayoutGroup KeyLayout { get; set; }

    public void Awake()
    {
        KeyLayout = transform.Find("KeyLayout").GetComponent<HorizontalLayoutGroup>();
        
    }

    public void ClearPuzzleDisplay()
    {
        for(int i = KeyLayout.transform.childCount-1; i>=0; i-- )
        {
            Destroy(KeyLayout.transform.GetChild(i).gameObject);
        }
    }


    public void ShowSequencePuzzle(KeySequencePuzzle puzzle)
    {
        ClearPuzzleDisplay();
        foreach (var key in puzzle.KeySequence)
        {
            var keyDisplay = Instantiate(Resources.Load<UIKeyDisplay>(KEY_DISPLAY_PATH), KeyLayout.transform);
            keyDisplay.Init(key);
        }
    }

}
