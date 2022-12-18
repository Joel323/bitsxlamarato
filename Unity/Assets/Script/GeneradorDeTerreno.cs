using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]
public class GeneradorDeTerreno : MonoBehaviour
{
  Vector3[] vertex; // para los vértices
  Mesh mesh; // nuestro modelo 3D
  int[] triangles;
  public int ySize=100;
  private Vector3[] verticesUnity;
  private Vector3[] verticesCartesianas;
  public Transform baseReferencia;
  public float scaleFactor=0.15f;
  public float exponentialFactor=0.07f;

  private List<float[]> capes;
  private int capa;
  public Text text_capa;
  private GameObject[] points2D;
  public GameObject puntoReferencia;
  public GameObject planeIndicator;
  public GameObject canva;
  private bool modoAnalisis=true;
  private int fileInfo_num;
  private int file_id=1;

  public Slider filterSlider;
  public Slider scaleSlider;
  public Text filterText;
  public Text scalerText;
  public Text mediaText;

  void Start(){
    points2D = GameObject.FindGameObjectsWithTag("Canvas");
    MeshPrepare(file_id);
  }
  void MeshPrepare(int num_txt){
    mesh=new Mesh();
    capes=new List<float[]>();
    var info = new DirectoryInfo("TXT");
    var fileInfo = info.GetFiles();
    fileInfo_num=fileInfo.Length;
    TextReader Leer=new StreamReader(fileInfo[num_txt].ToString());
    Debug.Log(fileInfo[num_txt]);
    GetComponent<MeshFilter>().mesh=mesh;
    int i=0;
    verticesCartesianas=new Vector3[36*ySize];
    float[] lastcapa=new float[37];
    string[] last_radis=new string[40];
    for(int y=0;y<ySize;y++){
      string[] radis=Leer.ReadLine().Split('_');
      string[] actual_radis=new string[radis.Length];
      if(float.Parse(radis[37])>(3000)){
        Debug.Log("Se paso");
        actual_radis=last_radis;
      }else{
        Debug.Log("No se paso");
        actual_radis=radis;
        last_radis=actual_radis;
      }
      float[] capa=new float[37];
      // if(float.Parse(radis[37])>4000){
      //   radis=last_radis;
      // }else{
      //   Debug.Log(y);
      //   last_radis=radis;
      // }
      
      for(int j=1;j<37;j++){
        //float radi=Mathf.Log(float.Parse(radis[j]))*scaleFactor;
        //float radi=float.Parse(radis[j])*scaleFactor*0.1f;
        
        float radi=Mathf.Exp(float.Parse(actual_radis[j])*filterSlider.value)*scaleSlider.value*0.1f*file_id;
        if(radi<0){
          radi=0;
        }
        capa[j-1]=radi;
        //float radi=float.Parse(radis[j])*scaleFactor;
        //Debug.Log(radi);
        verticesCartesianas[i]=new Vector3(radi*Mathf.Cos(j*0.1745f),y/50.0f,radi*Mathf.Sin(j*0.1745f));
        i++;
      }
      capa[36]=Mathf.Round(float.Parse(actual_radis[37]));
      capes.Add(capa);

    }

    verticesUnity=canviBase(verticesCartesianas);
    triangles=new int[6*36*99];
    triangles[0]=0;
    triangles[1]=36;
    triangles[2]=1;
    triangles[3]=1;
    triangles[4]=36;
    triangles[5]=37;
    i=0;
    for(int capa1=0;capa1<98;capa1++){
      for(int angulo=0;angulo<35;angulo++){
        triangles[i]=capa1*36+angulo;
        triangles[i+1]=(capa1+1)*36+angulo;
        triangles[i+2]=capa1*36+angulo+1;

        triangles[i+3]=capa1*36+angulo+1;
        triangles[i+4]=(capa1+1)*36+angulo;
        triangles[i+5]=(capa1+1)*36+angulo+1;
        i+=6;
      }
    }
    UpdateMesh();
  }
  void Update(){
    if(modoAnalisis){
      if(Input.GetKey(KeyCode.M) && capa<99){
        capa++;
        text_capa.text=capa.ToString();
        float[] capa_radios=capes[capa];
        for(int phi=0;phi<36;phi+=1){
          float radi=capa_radios[phi]*200;
          points2D[phi].transform.position=new Vector3(radi*Mathf.Cos(phi*0.1745f), radi*Mathf.Sin(phi*0.1745f),0)+puntoReferencia.transform.position;
        }
        planeIndicator.transform.position=baseReferencia.position+baseReferencia.transform.up*(capa+2)/51;
        Debug.Log(capa_radios[36].ToString());
        mediaText.text=capa_radios[36].ToString();
      }
      if(Input.GetKey(KeyCode.N) && capa>0){
        capa--;
        text_capa.text=capa.ToString();
        float[] capa_radios=capes[capa];
        for(int phi=0;phi<36;phi+=1){
          float radi=capa_radios[phi]*200;
          points2D[phi].transform.position=new Vector3(radi*Mathf.Cos(phi*0.1745f), radi*Mathf.Sin(phi*0.1745f),0)+puntoReferencia.transform.position;
        }
        planeIndicator.transform.position=baseReferencia.position+baseReferencia.transform.up*(capa+2)/51;
        mediaText.text=capa_radios[36].ToString();
      }
      planeIndicator.SetActive(true);
      canva.SetActive(true);
    }else{
      planeIndicator.SetActive(false);
      canva.SetActive(false);
    }
    if(Input.GetKeyDown(KeyCode.A)){
      modoAnalisis=!modoAnalisis;
    }
    if(Input.GetKeyDown(KeyCode.P)){
      file_id++;
      if(file_id>fileInfo_num-1){
        file_id=1;
      }
      MeshPrepare(file_id);
    }
    scalerText.text="Scaler: "+scaleSlider.value.ToString();
    filterText.text="Constrast: "+filterSlider.value.ToString();
    
  }

  void UpdateMesh(){
    mesh.Clear();
    mesh.vertices=verticesCartesianas;
    mesh.triangles=triangles;
    mesh.RecalculateNormals();
  }
  // private void OnDrawGizmos(){
  //   if (verticesUnity==null){
  //     return;
  //   }
  //   for(int i=0; i<verticesUnity.Length;i++){
  //     Gizmos.DrawSphere(verticesCartesianas[i], .1f);
  //   }
  // }
  Vector3[] canviBase(Vector3[] puntos_cartesiano){
    Vector3[] puntos_unity=new Vector3[puntos_cartesiano.Length];
    for(int i=0;i<puntos_cartesiano.Length;i++){
      puntos_unity[i]=puntos_cartesiano[i].x*baseReferencia.right+puntos_cartesiano[i].y*baseReferencia.up+puntos_cartesiano[i].z*baseReferencia.forward+baseReferencia.position;
    }
    return puntos_unity;
  }

}
