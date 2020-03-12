using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskFactory : ITaskFactory
{

    private static TaskFactory instance = null;
    private QuestionFactory questionFactory;

    public TaskFactory(QuestionFactory questionFactory)
    {
        this.questionFactory = questionFactory;
    }

    public static TaskFactory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TaskFactory(new QuestionFactory());
            }
            return instance;
        }
    }

    public Task getTask(int taskNumber)
    {
        Task task = null;
        switch (taskNumber)
        {
            case 1:
                task = new Task1(questionFactory);
                break;
            case 2:
                task = new Task2(questionFactory);
                break;
            default:
                break;
        }

        return task;
    }
}
