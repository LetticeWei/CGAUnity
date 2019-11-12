using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;
using System;

public class CreatePlane : MonoBehaviour
{
    // Start is called before the first frame update
    public CGA.CGA QuatToRotor(Quaternion q){
        return q.w + q.x*(e2^e3) + q.y*(e1^e3) + q.z*(e1^e2);
    }
    public CGA.CGA GenerateTranslationRotor(CGA.CGA mv){
        return 1 + 0.5f*ei*mv;
    }


    public Quaternion RotorToQuat(CGA.CGA R){
        return new Quaternion(R[10], R[7], R[6], R[0]);
    }

    public CGA.CGA Create5DPlane(Vector3 a, Vector3 b, Vector3 c)
    {
        var A = up(a.x, a.y, a.z);
        var B = up(b.x, b.y, b.z);
        var C = up(c.x, c.y, c.z);
        var Plane5D = A ^ B ^ C ^ ei;
        return Plane5D;
    }
    public float GetPlaneDist(CGA.CGA Plane5D){
        return  (float) (0.5f)*((!Plane5D.normalized())|eo)[0];
    }
    public CGA.CGA GenerateRotationRotor2(float theta,CGA.CGA eab){
        return (float)Math.Cos(theta/2)+ (float)Math.Sin(theta/2)*eab;
    }

    public Vector3 GetPlaneNormal(CGA.CGA Plane5D){
        var n_roof=(!Plane5D.normalized())-0.5f*((!Plane5D.normalized())|eo)*ei;
        return pnt_to_vector(n_roof);
    }
    

    Renderer m_ObjectRenderer;
    void Start()
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        // define three points to represent the plane.
        var a = new Vector3(1f, 0, 0);
        var b = new Vector3(0, 1f, 0);
        var c = new Vector3(0, 0, 1f);
        
        //label the original x & y & z direction
        plane.transform.localScale = new Vector3 (1f, 2f, 3f); 

        //find the Plane, normal and dist.
        var Plane5D=Create5DPlane(a, b, c);
        var dist=GetPlaneDist(Plane5D);
        var n_roof=GetPlaneNormal(Plane5D); //n_roof=(A,B,C)
        Debug.Log((!Plane5D));
        Debug.Log((!Plane5D.normalized())*(!Plane5D.normalized()));
        Debug.Log(2*dist);        
        Debug.Log(n_roof);

        //rotation from old plane normal (0,0,1) to n_roof
        //rotation angle = the angle between (0,0,1) and (A,B,C)
        float scale_of_norm=Mathf.Sqrt(n_roof[0]*n_roof[0]+n_roof[1]*n_roof[1]+n_roof[2]*n_roof[2]);
        float theta= (float) Math.Acos(n_roof[2]/scale_of_norm);
        //rotation plane= the plane spaned by (0,0,1) and (A,B,C)
        var rot_plane=e3^(n_roof[0]*e1+n_roof[1]*e2+n_roof[2]*e3);
        CGA.CGA R =  GenerateRotationRotor2(theta,rot_plane);
        CGA.CGA currentRoter=QuatToRotor(plane.transform.rotation);
        var newR = R*currentRoter;
        var new_Q=RotorToQuat(newR);
        plane.transform.rotation=new_Q;

        //define the translation vector
        CGA.CGA d = vector_to_pnt(2*dist*n_roof/scale_of_norm);
        CGA.CGA Rt = GenerateTranslationRotor(d);
        CGA.CGA pos_pnt = up(transform.position.x, 
                            transform.position.y, 
                            transform.position.z);
        var X = Rt*pos_pnt*~Rt;
        var downx = down(X);
        plane.transform.position = pnt_to_vector(downx);

        m_ObjectRenderer = plane.GetComponent<Renderer>();
        //Change the GameObject's Material Color to red
        m_ObjectRenderer.material.color = Color.red;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
