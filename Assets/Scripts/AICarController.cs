using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerController))]

public class AICarController : MonoBehaviour {
    public WaypointContainer waypointContainer;
    public List<Transform> waypoints;
    public int currentWaypoint;
    private PlayerController carController;
    public float waypointRange;
    private float currentAngle;

    void Start(){
        carController = GetComponent<PlayerController>();
        waypoints = waypointContainer.waypoints;
        currentWaypoint = 0; 
    }

    void Update(){
        if(Vector3.Distance(waypoints[currentWaypoint].position,transform.position) < waypointRange){
            currentWaypoint++;
            if(currentWaypoint >= waypoints.Count){
                currentWaypoint = 0;
            }
        }

        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        currentAngle = Vector3.SignedAngle(fwd, waypoints[currentWaypoint].position - transform.position, Vector3.up);
        carController.SetInputs(1,currentAngle,0,0);


        Debug.DrawRay(transform.position, waypoints[currentWaypoint].position - transform.position, Color.red);
    }


} 