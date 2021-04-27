using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoStraight : MonoBehaviour
{
    [SerializeField]
    float horizontalDirection;

    Vector3 startingPosition;
    private void Start()
    {
        startingPosition = transform.position;
    }
    void Update()
    {
        transform.position += new Vector3(horizontalDirection, 0, 0) * Time.deltaTime;
        if (Vector3.Distance(transform.position, startingPosition) > 25)
            transform.position = startingPosition;
    }
}
