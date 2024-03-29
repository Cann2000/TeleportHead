using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Level> Levels = new();

    public int LevelIndex;

    [SerializeField] private GameObject LoadPanel;

    private GameObject CurrentLevel;

    private void Awake()
    {
        LoadData();
        LoadLevel(0, false);
    }

    private void Start()
    {
        GameManager.Instance.GameReady.Invoke();
    }

    public void LoadLevel(int value, bool loadpanel = true)
    {
        if (Levels.Count == 0)
        {
            if (loadpanel)
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);

            }

            return;
        }

        LevelIndex += value;
        LevelIndex %= Levels.Count;

        if (!loadpanel)
        {
            ShowLoadPanel();
        }
        else
        {
            LoadPanel.SetActive(true);
            Destroy(CurrentLevel, 1);
            Invoke(nameof(ShowLoadPanel), 1);

        }
    }

    public void ShowLoadPanel()
    {
        GameManager.Instance.GameReady.Invoke();
        if (CurrentLevel) Destroy(CurrentLevel);
        if (Levels[LevelIndex]) CurrentLevel = Instantiate(Levels[LevelIndex].LevelPrefab) as GameObject;
    }

    public void LevelSave()
    {
        SaveData();
    }

    void LoadData()
    {
        LevelIndex = PlayerPrefs.GetInt("Level", 0);
    }

    void SaveData()
    {
        PlayerPrefs.SetInt("Level", LevelIndex);
    }
}
