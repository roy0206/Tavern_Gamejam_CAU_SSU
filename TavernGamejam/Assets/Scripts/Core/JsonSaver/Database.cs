using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Database
{
    public int checkpoint;
    public int attempt;
    public bool isCleared;







    public Database()
    {
        checkpoint = 0;
        attempt = 1;
        isCleared = false;
    }
}
