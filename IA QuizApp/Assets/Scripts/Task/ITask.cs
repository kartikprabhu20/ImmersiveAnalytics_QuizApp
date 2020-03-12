using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITask 
{
    void init(ISubmitButtonListner submitButtonListener);
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