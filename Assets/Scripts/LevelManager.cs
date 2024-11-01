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
    [SerializeField] BoxCollider2D levelShapeBlackAndWhiteColider;
    public GameObject dragGuideHand, dragCircle, endPanel;
    [HideInInspector] public bool levelStarted, levelFinished;
    [SerializeField] RTLTextMeshPro ayeText;
    public int numberOfAyeBalloons;
    public List<float> AyesPercentage;
    [SerializeField] List<string> surasTexts;
    [SerializeField]
    [Range(100, 10000)]
    public int requiredMasksForFinishingLevel = 175;

    [Space]
    [Header("SFX")]
    [SerializeField] AudioSource levelCompleteAudioSource;
    [SerializeField] AudioSource suraAudioSource, bismAllahAudioSource;

    [SerializeField] Queue<AudioClip> fetchedSuras;
    [SerializeField] List<AudioClip> suraSFX;
    Coroutine suraCoroutine;
    bool playBismAllah;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        dragCircle.SetActive(false);
        dragGuideHand.SetActive(false);
        fetchedSuras = new Queue<AudioClip>();

    }

    public void StartTheGame() 
    {
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
        levelShapeBlackAndWhiteColider.enabled = true;
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

    public void PlaySura()
    {
        if (!playBismAllah) 
        {
            bismAllahAudioSource.Play();
            playBismAllah = true;
            return;
        }
        AyesPercentage.RemoveAt(0);
        fetchedSuras.Enqueue(suraSFX[0]);
        suraSFX.RemoveAt(0);
        if (suraCoroutine == null)
        {
            suraCoroutine = StartCoroutine(PlaySuraIE());
        }
    }

    public IEnumerator PlaySuraIE()
    {
        ayeText.gameObject.SetActive(true);
        ayeText.text = surasTexts[0];
        surasTexts.RemoveAt(0);
        suraAudioSource.clip = fetchedSuras.Dequeue();
        suraAudioSource.Play();
        yield return new WaitForSeconds(suraAudioSource.clip.length);
        if (fetchedSuras.Count != 0)
        {
            suraCoroutine = StartCoroutine(PlaySuraIE());
        }
        else
            suraCoroutine = null;
    }
}
