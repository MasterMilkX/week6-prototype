using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Camera mainCamera;

    public GameLogic gameLogic;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 movement;
    //[SerializeField] private Transform bullet;

    [SerializeField] private float moveSpeed;

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

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();

        Vector3 mousePos = Input.mousePosition;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);
        targetPos.z = transform.position.z;

        Vector3 direction = (targetPos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //if (Input.GetMouseButtonDown(0))
            //FireAt(direction);

        transform.position += (Vector3)movement * moveSpeed * Time.deltaTime;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Vector3 cameraPos = transform.position;

        mainCamera.transform.position = new Vector3(cameraPos.x, cameraPos.y-10.0f, -10f);

    }
    
}
