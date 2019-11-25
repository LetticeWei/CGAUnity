using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;
using System;

public class SphereandLineIntersect : MonoBehaviour
{
    private static Vector3 pnt_a = new Vector3(10f, 0, 0);
    private static Vector3 pnt_b = new Vector3(9f, 1f, 0);
    private static Vector3 pnt_c = new Vector3(9f, 0, 1f);
    private static Vector3 pnt_d = new Vector3(8f, 0, 0);

    private static Vector3 pnt_e = new Vector3(12.5f, 0, 0);
    private static Vector3 pnt_f = new Vector3(9f, -6f, 0);

    LineRenderer line;
    private static int segments = 1;

    private static CGA.CGA Sphere5D1;
    private static CGA.CGA Line5D1;
    private static GameObject SphereObj1;
    private static GameObject LineVertix1;
    private static GameObject LineVertix2;

    private static GameObject PointAObj;
    private static GameObject PointBObj;

    Renderer PointAObjRenderer;
    Renderer PointBObjRenderer;

    Renderer LineVertix1Renderer;
    Renderer LineVertix2Renderer;
    
    public GameObject GenerateGameObjSphere(CGA. CGA Sphere5D){
        GameObject SphereObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Vector3 centre = findCentre(Sphere5D);
        float radius = findSphereRadius(Sphere5D);
        SphereObj.transform.position = centre;
        SphereObj.transform.localScale = new Vector3(1, 1, 1) * radius * 2;
        return SphereObj;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        Sphere5D1 = Generate5DSphere(pnt_a, pnt_b, pnt_c, pnt_d);
        SphereObj1 = GenerateGameObjSphere(Sphere5D1);
        Line5D1=Create5DLine(pnt_e,pnt_f);
        LineVertix1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        LineVertix2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        LineVertix1.transform.position=pnt_e;
        LineVertix2.transform.position=pnt_f;
        LineVertix1.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f) ;
        LineVertix2.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f) ;
        LineVertix1Renderer = LineVertix1.GetComponent<Renderer>();
        LineVertix1Renderer.material.color= new Color(0,1.0f,0,0);
        LineVertix2Renderer = LineVertix2.GetComponent<Renderer>();
        LineVertix2Renderer.material.color= new Color(0,1.0f,0,0);

        line = LineVertix1.AddComponent<LineRenderer>();
        Color c1 = Color.white;
        line.SetVertexCount (segments + 1);
        line.SetColors(c1, c1);
        line.SetWidth(0.1f, 0.1f);
        line.useWorldSpace = true;
        

        PointAObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        PointAObjRenderer = PointAObj.GetComponent<Renderer>();
        PointAObjRenderer.material.color= new Color(1.0f,0,0,0);
        
        PointBObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        PointBObjRenderer = PointBObj.GetComponent<Renderer>();
        PointBObjRenderer.material.color= new Color(1.0f,0,0,0);

        PointAObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f) ;
        PointBObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f) ;
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 SphereCentre1=SphereObj1.transform.position;
        float SphereRadius1=SphereObj1.transform.localScale.x/2;
        Sphere5D1=Generate5DSpherebyCandRou(SphereCentre1,SphereRadius1);
        Line5D1=Create5DLine(LineVertix1.transform.position ,LineVertix2.transform.position );
        line.SetPosition (0,LineVertix1.transform.position);
        line.SetPosition (1,LineVertix2.transform.position);
        CGA.CGA IntersectPointPair5D = Intersection5D(Sphere5D1, Line5D1);

        if (pnt_to_scalar_pnt(IntersectPointPair5D*IntersectPointPair5D)>0){
            PointAObj.active = true;
            PointBObj.active = true;
            Vector3 IntersectionPntA3D=pnt_to_vector(down(ExtractPntAfromPntPairs(IntersectPointPair5D)));
            Vector3 IntersectionPntB3D=pnt_to_vector(down(ExtractPntBfromPntPairs(IntersectPointPair5D)));
            PointAObj.transform.position = IntersectionPntA3D;
            
            PointBObj.transform.position = IntersectionPntB3D;
            
            }
        else{
            PointAObj.active = false;
            PointBObj.active = false;
        }
        

    }
}
