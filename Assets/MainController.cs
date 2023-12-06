using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    void Update()
    {
        if(Input.anyKeyDown)
        {
            SceneManager.LoadScene(1);
        }   
    }
}
