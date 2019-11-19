using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using System;
using static CGA.CGA;
public class cube_rot : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CGA.CGA pos_pnt = up(transform.position.x, 
                            transform.position.y, 
                            transform.position.z);
        
        CGA.CGA S = !(eo - 0.5f*(3*3)*ei);
        var X2 = S*pos_pnt*S;
        var downx = down(X2);
        transform.position = pnt_to_vector(downx);

    }
}
