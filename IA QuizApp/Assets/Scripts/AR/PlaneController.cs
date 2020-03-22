using System;
using System.Collections.Generic;
using GoogleARCore;
using GoogleARCore.Examples.Common;
using GoogleARCore.Examples.ObjectManipulation;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaneController : Manipulator
{
    /// <summary>
    /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
    /// </summary>
    public Camera FirstPersonCamera;

    /// <summary>
    /// A prefab to place when a raycast from a user touch hits a horizontal plane.
    /// </summary>
    public GameObject plotPrefab;
    public GameObject planeGenerator;
    public GameObject pointCloud;
    public GameObject planeDiscovery;
    public GameObject manipulatorPrefab;
    public GameObject masterCanvas;
    public GameObject arController;
    public GameObject scatterPlotManager;
    //public GameObject settingsManager;

    /// <summary>
    /// The rotation in degrees need to apply to prefab when it is placed.
    /// </summary>
    private const float k_PrefabRotation = 180.0f;

    /// <summary>
    /// True if the app is in the process of quitting due to an ARCore connection error,
    /// otherwise false.
    /// </summary>
    private bool m_IsQuitting = false;


    /// <summary>
    /// True if the plotObject is already anchored, so that multiple objects are not created
    /// </summary>
    private bool m_IsAnchored = false;

    /// <summary>
    /// The Unity Awake() method.
    /// </summary>
    public void Awake()
    {
        // Enable ARCore to target 60fps camera capture frame rate on supported devices.
        // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
        Application.targetFrameRate = 60;
    }
    // Start is called before the first frame update
    void Start()
    {
        planeGenerator = Instantiate(planeGenerator);
        pointCloud = Instantiate(pointCloud);
        planeDiscovery = Instantiate(planeDiscovery);
        masterCanvas.SetActive(false);

    }

    private void stopPlaneDetection()
    {
        Destroy(planeGenerator);
        Destroy(pointCloud);
        Destroy(planeDiscovery);
    }

    /// <summary>
    /// Check and update the application lifecycle.
    /// </summary>
    private void _UpdateApplicationLifecycle()
    {
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Only allow the screen to sleep when not tracking.
        if (Session.Status != SessionStatus.Tracking)
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        if (m_IsQuitting)
        {
            return;
        }

        // Quit if ARCore was unable to connect and give Unity some time for the toast to
        // appear.
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            _ShowAndroidToastMessage("Camera permission is needed to run this application.");
            m_IsQuitting = true;
            Invoke("_DoQuit", 0.5f);
        }
        else if (Session.Status.IsError())
        {
            _ShowAndroidToastMessage(
                "ARCore encountered a problem connecting.  Please start the app again.");
            m_IsQuitting = true;
            Invoke("_DoQuit", 0.5f);
        }
    }

    /// <summary>
    /// Actually quit the application.
    /// </summary>
    private void _DoQuit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Show an Android toast message.
    /// </summary>
    /// <param name="message">Message string to show in the toast.</param>
    private void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity =
            unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject =
                    toastClass.CallStatic<AndroidJavaObject>(
                        "makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }


    /// <summary>
    /// Returns true if the manipulation can be started for the given gesture.
    /// </summary>
    /// <param name="gesture">The current gesture.</param>
    /// <returns>True if the manipulation can be started.</returns>
    protected override bool CanStartManipulationForGesture(TapGesture gesture)
    {
        return gesture.TargetObject == null;
    }

    /// <summary>
    /// Function called when the manipulation is ended.
    /// </summary>
    /// <param name="gesture">The current gesture.</param>
    protected override void OnEndManipulation(TapGesture gesture)
    {
        if (gesture.WasCancelled)
        {
            return;
        }

        // If gesture is targeting an existing object we are done.
        if (gesture.TargetObject != null)
        {
            return;
        }

        // Raycast against the location the player touched to search for planes.
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon;

        if (Frame.Raycast(
            gesture.StartPosition.x, gesture.StartPosition.y, raycastFilter, out hit))
        {
            // Use hit pose and camera pose to check if hittest is from the
            // back of the plane, if it is, no need to create the anchor.
            if ((hit.Trackable is DetectedPlane) &&
                Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                    hit.Pose.rotation * Vector3.up) < 0)
            {
                Debug.Log("Hit at back of the current DetectedPlane");
            }
            else
            {

                if (!m_IsAnchored)
                {
                    // Instantiate game object at the hit pose.
                    plotPrefab = Instantiate(plotPrefab, hit.Pose.position, hit.Pose.rotation);
                    stopPlaneDetection();
                    m_IsAnchored = true;

                    // Instantiate manipulator.
                    manipulatorPrefab = Instantiate(manipulatorPrefab, hit.Pose.position, hit.Pose.rotation);

                    // Create an anchor to allow ARCore to track the hitpoint as understanding of
                    // the physical world evolves.
                    var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                    // Make manipulator a child of the anchor.
                    manipulatorPrefab.transform.parent = anchor.transform;

                    // Make game object a child of the manipulator.
                    plotPrefab.transform.parent = manipulatorPrefab.transform;


                    // Select the placed object.
                    manipulatorPrefab.GetComponent<Manipulator>().Select();

                    masterCanvas.SetActive(true);

                    //scatterPlotManager.GetComponent<ScatterplotGenerator>().assignPointsHolder(plotPrefab.transform.Find("Points").gameObject);
                    scatterPlotManager.GetComponent<ScatterplotGenerator>().assignScatterPlot(plotPrefab);

                    scatterPlotManager = Instantiate(scatterPlotManager);

                    //PlotLoader plotLoader = new PlotLoader(settingsManager, scatterPlotManager);
                    scatterPlotManager.GetComponent<ScatterplotGenerator>().initPlot();

                    //Debug.Log("Glyph: " + settingsManager.GetComponent<SettingsManager>().getScatterPlotGenerator().getGlyphList().Count);

                }
            }
        }
    }



    public class PlotLoad : IPlotLoad
    {
        private GameObject settingsManager;
        private GameObject scatterPlotManager;

        public PlotLoad(GameObject settingsManager, GameObject scatterPlotManager)
        {
            this.settingsManager = settingsManager;
            this.scatterPlotManager = scatterPlotManager;
        }

        public void callback()
        {
            //settingsManager.GetComponent<SettingsManager>().setScatterPlotGenerator(scatterPlotManager.GetComponent<ScatterplotGenerator>());
            //settingsManager.GetComponent<SettingsManager>().initSettings();
        }
    }
}
