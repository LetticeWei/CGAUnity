using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;
using System;

public class InstantiationExample : MonoBehaviour
{


    private static CGA.CGA Sphere5D1;
    private static CGA.CGA Sphere5D2;
    private static GameObject SphereObj1;
    private static GameObject SphereObj2;
    private static GameObject PlaneObj1;
    private static GameObject PlaneObj2;

    // use four points to define a sphere
    private static Vector3 pnt_a = new Vector3(1f, 0, 0);
    private static Vector3 pnt_b = new Vector3(0, 1f, 0);
    private static Vector3 pnt_c = new Vector3(0, 0, 1f);
    private static Vector3 pnt_d = new Vector3(-1f, 0, 0);
    // another four points to define another sphere
    private static Vector3 pnt_e = new Vector3(1f, 0, 0.5f);
    private static Vector3 pnt_f = new Vector3(0, 1f, 0.5f);
    private static Vector3 pnt_g = new Vector3(0, 0, 3f);
    private static Vector3 pnt_h = new Vector3(-1f, 0, 0.5f);

    Renderer PlaneRenderer1;
    Renderer PlaneRenderer2;
    LineRenderer line;
    private int segments = 100;


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

    public GameObject GenerateGameObjSphere(CGA. CGA Sphere5D){
        GameObject SphereObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Vector3 centre = findCentre(Sphere5D);
        float radius = findSphereRadius(Sphere5D);
        SphereObj.transform.position = centre;
        SphereObj.transform.localScale = new Vector3(1, 1, 1) * radius * 2;
        return SphereObj;
    }


    public void UpdateGameObjPlane(GameObject p, Vector3 new_n_roof, Vector3 new_CentrePntOnPlane){
        p.transform.rotation = SetRotParamforPlane(new_n_roof);
        p.transform.position = new_CentrePntOnPlane;
    }




    void Start()
    {

        //define a CGA sphere
        Sphere5D1 = Generate5DSphere(pnt_a, pnt_b, pnt_c, pnt_d);
        Sphere5D2 = Generate5DSphere(pnt_e, pnt_f, pnt_g, pnt_h);
        SphereObj1 = GenerateGameObjSphere(Sphere5D1);
        SphereObj2 = GenerateGameObjSphere(Sphere5D2);

        PlaneObj1 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        PlaneObj2 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //Change the GameObject's Material Color to red
        PlaneRenderer1 = PlaneObj1.GetComponent<Renderer>();
        PlaneRenderer2 = PlaneObj2.GetComponent<Renderer>();
        //PlaneRenderer.material.color = Color.red;
        PlaneRenderer1.material.color= new Color(1.0f,1.0f,1.0f,0);
        PlaneRenderer2.material.color= PlaneRenderer1.material.color;

        // Add a line render 
        line = PlaneObj1.AddComponent<LineRenderer>();
        Color c1 = Color.white;
        line.SetVertexCount (segments + 1);
        line.SetColors(c1, c1);
        line.SetWidth(0.3f, 0.3f);
        line.useWorldSpace = false;


    }
    
    void Update()
    {   //will include dilation later

        Vector3 SphereCentre1=SphereObj1.transform.position;
        Vector3 SphereCentre2=SphereObj2.transform.position;
        float SphereRadius1=SphereObj1.transform.localScale.x/2;
        float SphereRadius2=SphereObj2.transform.localScale.x/2;
        Sphere5D1=Generate5DSpherebyCandRou(SphereCentre1,SphereRadius1);
        Sphere5D2=Generate5DSpherebyCandRou(SphereCentre2,SphereRadius2);

        CGA.CGA CircleofIntersection=CircleByTwoSpheres(Sphere5D1, Sphere5D2);
        CGA.CGA PlaneofIntersection=createIc(CircleofIntersection);
        Vector3 new_n_roof=GetPlaneNormal(PlaneofIntersection);
        Vector3 new_CentrePntOnPlane=findCentre(CircleofIntersection);
        // Destroy(PlaneObj1);
        float RadiusofCircleofIntersection= findCircleRadius(CircleofIntersection);
        // if (RadiusofCircleofIntersection>0){
            UpdateGameObjPlane(PlaneObj1, new_n_roof, new_CentrePntOnPlane);
            UpdateGameObjPlane(PlaneObj2, -new_n_roof, new_CentrePntOnPlane);

            //display the circle of intersection on the plane
            line = PlaneObj1.GetComponent<LineRenderer>();
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

