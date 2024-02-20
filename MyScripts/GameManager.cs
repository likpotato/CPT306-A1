using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStatus{
    menu, pause, resume, game, over

}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameStatus curStatus = GameStatus.menu;

    public Canvas menuCanvas;
    public Canvas pauseCanvas;
    public Canvas gameCanvas;
    public Canvas overCanvas;

    private bool isPaused = false;

    public int debrisCount = 0;
    public GameObject HelpDoc;
    private bool isShowHelp = false;

    public GameObject AcknowledgeDoc;
    private bool isShowAcknowledge = false;

    private void Awake(){
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        BackToMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if ((curStatus == GameStatus.game || curStatus == GameStatus.pause) && Input.GetKeyDown(KeyCode.Space)){
            if (!isPaused){
                isPaused = true;
                PauseGame();
            }
            else{
                isPaused = false;
                ResumeGame();
            }
            
       
        }
        if (curStatus == GameStatus.game){
            CheckOver();
        }
        
        
        
    }

    private void CheckOver(){
        if (UIManager.instance.S >= 100){
            UIManager.instance.ShowMsg(0);
            GameOver();
        }
        if (debrisCount >5){
            UIManager.instance.ShowMsg(2);
            GameOver();
        }

    }

    public void StartGame(){

        Time.timeScale = 1f;
        debrisCount = 0;
        isPaused = false;
        SwitchGameStatus(GameStatus.game);
    }
    public void PauseGame(){

        Time.timeScale = 0f;
        SwitchGameStatus(GameStatus.pause);
    }
    public void ResumeGame(){

        Time.timeScale = 1f;
        SwitchGameStatus(GameStatus.resume);
    }
    public void BackToMenu(){

       
        SwitchGameStatus(GameStatus.menu);
    }
    public void GameOver(){

        
        SwitchGameStatus(GameStatus.over);
    }

    public void Quit(){
        Application.Quit();
        
    
    }


    public void ShowHelp(){
        if (!isShowHelp){
            HelpDoc.SetActive(true);
            isShowHelp = true;

        }
        else{
            HelpDoc.SetActive(false);
            isShowHelp = false;
        }
        

    }

    public void ShowAcknowledge(){
        if (!isShowAcknowledge){
            AcknowledgeDoc.SetActive(true);
            isShowAcknowledge = true;

        }
        else{
            AcknowledgeDoc.SetActive(false);
            isShowAcknowledge = false;
        }
        

    }

    public void SwitchGameStatus(GameStatus status){
        switch (status)
        {
            case GameStatus.menu:

                menuCanvas.enabled = true;
                pauseCanvas.enabled = false;
                gameCanvas.enabled = false;
                overCanvas.enabled = false;
                break;
            case GameStatus.pause:
                
                menuCanvas.enabled = false;
                pauseCanvas.enabled = true;
                gameCanvas.enabled = false;
                overCanvas.enabled = false;
                break;
            case GameStatus.resume:

                menuCanvas.enabled = false;
                pauseCanvas.enabled = false;
                gameCanvas.enabled = true;
                overCanvas.enabled = false;

                isPaused = false;

                status = GameStatus.game;

                break;
            case GameStatus.game:
                
                menuCanvas.enabled = false;
                pauseCanvas.enabled = false;
                gameCanvas.enabled = true;
                overCanvas.enabled = false;

                isShowHelp = false;
                HelpDoc.SetActive(false);
                isShowAcknowledge = false;
                AcknowledgeDoc.SetActive(false);
                PlayerController.instance.GameBegin();
                ItemGenerator.instance.RestartGame();
                UIManager.instance.GameBegin();
                break;
            case GameStatus.over:

                menuCanvas.enabled = false;
                pauseCanvas.enabled = false;
                gameCanvas.enabled = false;
                overCanvas.enabled = true;
                break;
        }
        curStatus = status;
    }
}
