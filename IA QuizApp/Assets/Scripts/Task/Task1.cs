using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task1 : Task
{
    private QuestionFactory questionFactory;
    private Question question;

    public Task1(QuestionFactory questionFactory)
    {
        this.questionFactory = questionFactory;
    }

    public override void init(ITaskListener taskListener, GameObject masterCanvas, GameObject scatterPlotManager)
    {
        base.init(taskListener, masterCanvas,scatterPlotManager);
        setState(TaskState.INIT);
        base.taskListener = taskListener;
        question = questionFactory.getQuestion(1);
        execute();
    }

    public override void execute()
    {
        Debug.Log("task1 execute");
        setState(TaskState.EXECUTION);
        questionObject.GetComponent<Text>().text = question.getText();
        scatterplotManager.GetComponent<ScatterplotGenerator>().initPlot(question.getDatasetName());
        //result();
    }


    public override void result()
    {
        hideOverlayButton.onClick.Invoke();
        masterCanvas.transform.Find("Overlay - Input").gameObject.SetActive(false);
        Debug.Log("task1 result");
        setState(TaskState.RESULT);
        taskListener.submitted();
    }
}
