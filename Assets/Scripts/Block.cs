using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Block
{

    GameObject ob;
    public GameObject trail;
    Vector2 pos;
    Color col;

    public Block(Vector2 _pos, Color _col , GameObject _effect = null)
    {

        pos = _pos;
        col = _col;
        ob = GameObject.Instantiate(Board._blockPrefab);

        GameObject temp = _effect != null ? _effect : Board._effectTrail;


        if (temp != null)
        {
            trail = GameObject.Instantiate(temp);
            trail.transform.SetParent(obj.transform);
            trail.GetComponent<TrailRenderer>().startColor = col;
        }
        

        ob.transform.position = new Vector3(pos.x, pos.y, 0);
        ob.transform.localScale = new Vector3(Board._widthB, Board._widthB, Board._widthB);
        ob.GetComponent<SpriteRenderer>().color = col;
        


    }

    public void Bum()
    {
        GameObject b = GameObject.Instantiate(Board._bum);
        b.transform.position = obj.transform.position;
        
    }

    public void Bum2()
    {
        GameObject b = GameObject.Instantiate(Board._bum2);
        b.transform.position = obj.transform.position;

    }

    public int SetOrder
    {
        set
        {
            ob.GetComponent<SpriteRenderer>().sortingOrder = value;
        }
    }

    public GameObject obj
    {
        get
        {
            return ob;
        }
    }

    public Vector2 Pos
    {
        get
        {
            return pos;
        }

        set
        {
            pos = value;
            ob.transform.position = pos;
        }
    }


    public float posZ
    {
        set
        {
            ob.transform.position = new Vector3(pos.x, pos.y , value);
        }
    }


    public Color Col
    {
        set
        {
            col = value;
            ob.GetComponent<SpriteRenderer>().color = col;
        }
        get
        {
            return col;
        }
    }


    public void Destroy()
    {
        GameObject.Destroy(ob);
    }

}