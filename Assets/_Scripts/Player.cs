using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string _name { get; set; }
    //public int _points { get; set; }
    public float _juice { get; set; }

    public Player(string name, float juice)
    {
        _name = name;
        //_points = points;
        _juice = juice;
    }
}
