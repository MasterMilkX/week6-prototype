using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Camera mainCamera;

    public GameLogic gameLogic;

    //player movement
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 movement;
    [SerializeField] private float moveSpeed;
    public float camOffY = 0.0f;        //offset for the camera

    //acorn and camera movement
    public GameObject acornPrefab;

    void Awake()
    {
        moveSpeed = 6f;
        rb = GetComponent<Rigidbody2D>();
        gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (gameLogic.currState != GameLogic.GameState.GAME_RUNNING)
            //return;


        //player physical movement
        movement.x = 0;
        movement.y = 0;
        if(Input.GetKey("a")){
            movement.x = -1;
        }else if(Input.GetKey("d")){
            movement.x = 1;
        }

        if(Input.GetKey("w")){
            movement.y = 1;
        }else if(Input.GetKey("s")){
            movement.y = -1;
        }



        Vector2 mousePos = Input.mousePosition;
        Vector2 targetPos = mainCamera.ScreenToWorldPoint(mousePos);

        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;

        if (Input.GetMouseButtonDown(0))
            ThrowAcorn(direction);

        Vector2 changePos = movement * moveSpeed * Time.deltaTime;

        transform.position += new Vector3(changePos.x, changePos.y, 0);
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Vector2 cameraPos = transform.position;

        mainCamera.transform.position = new Vector3(cameraPos.x, cameraPos.y+camOffY,-10.0f);

    }
    

    void ThrowAcorn(Vector2 target){
        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        GameObject p = Instantiate(acornPrefab, new Vector2(transform.position.x,transform.position.y) + 1f * target, Quaternion.identity);
        float thrust = Vector2.Distance(transform.position,target)*50.0f;
        p.GetComponent<Rigidbody2D>().AddForce(p.transform.forward*thrust,ForceMode2D.Impulse);
        gameLogic.DecreaseAcorns();
    }
}
