using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RobotBehaviour: MonoBehaviour {
    private ARRaycastManager rays;
    public GameObject robotPrefab;
    public Camera myCamera;
    public float cooldown, cooldownCount;
    private ARAnchorManager anc;
    private ARPlaneManager plan;

    private static ILogger logger = Debug.unityLogger;

    void Start() {
        cooldown = 2;
        myCamera = gameObject.transform.Find("AR Camera").gameObject.GetComponent<Camera>();

        rays = gameObject.GetComponent<ARRaycastManager>();
        anc = gameObject.GetComponent<ARAnchorManager>();
        plan = gameObject.GetComponent<ARPlaneManager>();

    }

    void Update()
    {

			cooldownCount += Time.deltaTime;

			
			if (cooldownCount > cooldown && Input.touchCount == 1) {
                cooldownCount = 0;
                doSpawnRobot();
		 	}
		 	
     	    	
		}

    public void doSpawnRobot() {
        logger.Log("doSpawnRobot");
        GameObject robot;
        Vector3 screenCenter;
        bool hit;
        ARRaycastHit nearest;
        List<ARRaycastHit> myHits = new List <ARRaycastHit>();
        ARPlane plane;
        ARAnchor point;

        screenCenter = myCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        if (screenCenter == null) { logger.Log("screencenter is null"); return; }

        rays = gameObject.GetComponent<ARRaycastManager>(); //not reading it in start for some reason.
        if (rays == null) { logger.Log("rays is null"); return; }

        hit = rays.Raycast(screenCenter,
            myHits,
            TrackableType.FeaturePoint | TrackableType.PlaneWithinPolygon);

        logger.Log("Hit: " + hit);
       

        if (hit == true) {
            nearest = myHits[0];
            robot = Instantiate(robotPrefab, nearest.pose.position + nearest.pose.up * 0.1f, nearest.pose.rotation);

            robot.transform.localScale = new Vector3(3, 3, 3);
            robot.tag = "SpawnedObject";

            logger.Log("spawned at " + robot.transform.position.x + ", " + robot.transform.position.y + ", " + robot.transform.position.z);

            anc = gameObject.GetComponent<ARAnchorManager>(); //not reading it in start for some reason.
            if (anc == null) { logger.Log("anc is null"); return; }
            plan = gameObject.GetComponent<ARPlaneManager>(); //not reading it in start for some reason.
            if (plan == null) { logger.Log("plan is null"); return; }

            plane = plan.GetPlane(nearest.trackableId);

            if (plane != null) {
                point = anc.AttachAnchor(plane, nearest.pose); 
                logger.Log("Added an anchor to a plane " + nearest);
            } else {
                // Make sure the new GameObject has an ARAnchor component
                point = robot.GetComponent<ARAnchor>();
                if (point == null)
                {
                    point = robot.AddComponent<ARAnchor>();
                }

                logger.Log("Added another anchor " + nearest);

            }

            robot.transform.parent = point.transform;
        }
    }

    //apply force in the direction of the camera - with one finger touch
	public void doFusRoDah() { 
    	RaycastHit[] myHits;
			Ray r;
			
			r = myCamera.ScreenPointToRay(Input.GetTouch(0).position);

	 		myHits = Physics.RaycastAll (r);

			foreach (RaycastHit hit in myHits) {
					logger.Log ("Detected " + hit.transform.gameObject.name);
				
				if (hit.transform.gameObject.tag == "SpawnedObject") {
					logger.Log ("Applying force");
					hit.transform.gameObject.GetComponent<Rigidbody>().AddForce (r.direction * 100);
				}
			}
		}
}