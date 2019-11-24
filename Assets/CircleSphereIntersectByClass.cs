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
   
    // Start is called before the first frame update
    void Start()
    {
        Circle1=new Circle();
        Circle1.PointA=new Vector3(1f, 0, 5f);
        Circle1.PointB=new Vector3(0, 1f, 5f);
        Circle1.PointC=new Vector3(-1f, 0, 5f);
        Circle1.CreateCircle5DusingThreePoints();
        Circle1.UpdateCircleCentre();
        Circle1.FindIcUsingCircle5D();
        Circle1.UpdateCircleRadius();
        Circle1.CreateCirclePlaneObjOnScene();
        Circle1.drawCircleOnPlaneOnScene();
        
        
        Sphere1=new Sphere();
        Sphere1.FindSphere5Dfrom4Points(new Vector3(0, 0, 5f),new Vector3(2f, 0, 5f),new Vector3(1f, 1f, 5f),new Vector3(1f, 0, 6f));
        Sphere1.findSphereCentrefromSphere5D();
        Sphere1.findSphereRadius();
        Sphere1.GenerateGameObjSphere();



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
