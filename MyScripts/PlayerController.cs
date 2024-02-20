using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed = 3f;



    
    private void Awake(){
        instance = this;
    
    }
    //Start is called before the first frame update
    void Start(){

    }

    public void GameBegin(){
        transform.position = Constant.RebornPoint;
    }

    private void FixedUpdate(){
        if (GameManager.instance.curStatus == GameStatus.game){
            Move();
        }
        
    }

    //Update is called once per frame
    void Update(){

    }

    private void Move(){
        float horMove = Input.GetAxis("Horizontal");
        float verMove = Input.GetAxis("Vertical");

        Vector3 posNow = transform.position;
        posNow.x += horMove * moveSpeed * Time.deltaTime;
        posNow.y += verMove * moveSpeed * Time.deltaTime;

        float px = Mathf.Clamp(posNow.x, Constant.L +0.75f, Constant.R -0.75f);
        float py = Mathf.Clamp(posNow.y, Constant.D +0.75f, Constant.U -0.75f);
        
        transform.position = new Vector3(px, py, 0);

    }
    private void OnCollisionEnter2D(Collision2D collision){
        Vector3 dir = collision.transform.position - transform.position;
        if (collision.transform.tag == "D"){
            if (Vector3.Angle(Vector3.up, dir) <= 22.5f || Vector3.Angle(Vector3.up, dir) >= 337.5f){
                
                UIManager.instance.DH += 1;
                UIManager.instance.S += 5;
                Destroy(collision.gameObject);
            }
        }
        
    }

    

}
