using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapRing : MonoBehaviour
{
	public Transform innerRing;
	public SpriteRenderer innerRingColor;
	public float curSize = 2.0f;
	public Transform squirrel = null;

    // Start is called before the first frame update
    void Start()
    {
        innerRing = transform.Find("InnerRing");
        innerRingColor = innerRing.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)){
        	RingScale();
        }

        if(!this.gameObject.activeSelf){
        	squirrel = null;
        }
    }

    //shrink the ring
    void RingScale(){
    	curSize -= 1f * Time.deltaTime;
    	if(curSize <= 0){	//reset if too small
    		curSize = 2.0f;
    	}
    	innerRing.transform.localScale = new Vector3(curSize,curSize, 1);

    	if(squirrel != null && squirrel.GetComponent<SquirrelAi>().eating){
    		if(Hit()){
    			innerRingColor.color = new Color(0, 74, 255);
    		}else{
				innerRingColor.color = new Color(255,50,0);
    		}
		}else{
			innerRingColor.color = new Color(255,0,0);
		}
    }

    //if able to snap picture when ring is within the size
    public bool Hit(){
    	return Mathf.Abs(transform.localScale.x - curSize) < 0.35;
    }

    //hover over squirrel
    void OnTriggerEnter2D(Collider2D c){
    	if(c.transform.tag == "squirrel" && squirrel == null){
    		squirrel = c.transform.parent;
    		//Debug.Log("squirrel located");
    	}
    }

    void OnTriggerExit2D(Collider2D c){
    	
    	if(c.transform.parent == squirrel){
    		squirrel = null;
    		//Debug.Log("bye squirrel");
    	}
    }
    
}
