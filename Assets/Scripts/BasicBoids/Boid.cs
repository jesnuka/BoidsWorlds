using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
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

    private Vector3 separationAvg, alignmentAvg, cohesionAvg;
    private BoidController boidController;

    public void SetController(BoidController newController)
    {
        boidController = newController;
    }

    private void Awake()
    {
        if (boidController == null)
            boidController = GetComponentInParent<BoidController>();
    }

    public void UpdateBoidValues(float cohesionRadius, float separationRadius, float alignmentRadius, float containmentRadius, float repulsionForce, float attractionForce, float maxVelocity)
    {
        this.cohesionRadius = cohesionRadius;
        this.separationRadius = separationRadius;
        this.alignmentRadius = alignmentRadius;
        this.containmentRadius = containmentRadius;
        this.repulsionForce = repulsionForce;
        this.attractionForce = attractionForce;
        this.maxVelocity = maxVelocity;
    }

    private void Start()
    {
        InitializeVelocity();
    }
    void InitializeVelocity()
    {
        velocity = this.transform.forward * maxVelocity;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();

        //Calculate average is used to calculate Cohesion, Alignment and Separation for the boids.
        CalculateAverage();
        StayInRadius();
    }

    private void HandleMovement()
    {
        //Clamp the velocity to avoid accelerating infinitely
        if (velocity.magnitude > maxVelocity)
        {
            velocity = velocity.normalized * maxVelocity;
        }
        this.transform.position += velocity * Time.deltaTime;
    }
    private void HandleRotation()
    {
        this.transform.rotation = Quaternion.LookRotation(velocity);
    }

    void CalculateAverage()
    {
        Vector3 separationAvg = Vector3.zero, alignmentAvg = Vector3.zero, cohesionAvg = Vector3.zero;
        int sFound = 0, aFound = 0, cFound = 0;

        foreach (Boid boid in boidController.GetBoids())
        {
            if (boid != this)
            {
                //For each boid, calculating difference means calculating their distance from this boid
                //This average amount is then used to determine the new position for this boid
                Vector3 difference = boid.transform.position - this.transform.position;

                //Cohesion
                if (difference.magnitude < cohesionRadius)
                {
                    cohesionAvg += difference;
                    cFound += 1;
                }

                //Separation
                if (difference.magnitude < separationRadius)
                {
                    separationAvg += difference;
                    sFound += 1;
                }
                //Alignment
                if (difference.magnitude < alignmentRadius)
                {
                    alignmentAvg += boid.velocity;
                    aFound += 1;
                }
            }
        }
        //Boid Separation: Steer to avoid crowding local flockmates.
        //The boid will repulse itself away from the other boids in the flock
        if (sFound > 0)
            BoidSeparation(separationAvg / sFound);

        //Boid Alignment: Steer towards the average heading of local flockmates.
        //The boid will adjust its velocity to match the velocity of the average of the flock.
        if (aFound > 0)
            BoidAlignment(alignmentAvg / aFound);

        //Boid Cohesion: Steer to move towards the average position (center of mass) of local flockmates
        //As such, we need to calculate the average for each local flockmate, meaning each boid inside our radius
        if (cFound > 0)
            BoidCohesion(cohesionAvg / cFound);
    }

    void BoidCohesion(Vector3 average)
    {
        //Calculate velocity based on how far away average position is. Radius is the maximum distance possible
        //Then sum this average to the velocity, increasing value to the maximum of the average velocity
        velocity += Vector3.Lerp(Vector3.zero, average, average.magnitude / cohesionRadius);
    }

    void BoidAlignment(Vector3 average)
    {
        //Match the velocity from the low point of the boid.velocity, to the maximum of the average, over a smooth Time.deltaTime curve
        velocity += Vector3.Lerp(velocity, average, Time.deltaTime);
    }

    void BoidSeparation(Vector3 average)
    {
        //Calculate velocity based on how far away average position is. Radius is the maximum distance possible
        //Here, the average value represents nearby boids that this boid wants to move away from,
        //Thus the velocity is subtract using the repulsionForce
        velocity -= Vector3.Lerp(Vector3.zero, average, average.magnitude / separationRadius) * repulsionForce;
    }

    void StayInRadius()
    {
        //If boid leaves the radius, move them back towards the center
        if (transform.position.magnitude > containmentRadius)
        {
            velocity += this.transform.position.normalized * (containmentRadius - transform.position.magnitude) * attractionForce * Time.deltaTime;
        }
    }
}
