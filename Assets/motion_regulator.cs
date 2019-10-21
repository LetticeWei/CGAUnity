using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using System;
using static CGA.CGA;
public class motion_regulator : MonoBehaviour
{
    public CGA.CGA GenerateRotationRotor(float theta){
        var e12=e1^e2;
        return (float)Math.Cos(theta/2)+ (float)Math.Sin(theta/2)*e12;
    }
    public CGA.CGA normalise_pnt_minus_one(CGA.CGA pnt){
        return (pnt*(-1.0f/(pnt|ei)[0]));
    }
    public CGA.CGA down(CGA.CGA pnt){
        CGA.CGA normed_p = normalise_pnt_minus_one(pnt);
        return normed_p[1]*e1 + normed_p[2]*e2 + normed_p[3]*e3;
    }

    public Vector3 pnt_to_vector(CGA.CGA pnt){
        return new Vector3(pnt[1], pnt[2], pnt[3]);
    }

    public CGA.CGA vector_to_pnt(Vector3 vec){
        return vec.x*e1 + vec.y*e2 + vec.z*e3;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float theta = 0.05f;
        CGA.CGA R =  GenerateRotationRotor(theta);
        CGA.CGA pos_pnt = up(transform.position.x, 
                            transform.position.y, 
                            transform.position.z);
        var X = R*pos_pnt*R.Conjugate();
        var downx = down(X);
        transform.position = pnt_to_vector(downx);
    }
}
