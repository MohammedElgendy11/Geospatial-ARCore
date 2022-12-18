using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Google.XR.ARCoreExtensions;
using System;

public class VPS_Manager : MonoBehaviour
{

    [SerializeField] private AREarthManager earthManager;

    [Serializable]
    public struct GeospatialObject
    {
        public GameObject ObjectPrefab;
        public EarthPosition EarthPosition;
    }


    [Serializable]
    public struct EarthPosition
    {
        public double Latitude;
        public double Longitude;
        public double Alttitude;

    }

    [SerializeField] private ARAnchorManager aRAnchorManager;
    [SerializeField] private List<GeospatialObject> geospatialObjects = new List<GeospatialObject>();





    // Start is called before the first frame update
    void Start()
    {
        VerifyGeospatialSupport();
    }

    private void VerifyGeospatialSupport()
    {
        var result = earthManager.IsGeospatialModeSupported(GeospatialMode.Enabled);

        switch (result)
        {
            case FeatureSupported.Supported:
                Debug.Log("Ready To Use");
                PlaceObject();

                break;
            case FeatureSupported.Unknown:
                Debug.Log("UnKnown....");
                Invoke("VerifyGeospatialSupport", 5.0f);

                break;
            case FeatureSupported.Unsupported:
                Debug.Log("VPS UnSupported");


                break;

        }
    }

    private void PlaceObject()
    {
        if (earthManager.EarthTrackingState == TrackingState.Tracking)

        {
            var geospatialpose = earthManager.CameraGeospatialPose;

            foreach (var obj in geospatialObjects)
            {
                var earthPosition = obj.EarthPosition;
                var objAnchor = ARAnchorManagerExtensions.AddAnchor(aRAnchorManager, earthPosition.Latitude, earthPosition.Longitude, earthPosition.Alttitude, Quaternion.identity);
                Instantiate(obj.ObjectPrefab, objAnchor.transform);
            }
        }

        else if (earthManager.EarthTrackingState == TrackingState.None)

        {
            Invoke("PlaceObject", 5.0f);
        }
    }


}
