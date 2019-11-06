using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;

public class InstantiationExample : MonoBehaviour 
{
    // create a sphere

    public Vector3 findCentre(CGA.CGA sigma){
        //can be used for both sphere and circle
        var CGAVector=sigma*ei*sigma;
        var CGAVector2=down(CGAVector);
        return new Vector3 (CGAVector2[1], CGAVector2[2], CGAVector2[3]);
    }

    public CGA.CGA Create5DCircle(Vector3 a,Vector3 b,Vector3 c){
        var A= up (a.x, a.y, a.z);
        var B= up (b.x, b.y, b.z);
        var C= up (c.x, c.y, c.z);
        var CircleC=A^B^C;
        return CircleC;
    }
    public CGA.CGA Generate5DSphere(Vector3 a,Vector3 b,Vector3 c,Vector3 d ){
        var A= up (a.x, a.y, a.z);
        var B= up (b.x, b.y, b.z);
        var C= up (c.x, c.y, c.z);
        var D= up (d.x, d.y, d.z);
        var sigma=A^B^C^D;
        //Debug.Log(A^sigma);
        return sigma;
    }

    public CGA.CGA createIc(CGA.CGA CircleC){
        var element=ei^CircleC;
        var denom=Mathf.Sqrt((element*element)[0]*(-1f));
        var Ic=element*(1/denom);
        return Ic;
    }

    public CGA.CGA createDualofCircle(CGA.CGA CircleC){
        var Ic=createIc( CircleC);
        return CircleC*Ic;
    }

    public float findRadius(CGA.CGA sigma){
        //can be used for both sphere and circle
        var sigmanD=normalise_pnt_minus_one(!sigma);
        float result= (sigmanD*sigmanD)[0];
        return Mathf.Sqrt(result);
    }

    public float findCircleRadius(CGA.CGA CircleC){
        //can be used for both sphere and circle
        var CircleCStar=createDualofCircle(CircleC);
        return Mathf.Sqrt((CircleCStar*CircleCStar)[0]);
    }

    public CGA.CGA CircleByTwoSpheres(CGA.CGA sigma1, CGA.CGA sigma2){
        return !((sigma1*sigma2));
    }

    public float meleeRadius =  2f;
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
    void Start()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // use four points to define a sphere
        var a=new Vector3(1f, 0, 0);
        var b=new Vector3(0, 0, 1f);
        var c=new Vector3(-1f, 0, 0);
        var d=new Vector3(0, 1f, 0);
        var e=new Vector3(0, -2f, 0);
        //define a CGA sphere
        var sigma=Generate5DSphere(a, b,c,d);
        var centre= findCentre(sigma);
        var radius=findRadius(sigma);
        sphere.transform.position = centre;

        //Debug.Log(centre);
        sphere.transform.localScale = new Vector3 (1, 1, 1)*radius*2;
        GameObject sphere2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        var sigma2=Generate5DSphere(a,b,c,e);
        var centre2= findCentre(sigma2);
        var radius2=findRadius(sigma2);
        sphere2.transform.position = centre2;
        sphere2.transform.localScale = new Vector3 (1, 1, 1)*radius2*2;

        var CircleIntersect= CircleByTwoSpheres(sigma,sigma2);
        var Ccentre= findCentre(CircleIntersect);
        var Cradius=findCircleRadius(CircleIntersect);
        // Debug.Log(CircleIntersect);
        // Debug.Log(!CircleIntersect);
        // Debug.Log(Ccentre);
        // Debug.Log(Cradius);

        meleeRadius= Cradius;
        //global position for the circle
        //transform.position=Ccentre;

        //define a cube
        // var : GameObject  cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // cube.transform.position = new Vector3(0, 0.5f, 0);
        // cube.transform.localScale = new Vector3 (1.25f, 1.5f, 1f);
        //Debug.Log(!(e1*e2));

    }
}

