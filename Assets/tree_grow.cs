using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using System;
using static CGA.CGA;

public class tree_grow : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = 0.95f;
        CGA.CGA R2 =  GenerateDilationRotor(alpha);
        CGA.CGA pos_pnt = up(transform.localScale .x, 
                            transform.localScale .y, 
                            transform.localScale .z);
        var X = R2*pos_pnt*(~R2);
        var downx = down(X);
        transform.localScale  = pnt_to_vector(downx);
    }
}
