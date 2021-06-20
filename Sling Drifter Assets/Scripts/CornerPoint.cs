using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerPoint : MonoBehaviour
{
    
    public enum Orientation {Clockwise, counterClokwise }
    public Orientation orientation;

    public bool _isUsed = false;

    void Start()
    {
        if (orientation == Orientation.Clockwise)
        {
            
        }
        if(orientation == Orientation.counterClokwise)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
