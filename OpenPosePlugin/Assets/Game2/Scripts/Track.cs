using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public GameObject[] coins;
    public GameObject[] obstacles;
    public Vector2      coinsNumber;
    public Vector2      obstaclesNumber;

    private float trackEndPosition = 314f;

    [SerializeField] private int trackIndex = 0;

    [SerializeField] private int currenTrackIndex = 0;

    void Start()
    {
        //CreateTrackItems(coinsNumber,     coins, true);
        CreateTrackItems(obstaclesNumber, obstacles, false);
    }

    void CreateTrackItems(Vector2 itemsNumber, GameObject[] items, bool coin)
    {
        List<GameObject> itemsList      = new List<GameObject>();
        int              newItemsNumber = (int)Random.Range(itemsNumber.x, itemsNumber.y);

        for (int i = 0; i < newItemsNumber; i++)
        {
            GameObject item           = items[Random.Range(0, items.Length)];
            float      itemPosition   = (trackEndPosition / newItemsNumber) + (trackEndPosition / newItemsNumber) * i;
            float      randomPosition = Random.Range(itemPosition, itemPosition + 1);

            itemsList.Add(Instantiate(item, transform));

            if (coin)
            {
                itemsList[i].transform.localPosition =
                    new Vector3(transform.position.x, transform.position.y + 0.8f, randomPosition);
            }
            else
            {
                itemsList[i].transform.localPosition =
                    new Vector3(transform.position.x, transform.position.y, randomPosition);
            }

            if (itemsList[i].GetComponent<ChangeLane>() != null)
            {
                itemsList[i].GetComponent<ChangeLane>().PositionLane();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().IncreaseSpeed();
            //transform.position = new Vector3(0, 0, transform.position.z + trackEndPosition * 2);

            // if (trackIndex == 0)
            // {
            //     if (currenTrackIndex != 0)
            //     {
            //         transform.position = new Vector3(0, 0, (trackEndPosition * (2 + currenTrackIndex + 1)));
            //     }
            //     else
            //     {
            //         transform.position = new Vector3(0, 0, trackEndPosition * 2);
            //     }
            //     
            //     currenTrackIndex++;
            // }
            // else
            // {
            //     if (transform.position.z == 0)
            //     {
            //         transform.position = new Vector3(0, 0, trackEndPosition * 2);
            //     }
            //     else
            //     {
            //         
            //     }
            //     transform.position = new Vector3(0, 0, transform.position.z * 2);
            // }

            currenTrackIndex += 2;

            transform.position = new Vector3(0, 0, trackEndPosition * currenTrackIndex);

            UnityEngine.Debug.Log(string.Format("Z: {0} | currenTrackIndex: {1}", trackEndPosition * currenTrackIndex,
                                                currenTrackIndex));
        }
    }
}