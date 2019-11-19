using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;
using System;

public class InstantiationExample : MonoBehaviour
{

    public Quaternion UpdateRotParamforPlane(Vector3 n_roof,CGA.CGA currentRoter){
        //rotation from old plane normal (0,1,0) to n_roof
        //rotation angle = the angle between (0,1,0) and (A,B,C)
        float scale_of_norm=Mathf.Sqrt(n_roof[0]*n_roof[0]+n_roof[1]*n_roof[1]+n_roof[2]*n_roof[2]);
        float theta= (float) Math.Acos(n_roof[1]/scale_of_norm);
        //rotation plane= the plane spaned by (0,1,0) and (A,B,C)
        var rot_plane=e2^(n_roof[0]*e1+n_roof[1]*e2+n_roof[2]*e3);
        CGA.CGA R =  GenerateRotationRotor(theta,rot_plane);
        var newR = R*currentRoter;
        var new_Q=RotorToQuat(newR);
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


    public GameObject UpdateGameObjPlane(Vector3 new_n_roof, Vector3 new_CentrePntOnPlane){
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        CGA.CGA currentRoter = QuatToRotor(plane.transform.rotation);
        plane.transform.rotation = UpdateRotParamforPlane(new_n_roof,currentRoter);
        CGA.CGA dist_tomove = vector_to_pnt(new_CentrePntOnPlane);
        CGA.CGA RT_plane_centr_diff = GenerateTranslationRotor(dist_tomove);
        CGA.CGA Plane_pos_pnt = up(plane.transform.position.x,plane.transform.position.y,plane.transform.position.z);
        var PlanePosition5D = RT_plane_centr_diff*Plane_pos_pnt*~RT_plane_centr_diff;
        var PlanePosition3DCGA = down(PlanePosition5D);
        plane.transform.position = pnt_to_vector(PlanePosition3DCGA);
        return plane;
    }


    private static CGA.CGA Sphere5D1;
    private static CGA.CGA Sphere5D2;
    private static GameObject SphereObj1;
    private static GameObject SphereObj2;
    private static GameObject PlaneObj1;

    private static Vector3 old_position1;
    private static Vector3 old_position2;

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
    private static Vector3 new_CentrePntOnPlane;

    Renderer PlaneRenderer;

    void Start()
    {

        //define a CGA sphere
        Sphere5D1 =  Generate5DSphere(pnt_a, pnt_b, pnt_c, pnt_d);
        Sphere5D2 =  Generate5DSphere(pnt_e, pnt_f, pnt_g, pnt_h);
        SphereObj1=GenerateGameObjSphere(Sphere5D1);
        SphereObj2=GenerateGameObjSphere(Sphere5D2);
        
        old_position1=SphereObj1.transform.position;
        old_position2=SphereObj2.transform.position;

    }

    void Update()
    {   //will include dilation later
        
        Vector3 dist_moved_by_hand1=SphereObj1.transform.position-old_position1;
        Vector3 dist_moved_by_hand2=SphereObj2.transform.position-old_position2;

        CGA.CGA dist_tomove1 = vector_to_pnt(dist_moved_by_hand1);
        CGA.CGA dist_tomove2 = vector_to_pnt(dist_moved_by_hand2);
        CGA.CGA RTforSphere1 = GenerateTranslationRotor(dist_tomove1);
        CGA.CGA RTforSphere2 = GenerateTranslationRotor(dist_tomove2);

        Sphere5D1=RTforSphere1*Sphere5D1*~RTforSphere1;
        Sphere5D2=RTforSphere2*Sphere5D2*~RTforSphere2;

        CGA.CGA CircleofIntersection=CircleByTwoSpheres(Sphere5D1, Sphere5D2);
        CGA.CGA PlaneofIntersection=createIc(CircleofIntersection);
        Vector3 new_n_roof=GetPlaneNormal(PlaneofIntersection);
        new_CentrePntOnPlane=findCentre(CircleofIntersection);
        Destroy(PlaneObj1);
        PlaneObj1=UpdateGameObjPlane(new_n_roof, new_CentrePntOnPlane);
        //Change the GameObject's Material Color to red
        PlaneRenderer = PlaneObj1.GetComponent<Renderer>();
        PlaneRenderer.material.color = Color.red;

        old_position1=SphereObj1.transform.position;
        old_position2=SphereObj2.transform.position;
    }
}

