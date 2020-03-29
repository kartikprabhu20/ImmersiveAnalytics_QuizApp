using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Controller : MonoBehaviour
{

    private static int COUNTER = 1;
    private TaskFactory taskFactory;
    public GameObject masterCanvas;
    public GameObject scatterplot;
    public GameObject scatterPlotManager;
    public GameObject selector;
    public string controlerType;
    private ITaskListener taskListener;

    // Start is called before the first frame update

    void Start()
    {
        masterCanvas.SetActive(true); //In case of AR canvas needs to be set to active
        scatterPlotManager.SetActive(true);
        taskFactory = TaskFactory.Instance; //singleton
        taskListener = new TaskListener(taskFactory, masterCanvas,scatterPlotManager,selector,controlerType);
        taskFactory.getTask(COUNTER).init(taskListener, masterCanvas, scatterPlotManager);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void selectScatterplot()
    {
        scatterplot.SetActive(true);
        //Selection.activeGameObject = scatterplot;
    }

    public void assignScatterplot(GameObject plotPrefab)
    {
        scatterplot = plotPrefab;
    }

    internal void assignMasterCanvas(GameObject canvas)
    {
        this.masterCanvas = canvas;
    }

    public void assignSelector(GameObject selectr)
    {
        this.selector = selectr;
    }

    internal void assignScatterPlotManager(GameObject scatterManager)
    {
        this.scatterPlotManager = scatterManager;
    }

    class TaskListener : ITaskListener
    {
        private TaskFactory taskFactory;
        private GameObject masterCanvas;
        private GameObject scatterPlotManager;
        private GameObject selector;
        private string controllerType;

        public TaskListener(TaskFactory taskFactory, GameObject masterCanvas, GameObject scatterPlotManager, GameObject selectr, string control)
        {
            this.taskFactory = taskFactory;
            this.masterCanvas = masterCanvas;
            this.scatterPlotManager = scatterPlotManager;
            this.selector = selectr;
            this.controllerType = control;
        }

        void ITaskListener.submitted()
        {
            COUNTER++;
            if (COUNTER > 3)
            {
                masterCanvas.transform.Find("Overlay - Finish").gameObject.SetActive(true);
            }
            else
            {
                if(controllerType == "AR")
                {
                   this.selector.GetComponent<RayCastSelector>().restore();
                }
                Debug.Log("Counter:" + COUNTER);
                taskFactory.getTask(COUNTER).init(this, masterCanvas, scatterPlotManager);
            }
        }
    }

    internal void assignControllerType(string v)
    {
        this.controlerType = v;
    }
}

public interface ITaskListener
{
    void submitted();
}