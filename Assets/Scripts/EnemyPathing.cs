using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig currentWaveConfig;
    List<Transform> wayPoints;
    int wayPointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        wayPoints = currentWaveConfig.GetWayPoints();
        transform.position = wayPoints[wayPointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        currentWaveConfig = waveConfig;
    }

    private void Move()
    {
        if (wayPointIndex < wayPoints.Count)
        {
            var targetPosition = wayPoints[wayPointIndex].transform.position;
            var movementThisFrame = currentWaveConfig.GetMoveSpeed() * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

            if (targetPosition == transform.position)
                wayPointIndex++;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
