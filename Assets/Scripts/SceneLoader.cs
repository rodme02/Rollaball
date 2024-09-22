using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // This method will be called when the "Start Game" button is clicked
    public void StartGame()
    {
        // Load the GameScene and ensure the MainMenu is fully unloaded
        SceneManager.LoadScene("MiniGame", LoadSceneMode.Single);  // Replace "GameScene" with the actual name of your game scene
        
        // Rebuild the lighting environment after the scene is loaded to avoid lighting artifacts
        DynamicGI.UpdateEnvironment();
    }

    // This method will be called when the "Quit" button is clicked
    public void QuitGame()
    {
        Debug.Log("Quit the game!");
        Application.Quit();  // Exits the game application
    }
}
