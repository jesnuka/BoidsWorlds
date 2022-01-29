using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour //TODO: Change to "Movement" for flying boids etc
{
    public float angular; //Rotation from 0 to 360
    public Vector3 linear; //Instantaneous velocity

    public void SteeringInitial()
    {
        angular = 0f;
        linear = Vector3.zero;
    }

}
