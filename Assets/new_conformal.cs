using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using System;
using static CGA.CGA;
public class new_conformal : MonoBehaviour
{
    public CGA.CGA SpecialConformalRotorGenerator(float mv){
        return 1 +(- 0.5f*ei*mv);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        float mv = 0.07f;
        CGA.CGA R =  SpecialConformalRotorGenerator(mv);
        CGA.CGA pos_pnt = up(transform.position.x, 
                            transform.position.y, 
                            transform.position.z);
        var X = R*pos_pnt*(~R);
        var downx = down(X);
        transform.position = pnt_to_vector(downx);
    }
}
