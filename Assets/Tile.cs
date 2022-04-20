using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    float x, y;
    public int n_x, n_y;
    GameObject sprite;
    SpriteRenderer spriteRend;
    public bool clicked;


    public bool isClicked(Vector3 mousePos)
    {
        Vector3 size = transform.localScale;
        return (mousePos.x < x + size.x / 2) && (mousePos.x > x - size.x / 2)
            && (mousePos.y < y + size.y / 2) && (mousePos.y > y - size.y / 2);

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

    public SpriteRenderer GetSpriteRend()
    {
        return this.spriteRend;
    }
    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        x = this.transform.position.x;
        y = this.transform.position.y;
        spriteRend.color = new Color(1,1,1,1);
        clicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        return;
    }


}
