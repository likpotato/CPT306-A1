using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public static ItemGenerator instance;

    public List <GameObject> itemPrefabs = new List <GameObject>();

    private int randIndex = -1;
    private int preIndex = -1;

    private float item_timer = 0;

    public GameObject itemParent;

    private void Awake(){
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RestartGame(){
        Destroy(itemParent);
        itemParent = new GameObject("Storage");
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.curStatus == GameStatus.game){
            item_timer += Time.deltaTime;

            if (item_timer >= 2f){
                randIndex = Random.Range(0, itemPrefabs.Count);
                if (randIndex != preIndex){
                    GameObject new_item = Instantiate(itemPrefabs[randIndex], itemParent.transform);
                    new_item.transform.position = Constant.ItemGeneratePoint;
                    preIndex = randIndex;
                    item_timer = 0;
                }
            }
        }
        
    }
}
