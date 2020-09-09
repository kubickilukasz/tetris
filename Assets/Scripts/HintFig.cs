using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;

public class HintFig : MonoBehaviour {

  
    public List<PositionOfHint> poshints = new List<PositionOfHint>();


    private void Awake()
    {

        GameObject panel = GameObject.Find("Panel");
        RectTransform[] hints = panel.GetComponentsInChildren<RectTransform>(true);

        int x = -2;
        int y = -2;
        foreach (RectTransform hint in hints)
        {

            if (hint.gameObject.name == "Panel")
                continue;

            poshints.Add(new PositionOfHint(new Vector2Int(x , y) , hint.gameObject));
            y++;
            if(y > 2)
            {
                y = -2;
                x++;
            }

            
        }
    
        Clear();
    }


    public void DrawFigure(DataFigure df)
    {

        Clear();

        foreach (Vector2 v in df.v) {
                    
            var result = from a in poshints
                            where a.v2 == v
                            select a;

            foreach (PositionOfHint pos in result)
            {
                //pos.gObject.GetComponent<Image>().color = new Color32(255, 255, 0, 255);
                pos.gObject.GetComponent<Image>().color = df.col;
            }
       
        }
      

    }


    public void Clear()
    {
        foreach (PositionOfHint p in poshints)
        {
            p.gObject.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        }
    }


     private void LateUpdate()
    {
      /*  foreach (PositionOfHint p in poshints)
        {
            byte a = (byte)Mathf.Lerp(0,255,p.gObject.GetComponent<Image>().color.a);
            float a_v = a - (Time.deltaTime * (10000f/a));
            if (a_v < 0)
                  a_v = 0;
            a = (byte)a_v;
            Debug.Log(a);
            p.gObject.GetComponent<Image>().color = new Color32(255, 255, 0, a);
        }*/
    }

}

[System.Serializable]
public class PositionOfHint
{
    public Vector2Int v2 = new Vector2Int(0,0);
    public GameObject gObject = null;

    public PositionOfHint(Vector2Int _v , GameObject _g) {
        this.v2 = _v;
        this.gObject = _g;
    }

}







