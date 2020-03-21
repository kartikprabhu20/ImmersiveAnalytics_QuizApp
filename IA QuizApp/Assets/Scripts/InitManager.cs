using UnityEngine;
using UnityEngine.SceneManagement;

public class InitManager : MonoBehaviour
{
    public void OnClickSceneChange(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
