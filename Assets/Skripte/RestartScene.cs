using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    public void Reload()
    {
        Debug.Log("RELOAD CLICKED");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
