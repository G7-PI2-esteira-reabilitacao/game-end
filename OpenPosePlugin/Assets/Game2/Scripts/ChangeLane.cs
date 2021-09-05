using UnityEngine;

public class ChangeLane : MonoBehaviour
{
    public void PositionLane()
    {
        int randomLane    = Random.Range(0, 100);
        int directionLane = 0;

        if (randomLane >= 50)
        {
            directionLane = 1;
        }
        else
        {
            directionLane = -1;
        }

        transform.position = new Vector3(directionLane, transform.position.y, transform.position.z);
    }
}