using UnityEngine;
using UnityEngine.UI;

internal class Task3 : Task
{
    private QuestionFactory questionFactory;
    private Question question;

    public Task3(QuestionFactory questionFactory)
    {
        this.questionFactory = questionFactory;
    }

    public override void init(ITaskListener taskListener, GameObject masterCanvas, GameObject scatterPlotManager)
    {
        Debug.Log("task3 init");
        setType(TaskType.TYPE_2);
        base.init(taskListener, masterCanvas, scatterPlotManager);
        setState(TaskState.INIT);
        question = questionFactory.getQuestion(3);
        execute();
    }

    public override void execute()
    {
        Debug.Log("task3 execute");
        setState(TaskState.EXECUTION);
        questionObject.GetComponent<Text>().text = question.getText();
        scatterplotManager.GetComponent<ScatterplotGenerator>().reInit(question.getDatasetName());

        masterCanvas.transform.Find(baseQuestionPopup +"/Container/RadioGroup/Option 1/Label").gameObject.GetComponent<Text>().text = "Linear";
        masterCanvas.transform.Find(baseQuestionPopup +"/Container/RadioGroup/Option 2/Label").gameObject.GetComponent<Text>().text = "Non-linear";
        masterCanvas.transform.Find(baseQuestionPopup +"/Container/RadioGroup/Option 3/Label").gameObject.GetComponent<Text>().text = "Planar";
        masterCanvas.transform.Find(baseQuestionPopup +"/Container/RadioGroup/Option 4/Label").gameObject.GetComponent<Text>().text = "Curve-Plane";
    }

    public override void result()
    {
        hideOverlayButton.onClick.Invoke();
        Debug.Log("task3 result");
        setState(TaskState.RESULT);
        taskListener.submitted();
    }
}