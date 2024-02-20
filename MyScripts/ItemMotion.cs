using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMotion : MonoBehaviour
{
    private float partSpeed = 2f;
    

    private bool isDebris = false;
    private bool isinLine = false;
    private bool isStop = false;
    private Rigidbody2D rigidbody2D;

    private bool isPlayer = false;

    private int overItem = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        if (tag == "D"){
            isDebris = true;
        }

    }

    private void FixedUpdate(){

        if (GameManager.instance.curStatus == GameStatus.game){
            if (transform.position.y < Constant.D && !isinLine){
            CheckLine();
        }
        if (transform.position.y - Constant.BOTTOM - 0.5f <= 0.04f && !isStop){
            CheckBottom();
        }
        if (!isStop){
           Move(); 
        }
        }
        
        
    }
    private void CheckBottom(){
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        isStop = true;
        if (!isDebris){
            Eliminate();
        }
    }

    private void CheckLine(){

        if (isDebris){
            GameManager.instance.debrisCount += 1;
        }


        Vector3 nowPos = transform.position;
        float x_now = nowPos.x;
        if (x_now < -1.5f){
            transform.position = new Vector3(-2f, nowPos.y, 0);
        }
        else if (x_now >= -1.5f && x_now < -0.5f){
            transform.position = new Vector3(-1f, nowPos.y, 0);
        }
        else if (x_now >= -0.5f && x_now < 0.5f){
            transform.position = new Vector3(0f, nowPos.y, 0);
        }
        else if (x_now >= 0.5f && x_now < 1.5f){
            transform.position = new Vector3(1f, nowPos.y, 0);
        }
        else{
            transform.position = new Vector3(2f, nowPos.y, 0);
        }
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
        isinLine = true;


    }

    private void Move(){
        Vector3 posNow = transform.position;
    

        posNow.y -= partSpeed * Time.deltaTime;
        float px = Mathf.Clamp(posNow.x, Constant.L +0.5f, Constant.R -0.5f);
        float py = Mathf.Clamp(posNow.y, Constant.BOTTOM +0.5f, Constant.TOP -0.5f);

        transform.position = new Vector3(px, py, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.curStatus == GameStatus.game){
           if (isStop && transform.position.y > (Constant.D -0.1f)){
                UIManager.instance.ShowMsg(1);
                GameManager.instance.GameOver();
            }

            if (overItem == 2 && isPlayer){
                UIManager.instance.ShowMsg(3);
                GameManager.instance.GameOver();
            } 
        }
        
    }


    private void OnCollisionEnter2D(Collision2D collision){
        Vector3 col_pos = collision.transform.position;
        Vector3 dir = col_pos - transform.position;
        if (transform.position.y < (Constant.D +0.6f)){
            if (collision.transform.tag != "Player" && transform.position.y - col_pos.y > 0.92f){
                isStop = true;
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                if (!isDebris){
                    Eliminate();
                }
            }
        }
        if (collision.transform.tag == "Player"){
            isPlayer = true;
                
        }
        if (Vector3.Angle(Vector3.down, dir) <= 270f || Vector3.Angle(Vector3.down, dir) >= 90f){
            overItem += 1;
        }
    }

    private void OnCollisionExit2D(Collision2D collision){
        Vector3 col_pos = collision.transform.position;
        Vector3 dir = col_pos - transform.position;
        if (transform.position.y < Constant.D){
            if (collision.transform.tag != "Player" && transform.position.y > col_pos.y){
                isStop = false;
                
            }
            
        }
        if (collision.transform.tag == "Player"){
            isPlayer = false;
        }
        if (Vector3.Angle(Vector3.down, dir) <= 270f || Vector3.Angle(Vector3.down, dir) >= 90f){
            overItem -= 1;
        }

        

    }

    public void Eliminate(){
        Vector2 posNow = transform.position;

        RaycastHit2D[] leftParts = Physics2D.RaycastAll(new Vector2(posNow.x -0.6f, posNow.y), Vector2.left, 1.2f);
        RaycastHit2D[] rightParts = Physics2D.RaycastAll(new Vector2(posNow.x +0.6f, posNow.y), Vector2.right, 1.2f);

        RaycastHit2D[] leftPart = Physics2D.RaycastAll(new Vector2(posNow.x -0.6f, posNow.y), Vector2.left, 0.5f);
        RaycastHit2D[] rightPart = Physics2D.RaycastAll(new Vector2(posNow.x +0.6f, posNow.y), Vector2.right, 0.5f);

        RaycastHit2D[] downParts = Physics2D.RaycastAll(new Vector2(posNow.x, posNow.y -0.6f), Vector2.down, 1.2f);
        RaycastHit2D[] upParts = Physics2D.RaycastAll(new Vector2(posNow.x, posNow.y +0.6f), Vector2.up, 1.2f);

        bool isElim = false;
        if (!isElim) isElim = CheckHorEliminate(leftParts);

        if (!isElim) isElim = CheckMidEliminate(leftPart, rightPart);

        if (!isElim) isElim = CheckHorEliminate(rightParts);

        if (!isElim) isElim = CheckVerEliminate(downParts);
        if (!isElim) isElim = CheckVerEliminate(upParts);
    }

    private bool CheckMidEliminate(RaycastHit2D[] leftPart, RaycastHit2D[] rightPart){
        if (leftPart.Length >0 && rightPart.Length >0){
            if (leftPart[0].collider.tag == tag && rightPart[0].collider.tag == tag){

                Destroy(gameObject);
                Destroy(leftPart[0].collider.gameObject);
                Destroy(rightPart[0].collider.gameObject);

                CheckColor();
                UIManager.instance.S += 10;
                UIManager.instance.RGB += 1;
                UIManager.instance.IPG += 1;

                return true;
            }

            if (leftPart[0].collider.tag != "D" && rightPart[0].collider.tag !="D" && 
            leftPart[0].collider.tag != rightPart[0].collider.tag && 
            leftPart[0].collider.tag != tag && rightPart[0].collider.tag != tag){

                Destroy(gameObject);
                Destroy(leftPart[0].collider.gameObject);
                Destroy(rightPart[0].collider.gameObject);

                CheckColor();
                UIManager.instance.S += 15;
                UIManager.instance.RGB += 1;
                UIManager.instance.IPG += 1;

                return true;

            }
        }
        return false;
    }

    private bool CheckHorEliminate(RaycastHit2D[] Parts){
        if (Parts.Length == 2){

            if (Parts[0].collider.tag == tag && Parts[1].collider.tag == tag){

                Destroy(gameObject);
                Destroy(Parts[0].collider.gameObject);
                Destroy(Parts[1].collider.gameObject);

                CheckColor();
                UIManager.instance.S += 10;
                UIManager.instance.RGB += 1;
                UIManager.instance.IPG += 1;

                return true;
            }

            if (Parts[0].collider.tag != "D" && Parts[1].collider.tag !="D" && 
            Parts[0].collider.tag != Parts[1].collider.tag && 
            Parts[0].collider.tag != tag && Parts[1].collider.tag != tag){

                Destroy(gameObject);
                Destroy(Parts[0].collider.gameObject);
                Destroy(Parts[1].collider.gameObject);

                UIManager.instance.S += 15;
                UIManager.instance.RGB += 1;
                UIManager.instance.IPG += 1;

                return true;
            }

        }
        return false;
    }

    private bool CheckVerEliminate(RaycastHit2D[] Parts){
        if (Parts.Length == 2){

            if (Parts[0].collider.tag == tag && Parts[1].collider.tag == tag){

                Destroy(gameObject);
                Destroy(Parts[0].collider.gameObject);
                Destroy(Parts[1].collider.gameObject);

                CheckColor();
                UIManager.instance.S += 10;
                UIManager.instance.RGB += 1;
                UIManager.instance.IPG += 1;

                return true;
            }

        }
        return false;
    }

    

    private void CheckColor(){
        if (tag == "R"){
            UIManager.instance.R += 1;
        }
        if (tag == "G"){
            UIManager.instance.G += 1;
        }
        if (tag == "B"){
            UIManager.instance.B += 1;
        }
    }
}
