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

    public override void init(ISubmitButtonListner submitButtonListener)
    {
        setState(TaskState.INIT);
        base.submitButtonListener = submitButtonListener;
        question = questionFactory.getQuestion(1);
        execute();
    }

    public override void execute()
    {
        Debug.Log("task1 execute");
        setState(TaskState.EXECUTION);
        result();
    }


    public override void result()
    {
        Debug.Log("task1 result");

        setState(TaskState.RESULT);
        submitButtonListener.submitted();
    }
}
