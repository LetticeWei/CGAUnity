using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;
using System;
using static ClassCircleTry;


public class CircleSphereIntersectByClass : MonoBehaviour
{
    public static Circle Circle1;
    public static Sphere Sphere1;
    public static Point IntersectPoint1;
    public static Point IntersectPoint2;
    public static CGA.CGA CSIntersection;
   
    // Start is called before the first frame update
    void Start()
    {
        Circle1=new Circle();
        Circle1.PointA=new Vector3(1f, -6f, 5f);
        Circle1.PointB=new Vector3(0, -5f, 5f);
        Circle1.PointC=new Vector3(-1f, -6f, 5f);
        Circle1.initialiseCircle();
        
        Sphere1=new Sphere();
        Sphere1.FindSphere5Dfrom4Points(new Vector3(0, -6f, 5f),new Vector3(2f, -6f, 5f),new Vector3(1f, -5f, 5f),new Vector3(1f, -6f, 6f));
        Sphere1.findSphereCentrefromSphere5D();
        Sphere1.findSphereRadius();
        Sphere1.GenerateGameObjSphere();
        
        IntersectPoint1=new Point();
        IntersectPoint2=new Point();
        IntersectPoint1.SetupPointObject(0,1f,0);
        IntersectPoint2.SetupPointObject(0,1f,0);




    }

    // Update is called once per frame
    void Update()
    {
        Sphere1.UpdateSphereFromObj();
        Circle1.UpdateCircleandCircle5DfromCircleObj();
        CSIntersection=Intersection5D(Circle1.Circle5D,Sphere1.Sphere5D);
        if (pnt_to_scalar_pnt(CSIntersection*CSIntersection)>0){//two point intersection
            IntersectPoint1.Point5D=ExtractPntAfromPntPairs(CSIntersection);
            IntersectPoint1.FindPoint3Dfrom5D();
            IntersectPoint1.UpdatePointObject();
            IntersectPoint1.PointObj.active = true;
            IntersectPoint2.Point5D=ExtractPntBfromPntPairs(CSIntersection);
            IntersectPoint2.FindPoint3Dfrom5D();
            IntersectPoint2.UpdatePointObject();
            IntersectPoint2.PointObj.active = true;
        }
        else{
            IntersectPoint1.PointObj.active = false;
            IntersectPoint2.PointObj.active = false;
        }

    }
}
