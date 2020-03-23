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
    public Button submitButton;

    public virtual void init(ITaskListener taskListener, GameObject masterCanvas)
    {
        this.taskListener = taskListener;
        this.masterCanvas = masterCanvas;

        masterCanvas.transform.Find("Overlay - Input").gameObject.SetActive(taskType == TaskType.TYPE_1);
        masterCanvas.transform.Find("Overlay - Option").gameObject.SetActive(taskType == TaskType.TYPE_2);

        String baseOverlay = (taskType == TaskType.TYPE_1) ? "Overlay - Input" : "Overlay - Option";

        Debug.Log("is empty:"+ baseOverlay);

        questionObject = masterCanvas.transform.Find(baseOverlay+"/Popup - Question/Container/Question").gameObject;
        submitButton = masterCanvas.transform.Find(baseOverlay + "/Popup - Question/Container/Button - Submit").GetComponent<Button>();
        submitButton.onClick.AddListener(result);
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