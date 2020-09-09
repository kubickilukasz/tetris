using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoresManager : MonoBehaviour{


    const string URL_d = "http://dreamlo.com/lb/";
    const string public_code = "5b544f61191a8a0bcc6d93b7";
    const string private_code = "6vpnJKI7GkeJsiaT6ItBXAaiK8N3mvn0iu8Bt6NE7AXA";

    public Dictionary<string, int> globalScores = new Dictionary<string, int>();

    public void SaveScore(string name , int score)
    {

        int lastScore = PlayerPrefs.HasKey("score") ? PlayerPrefs.GetInt("score") : 0;
        if (lastScore < score)
            PlayerPrefs.SetInt("score" , score);

        StartCoroutine(SaveScoreURL(name, score));

        PlayerPrefs.Save();

    }


    public IEnumerator DeleteAll()
    {

        WWW www = new WWW(URL_d + private_code + "/clear/");

        yield return www;

        PlayerPrefs.DeleteAll();

    }


    public void GetScore(bool changeMenu = false) // ?
    {
        StartCoroutine(GetScores(changeMenu));
        int yr =  PlayerPrefs.HasKey("score") ? PlayerPrefs.GetInt("score") : 0;

        GameManager.instance.SetTextScore(yr);
        
    }

    IEnumerator GetScores(bool changeMenu)
    {
        WWW www = new WWW(URL_d + private_code + "/pipe/10" );

        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            globalScores = new Dictionary<string, int>();
            string[] output = WWW.UnEscapeURL(www.text).Split('\n');
            string name = ""; int score = 0;
            foreach (string row in output)
            {

                if (string.IsNullOrEmpty(row))
                    continue;

                string[] cells = row.Split('|');
                name = cells[0];
                score = int.Parse(cells[1]);

                globalScores.Add(name, score);

            }

            int i = 0;

            foreach (var tx in GameManager.instance.tx_scores)
            {
                tx.text = "";
            }


            foreach (KeyValuePair<string, int> pl in globalScores)
            {

                if (i >= 7)
                    break;

                if (GameManager.instance.tx_scores[i] == null)
                    break;

                GameManager.instance.tx_scores[i].text = (i + 1).ToString() + ". <" + pl.Key + "> " + pl.Value;

                i++;
            }


            GetBestScore(changeMenu);


        }
        else
        {
            Debug.Log(www.error);
            if(changeMenu)
                GameManager.instance.currentPosMenu = GameManager.stateMenu.Main;

        }

        
    }


    public void TestConnection()
    {
        StartCoroutine(TestConnect());
    }

    IEnumerator TestConnect()
    {
        WWW www = new WWW(URL_d + private_code);

        yield return www;


        if (string.IsNullOrEmpty(www.error))
            GameManager.instance.isConnection = true;
        else
            GameManager.instance.isConnection = false;

    }


    public void GetBestScore(bool changeMenu = false)
    {
        int max = 0;
        string text = "";
        foreach (KeyValuePair<string, int> row in globalScores)
        {
            if(row.Value > max)
            {
                max = row.Value;
                text = max+" <"+ row.Key + ">";

            }
        }


        GameManager.instance.SetTextScore(text);

        if (changeMenu)
            GameManager.instance.currentPosMenu = GameManager.stateMenu.Scores;

    }


    IEnumerator SaveScoreURL(string name, int score)
    {

        WWW www = new WWW(URL_d + private_code + "/add/" + WWW.EscapeURL(name) + "/" + score);
        yield return www;

        GameManager.instance.currentPosMenu = GameManager.stateMenu.Main;

        /*if (string.IsNullOrEmpty(www.error))
            Debug.Log("Udalo");*/
   
    }


}
