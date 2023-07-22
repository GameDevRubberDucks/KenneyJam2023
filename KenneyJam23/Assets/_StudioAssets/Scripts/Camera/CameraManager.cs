/*
This script manages the cinemachine cameras.

By: Bo
 
*/
using UnityEngine;
using Cinemachine;
using RubberDucks.KenneyJam.GameManager;
namespace RubberDucks.KenneyJam.Camera
{
    public class CameraManager : MonoBehaviour
    {

        public CinemachineTargetGroup targetGroup;
        public float breakAwayDistance;

        private Vector3 groupCenter;
        private float[] targetDistanceFromCenter;

        // Start is called before the first frame update
        void Start()
        {
            foreach (var player in GameManager.GameManager.Instance.PlayerList)
            {
                targetGroup.AddMember(player.Value.transform, 1.0f, 2.0f);
            }
            targetDistanceFromCenter = new float[targetGroup.m_Targets.Length];
        }

        // Update is called once per frame
        void Update()
        {
            DetchTargetFromGroup();
        }

        /*
        This function will check the distance of each target from the center of the group.
        */
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
}