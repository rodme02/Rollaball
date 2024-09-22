using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Ensure that only the game scene is loaded, and the MainMenu scene is completely unloaded.
        SceneManager.LoadScene("MiniGame");
    }

    public void QuitGame()
    {
        Debug.Log("Quit the game!");
        Application.Quit();
    }
}
