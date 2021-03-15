using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class GalleryEntry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Do this when the cursor enters the rect area of this selectable UI object.
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("The cursor entered the selectable UI element.");
    }
}
