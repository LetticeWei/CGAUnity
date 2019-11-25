using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;
using System;
using static ClassCircleTry;
public class PlaneLineIntersectUsingClass : MonoBehaviour
{
    public static Plane Plane1;
    public static Line Line1;
    public static Point Intersection1; 
    private static Vector3 pnt_c = new Vector3(10f, 0, 14f);
    private static Vector3 pnt_d = new Vector3(10f, 1f, 14f);
    private static Vector3 pnt_a = new Vector3(10f, 0, 15f);
    private static Vector3 pnt_b = new Vector3(10f, 3.5f, 10f);
    private static Vector3 pnt_e = new Vector3(10f, 0, 4f);
    // Start is called before the first frame update
    void Start()
    {   //set up the plane
        Plane1=new Plane();
        Plane1.FindPlane5DbyThreePoints(pnt_a,pnt_b,pnt_c);
        Plane1.GetPlaneNormal();
        Plane1.UpdateGameObjPlane();
        
        //set up the line
        Line1=new Line();
        Line1.SetUpVertices3Dand5D(pnt_d,pnt_e);
        Line1.SetupVertexObj();
        Line1.UpdateVerticesFromObject();
        Line1.UpdateLine5DfromVertices();
        Line1.SetUpLineRenderer();
        //set up the intersection
        Intersection1=new Point();
        Intersection1.SetupPointObject(0,1,0);
        Intersection1.PointObj.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        Plane1.GameObjPlaneToPlane5D();
        Line1.UpdateVerticesFromObject();
        Line1.UpdateLine5DfromVertices();
        Line1.UpdateLineObj();
        Intersection1.Point5D=Intersection5D(Plane1.Plane5D, Line1.Line5D);
        if (pnt_to_scalar_pnt(Intersection1.Point5D*Intersection1.Point5D)>0.3f){ 
            //0.3, not 0 to allow digitisation error
            Vector3 IntersectionPnt3D;
            Intersection1.PointObj.active = true;
            if (Intersection1.Point5D[15]<0){
            IntersectionPnt3D=pnt_to_vector(ExtractPntfromTwoBlade(Intersection1.Point5D));}
            else{IntersectionPnt3D=pnt_to_vector(ExtractPntfromTwoBlade(-1f*Intersection1.Point5D));}
            Intersection1.PointObj.transform.position = IntersectionPnt3D;
            }
        else {

            Intersection1.PointObj.active = false;}

    }
}
