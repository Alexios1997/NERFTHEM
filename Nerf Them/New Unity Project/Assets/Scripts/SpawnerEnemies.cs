using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemies : MonoBehaviour
{

    public GameObject SpawnerPoint;
    public GameObject CubesRef;
    public Material[] materials;
    List<GameObject> Cubes = new List<GameObject>();

    private float Timer;
    private int ObjectId = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;

        if((int)Timer == 4)
        {
            int RandNumber = Random.Range(0,3);
            GameObject ga;
            ga = Instantiate(CubesRef, SpawnerPoint.transform.position, Quaternion.identity);
            ga.tag = "CubeEnemies";
            ga.name = "Cube" + ObjectId.ToString();
            ga.GetComponent<MeshRenderer>().material = materials[RandNumber];
            Cubes.Add(ga);
            ObjectId++;
            Timer = 0;
        }
        MoveCubes();
        
    }
    void MoveCubes()
    {
        for (int i=0;i<Cubes.Count;i++)
        {
            Cubes[i].GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, -5f);
        }
        
    }

}
