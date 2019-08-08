using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARTapToPlaceObject : MonoBehaviour
{
    [Tooltip("the visual indicator of where an object can be placed")]
    public GameObject placementIndicator;

    [SerializeField]
    [Tooltip("instantiate this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }


    private Pose m_PlacementPose;
    private ARRaycastManager m_RaycastManager;
    private bool m_PlacementPoseIsValid = false;


    private void Start()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    private void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();


        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if(m_PlacementPoseIsValid)
            {
                PlaceObject();
            } else
            {
                RemoveObject();
            }
        }
 
    }

    private void PlaceObject()
    {

        if(spawnedObject == null)
        {
            spawnedObject = Instantiate(m_PlacedPrefab, m_PlacementPose.position, m_PlacementPose.rotation);
        } else
        {
            RemoveObject();
            //spawnedObject.transform.position = m_PlacementPose.position;
        }
    }

    private void RemoveObject()
    {

        Destroy(spawnedObject);
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        m_RaycastManager.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon);
        m_PlacementPoseIsValid = hits.Count > 0;

        if (m_PlacementPoseIsValid)
        {
            m_PlacementPose = hits[0].pose;

            // calculate new rotation based on camera orientation
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

            m_PlacementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
            
       
    }


    private void UpdatePlacementIndicator()
    {
        if(m_PlacementPoseIsValid && spawnedObject == null)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(m_PlacementPose.position, m_PlacementPose.rotation);
        } else
        {
            placementIndicator.SetActive(false);
        }
    }

}
