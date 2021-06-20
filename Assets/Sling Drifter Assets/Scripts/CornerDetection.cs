using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerDetection : MonoBehaviour
{
    //[HideInInspector]
    public GameObject cornerToHook;
    [HideInInspector]
    public bool clockwise;


  

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "corner" && cornerToHook == null)
        {
            
            CornerPoint _cpScript;
            _cpScript = collision.gameObject.GetComponent<CornerPoint>();

            if (!_cpScript._isUsed)
            {
                
                cornerToHook = collision.gameObject;
                if (_cpScript.orientation == CornerPoint.Orientation.Clockwise)
                {                    
                  clockwise = true;                    
                }
                if (_cpScript.orientation == CornerPoint.Orientation.counterClokwise)
                {
                    clockwise = false;                 
                }
            }
        }
    }
}
