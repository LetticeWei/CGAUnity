using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using System;
using static CGA.CGA;
public class Transform{
    public void selfRotation(Geometry theGeometry, float rotation_speed, CGA.CGA rotation_Plane){
    CGA.CGA R =  GenerateRotationRotor(rotation_speed,rotation_Plane);
    CGA.CGA currentRoter=QuatToRotor(theGeometry.PlaneGameObj.transform.rotation);
    theGeometry.PlaneGameObj.transform.rotation=RotorToQuat(R*currentRoter);
    }

}

public class TransformationClass : MonoBehaviour
{   
    // public static PlaneTry Plane1;
    public static Transform Rotation1;

    public static Plane Plane2;

    // Start is called before the first frame update
    void Start()
    {   
        // Plane1=new PlaneTry(13f);
        Rotation1=new Transform();
        Plane2=new Plane();

    }

    // Update is called once per frame
    void Update()
    {
        // Rotation1.selfRotation(Plane1,0.1f, e1^e2);
        Rotation1.selfRotation(Plane2,0.1f, e2^e3);
    }
}
