using UnityEngine;

internal class Task2 : Task
{
    private QuestionFactory questionFactory;
    private Question question;

    public Task2(QuestionFactory questionFactory)
    {
        this.questionFactory = questionFactory;
    }

    public override void init(ITaskListener submitButtonListener, GameObject masterCanvas)
    {
        setType(TaskType.TYPE_2);
        base.init(submitButtonListener, masterCanvas);
        setState(TaskState.INIT);
        question = questionFactory.getQuestion(2);
        execute();
    }

    public override void execute()
    {
        Debug.Log("task2 execute");
        setState(TaskState.EXECUTION);
        //result();
    }


    public override void result()
    {
        Debug.Log("task2 result");
        setState(TaskState.RESULT);
        taskListener.submitted();

    }
}