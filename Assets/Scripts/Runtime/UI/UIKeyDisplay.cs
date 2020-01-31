using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIKeyDisplay : MonoBehaviour
{
    public Text KeyLabel { get; private set; }
    public void Awake()
    {
        KeyLabel = transform.Find("KeyTextLabel").GetComponent<Text>();
    }

    public void Init(char key)
    {
        KeyLabel.text = key.ToString();
    }

    
}
