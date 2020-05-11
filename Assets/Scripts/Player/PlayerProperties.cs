using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerProperties : MonoBehaviour
{
    public GameObject restartButton;
    public GameObject gameWinSprite;
    public GameObject gameLoseSprite;
    public Text scoreText;
    public Text bestScoreText;
    
    public float loseDistance = 10f;
    public bool win;
    public bool lose;

    public int maxScore;
    private float _baseScore;
    public float scoreMultiplier;
    
    private bool _endOfSession;
    
    public PlayerRecordList records;
    public PlayerRecordList recordsOnline;
    private string _defaultPlayerName;
    public string playerName;
    
    private string _backendUrl = "https://backend-develop-rcppsocprq-ew.a.run.app";
    
    [Serializable] 
    public class PlayerRecord {
        public int score;
        public string name;
    }
    
    public class PlayerRecordList {
        public List<PlayerRecord> RecordsList;
    }
    
    void Awake()
    {
        _defaultPlayerName = "Player " + Random.Range(0, 999) + Random.Range(0, 999);
        playerName = PlayerPrefs.GetString("username", _defaultPlayerName);
    }

    private void Start()
    {
        var jsonString = PlayerPrefs.GetString("recordsStorage");
        records = JsonUtility.FromJson<PlayerRecordList>(jsonString);
        if (records == null) {
            records = new PlayerRecordList
            {
                RecordsList = new List<PlayerRecord>()
            };
        }

        StartCoroutine(LoadOnlineScores());
        
        _baseScore = transform.position.y;
        SetBestScore();
    }

    private void FixedUpdate()
    {
        var score = (transform.position.y - _baseScore) * scoreMultiplier;
        var chosenScore = score > maxScore ? score : maxScore;
        maxScore = (int) Math.Ceiling(chosenScore);
        scoreText.text = "Score: " + maxScore;
        
        if ((!win && !lose) || _endOfSession) return;
        
        AddRecord(maxScore);

        if (win)
        {
            gameWinSprite.SetActive(true);
        }
        else if (lose)
        {
            gameLoseSprite.SetActive(true);

        }
        
        restartButton.SetActive(true);
        _endOfSession = true;
    }
    
    private int GetRecord()
    {
        var localId = -1;

        for (var i = 0; i < records.RecordsList.Count; i++)
        {
            if (records.RecordsList[i].name == playerName)
            {
                localId = i;
                break;
            }
        }

        return localId;
    }

    private void AddRecord(int playerScore) {
        var playerRecord = new PlayerRecord { score = playerScore, name = playerName };

        var localId = GetRecord();
        if (localId != -1)
        {
            if (records.RecordsList[localId].score < playerScore)
            {
                records.RecordsList[localId].score = playerScore;
            }
        }
        else
        {
            records.RecordsList.Add(playerRecord);
        }

        for (int i = 0; i < records.RecordsList.Count; i++)
        {
            for (int k = i + 1; k < records.RecordsList.Count; k++)
            {
                if (records.RecordsList[i].score < records.RecordsList[k].score)
                {
                    var tmp = records.RecordsList[i];
                    records.RecordsList[i] = records.RecordsList[k];
                    records.RecordsList[k] = tmp;
                }
            }
        }

        var json = JsonUtility.ToJson(records);
        PlayerPrefs.SetString("recordsStorage", json);
        PlayerPrefs.Save();
        
        SetBestScore();
    }

    private void SetBestScore()
    {
        int record;
        
        var localId = GetRecord();
        if (localId != -1)
        {
            record = records.RecordsList[localId].score;
        }
        else
        {
            record = 0;
        }
        bestScoreText.text = "Best Score: " + record;
        StartCoroutine(SaveScoreOnline(record));
    }
    
    [Serializable]
    public class UserDataOnline
    {
        public string name;
        public string device_id;
        public int score;
    }

    IEnumerator SaveScoreOnline(int score)
    {
        var userData = new UserDataOnline { name=playerName, device_id=SystemInfo.deviceUniqueIdentifier, score=score };
        var jsonData = JsonUtility.ToJson(userData);
        
        var request = UnityWebRequest.Put(_backendUrl + "/save", jsonData);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.downloadHandler.text == "Different device ID")
        {
            playerName = playerName + " " + Random.Range(0, 999) + Random.Range(0, 999);
            StartCoroutine(SaveScoreOnline(score));
        }
    }

    IEnumerator LoadOnlineScores()
    {
        var request = UnityWebRequest.Get(_backendUrl + "/top");

        yield return request.SendWebRequest();

        recordsOnline = JsonUtility.FromJson<PlayerRecordList>(request.downloadHandler.text);
    }
}
