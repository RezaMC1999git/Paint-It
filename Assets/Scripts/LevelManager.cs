using RTLTMPro;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Summary : All Relative Variables To Controll A Level 

    public static LevelManager instance;

    [Header("UI")]
    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public Vector3 endPosition;
    public SpriteRenderer levelShapeComplete, levelShapeBlackAndWhite;
    [SerializeField] GameObject clickCirclePrefab;
    public GameObject dragGuideHand, dragCircle, endPanel;
    [HideInInspector] public bool levelStarted, levelFinished;

    [SerializeField]
    [Range(100, 10000)]
    public int requiredMasksForFinishingLevel = 175;

    [Space]
    [Header("SFX")]
    [SerializeField] AudioSource levelCompleteAudioSource;

    [SerializeField] Queue<AudioClip> fetchedSuras;
    Coroutine suraCoroutine;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        dragCircle.SetActive(false);
        dragGuideHand.SetActive(false);
        StartCoroutine(StartTheGameIE());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateWave();
        }
    }

    void CreateWave()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // to match camera distance
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Instantiate(clickCirclePrefab, new Vector3(worldPosition.x, worldPosition.y, 0), Quaternion.identity);
    }

    public IEnumerator StartTheGameIE()
    {
        levelStarted = true;
        levelShapeComplete.enabled = true;

        dragGuideHand.SetActive(true);
        yield return new WaitForSeconds(2);
        dragGuideHand.SetActive(false);
    }

    public void FinishGame() 
    {
        StartCoroutine(FinishGameIE());
    } 

    IEnumerator FinishGameIE()
    {
        levelFinished = true;
        dragCircle.SetActive(false);
        levelShapeBlackAndWhite.gameObject.SetActive(false);
        levelShapeComplete.maskInteraction = SpriteMaskInteraction.None;
        levelCompleteAudioSource.Play();

        yield return new WaitForSeconds(2f);
        endPanel.SetActive(true);
    }

    public void RepeatLevelOnClick() 
    {
        Application.LoadLevel(SceneManager.GetActiveScene().name);
    }

    public void NextLevelOnClick()
    {
        string levelName = SceneManager.GetActiveScene().name;
        Match match = Regex.Match(levelName, @"\d+"); // Find digits in the string
        if (match.Success)
        {
            int number = int.Parse(match.Value);
            if (number < 2)  // Load Next Level
                SceneManager.LoadScene("Level " + (number + 1).ToString());
            else // Back To Main Menu
            {
                SceneManager.LoadScene("Main Menu");
            }
        }
    }
}