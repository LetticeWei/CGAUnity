using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using System;
using static CGA.CGA;

public class allrotation : MonoBehaviour
{

    public float rotation_speed = 0.1f;
    public float translation_speed =  0.1f;

    public CGA.CGA GenerateRotationRotor(float theta,CGA.CGA ea ,CGA.CGA eb){
        var eab=ea^eb;
        return (float)Math.Cos(theta/2)+ (float)Math.Sin(theta/2)*eab;
    }

    public CGA.CGA QuatToRotor(Quaternion q){
        return q.w + q.x*(e2^e3) + q.y*(e1^e3) + q.z*(e1^e2);
    }

    public Quaternion RotorToQuat(CGA.CGA R){
        return new Quaternion(R[10], R[7], R[6], R[0]);
    }

    public CGA.CGA GenerateTranslationRotor(CGA.CGA mv){
        return 1 + 0.5f*ei*mv;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float theta = rotation_speed;
        CGA.CGA R =  GenerateRotationRotor(theta,e3,e2);
        CGA.CGA currentRoter=QuatToRotor(transform.rotation);
        var newR = R*currentRoter;
        var new_Q=RotorToQuat(newR);
        transform.rotation=new_Q;


        CGA.CGA t = translation_speed*e1;
        CGA.CGA Rt = GenerateTranslationRotor(t);
        CGA.CGA pos_pnt = up(transform.position.x, 
                            transform.position.y, 
                            transform.position.z);
        var X = Rt*pos_pnt*~Rt;
        var downx = down(X);
        transform.position = pnt_to_vector(downx);
        
        //with rotation in position as well
        
        // CGA.CGA R2 =  GenerateRotationRotor(theta,e3,e1);
        // CGA.CGA pos_pnt = up(transform.position.x, 
        //                     transform.position.y, 
        //                     transform.position.z);
        // var X2 = R2*pos_pnt*R2.Conjugate();
        // var downx = down(X2);
        // transform.position = pnt_to_vector(downx);
        

        //Another type of eular rotation

        // CGA.CGA t = 0.01f*e3;
        // CGA.CGA R = GenerateTranslationRotor(t);
        // CGA.CGA pos_pnt = up(transform.rotation.x, 
        //                     transform.rotation.y, 
        //                     transform.rotation.z);
        // var X = R*pos_pnt*~R;
        // var downx = down(X);
        // var eular_vector = pnt_to_vector(downx);
        // transform.rotation=vector_to_euler(eular_vector);


        
        }
}
