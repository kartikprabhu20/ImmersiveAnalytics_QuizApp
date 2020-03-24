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

    private ITaskListener taskListener;

    // Start is called before the first frame update

    void Start()
    {
        masterCanvas.SetActive(true); //In case of AR canvas needs to be set to active
        scatterPlotManager.SetActive(true);
        taskFactory = TaskFactory.Instance; //singleton
        taskListener = new TaskListener(taskFactory, masterCanvas,scatterPlotManager);
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

    internal void assignMasterCanvas(GameObject masterCanvas)
    {
        this.masterCanvas = masterCanvas;
    }

    internal void assignScatterPlotManager(GameObject scatterPlotManager)
    {
        this.scatterPlotManager = scatterPlotManager;
    }

    class TaskListener : ITaskListener
    {
        private TaskFactory taskFactory;
        private GameObject masterCanvas;
        private GameObject scatterPlotManager;

        public TaskListener(TaskFactory taskFactory, GameObject masterCanvas, GameObject scatterPlotManager)
        {
            this.taskFactory = taskFactory;
            this.masterCanvas = masterCanvas;
            this.scatterPlotManager = scatterPlotManager;
        }

        public void submitted()
        {
            Debug.Log("Counter:" + COUNTER);
            COUNTER++;
            taskFactory.getTask(COUNTER).init(this, masterCanvas, scatterPlotManager);
        }
    }
}

public interface ITaskListener
{
    void submitted();
}