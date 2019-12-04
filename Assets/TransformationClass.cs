using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using System;
using static CGA.CGA;
public class Transform{
    public void selfRotation(Geometry theGeometry, float rotation_speed, CGA.CGA rotation_Plane){
        //this updates the GameObject
        CGA.CGA R =  GenerateRotationRotor(rotation_speed,rotation_Plane);
        CGA.CGA currentRoter=QuatToRotor(theGeometry.PlaneGameObj.transform.rotation);
        theGeometry.PlaneGameObj.transform.rotation=RotorToQuat(R*currentRoter);
        //also need to update other properties, like here I updates the 5D plane
        theGeometry.GameObjPlaneToPlane5D();
        
    }
}


public class TransformationClass : MonoBehaviour
{   
    // public static PlaneTry Plane1;
    public static Transform Rotation1;
    public static Plane Plane1;
    public static Plane Plane2;
    public static CGA.CGA IntersectLine5D;

    // Start is called before the first frame update
    void Start()
    {   
        // let two planes rotate 
        Rotation1=new Transform();
        Plane1=new Plane();
        
        Plane2=new Plane();
        Plane2.PlaneGameObj.transform.position=new Vector3(1f,2f,3f);
        Plane1.SetUpLineRenderOnPlaneObj(true);

    }

    // Update is called once per frame
    void Update()
    {
        // Rotation1.selfRotation(Plane1,0.1f, e1^e2);
        Rotation1.selfRotation(Plane1,0.005f, e1^e2);
        Rotation1.selfRotation(Plane2,0.003f, e2^e3);
        IntersectLine5D = LineByTwoPlanes(Plane1.Plane5D, Plane2.Plane5D);

        var Direction=ExtractDirectLine(IntersectLine5D);
        var PointonLine=ExtractPointOnLine(IntersectLine5D);

        Plane1.line.SetPosition (0,PointonLine-20*Direction );
        Plane1.line.SetPosition (1,PointonLine+20*Direction );




    }
}
