using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    
    [Header("OnGame HUD")]    
    public Text Rs;
    public Text Gs;
    public Text Bs;
    public Text RGBs;
    public Text DHs;
    public Text IPGs;
    public Text Ss;
    public Text Ts;

    [Header("GameOver Msg")]
    public List<GameObject> MsgList = new List<GameObject>();

    public int R = 0;
    public int G = 0;
    public int B = 0;
    public int RGB = 0;
    public int DH = 0;
    public int IPG = 0;
    public int S = 0;

    public float T = 0;

    private void Awake(){
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        T += Time.deltaTime;
        Refresh();
    }

    private void Refresh(){
        Rs.text = R.ToString();
        Gs.text = G.ToString();
        Bs.text = B.ToString();
        RGBs.text = RGB.ToString();
        DHs.text = DH.ToString();
        IPGs.text = IPG.ToString();
        Ss.text = S.ToString();

        int min = (int)(T / 60);
        int sec = (int)(T % 60);

        Ts.text = min.ToString() + ":" + sec.ToString();
    }

    public void GameBegin(){
        foreach (GameObject obj in MsgList)
        {
            obj.SetActive(false);
        }
        R = 0;
        G = 0;
        B = 0;
        RGB = 0;
        DH = 0;
        IPG = 0;
        S = 0;
        T = 0;

    }

    public void ShowMsg(int IPcode){
        MsgList[IPcode].SetActive(true);

    }
}
