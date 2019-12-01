using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using System;
using static CGA.CGA;
public class motion_regulator : MonoBehaviour
{   public static float theta = 0.05f;
    public static float alpha = -0.005f;
    public CGA.CGA GenerateRotationRotor(float theta){
        var e12=e1^e2;
        return (float)Math.Cos(theta/2)+ (float)Math.Sin(theta/2)*e12;
    }
    public CGA.CGA GenerateDilationRotor(float alpha){
        var eio=ei*eo;
        return (float)Math.Cos(alpha)+ (float)Math.Sin(alpha)*eio;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {

        CGA.CGA R =  GenerateRotationRotor(theta);
        CGA.CGA R2 =  GenerateDilationRotor(alpha);
        CGA.CGA pos_pnt = up(transform.position.x, 
                            transform.position.y, 
                            transform.position.z);
        var X = R2*R*pos_pnt*~R*(~R2);
        var downx = down(X);
        transform.position = pnt_to_vector(downx);
    }
}
