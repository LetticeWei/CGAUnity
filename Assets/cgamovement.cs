using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;

public class cgamovement : MonoBehaviour
{
    public CGA.CGA GenerateTranslationRotor(CGA.CGA mv){
        return 1 + 0.5f*ei*mv;
    }

    public CGA.CGA normalise_pnt_minus_one(CGA.CGA pnt){
        return (pnt*(-1.0f/(pnt|ei)[0]));
    }

    public CGA.CGA down(CGA.CGA pnt){
        CGA.CGA normed_p = normalise_pnt_minus_one(pnt);
        return normed_p[1]*e1 + normed_p[2]*e2 + normed_p[3]*e3;
    }

    public Vector3 pnt_to_vector(CGA.CGA pnt){
        return new Vector3(pnt[1], pnt[2], pnt[3]);
    }

    public CGA.CGA vector_to_pnt(Vector3 vec){
        return vec.x*e1 + vec.y*e2 + vec.z*e3;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(e1*(1 + 10*e2));
        // Debug.Log(R);

        // CGA.CGA point = up(1.0f, 0, 0);
        // CGA.CGA trans_pnt= R*point*~R ;
        // Debug.Log(trans_pnt);

        // Debug.Log(down(50*trans_pnt));
    }

    // Update is called once per frame
    void Update()
    {
        CGA.CGA t = 0.01f*e1;
        CGA.CGA R = GenerateTranslationRotor(t);
        CGA.CGA pos_pnt = up(transform.position.x, 
                            transform.position.y, 
                            transform.position.z);
        var X = R*pos_pnt*~R;
        var downx = down(X);
        transform.position = pnt_to_vector(downx);
    }
}
