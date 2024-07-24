using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void StartScene()
    {
        // Load the "Start" scene using its scene name
        SceneManager.LoadScene("GAME");
    }

    public void TestingScene()
    {
        // Load the "GeorgTesting" scene using its scene name
        SceneManager.LoadScene("GeorgTesting");
    }

    public void Quit()
    {
        // Quit the application
        Application.Quit();
    }
}
