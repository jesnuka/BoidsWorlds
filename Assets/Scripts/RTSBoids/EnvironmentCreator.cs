using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCreator : MonoBehaviour
{
    [SerializeField]
    GameObject[] treePrefabs;
    [SerializeField]
    int treeAmount;

    GameObject plane; //Rudimentary version with a flat plane

    private void Awake()
    {
        if (plane == null)
            plane = this.gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        SpawnTrees();
    }

    void SpawnTrees()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
