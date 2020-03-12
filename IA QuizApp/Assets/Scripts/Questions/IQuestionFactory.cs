using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestionFactory
{
    Question getQuestion(int questionNumber);
}
