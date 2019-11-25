using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;
using System;

public class PlaneandLineIntersect : MonoBehaviour
{
    private static Vector3 pnt_a = new Vector3(-9f, 0, 14f);
    private static Vector3 pnt_b = new Vector3(-10f, 1f, 14f);
    private static Vector3 pnt_c = new Vector3(-10f, 0, 15f);
    private static Vector3 pnt_e = new Vector3(-10f, 3.5f, 10f);
    private static Vector3 pnt_f = new Vector3(-10f, 0, 4f);

    private static CGA.CGA Plane5D1;
    private static GameObject PlaneObj1;
    private static CGA.CGA Line5D1;
    private static GameObject LineVertix1;
    private static GameObject LineVertix2;
    Renderer LineVertix1Renderer;
    Renderer LineVertix2Renderer;
    LineRenderer line;
    private static GameObject IntersectPointObj;
    Renderer IntersectPointObjRenderer;
    private int segments = 1;

    // Start is called before the first frame update
    public CGA.CGA GameObjPlaneToPlane5D(GameObject PlaneObj){
        var norm = vector_to_pnt(PlaneObj.transform.up);
        var d = (vector_to_pnt(PlaneObj.transform.position)|norm)[0];
        CGA.CGA Plane5D = !(norm + d*ei);
        return Plane5D;
    }
    public Quaternion SetRotParamforPlane(Vector3 n_roof){
        //rotation from old plane normal (0,1,0) to n_roof
        //rotation angle = the angle between (0,1,0) and (A,B,C)
        float scale_of_norm=Mathf.Sqrt(n_roof[0]*n_roof[0]+n_roof[1]*n_roof[1]+n_roof[2]*n_roof[2]);
        float theta= (float) Math.Acos(n_roof[1]/scale_of_norm);
        //rotation plane= the plane spaned by (0,1,0) and (A,B,C)
        var rot_plane=(e2^(n_roof[0]*e1+n_roof[1]*e2+n_roof[2]*e3)).normalized();
        CGA.CGA R = GenerateRotationRotor(theta,rot_plane);
        var new_Q=RotorToQuat(R);
        return new_Q;
    }  
    public void UpdateGameObjPlane(GameObject p, Vector3 new_n_roof, Vector3 new_CentrePntOnPlane){
        p.transform.rotation = SetRotParamforPlane(new_n_roof);
        p.transform.position = new_CentrePntOnPlane;
    }
    void Start()
    {
        Plane5D1 = Create5DPlane(pnt_a, pnt_b, pnt_c);
        PlaneObj1 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        Vector3 new_n_roof1=GetPlaneNormal(Plane5D1);
        UpdateGameObjPlane(PlaneObj1, new_n_roof1, pnt_a);

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

        line = PlaneObj1.AddComponent<LineRenderer>();
        Color c1 = Color.white;
        line.SetVertexCount (segments + 1);
        line.SetColors(c1, c1);
        line.SetWidth(0.1f, 0.1f);
        line.useWorldSpace = true;

        IntersectPointObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        IntersectPointObjRenderer = IntersectPointObj.GetComponent<Renderer>();
        IntersectPointObjRenderer.material.color= new Color(1.0f,0,0,0);
        IntersectPointObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f) ;
    }

    // Update is called once per frame
    void Update()
    {
        Plane5D1 = GameObjPlaneToPlane5D(PlaneObj1);
        Line5D1=Create5DLine(LineVertix1.transform.position ,LineVertix2.transform.position);
        line.SetPosition (0,LineVertix1.transform.position);
        line.SetPosition (1,LineVertix2.transform.position);

        CGA.CGA IntersectPoint5D = Intersection5D(Plane5D1, Line5D1);

        if (pnt_to_scalar_pnt(IntersectPoint5D*IntersectPoint5D)>0){
            IntersectPointObj.active = true;
            Vector3 IntersectionPnt3D;
            if (IntersectPoint5D[15]<0){
                IntersectionPnt3D=pnt_to_vector(ExtractPntfromTwoBlade(IntersectPoint5D));}
            else{IntersectionPnt3D=pnt_to_vector(ExtractPntfromTwoBlade(-1f*IntersectPoint5D));}

            IntersectPointObj.transform.position = IntersectionPnt3D;
            
            }
        else{
            IntersectPointObj.active = false;
        }
        
    }
}
