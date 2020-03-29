using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Task : ITask
{
    public TaskState state = TaskState.DEFAULT;
    public TaskType taskType = TaskType.TYPE_1; //#Defualt

    public ITaskListener taskListener;
    public GameObject masterCanvas;
    public GameObject questionObject;
    public GameObject scatterplotManager;
    public Button submitButton;
    public Button hideOverlayButton;
    public Button showOverlayButton;
    public string baseOverlay = "Overlay - Input"; //Defualt
    public string baseQuestionPopup = "Popup - InputQuestion";//Default

    public virtual void init(ITaskListener taskListener, GameObject masterCanvas, GameObject scatterPlotManager)
    {
        this.taskListener = taskListener;
        this.masterCanvas = masterCanvas;
        this.scatterplotManager = scatterPlotManager;

        baseOverlay = (taskType == TaskType.TYPE_1) ? "Overlay - Input" : "Overlay - Option";
        baseQuestionPopup = (taskType == TaskType.TYPE_1) ? "Popup - InputQuestion" : "Popup - OptionQuestion";


        masterCanvas.transform.Find("Overlay - Input").gameObject.SetActive(!(taskType == TaskType.TYPE_1));
        masterCanvas.transform.Find("Overlay - Option").gameObject.SetActive(!(taskType == TaskType.TYPE_2));

        masterCanvas.transform.Find("Overlay - Input").gameObject.SetActive(taskType == TaskType.TYPE_1);
        masterCanvas.transform.Find("Overlay - Option").gameObject.SetActive(taskType == TaskType.TYPE_2);

        try
        {
            questionObject = masterCanvas.transform.Find(baseOverlay + "/"+baseQuestionPopup+"/Container/Question").gameObject;
            submitButton = masterCanvas.transform.Find(baseOverlay + "/" + baseQuestionPopup + "/Container/Button - Submit").GetComponent<Button>();
            hideOverlayButton = masterCanvas.transform.Find(baseOverlay + "/" + baseQuestionPopup + "/Container/Button - HideOverlay").GetComponent<Button>();

        }
        catch (NullReferenceException ex)
        {
            questionObject = masterCanvas.transform.Find(baseQuestionPopup + "/Container/Question").gameObject;
            submitButton = masterCanvas.transform.Find(baseQuestionPopup + "/Container/Button - Submit").GetComponent<Button>();
            hideOverlayButton = masterCanvas.transform.Find(baseQuestionPopup + "/Container/Button - HideOverlay").GetComponent<Button>();

            //questionObject.transform.SetParent(masterCanvas.transform.Find(baseQuestionPopup).transform);

            masterCanvas.transform.Find("Popup - InputQuestion").gameObject.SetActive(!(taskType == TaskType.TYPE_1));
            masterCanvas.transform.Find("Popup - OptionQuestion").gameObject.SetActive(!(taskType == TaskType.TYPE_2));

            masterCanvas.transform.Find("Popup - InputQuestion").gameObject.SetActive(taskType == TaskType.TYPE_1);
            masterCanvas.transform.Find("Popup - OptionQuestion").gameObject.SetActive(taskType == TaskType.TYPE_2);

        }

        submitButton.onClick.AddListener(result);

        showOverlayButton = masterCanvas.transform.Find(baseOverlay + "/Button - ShowOverlay").GetComponent<Button>();
        showOverlayButton.onClick.Invoke();

       
    }

    public abstract void execute();
    public abstract void result();


    public TaskState getState()
    {
        return state;
    }

    public void setState(TaskState state)
    {
        this.state = state;
    }

    public TaskType getTyp()
    {
        return taskType;
    }

    public void setType(TaskType type)
    {
        this.taskType = type;
    }
}