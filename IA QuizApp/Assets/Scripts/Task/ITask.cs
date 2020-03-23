using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITask 
{
    void init(ITaskListener taskListener, GameObject masterCanvas);
    void execute();
    void result();
    TaskState getState();
}

public enum TaskState
{
    DEFAULT,
    INIT,
    EXECUTION,
    RESULT
}

public enum TaskType
{
    TYPE_1,// Fill the answer type #Default #Overlay - Input
    TYPE_2 // Multiple choice               #Overlay - Option
}