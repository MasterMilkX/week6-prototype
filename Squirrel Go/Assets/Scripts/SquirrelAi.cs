using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelAi : MonoBehaviour
{
	//features of the squirrel extracted from the CSV dataset
	public string color;				//cinnamon, gray, black
	public string playerBehavior;		//runs from, indifferent, approaches
	public string defaultBehavior;		//running, chasing, foraging, eating, climbing
	public string noise;				//moans, quaas, kuks


	//game specific stats
	private Transform player;				//keep track of player for their position
    private GameObject acorn;
	private Vector2 position;
	public float def_speed = 1.5f;			//movement speed when not running
	public float run_speed = 3.0f;			//max running speed

	private Vector2 curTarget;
	private bool resetingTarget = false;		//in the middle of reseting target
	private string curBehavior = "default";		//current acting behavior of the squirrel

	private SpriteRenderer sprRend;
    public Sprite running;
    public Sprite upright;
    public bool eating = false;


    // Start is called before the first frame update
    void Start()
    {
        SetRandomPos(Random.Range(2.0f,10.0f));
        sprRend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    	SelectBehavior();
        SetSprite();
    }


    //////////    AI BEHAVIORS      ///////////

    //decision-making of the ai 
    void SelectBehavior(){
    	//basic movement ai
        if(curBehavior == "default" || curBehavior == "indifferent"){
        	//goto target
        	if(Vector2.Distance(transform.position, curTarget) > 0.001f){
        		GoToTarget(def_speed);
        	}
        	//goto new position
        	else if(!resetingTarget){
        		resetingTarget = true;
        		//wait some time then reset the target
        		StartCoroutine(NewLocation(Random.Range(0.5f,3f)));
        	}

        	SetDirection(curTarget);
        }
        //run away from player if detected
        else if(curBehavior == "runs from" && player != null){
        	Vector3 dir = transform.position - player.position;
     		transform.Translate(dir.normalized * run_speed * Time.deltaTime);
     		Debug.Log("oh fuck!");

     		SetDirection(dir);
        }
        //approaches player
        else if(curBehavior == "approaches" && player != null){
        	curTarget = player.position;
        	if(Vector2.Distance(transform.position, curTarget) > 2.0f){
        		GoToTarget(def_speed);
        	}
        	Debug.Log("???");

        	SetDirection(curTarget);
        }else if(curBehavior == "get acorn"){
            if(acorn == null){
                curBehavior = "default";
            }
            curTarget = acorn.transform.position;
            if(Vector2.Distance(transform.position, curTarget) > 0.001f){
                GoToTarget(run_speed);
            }
            //goto new position
            else if(!eating){
                StartCoroutine(Eat());
            }
            SetDirection(curTarget);
        }
    }

    //makes squirrel move to a random position within some radial distance away
    void SetRandomPos(float distance){
    	float angle = Random.Range(0.0f,359.0f) * ((float)Mathf.PI/180.0f);
    	curTarget = new Vector2(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle));
    	//Debug.Log("Going to: " + curTarget);
    }

    //goto new location after set wait time
    IEnumerator NewLocation(float seconds){
		yield return new WaitForSeconds(seconds);
		SetRandomPos(Random.Range(3.0f,7.0f));
		resetingTarget = false;
    }

    //moves squirrel to target position
    void GoToTarget(float speed){
		float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, curTarget, step);
    }


    //set direction based on target
    void SetDirection(Vector2 t){
    	if(t.x > transform.position.x && !sprRend.flipX){
    		sprRend.flipX = false;
    	}else if(t.x < transform.position.x && sprRend.flipX){
    		sprRend.flipX = true;
    	}
    }

    void SetSprite(){
        if(eating){
            sprRend.sprite = upright;
        }else{
            sprRend.sprite = running;
        }
    }




    //radar detection
    void OnTriggerEnter2D(Collider2D c){
    	if(c.transform.tag == "Player" && acorn == null && !eating){
    		player = c.transform;
    		curBehavior = playerBehavior;
    	}else if(c.transform.tag == "acorn" && acorn == null && !eating){
            acorn = c.gameObject;
            curBehavior = "get acorn";
        }
    }
    void OnTriggerExit2D(Collider2D c){
    	if(c.transform.tag == "Player"){
    		player = null;
    		curBehavior = "default";
    		StartCoroutine(NewLocation(2.0f));
    	}
    }

    //eat the acorn
    IEnumerator Eat(){
        eating = true;
        Destroy(acorn.gameObject);
        acorn = null;
        yield return new WaitForSeconds(3);
        eating = false;
        curBehavior = "default";
    }
}
