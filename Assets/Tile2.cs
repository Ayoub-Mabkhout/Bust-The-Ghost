using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Tile2 
{
    string color;
    float x, y;
    GameObject sprite;

    public Tile2(string color, GameObject sprite)
    {
        this.sprite = sprite;
        this.color = color;
        x = sprite.transform.position.x;
        y = sprite.transform.position.y;
    }

    public void isClicked(Vector3 mousePos)
    {


    }

    public float GetX()
    {
        return this.x;
    }

    public float GetY()
    {
        return this.y;
    }

    public GameObject GetSprite()
    {
        return this.sprite;
    }
}
