using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform[] camera_points;
    public GameObject esquelet;
    public GameObject cos;
    public GameObject cor;
    private bool esquelet_visible=true;
    private bool cos_visible=true;
    private bool cor_visible=true;
    private int pov=0;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            pov++;
            if(pov>=camera_points.Length){
                pov=0;
            }
        }
        transform.position=camera_points[pov].position;
        transform.rotation=camera_points[pov].rotation;
        if(Input.GetKeyDown(KeyCode.S)){
            esquelet_visible=!esquelet_visible;
        }
        if(Input.GetKeyDown(KeyCode.B)){
            cos_visible=!cos_visible;
        }
        if(Input.GetKeyDown(KeyCode.H)){
            cor_visible=!cor_visible;
        }
        esquelet.SetActive(esquelet_visible);
        cos.SetActive(cos_visible);
        cor.SetActive(cor_visible);
    }
}
