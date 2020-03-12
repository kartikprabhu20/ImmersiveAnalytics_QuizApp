using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Question : IQuestion
{
    public string datasetName;
    public string questionText;
    public string answer;

    public string getDatasetName()
    {
        return datasetName;
    }

    public string getText()
    {
        return questionText;
    }

    public void setAnswer(string answer)
    {
        this.answer = answer;
    }
}
