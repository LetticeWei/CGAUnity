using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;
using System;
using static ClassCircleTry;

public class CircleCircleIntersectByClass : MonoBehaviour
{
    public static Circle Circle1;
    public static Circle Circle2;
    public static Point CCInersectPoint1;
    public static Point CCInersectPoint2;
    public static CGA.CGA CCInersectPoint5D;
    public static float valve =0.0001f;

    // Start is called before the first frame update
    void Start()
    {   // if the circles lie on the same plane then this may not work!!!!
        Circle1 =new Circle();
        Circle1.PointA=new Vector3(1f, 0, -5f);
        Circle1.PointB=new Vector3(0, 1f, -5f);
        Circle1.PointC=new Vector3(-1f, 0, -5f);
        Circle1.initialiseCircle();

        Circle2 =new Circle();
        Circle2.PointA=new Vector3(1.5f, 0, -5f);
        Circle2.PointB=new Vector3(0, 1.5f, -6f);
        Circle2.PointC=new Vector3(-0.5f, 0, -5f);
        Circle2.initialiseCircle();

        CCInersectPoint1=new Point();
        CCInersectPoint1.SetupPointObject(0,1,0);
        CCInersectPoint1.PointObj.active = false;
        CCInersectPoint2=new Point();
        CCInersectPoint2.SetupPointObject(0,1,0);
        CCInersectPoint2.PointObj.active = false;

    }

    // Update is called once per frame
    void Update()
    {   Circle1.UpdateCircleandCircle5DfromCircleObj();
        Circle2.UpdateCircleandCircle5DfromCircleObj();
        CCInersectPoint5D=(!((!Circle1.Circle5D)^(!Circle2.Circle5D)));

        var mag=CCInersectPoint5D[1]*CCInersectPoint5D[1]+CCInersectPoint5D[2]*CCInersectPoint5D[2]
        +CCInersectPoint5D[3]*CCInersectPoint5D[3]+CCInersectPoint5D[4]*CCInersectPoint5D[4]
        +CCInersectPoint5D[5]*CCInersectPoint5D[5];

        Debug.Log(mag);
        Debug.Log(CCInersectPoint5D*CCInersectPoint5D);

        if (mag<valve){
            //intersect one circle with the plane that another lies on.
            Debug.Log(2);
            CGA.CGA IntersectPointPair5D = Intersection5D(Circle1.Ic, Circle2.Circle5D);            
            CCInersectPoint1.PointObj.active = true;
            CCInersectPoint2.PointObj.active = true;
            CCInersectPoint1.Point5D=ExtractPntAfromPntPairs(IntersectPointPair5D);
            CCInersectPoint2.Point5D=ExtractPntBfromPntPairs(IntersectPointPair5D);
            CCInersectPoint1.FindPoint3Dfrom5D();
            CCInersectPoint2.FindPoint3Dfrom5D();
            CCInersectPoint1.UpdatePointObject();
            CCInersectPoint2.UpdatePointObject();
        }
        else if (Math.Abs(pnt_to_scalar_pnt(CCInersectPoint5D*CCInersectPoint5D))<valve/2){
            Debug.Log(1);
            CCInersectPoint1.PointObj.active = true;
            CCInersectPoint2.PointObj.active = false;
            CCInersectPoint1.Point5D=CCInersectPoint5D;
            CCInersectPoint1.FindPoint3Dfrom5D();
            CCInersectPoint1.UpdatePointObject();
        }
        else{Debug.Log(0);
            CCInersectPoint1.PointObj.active = false;
            CCInersectPoint2.PointObj.active = false;
        }



    }
}
