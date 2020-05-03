using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettigsHandler : MonoBehaviour
{
    public GameObject player;
    public InputField usernameField;
    public GameObject settingsPanel;
    public GameObject recordsPanel;

    private PlayerProperties _playerProperties;

    private void Awake()
    {
        _playerProperties = player.GetComponent<PlayerProperties>();
        usernameField.text = PlayerPrefs.GetString("username", _playerProperties.playerName);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetString("username", usernameField.text);
        PlayerPrefs.Save();
        _playerProperties.playerName = usernameField.text;
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OpenRecords()
    {
        recordsPanel.SetActive(true);
    }
    
    public void CloseRecords()
    {
        recordsPanel.SetActive(false);
    }
}
