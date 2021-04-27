using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalArrow : MonoBehaviour
{
    [SerializeField]
    Transform goalTransform, playerTransform;
    void Update()
    {
        if(playerTransform != null)
        {
            Vector3 vec = goalTransform.position - playerTransform.position;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(vec.x, vec.y) * Mathf.Rad2Deg * (-1));
        }
    }
}
