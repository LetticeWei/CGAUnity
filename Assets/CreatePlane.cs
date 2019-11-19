using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;
using System;

public class CreatePlane : MonoBehaviour
{
    // Start is called before the first frame update



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


    public int segments = 100;
    public float xradius = 3;
    public float yradius = 3;
    
    public Quaternion FindRotationforPlane(Vector3 n_roof,CGA.CGA currentRoter){
        //rotation from old plane normal (0,0,1) to n_roof
        //rotation angle = the angle between (0,0,1) and (A,B,C)
        
        float scale_of_norm=Mathf.Sqrt(n_roof[0]*n_roof[0]+n_roof[1]*n_roof[1]+n_roof[2]*n_roof[2]);
        float theta= (float) Math.Acos(-1*n_roof[1]/scale_of_norm);
        //rotation plane= the plane spaned by (0,0,1) and (A,B,C)
        var rot_plane=e2^(n_roof[0]*e1+n_roof[1]*e2+n_roof[2]*e3);
        CGA.CGA R =  GenerateRotationRotor2(theta,rot_plane);
        var newR = R*currentRoter;
        var new_Q=RotorToQuat(newR);
        return new_Q;
    }   



    Renderer m_ObjectRenderer;
    LineRenderer line;
    void Start()
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        // define three points to represent the plane.
        var a = new Vector3(1f, 1f, 0);
        var b = new Vector3(0, 1f, 0);
        var c = new Vector3(0, 1f, 1f);
        
        //label the original x & y & z direction
        
        //plane.transform.localScale = new Vector3 (1f, 2f, 3f); 

        //find the Plane, normal and dist.
        var Plane5D=Create5DPlane(a, b, c);
        var dist=GetPlaneDist(Plane5D);
        var n_roof=GetPlaneNormal(Plane5D); //n_roof=(A,B,C)

        CGA.CGA currentRoter=QuatToRotor(plane.transform.rotation);
        var new_Q =FindRotationforPlane(n_roof,currentRoter);
        plane.transform.rotation=new_Q;


        float scale_of_norm=Mathf.Sqrt(n_roof[0]*n_roof[0]+n_roof[1]*n_roof[1]+n_roof[2]*n_roof[2]);
        //define the translation vector
        CGA.CGA d = vector_to_pnt(2*dist*n_roof/scale_of_norm);
        CGA.CGA Rt = GenerateTranslationRotor(d);
        CGA.CGA pos_pnt = up(plane.transform.position.x, 
                            -1*plane.transform.position.y, 
                            plane.transform.position.z);
        var X = Rt*pos_pnt*~Rt;
        var downx = down(X);
        plane.transform.position = pnt_to_vector(downx);

        m_ObjectRenderer = plane.GetComponent<Renderer>();
        //Change the GameObject's Material Color to red
        m_ObjectRenderer.material.color = Color.red;

        //create a cirle by drawing lines
        line = plane.AddComponent<LineRenderer>();
        line = plane.GetComponent<LineRenderer>();
        
        Color c1 = Color.white;
        line.SetVertexCount (segments + 1);
        line.SetColors(c1, c1);

        line.SetWidth(0.3f, 0.3f);
        line.useWorldSpace = false;

        

        float x;
		float y;
		float z;
		float angle = 20f;
		for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
            z = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;
            line.SetPosition (i,new Vector3(x,0,z) );
            angle += (360f / segments);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
