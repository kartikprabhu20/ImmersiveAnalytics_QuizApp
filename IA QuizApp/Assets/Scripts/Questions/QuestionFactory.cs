using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionFactory : IQuestionFactory
{
    private static QuestionFactory instance = null;

    public static QuestionFactory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new QuestionFactory();
            }
            return instance;
        }
    }

    public Question getQuestion(int questionNumber)
    {
        Question question = null;
        switch (questionNumber)
        {
            case 1:
                question = new Question_1("Enter the Point number which seems to be an outlier", "Dataset/haberman");
                break;
            case 2:
                question = new Question_1("To which cluster does the Point in red color belong to?", "Dataset/iris");
                break;
            case 3:
                question = new Question_1("What is the correlation between the points", "Dataset/correlation1");
                break;
            default:
                break;
        }

        return question;
    }
}

