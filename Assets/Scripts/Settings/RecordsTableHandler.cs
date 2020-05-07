using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordsTableHandler : MonoBehaviour
{
    public GameObject player;
    
    private Transform _entryContainer;
    private Transform _entryTemplate;
    private List<Transform> _highscoreEntryTransformList;
    public int tableSize = 10;

    private static int Hex_to_Dec(string hex) {
        return Convert.ToInt32(hex, 16);
    }

    private static float Hex_to_Dec01(string hex) {
        return Hex_to_Dec(hex)/255f;
    }

    private static Color GetColorFromString(string color) {
        float red = Hex_to_Dec01(color.Substring(0,2));
        float green = Hex_to_Dec01(color.Substring(2,2));
        float blue = Hex_to_Dec01(color.Substring(4,2));
        float alpha = 1f;
        if (color.Length >= 8) {
            // Color string contains alpha
            alpha = Hex_to_Dec01(color.Substring(6,2));
        }
        return new Color(red, green, blue, alpha);
    }

    private void Start()
    {
        var playerProperties = player.GetComponent<PlayerProperties>();
        _entryContainer = transform.Find("highscoreEntryContainer");
        _entryTemplate = _entryContainer.Find("highscoreEntryTemplate");

        _entryTemplate.gameObject.SetActive(false);

        var highscores = playerProperties.records;

        _highscoreEntryTransformList = new List<Transform>();
        var tableLimit = (highscores.RecordsList.Count > tableSize) ? tableSize : highscores.RecordsList.Count;
        for (var i = 0; i < tableLimit; i++)
        {
            var highscoreEntry = highscores.RecordsList[i];
            CreateHighscoreEntryTransform(highscoreEntry, _entryContainer, _highscoreEntryTransformList);
        }
    }

    private void CreateHighscoreEntryTransform(PlayerProperties.PlayerRecord highscoreEntry, Transform container, List<Transform> transformList) {
        float templateHeight = 31f;
        Transform entryTransform = Instantiate(_entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank) {
        default:
            rankString = rank + "TH"; break;

        case 1: rankString = "1ST"; break;
        case 2: rankString = "2ND"; break;
        case 3: rankString = "3RD"; break;
        }

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        int score = highscoreEntry.score;

        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        string entryName = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = entryName;

        // Set background visible odds and evens, easier to read
        entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);
        
        // Highlight First
        if (rank == 1) {
            entryTransform.Find("posText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<Text>().color = Color.green;
        }

        // Set tropy
        switch (rank) {
        default:
            entryTransform.Find("trophy").gameObject.SetActive(false);
            break;
        case 1:
            entryTransform.Find("trophy").GetComponent<Image>().color = GetColorFromString("FFD200");
            break;
        case 2:
            entryTransform.Find("trophy").GetComponent<Image>().color = GetColorFromString("C6C6C6");
            break;
        case 3:
            entryTransform.Find("trophy").GetComponent<Image>().color = GetColorFromString("B76F56");
            break;

        }

        transformList.Add(entryTransform);
    }
}
