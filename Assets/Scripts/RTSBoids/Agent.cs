using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float weight, maxSpeed, trueMaxSpeed, maxAcceleration, orientation, rotation, maxRotation, maxAngularAcceleration, closestApproachDistance, maximumAttackDistance, rotationSpeedScale;
    public GameObject target;

    [SerializeField]
    Vector3 velocity;
    Vector3 destination;

    [SerializeField]
    protected Steering steering;

    [SerializeField]
    MovementType movementType;

    public enum MovementType
    {
        Tank, //Needs to stop to turn
        Mech, //Half-way between, can turn slightly while moving
        Hover //Floaty movement

    }

    private void Start()
    {
        InitializeValues();
    }

    void InitializeValues()
    {
        velocity = Vector3.zero;
        steering = new Steering();
        trueMaxSpeed = maxSpeed;
    }

    public void SetSteering(Steering steering)
    {
        this.steering.linear += (weight * steering.linear);
        this.steering.angular += (weight * steering.angular);
    }

    public virtual void Update()
    {
        PerformPhysicsLastFrame();
    }

    //Update movement based on the last frame steering
    void PerformPhysicsLastFrame()
    {
        Vector3 displacement = velocity * Time.deltaTime;
        displacement.y = 0f;

        orientation += rotation * Time.deltaTime;

        //Limit orientation from 0 to 360
        if (orientation < 0.0f)
            orientation += 360f;
        else if (orientation > 360f)
            orientation -= 360f;

        transform.Translate(displacement, Space.World);
        transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.Rotate(Vector3.up, orientation);

    }
    private void LateUpdate()
    {
        PerformPhysicsNextFrame();
    }

    

    //Update movement for the next frame
    void PerformPhysicsNextFrame()
    {
        velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;
        if(velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity = velocity * maxSpeed;
        }
        if (steering.linear.magnitude == 0f)
            velocity = Vector3.zero;
        steering = new Steering();
    }

    //Match speed with other objects in the group
    public void ResetSpeed()
    {
        maxSpeed = trueMaxSpeed;
    }
}
