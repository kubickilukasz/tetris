using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    List<GameObject> figs = new List<GameObject>();

    public float hz = 2f;

    public float number = 4;

    public float speed = 2f;

    public GameObject effect;

    public Vector2 maxX;


    public AnimationCurve fader;
    public AnimationCurve faderDuringPlay;

    float time = 0f;

    float lastPos = 0;

    float maxY = 0;

    float dirX = 0f;


    private void NewFigure(float partOfWindowX)
    {

        AnimationCurve fad = fader;

        if (GameManager.instance.currentPosMenu == GameManager.stateMenu.Ingame)
            fad = faderDuringPlay;

        int rand = Random.Range(0, GameManager.instance.board.dfig.Length);
        DataFigure currentFig = GameManager.instance.board.dfig[rand];

        Block[] bls = new Block[currentFig.v.Length];

        int i = 0;

        GameObject parent = new GameObject();

        float x = Random.Range(Mathf.Clamp01(partOfWindowX - (0.48f / number)), Mathf.Clamp01(partOfWindowX + (0.48f / number)));

        float xTo255 = partOfWindowX * 255f;

        foreach (Vector2 v2 in currentFig.v)
        {
            
            Color32 col = new Color32((byte)(currentFig.col.r *255), (byte)(currentFig.col.g * 255), (byte)(currentFig.col.b * 255), (byte)(fad.Evaluate(partOfWindowX) * 255));
            bls[i] = new Block(v2, col , effect);
            bls[i].obj.transform.SetParent(parent.transform , true);
            bls[i].obj.GetComponent<SpriteRenderer>().sortingOrder = -10;
            i++;
        }

        
        

        lastPos = x;

        Vector3 v3 = GameManager.instance.MainCamera.ViewportToWorldPoint(new Vector3(x, 1f, 1f));


        v3 = new Vector3(v3.x, v3.y +2f , 3);

        parent.transform.position = v3;

        figs.Add(parent);

        
    }


    public void Start()
    {
        maxY = GameManager.instance.MainCamera.ViewportToWorldPoint(new Vector3(0, 0f, 1f)).y - 5f;
        time = hz;
    }


    public void Update()
    {
        time += Time.deltaTime;

        if(hz < time)
        {
            float width = (float)Screen.width;
            float widthPerN = width / number;
            float x = width / number / 2;

            for(int i =0; i <number; i++)
            {
                NewFigure(x/width);
                x += widthPerN;
            }
            
            
            time = 0;
            dirX = Random.Range(maxX.x, maxX.y);
        }

        foreach (GameObject ob in figs)
        {
            if (ob == null)
                continue;

            ob.transform.Translate(new Vector3( dirX , -speed * Time.deltaTime, 0));

            if (ob.transform.position.y < maxY)
            {
                Destroy(ob);
            }
        }


    }



}
