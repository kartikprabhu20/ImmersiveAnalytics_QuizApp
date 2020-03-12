using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITaskFactory
{
    Task getTask(int taskNumber);

}
