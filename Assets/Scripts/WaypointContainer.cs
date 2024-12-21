using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointContainer : MonoBehaviour
{
    public List<Transform> waypoints;

    void Awake(){
        foreach(Transform child in gameObject.GetComponentsInChildren<Transform>()){
            waypoints.Add(child);
        }
    }

    void Update(){

    }


}

