using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnStartGamePressed()
    {
        SceneManager.LoadScene("sampleScene");
        
    }

    // Update is called once per frame
    public void OnQuitPressed()
    {
        Application.Quit();
        
    }
}
