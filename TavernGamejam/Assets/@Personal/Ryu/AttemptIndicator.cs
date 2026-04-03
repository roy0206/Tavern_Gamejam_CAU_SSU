using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;


public class AttemptIndicator : MonoUI, ISavable
{ 
    public void LoadData(Database data)
    {
        linkedText.text = "Attempt " + data.attempt.ToString();
    }

    public void SaveData(ref Database data)
    {

    }
}

