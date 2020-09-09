using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Figure
{

    Block[] block;
    int center;

    public Figure(Block[] _block, int _center)
    {
        block = _block;
        center = _center;
    }


    public Block[] Blocks
    {
        get
        {
            return block;
        }

    }

    public bool Rotate(bool left, List<Block> blocks)
    {

        if (block.Length > 0 && (center >= block.Length || center < 0))
            return false;


        Vector2[] copy_v = new Vector2[block.Length];

        Vector2 posC = block[center].Pos;

        bool isCol = false;

        int i = 0;
        foreach (Block bl in block)
        {

            float x = 0f;
            float y = 0f;

            x = posC.x + (bl.Pos.y - posC.y);
            y = posC.y - (bl.Pos.x - posC.x);

            Vector2 newpos = new Vector2(x, y);

            copy_v[i] = newpos;

            if (Board.isCollision(newpos, blocks))
            {
                isCol = true;
                break;
            }

            i++;
        }


        if (!isCol)
            for (int a = 0; a < block.Length; a++)
            {
                block[a].Pos = copy_v[a];
            }

        return !isCol;

    }


    public bool Move(Vector2 dir, List<Block> blocks)
    {

        Vector2[] copy_v = new Vector2[block.Length];

        bool isCol = false;

        int i = 0;

        foreach (Block bl in block)
        {

            float x = 0f;
            float y = 0f;

            x = bl.Pos.x + dir.x;
            y = bl.Pos.y + dir.y;


            Vector2 newpos = new Vector2(x, y);

            copy_v[i] = newpos;

            if (Board.isCollision(newpos, blocks))
            {
                isCol = true;
                break;
            }

            i++;

        }

        if (!isCol)
            for (int a = 0; a < block.Length; a++)
            {
                block[a].Pos = copy_v[a];
            }

        return !isCol;

    }

    public void Set(Vector2 center)
    {
        foreach (Block bl in block)
        {
            Vector2 new_v2 = new Vector2(bl.Pos.x + center.x , bl.Pos.y + center.y);
            bl.Pos = new_v2;

        }
    }

    public void ChangeColor(Color newc)
    {
        foreach (Block bl in block)
        {
            bl.Col = newc;
        }
    }

}