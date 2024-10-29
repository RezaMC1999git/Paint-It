using System.Collections.Generic;
using UnityEngine;

public class Scratch : MonoBehaviour
{
    // Summary : An Attempt to Attaching Mask to LineRenderer, which has Failed !

    [SerializeField] SpriteRenderer bwSpriteRenderer;
    [SerializeField] LevelManager levelManager;
    [SerializeField] GameObject scratchMaskPrefab;
    [SerializeField] Transform scratchMasksParent;

    List<ScratchMask> scratckMasksCreated;
    Vector2 mousePosition;
    int pointsCounter = 0;

    private void Start()
    {
        scratckMasksCreated = new List<ScratchMask>();
    }

    private void OnMouseDrag()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (scratckMasksCreated.Count >= levelManager.requiredMasksForFinishingLevel)
        {
            Destroy(scratchMasksParent.gameObject);
            levelManager.FinishGame();
            return;
        }
        if (levelManager.levelStarted)
        {
            if (!levelManager.dragCircle.activeInHierarchy)
            {
                levelManager.dragCircle.SetActive(true);
            }
            levelManager.dragCircle.transform.position = mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100);
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("BlackAndWhite"))
                {
                    if (scratckMasksCreated.Count >= 3)
                    {
                        for (int i = scratckMasksCreated.Count - 3; i < scratckMasksCreated.Count; i++)
                        {
                            if (Vector3.Distance(hit.point, scratckMasksCreated[i].transform.position) >= 0.2f)
                            {
                                pointsCounter++;
                                continue;
                            }
                        }
                        if(pointsCounter == 3)
                            AddMask();
                        pointsCounter = 0;
                    }
                    else
                    {
                        AddMask();
                    }
                }
            }
        }
    }

    private void AddMask()
    {
        ScratchMask newMask = ScratchMaskObjectPool.instance.getMaskPooledObject();

        if (newMask != null)
        {
            newMask.transform.position = mousePosition;
            newMask.gameObject.SetActive(true);
            newMask.waterEffectAnimtor.enabled = true;
        }
        scratckMasksCreated.Add(newMask);
    }
}
