using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneLoaded;

    public void ChangeScene(string menuToOpen)
    {
        MenuManage.SetTargetMenu(menuToOpen);
        SceneManager.LoadScene(sceneLoaded);
    }
}
