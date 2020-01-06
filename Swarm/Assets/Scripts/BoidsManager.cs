using UnityEngine;

public class BoidsManager : MonoBehaviour
{
    public float MaxSight = 180f;
    public float MaxAcceleration;
    public float MaxVelocity;

    //Wander Variables
    public float WanderJitter;
    public float WanderRadius;
    public float WanderDistance;
    public float WanderPriority;

    //Cohesion Variables
    public float CohesionRadius;
    public float CohesionPriority;

    //Aligment Variables
    public float AlignmentRadius;
    public float AlignmentPriority;

    //Seperation Variables
    public float SeperationRadius;
    public float SeperationPriority;

    //Avoidance Variables
    public float AvoidanceRadius;
    public float AvoidancePriority;
}
