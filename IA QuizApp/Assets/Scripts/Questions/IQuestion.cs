using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQuestion
{
    string getText();
    void setAnswer(string answer);
    string getDatasetName();

}
