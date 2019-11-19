using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGA;
using static CGA.CGA;

public class cgamovement : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
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
