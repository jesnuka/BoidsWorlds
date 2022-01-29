using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidController: MonoBehaviour
{
    [SerializeField]
    Boid[] boids;
   // List<Boid> boidsContinous = new List<Boid>();

    [SerializeField]
    GameObject boidPrefab;
    [SerializeField]
    int spawnAmount, continuousSpawnAmount;
    [SerializeField]
    float spawnRadius;

    [SerializeField]
    bool canSpawn;

    [Header("Boid Rules Radiuses")]
    public float cohesionRadius;
    public float separationRadius;
    public float alignmentRadius;
    public float containmentRadius;

    [Header("Boid Forces")]
    public float repulsionForce;
    public float attractionForce;

    [Header("Boid Velocity")]
    public Vector3 velocity;
    public float maxVelocity;

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
            SpawnBoids(continuousSpawnAmount);
    }
    private void Start()
    {
        SpawnBoids();
    }
    public void UpdateBoidValues()
    {
        Debug.Log("Updating values!");
        foreach(Boid boid in boids)
        {
            boid.UpdateBoidValues(cohesionRadius,separationRadius,alignmentRadius,containmentRadius,repulsionForce,attractionForce,maxVelocity);
        }
    }

    void UpdateControllerValues(float cohesionRadius, float separationRadius, float alignmentRadius, float containmentRadius, float repulsionForce, float attractionForce, float maxVelocity)
    {
        this.cohesionRadius = cohesionRadius;
        this.separationRadius = separationRadius;
        this.alignmentRadius = alignmentRadius;
        this.containmentRadius = containmentRadius;
        this.repulsionForce = repulsionForce;
        this.attractionForce = attractionForce;
        this.maxVelocity = maxVelocity;
    }

    //Randomize values for each boid separately
    public void RandomizeBoidValuesSeparately()
    {
        Debug.Log("Randomizing values for each");
        foreach (Boid boid in boids)
        {
            float randomValue = Random.Range(0, 10f);
            Debug.Log("New assigned value for each is " + randomValue);
            boid.UpdateBoidValues(randomValue, randomValue, randomValue, randomValue, randomValue, randomValue, randomValue);

        }
    }
    //Randomize values to be used for all boids
    public void RandomizeBoidValuesTogether()
    {
        float randomValue = Random.Range(0, 10f);
        Debug.Log("Randomizing values for all");
        Debug.Log("New assigned value for each is " + randomValue);
        foreach (Boid boid in boids)
        {
            
            boid.UpdateBoidValues(randomValue, randomValue, randomValue, randomValue, randomValue, randomValue, randomValue);
            
        }
    }

    public void RandomizeEachBoidValueSeparately()
    {
        foreach (Boid boid in boids)
            boid.UpdateBoidValues(Random.Range(0, 10f), Random.Range(0, 10f), Random.Range(0, 10f), Random.Range(0, 10f), Random.Range(0, 10f), Random.Range(0, 10f), Random.Range(0, 10f));
    }

    public void RandomizeEachBoidValueTogether()
    {
        float rValue1 = Random.Range(0, 10f);
        float rValue2 = Random.Range(0, 10f);
        float rValue3 = Random.Range(0, 10f);
        float rValue4 = Random.Range(0, 10f);
        float rValue5 = Random.Range(0, 10f);
        float rValue6 = Random.Range(0, 10f);
        float rValue7 = Random.Range(0, 10f);
        foreach (Boid boid in boids)
            boid.UpdateBoidValues(rValue1, rValue2, rValue3, rValue4, rValue5, rValue6, rValue7);
    }

    public Boid[] GetBoids()
    {
        return boids;
    }
    void FindBoids()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Boid>() != null)
            {
          //      boidsContinous.Add(transform.GetChild(i).GetComponent<Boid>());
            }
                
        }
    }
    void SpawnBoids(int contAmount = -1)
    {
        boids = new Boid[spawnAmount];
        spawnAmount = (contAmount == -1) ? spawnAmount : continuousSpawnAmount;
        for(int i = 0; i < spawnAmount; i++)
        {
            GameObject newBoid = Instantiate(boidPrefab, transform.position + Random.insideUnitSphere * spawnRadius, Random.rotation);
            newBoid.GetComponent<Boid>().SetController(this);
            newBoid.GetComponent<Boid>().gameObject.transform.parent = this.gameObject.transform;
            boids[i] = (newBoid.GetComponent<Boid>());
        }
    }
}
