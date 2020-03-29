using UnityEngine;
using UnityEngine.UI;

internal class Task2 : Task
{
    private QuestionFactory questionFactory;
    private Question question;

    public Task2(QuestionFactory questionFactory)
    {
        this.questionFactory = questionFactory;
    }

    public override void init(ITaskListener taskListener, GameObject masterCanvas, GameObject scatterPlotManager)
    {
        setType(TaskType.TYPE_2);
        base.init(taskListener, masterCanvas,scatterPlotManager);
        setState(TaskState.INIT);
        question = questionFactory.getQuestion(2);
        execute();
    }

    public override void execute()
    {
        Debug.Log("task2 execute");
        setState(TaskState.EXECUTION);
        questionObject.GetComponent<Text>().text = question.getText();
        scatterplotManager.GetComponent<ScatterplotGenerator>().reInit(question.getDatasetName());

        masterCanvas.transform.Find(baseQuestionPopup + "/Container/RadioGroup/Option 1/Label").gameObject.GetComponent<Text>().text = "Orange";
        masterCanvas.transform.Find(baseQuestionPopup + "/Container/RadioGroup/Option 2/Label").gameObject.GetComponent<Text>().text = "Green";
        masterCanvas.transform.Find(baseQuestionPopup + "/Container/RadioGroup/Option 3/Label").gameObject.GetComponent<Text>().text = "Yellow";
        masterCanvas.transform.Find(baseQuestionPopup + "/Container/RadioGroup/Option 4/Label").gameObject.GetComponent<Text>().text = "None";

    }


    public override void result()
    {
        //hideOverlayButton.onClick.Invoke();
        Debug.Log("task2 result");
        setState(TaskState.RESULT);
        taskListener.submitted();

    }
}