using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;
using System;

public class InstantiationExample : MonoBehaviour
{
    // create a sphere

    public Vector3 findCentre(CGA.CGA sigma)
    {
        //can be used for both sphere and circle
        var CGAVector = sigma * ei * sigma;
        var CGAVector2 = down(CGAVector);
        return new Vector3(CGAVector2[1], CGAVector2[2], CGAVector2[3]);
    }

    public CGA.CGA Generate5DSphere(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        var A = up(a.x, a.y, a.z);
        var B = up(b.x, b.y, b.z);
        var C = up(c.x, c.y, c.z);
        var D = up(d.x, d.y, d.z);
        var sigma = A ^ B ^ C ^ D;
        return sigma;
    }

    public CGA.CGA createIc(CGA.CGA CircleC)
    {
        var element = ei ^ CircleC;
        var denom = Mathf.Sqrt((element * element)[0] * (-1f));
        var Ic = element * (1 / denom);
        return Ic;
    }

    public float findCircleRadius(CGA.CGA C)
    {
        var Ic=createIc(C);
        var C_star=C*Ic;
        var C_star2 = normalise_pnt_minus_one(C_star);
        float result = (C_star2 * C_star2)[0];
        return Mathf.Sqrt(result);
    }



    public CGA.CGA Create5DCircle(Vector3 a, Vector3 b, Vector3 c)
    {
        var A = up(a.x, a.y, a.z);
        var B = up(b.x, b.y, b.z);
        var C = up(c.x, c.y, c.z);
        var CircleC = A ^ B ^ C;
        return CircleC;
    }

    public float findRadius(CGA.CGA sigma)
    {
        //can be used for both sphere and circle
        var sigmanD = normalise_pnt_minus_one(!sigma);
        float result = (sigmanD * sigmanD)[0];
        return Mathf.Sqrt(result);
    }


    public CGA.CGA CircleByTwoSpheres(CGA.CGA sigma1, CGA.CGA sigma2)
    {
        //get all two blades
        var sigma12 = sigma1 * sigma2;
        var sigma12_2blades = sigma12[6] * (e1 ^ e2) + sigma12[7] * (e1 ^ e3) + sigma12[8] * (e1 ^ e4) + sigma12[9] * (e1 ^ e5) + sigma12[10] * (e2^ e3) 
                        + sigma12[11] * (e2 ^ e4) + sigma12[12] *(e2^e5)+ sigma12[13]*(e3^e4) + sigma12[14]*(e3^e5) + sigma12[15]*(e4^e5);
        return !sigma12_2blades;
    }

    public CGA.CGA GenerateRotationRotor(float theta,CGA.CGA ea ,CGA.CGA eb){
        var eab=ea^eb;
        return (float)Math.Cos(theta/2)+ (float)Math.Sin(theta/2)*eab;
    }
    public CGA.CGA GenerateGameObjSphere(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        var sigma = Generate5DSphere(a, b, c, d);
        var centre = findCentre(sigma);
        var radius = findRadius(sigma);
        sphere.transform.position = centre;
        sphere.transform.localScale = new Vector3(1, 1, 1) * radius * 2;
        return sigma;
    }

    public float meleeRadius = 2f;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        float theta = 0;
        float x = meleeRadius * Mathf.Cos(theta);
        float y = meleeRadius * Mathf.Sin(theta);
        Vector3 pos = transform.position + new Vector3(x, 0, y);
        Vector3 newPos = pos;
        Vector3 lastPos = pos;
        for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
        {
            x = meleeRadius * Mathf.Cos(theta);
            y = meleeRadius * Mathf.Sin(theta);
            newPos = transform.position + new Vector3(x, 0, y);
            Gizmos.DrawLine(pos, newPos);
            pos = newPos;
        }
        Gizmos.DrawLine(pos, lastPos);
    }

    // This script will simply instantiate the objects when the game starts.

    public int segments = 100;

    Renderer m_ObjectRenderer;
    LineRenderer line;

    void Start()
    {
        // use four points to define a sphere
        var a = new Vector3(1f, 1f, 0);
        var b = new Vector3(0, 1f, 1f);
        var c = new Vector3(-1f, 1f, 0);
        var d = new Vector3(0, 5f, 0);
        var e = new Vector3(0, -1f, 0);




        //define a CGA sphere
        var sigma = GenerateGameObjSphere(a, b, c, d);
        var sigma2 = GenerateGameObjSphere(a, b, c, e);

        var CircleIntersect = CircleByTwoSpheres(sigma, sigma2);
        var Ccentre = findCentre(CircleIntersect);
        var Cradius = findCircleRadius(CircleIntersect);

        var PlaneofIntersecCircle=createIc(CircleIntersect);

        var dist=GetPlaneDist(PlaneofIntersecCircle);
        var n_roof=GetPlaneNormal(PlaneofIntersecCircle); //n_roof=(A,B,C)

        Debug.Log("PlaneofIntersecCircle");
        Debug.Log(dist);
        Debug.Log(n_roof);
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

        CGA.CGA currentRoter=QuatToRotor(plane.transform.rotation);
        var new_Q =FindRotationforPlane(n_roof,currentRoter);
        plane.transform.rotation=new_Q;

        float scale_of_norm=Mathf.Sqrt(n_roof[0]*n_roof[0]+n_roof[1]*n_roof[1]+n_roof[2]*n_roof[2]);
        //define the translation vector
        //CGA.CGA d2 = vector_to_pnt(2*dist*n_roof/scale_of_norm);
        CGA.CGA d2 = vector_to_pnt(Ccentre);
        CGA.CGA Rt = GenerateTranslationRotor(d2);
        //this minus one will be changed later maybe
        CGA.CGA pos_pnt = up(plane.transform.position.x, 
                            plane.transform.position.y, 
                            plane.transform.position.z);
        var X = Rt*pos_pnt*~Rt;
        var downx = down(X);
        plane.transform.position = pnt_to_vector(downx);
        
        m_ObjectRenderer = plane.GetComponent<Renderer>();
        //Change the GameObject's Material Color to red
        m_ObjectRenderer.material.color = Color.blue;

        //create a cirle by drawing lines
        line = plane.AddComponent<LineRenderer>();
        line = plane.GetComponent<LineRenderer>();
        
        Color c1 = Color.white;
        line.SetVertexCount (segments + 1);
        line.SetColors(c1, c1);

        line.SetWidth(0.3f, 0.3f);
        line.useWorldSpace = false;

        //draw the circle wrt the plane
        float x;
		float y;
		float z;
		float angle = 20f;
		for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * Cradius;
            z = Mathf.Cos (Mathf.Deg2Rad * angle) * Cradius;
            line.SetPosition (i,new Vector3(x,0,z) );
            angle += (360f / segments);
        }
        


        meleeRadius = Cradius;
        //global position for the circle
        //transform.position=Ccentre;
        //define a cube
        // var : GameObject  cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // cube.transform.position = new Vector3(0, 0.5f, 0);
        // cube.transform.localScale = new Vector3 (1.25f, 1.5f, 1f);

    
    }

    public float GetPlaneDist(CGA.CGA Plane5D){
        return  (float) (0.5f)*((!Plane5D.normalized())|eo)[0];
    }
    public Vector3 GetPlaneNormal(CGA.CGA Plane5D){
        var n_roof=(!Plane5D.normalized())-0.5f*((!Plane5D.normalized())|eo)*ei;
        return pnt_to_vector(n_roof);
    }

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

    public CGA.CGA GenerateRotationRotor2(float theta,CGA.CGA eab){
        return (float)Math.Cos(theta/2)+ (float)Math.Sin(theta/2)*eab;
    }

}

