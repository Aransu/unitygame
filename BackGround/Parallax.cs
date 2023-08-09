using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallex : MonoBehaviour
{
    public Transform mainCam;
    public Transform midBackGround;
    public Transform sideBackGround;

    public float length = 39.9749f; 

    // Update is called once per frame
    void Update()
    {
        if(mainCam.position.x > midBackGround.position.x)
        {
            sideBackGround.position = midBackGround.position + Vector3.right * length;
        }
        if(mainCam.position.x < midBackGround.position.x)
        {
            sideBackGround.position = midBackGround.position + Vector3.left * length;
        }
        if(mainCam.position.x > sideBackGround.position.x || mainCam.position.x < sideBackGround.position.x)
        {
            Transform x = midBackGround;
            midBackGround = sideBackGround;
            sideBackGround = x;
        }
    }
}
