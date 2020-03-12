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
                question = new Question_1("Selector the points which seem to be an outlier","iris");
                break;
            case 2:
                break;
            default:
                break;
        }

        return question;
    }
}

