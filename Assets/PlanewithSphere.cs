using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;
using System;

public class PlanewithSphere : MonoBehaviour
{
    private static CGA.CGA Sphere5D1;
    private static CGA.CGA Plane5D1;
    private static GameObject SphereObj1;
    private static GameObject PlaneObj1;

    private static GameObject PlaneObjIntersect;

        // use four points to define a sphere
    private static Vector3 pnt_a = new Vector3(1f, 0, 6f);
    private static Vector3 pnt_b = new Vector3(0, 1f, 6f);
    private static Vector3 pnt_c = new Vector3(0, 0, 7f);
    private static Vector3 pnt_d = new Vector3(-1f, 0, 6f);
    // another three points to define a plane
    private static Vector3 pnt_e = new Vector3(1f, 0,6f);
    private static Vector3 pnt_f = new Vector3(0, 1f, 6f);
    private static Vector3 pnt_g = new Vector3(0, 0, 7f);

    Renderer PlaneRendererIntersect;
    LineRenderer line;
    private int segments = 100;

    public GameObject GenerateGameObjSphere(CGA. CGA Sphere5D){
    GameObject SphereObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    Vector3 centre = findCentre(Sphere5D);
    float radius = findSphereRadius(Sphere5D);
    SphereObj.transform.position = centre;
    SphereObj.transform.localScale = new Vector3(1, 1, 1) * radius * 2;
    return SphereObj;
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

    public CGA.CGA GameObjPlaneToPlane5D(GameObject PlaneObj){
        var norm = vector_to_pnt(PlaneObj.transform.up);
        var d = (vector_to_pnt(PlaneObj.transform.position)|norm)[0];
        CGA.CGA Plane5D = !(norm + d*ei);
        return Plane5D;
    }

    // Start is called before the first frame update
    void Start()
    {
        Sphere5D1 = Generate5DSphere(pnt_a, pnt_b, pnt_c, pnt_d);
        SphereObj1 = GenerateGameObjSphere(Sphere5D1);
        Plane5D1 = Create5DPlane(pnt_e, pnt_f, pnt_g);
        PlaneObj1 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        Vector3 new_n_roof1=GetPlaneNormal(Plane5D1);
        UpdateGameObjPlane(PlaneObj1, new_n_roof1, pnt_e);

        PlaneObjIntersect = GameObject.CreatePrimitive(PrimitiveType.Plane);
        UpdateGameObjPlane(PlaneObjIntersect, new_n_roof1, pnt_e);


        PlaneRendererIntersect = PlaneObjIntersect.GetComponent<Renderer>();
        PlaneRendererIntersect.material.color= new Color(1.0f,1.0f,1.0f,0);
        
        // Add a line render 
        line = PlaneObjIntersect.AddComponent<LineRenderer>();
        Color c2 = Color.blue;
        line.SetVertexCount (segments + 1);
        line.SetColors(c2, c2);
        line.SetWidth(0.3f, 0.3f);
        line.useWorldSpace = false;
    }




    // Update is called once per frame
    void Update()
    {
        Vector3 SphereCentre1=SphereObj1.transform.position;
        float SphereRadius1=SphereObj1.transform.localScale.x/2;
        Sphere5D1=Generate5DSpherebyCandRou(SphereCentre1,SphereRadius1);
        Plane5D1 = GameObjPlaneToPlane5D(PlaneObj1);
        

        CGA.CGA CircleofIntersection=CircleBySphereAndPlane(Sphere5D1, Plane5D1);
        CGA.CGA PlaneofIntersection=createIc(CircleofIntersection);
        Vector3 new_n_roof=GetPlaneNormal(PlaneofIntersection);
        Vector3 new_CentrePntOnPlane=findCentre(CircleofIntersection);
        float RadiusofCircleofIntersection= findCircleRadius(CircleofIntersection);
        UpdateGameObjPlane(PlaneObjIntersect, new_n_roof, new_CentrePntOnPlane);

        line = PlaneObjIntersect.GetComponent<LineRenderer>();
        float x;
        float z;
        float angle = 2f;
        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * RadiusofCircleofIntersection;
            z = Mathf.Cos (Mathf.Deg2Rad * angle) * RadiusofCircleofIntersection;
            line.SetPosition (i,new Vector3(x,0,z) );
            angle += (360f / segments);
        }

    }
}
