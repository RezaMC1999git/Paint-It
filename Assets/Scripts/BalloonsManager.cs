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

    Vector3 previousBalloonPosition, newBalloonPosition;

    private void Start()
    {
        previousBalloonPosition = new Vector3(Random.Range(-5, 5), Random.Range(-3, 3), 1f);

        for (int i = 0; i < numberOfBalloons; i++)
        {
            Balloon newBalloon = Instantiate(balloonPrefab, balloonsParent);
            newBalloonPosition = new Vector3(Random.Range(-5, 5), Random.Range(-3, 3), 1f);
            while(Vector3.Distance(newBalloonPosition, previousBalloonPosition) <= 2f) 
            {
                newBalloonPosition = new Vector3(Random.Range(-5, 5), Random.Range(-3, 3), 1f);
            }
            newBalloon.transform.position =  newBalloonPosition;
            previousBalloonPosition = newBalloonPosition;
            newBalloon.balloonSpriteRenderer.sprite = balloonSprites[Random.Range(0, balloonSprites.Count)];
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
        if(numberOfBalloons <= 0) 
        {
            LevelManager.StartTheGame();
        }
    }
}
