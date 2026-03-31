using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class CheckPoint : MonoThing, ISavable
{
    public static List<CheckPoint> CheckPoints = new List<CheckPoint>();
    [SerializeField] int id;
    public bool saved;

    private void Start()
    {
        CheckPoints.Add(this);
    }

    public void LoadData(Database data)
    {
        if(data.checkpoint == id)
        {
            saved = true;
            FindFirstObjectByType<Player>().transform.position = transform.position;

        }
    }

    public void SaveData(ref Database data)
    {
        if (saved) { data.checkpoint = id; CheckPoints.Clear(); }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (saved) return;
        foreach (var checkPoint in CheckPoints)
        {
            checkPoint.saved = false;
        }
        saved = true;


    }
}
