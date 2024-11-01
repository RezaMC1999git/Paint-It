using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonsManager : MonoBehaviour
{
    [SerializeField] LevelManager LevelManager;
    [SerializeField] List<Sprite> balloonSprites;
    [SerializeField] Balloon balloonPrefab;
    [SerializeField] AudioSource balloonPopAudioSource;
    [SerializeField] Transform balloonsParent;
    [SerializeField] int numberOfBalloons;

    List<Balloon> allBalloons;

    private void Start()
    {
        allBalloons = new List<Balloon>();

        for (int i = 0; i < numberOfBalloons; i++)
        {
            Balloon newBalloon = Instantiate(balloonPrefab, balloonsParent);
            newBalloon.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-4, 4), 1f);
            newBalloon.balloonSpriteRenderer.sprite = balloonSprites[Random.Range(0, balloonSprites.Count)];
            allBalloons.Add(newBalloon);
        }

        for (int i = 0; i < LevelManager.numberOfAyeBalloons; i++)
        {
            int index = Random.Range(0, allBalloons.Count);
            allBalloons[index].choosenBalloon = true;
            allBalloons.RemoveAt(index);
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Balloon"))
                {
                    StartCoroutine(PopBalloonIE(hit.transform.gameObject));
                }
            }
        }
    }

    IEnumerator PopBalloonIE(GameObject baloon) 
    {
        balloonPopAudioSource.Play();
        baloon.transform.GetComponent<Balloon>().balloonAnimator.SetTrigger("Pop");
        baloon.transform.GetComponent<Balloon>().balloonCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        Destroy(baloon.gameObject);
        numberOfBalloons--;
        if (baloon.transform.GetComponent<Balloon>().choosenBalloon)
        {
            LevelManager.PlaySura();
        }
        if(numberOfBalloons <= 0) 
        {
            LevelManager.StartTheGame();
        }
    }
}
