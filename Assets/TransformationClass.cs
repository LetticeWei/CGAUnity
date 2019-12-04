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
    public static Plane Plane3;
    public static Plane Plane4;

    public static GameObject OBJ12;
    public static GameObject OBJ13;
    public static GameObject OBJ14;
    public static GameObject OBJ23;
    public static GameObject OBJ24;
    public static GameObject OBJ34;

    public static LineRenderer LineRenderer12;
    public static LineRenderer LineRenderer13;
    public static LineRenderer LineRenderer14;
    public static LineRenderer LineRenderer23;
    public static LineRenderer LineRenderer24;
    public static LineRenderer LineRenderer34;

    public static CGA.CGA IntersectLine5D12;
    public static CGA.CGA IntersectLine5D13;
    public static CGA.CGA IntersectLine5D14;
    public static CGA.CGA IntersectLine5D23;
    public static CGA.CGA IntersectLine5D24;
    public static CGA.CGA IntersectLine5D34;




    public LineRenderer SetUpLineRenderOnPlaneObj(GameObject OBJ, bool use_world_space){
        LineRenderer thisLineRenderer = OBJ.AddComponent<LineRenderer>();

        Gradient gradient = new Gradient();
        float alpha=1.0f;
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        thisLineRenderer.colorGradient = gradient;

        Color c1 = Color.red;
        int segments=1;
        thisLineRenderer.SetVertexCount (segments + 1);
        thisLineRenderer.SetColors(c1, c1);

        AnimationCurve curve = new AnimationCurve();
        float scale=0.2f;
        curve.AddKey(0, scale);
        curve.AddKey(1, scale);

        thisLineRenderer.widthCurve=curve;
        thisLineRenderer.useWorldSpace = use_world_space;
        return thisLineRenderer;
        }

    // Start is called before the first frame update
    void Start()
    {   
        Vector3 scaleVector=new Vector3(5f,5f,5f);

        Rotation1=new Transform();
        Plane1=new Plane();
        Plane1.PlaneGameObj.transform.position=new Vector3(1f,-2f,-3f);
        Plane1.PlaneGameObj.transform.localScale=scaleVector;
        Plane2=new Plane();
        Plane2.PlaneGameObj.transform.position=new Vector3(1f,2f,3f);
        Plane2.PlaneGameObj.transform.localScale=scaleVector;
        Plane3=new Plane();
        Plane3.PlaneGameObj.transform.position=new Vector3(-3f,-2f,-1f);
        Plane3.PlaneGameObj.transform.localScale=scaleVector;
        Plane4=new Plane();
        Plane4.PlaneGameObj.transform.position=new Vector3(3f,-2f,-1f);
        Plane4.PlaneGameObj.transform.localScale=scaleVector;


        OBJ12=new GameObject();
        OBJ13=new GameObject();
        OBJ14=new GameObject();
        OBJ23=new GameObject();
        OBJ24=new GameObject();
        OBJ34=new GameObject();


        // set up an array to store existing geometies 
        Geometry[] GeoArray1=new Geometry[] {Plane1,Plane2,Plane3,Plane4};
        LineRenderer12=SetUpLineRenderOnPlaneObj(OBJ12,  true);
        LineRenderer13=SetUpLineRenderOnPlaneObj(OBJ13,  true);
        LineRenderer14=SetUpLineRenderOnPlaneObj(OBJ14,  true);
        LineRenderer23=SetUpLineRenderOnPlaneObj(OBJ23,  true);
        LineRenderer24=SetUpLineRenderOnPlaneObj(OBJ24,  true);
        LineRenderer34=SetUpLineRenderOnPlaneObj(OBJ34,  true);

        LineRenderer12 = OBJ12.GetComponent<LineRenderer>();
        LineRenderer13 = OBJ13.GetComponent<LineRenderer>();
        LineRenderer14 = OBJ14.GetComponent<LineRenderer>();
        LineRenderer23 = OBJ23.GetComponent<LineRenderer>();
        LineRenderer24 = OBJ24.GetComponent<LineRenderer>();
        LineRenderer34 = OBJ34.GetComponent<LineRenderer>();
        // LineRenderer[] LineRendererArray1= new LineRenderer[] {LineRenderer12,LineRenderer13,LineRenderer14,LineRenderer23,LineRenderer24,LineRenderer34};
        // CGA.CGA[] IntersectLine5D_Array1= new CGA.CGA[] {IntersectLine5D12,IntersectLine5D13,IntersectLine5D14,IntersectLine5D23,IntersectLine5D24,IntersectLine5D34};
    }

    public static void update_line(CGA.CGA IntersectLine5D, Geometry thisGeometry1, Geometry thisGeometry2, GameObject thisOBJ,LineRenderer line){
        IntersectLine5D = LineByTwoPlanes(thisGeometry1.Plane5D, thisGeometry2.Plane5D);
        var Direction=ExtractDirectLine(IntersectLine5D);
        var PointonLine=ExtractPointOnLine(IntersectLine5D);
        
        // line = thisOBJ.GetComponent<LineRenderer>();
        line.SetPosition (0,PointonLine-20*Direction );
        line.SetPosition (1,PointonLine+20*Direction );
    }

    // Update is called once per frame
    void Update()
    {
        // Rotation1.selfRotation(Plane1,0.1f, e1^e2);
        Rotation1.selfRotation(Plane1,0.005f, e1^e2);
        Rotation1.selfRotation(Plane2,0.004f, e1^e3);
        Rotation1.selfRotation(Plane3,0.003f, e2^e3);
        Rotation1.selfRotation(Plane4,0.007f, e3^e2);

        update_line(IntersectLine5D12, Plane1, Plane2,OBJ12,LineRenderer12);
        update_line(IntersectLine5D13, Plane1, Plane3,OBJ13,LineRenderer13);
        update_line(IntersectLine5D14, Plane1, Plane4,OBJ14,LineRenderer14);
        update_line(IntersectLine5D23, Plane2, Plane3,OBJ23,LineRenderer23);
        update_line(IntersectLine5D24, Plane2, Plane4,OBJ24,LineRenderer24);
        update_line(IntersectLine5D34, Plane3, Plane4,OBJ34,LineRenderer34);


    }
}
