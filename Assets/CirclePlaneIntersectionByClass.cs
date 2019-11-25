using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;
using System;
using static ClassCircleTry;

public class CirclePlaneIntersectionByClass : MonoBehaviour
{   
    public static Circle Circle1;
    public static Plane Plane1;
    public static Point PCIntersectPoint1;
    public static Point PCIntersectPoint2;
    public static CGA.CGA PCIntersect;
    // Start is called before the first frame update
    void Start()
    {
        Circle1 =new Circle();
        Circle1.PointA=new Vector3(1f, 0, -15f);
        Circle1.PointB=new Vector3(0, 1f, -15f);
        Circle1.PointC=new Vector3(-1f, 0, -15f);
        Circle1.initialiseCircle();

        Plane1= new Plane();
        Plane1.FindPlane5DbyThreePoints(new Vector3(0, 1f, -15f),new Vector3(1f, 0, -15f),new Vector3(0, -1f, -12f));
        Plane1.GetPlaneNormal();
        Plane1.UpdateGameObjPlane();

        PCIntersectPoint1=new Point();
        PCIntersectPoint1.SetupPointObject(0,1,0);
        PCIntersectPoint2=new Point();
        PCIntersectPoint2.SetupPointObject(0,1,0);

    }

    // Update is called once per frame
    void Update()
    {
        Circle1.UpdateCircleandCircle5DfromCircleObj();
        Plane1.GameObjPlaneToPlane5D();
        PCIntersect=Intersection5D(Plane1.Plane5D, Circle1.Circle5D);
        if (pnt_to_scalar_pnt(PCIntersect*PCIntersect)>0){
            PCIntersectPoint1.Point5D=ExtractPntAfromPntPairs(PCIntersect);
            PCIntersectPoint1.FindPoint3Dfrom5D();
            PCIntersectPoint1.UpdatePointObject();
            PCIntersectPoint1.PointObj.active = true;
            PCIntersectPoint2.Point5D=ExtractPntBfromPntPairs(PCIntersect);
            PCIntersectPoint2.FindPoint3Dfrom5D();
            PCIntersectPoint2.UpdatePointObject();
            PCIntersectPoint2.PointObj.active = true;
        }
        else{
            PCIntersectPoint1.PointObj.active = false;
            PCIntersectPoint2.PointObj.active = false;
        }

        

    }
}
