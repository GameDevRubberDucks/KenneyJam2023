/*
This script manages the cinemachine cameras.

By: Bo
 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{

    public CinemachineTargetGroup targetGroup;
    public float breakAwayDistance;

    private Vector3 groupCenter;
    private float[] targetDistanceFromCenter;
    private int[] targetWeight;

    // Start is called before the first frame update
    void Start()
    {
        targetWeight = new int[targetGroup.m_Targets.Length];
        targetDistanceFromCenter = new float[targetGroup.m_Targets.Length];
    }

    // Update is called once per frame
    void Update()
    {
        DetchTargetFromGroup();
    }

    void DetchTargetFromGroup()
    {
        //store location of the center of the group
        groupCenter = targetGroup.transform.position;

        for (int i = 0; i < targetGroup.m_Targets.Length; i++)
        {
            targetDistanceFromCenter[i] = Vector3.Distance(groupCenter, targetGroup.m_Targets[i].target.position);

            //NOTE: there should be a way to have this not setting the weights every frame
            if (targetDistanceFromCenter[i] > breakAwayDistance)
            {
                targetGroup.m_Targets[i].weight = 0;
            }
            else
            {
                targetGroup.m_Targets[i].weight = 1;
            }
        }

    }
}
