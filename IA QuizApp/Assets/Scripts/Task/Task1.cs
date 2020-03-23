using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task1 : Task
{
    private QuestionFactory questionFactory;
    private Question question;

    public Task1(QuestionFactory questionFactory)
    {
        this.questionFactory = questionFactory;
    }

    public override void init(ITaskListener taskListener, GameObject masterCanvas)
    {
        setState(TaskState.INIT);
        base.init(taskListener, masterCanvas);
        base.taskListener = taskListener;
        question = questionFactory.getQuestion(1);

        execute();
    }

    public override void execute()
    {
        Debug.Log("task1 execute");
        setState(TaskState.EXECUTION);

        questionObject.GetComponent<UnityEngine.UI.Text>().text = question.getText();
        //result();
    }


    public override void result()
    {
        Debug.Log("task1 result");

        setState(TaskState.RESULT);
        taskListener.submitted();
    }
}
