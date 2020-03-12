using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public static int COUNTER = 0;
    private TaskFactory taskFactory;
    private ISubmitButtonListner submitButtonListener;

    // Start is called before the first frame update
    void Start()
    {
        COUNTER++;
        taskFactory = TaskFactory.Instance;
        submitButtonListener = new SubmitButtonListener(taskFactory);
        taskFactory.getTask(COUNTER).init(submitButtonListener);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    class SubmitButtonListener : ISubmitButtonListner
    {
        private TaskFactory taskFactory;

        public SubmitButtonListener(TaskFactory taskFactory)
        {
            this.taskFactory = taskFactory;
        }

        public void submitted()
        {
            Debug.Log("Counter:" + COUNTER);
            COUNTER++;
            taskFactory.getTask(COUNTER).init(this);
        }
    }
}

public interface ISubmitButtonListner
{
   void submitted();
}