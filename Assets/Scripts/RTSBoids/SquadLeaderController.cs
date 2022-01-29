using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadLeaderController : MonoBehaviour
{
    public SquadLeaderController Instance { get; private set; }

    private GameObject target;
    [SerializeField]
    GameObject childPrefab;
    [SerializeField]
    private List<GameObject> childList;
    [SerializeField]
    int unitAmount;
    [SerializeField]
    float unitSpawnOffset;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;
    }

    void Start()
    {
        InitializeChildUnits();   
    }

    private void InitializeChildUnits()
    {
        childList = new List<GameObject>();

        for (int i = 0; i < unitAmount; i++)
        {
            Vector3 relativeSpawnPoint = new Vector3(i % 4, (1f / 3f), i / 4);
            GameObject tempObject = Instantiate(childPrefab, transform.position + (relativeSpawnPoint * unitSpawnOffset), transform.rotation);
            tempObject.GetComponent<AgentBehaviour>().SetTarget(gameObject);
            childList.Add(tempObject);
        }
    }
}
