using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private string sceneLoaded;

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneLoaded);
    }
}
