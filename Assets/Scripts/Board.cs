using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board : MonoBehaviour {


    public static int _width = 10;
    public static int _height = 20;

    public HintFig hintFig;

    private float priv_points = 0;
    private float priv_multiper = 0.8f;
    

    public GameObject blockPrefab;
    public GameObject effectTrail;
    public GameObject bum;
    public GameObject bum2;
    public static GameObject _blockPrefab;
    public static GameObject _effectTrail;
    public static GameObject _bum;
    public static GameObject _bum2;

    public static float _widthB = 0.086f;

    public static Vector2 _downCenter;


    public DataFigure[] dfig;
    public Color background;
    public Color board;
    public Color merge;
    public Color getScore;


    private float timeV = 0f;
    bool isReadyToStep = true;

    Figure fig;

    public static float speedGame = 0.7f;
    public static int step = 0;
    private static float _speedGame = 0.7f;

    private Queue<int> queueFigures = new Queue<int>();

    public List<Block> blocks = new List<Block>();
    private List<Block> boards = new List<Block>();

    public void Awake()
    {
        _blockPrefab = blockPrefab;
        _effectTrail = effectTrail;
        _bum = bum;
        _bum2 = bum2;
    }


    /// <summary>
    /// Funckja tworząca nową grę;
    /// </summary>
    /// <param name="c_d"> Pozycja środkowo dolna gry/ekarnu </param>
    /// <param name="c_u"> Pozycja środkowo górna gry/ekarnu </param>

    public void Init(Vector2 c_d , Vector2 c_u)
    {

        blocks = new List<Block>();
        priv_multiper = 1f - GameManager.instance.multiplierPoint;

        _downCenter = c_d;

        for (int x = (int)(c_d.x - _width); x <= c_d.x  + _width; x++)
        {
            for (int i = (int)c_d.y; i < c_u.y; i++)
            {
                Block temp = new Block(new Vector2(x, i), board);
                temp.SetOrder = -1;
                boards.Add(temp);
            }
        }

    
        _height = (int)(c_u.y - c_d.y); 

        NewFigure();
    }

    public void GameOver()
    {

        GameManager.instance.soundManager.PlayEndGame();

        fig.ChangeColor(merge);

        step = 0;

        foreach (Block bl in fig.Blocks)
        {
            blocks.Add(bl);
        }

        for (int i = 0; i < boards.Count; i++) {
            boards[i].Destroy();
            boards[i] = null;
        }

        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].Destroy();
            blocks[i] = null;
        }


        blocks.RemoveAll(item => item == null);
        boards.RemoveAll(item => item == null);

        fig = null;

        hintFig.Clear();

        GameManager.instance.GameOver();

    }


    private void Update()
    {

        if (GameManager.instance.endGame)
            return;

        timeV += Time.deltaTime;
        if (timeV > _speedGame && isReadyToStep)
        {
            Step();
            timeV = 0;
        }

        if (Input.GetKeyDown("up") || Input.GetKeyDown("space"))
        {
            fig.Rotate(true, blocks);
        }

        if (Input.GetKeyDown("left"))
        {
            fig.Move(new Vector2(-1, 0), blocks);
        }


        if (Input.GetKeyDown("right"))
        {
            fig.Move(new Vector2(1, 0), blocks);
        }

        if (Input.GetKey("down"))

            _speedGame = 0.05f;
        else
            _speedGame = speedGame;
    }

    private void Step()
    {
        
        if(fig == null&&!GameManager.instance.endGame)
        {
            NewFigure();
            return;
        }



        if (!fig.Move(new Vector2(0, -1), blocks))
        {

            //Przenoszenie na board figury

            fig.ChangeColor(merge);

            foreach (Block bl in fig.Blocks)
            {
                blocks.Add(bl);
                bl.Bum();
                Destroy(bl.trail);
            }

            Block[,] temp_bl = new Block[_height, _width * 2 + 1];

            foreach (Block bl in blocks)
            {

                if ((int)(bl.Pos.y - _downCenter.y) >= _height - 2)
                {

                    GameOver();
                    return;
                }

                temp_bl[(int)(bl.Pos.y - _downCenter.y), (int)(bl.Pos.x - _downCenter.x + _width)] = bl;

            }

            NewFigure();

            step++;

            for (int y = 0; y < _height; y++)
            {
                int countRow = 0;
                for (int x = 0; x < _width * 2 + 1; x++)
                {
                    if (temp_bl[y, x] != null)
                    {
                        countRow++;
                    }
                }

                if (countRow == _width * 2 + 1)
                {
                    for (int x = 0; x < _width * 2 + 1; x++)
                    {
                        temp_bl[y, x].Bum2();
                        temp_bl[y, x].Destroy();
                        int index = blocks.IndexOf(temp_bl[y, x]);
                        temp_bl[y, x] = null;
                        blocks[index] = null;
                        priv_points += (GameManager.instance.pointsPerBlock );
                    }

                    priv_multiper += GameManager.instance.multiplierPoint;
                }


            }

            priv_points *= priv_multiper;

            if(priv_points > 0)
                GameManager.instance.soundManager.PlayGetPoints();

            GameManager.instance.points += (int)priv_points;
            priv_points = 0;
            priv_multiper = 1 - GameManager.instance.multiplierPoint;


            blocks.RemoveAll(item => item == null);

            for (int y = 1; y < _height; y++)
            {


                int shift = 0;
                for (int i = y - 1 ; i >= 0; i--)
                {
                    bool isCol = false ;
                    
                    for (int x = 0; x < _width * 2 + 1; x++)
                    {
                        
                        if (temp_bl[i,x]!= null)
                        {
                            isCol = true;
                            break;
                        }


                    }

                    if (!isCol)
                        shift++;
                    else
                        break;

                }


                if (shift > 0)
                {
                

                    for (int x = 0; x < _width * 2 + 1; x++)
                    {
                        if (temp_bl[y, x] != null)
                        {
                            Vector2 new_v2 = new Vector2(x - _width, temp_bl[y, x].Pos.y - shift);
                            temp_bl[y, x].Pos = new_v2;
                            temp_bl[y - shift, x] = temp_bl[y, x];
                            temp_bl[y, x] = null;
                        }
                    }
                }

                
            }

            GameManager.instance.soundManager.PlayMergeBlocks();
            
        }
        else
        {
            
        }
    }

    private void NewFigure()
    {

        int rand = Random.Range(0, dfig.Length);
        queueFigures.Enqueue(rand);

        if (queueFigures.Count == 1)
        {
            rand = Random.Range(0, dfig.Length);
            queueFigures.Enqueue(rand);
        }
       
        hintFig.DrawFigure(dfig[queueFigures.ElementAt(1)]);
        
        DataFigure currentFig = dfig[queueFigures.Dequeue()];

        //sprawdzanie czy jest zajęte miejsce

        Block[] bls = new Block[currentFig.v.Length];

        int i = 0;

        foreach (Vector2 v2 in currentFig.v)
        {
            bls[i] = new Block(v2, currentFig.col);
            i++;
        }

        fig = new Figure(bls, currentFig.center);
        fig.Set(new Vector2(_downCenter.x, _downCenter.y + _height));
    }

    public static bool isCollision(Vector2 np, List<Block> blocks)
    {


        if ((Board._downCenter.x - Board._width > np.x) || (Board._downCenter.x + Board._width < np.x) || (Board._downCenter.y > np.y))
        {
            return true;
        }

        foreach (Block bl in blocks)
        {
            if ((bl.Pos.x == np.x && bl.Pos.y == np.y))
                return true;

        }

        return false;
    }
}


