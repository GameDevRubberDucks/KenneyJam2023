/*
 * PlayerController.cs
 * 
 * Description:
 * - Controls for the player objects
 * 
 * Author(s): 
 * - Kody Wood
*/

using System;
using System.Collections.Generic;

using UnityEngine;

using RubberDucks.KenneyJam.Interactions;
using System;
using RubberDucks.KenneyJam.Level;

namespace RubberDucks.KenneyJam.Player
{
    public class PlayerController : MonoBehaviour
    {
        //--- Events ---//
        [System.Serializable]
        public class EventList
        {
        }
        [Header("Events")]
        public EventList Events = default;

        //--- Properties ---//
        //public Int32 PlayerIndex
        //{
        //    get => m_PlayerIndex; 
        //    set => m_PlayerIndex = value;
        //}

        //--- Public Variables ---//

        //--- Protected Variables ---//

        //--- Private Variables ---//
        [SerializeField] private Int32 m_PlayerIndex = -1;
        [SerializeField] private string m_InputAxisX = string.Empty;
        [SerializeField] private string m_InputAxisZ = string.Empty;
        [SerializeField] private string m_TransformInput = string.Empty;

        [Header("Movement Variables")]
        [SerializeField] private float m_Acceleration = 300.0f;
        [SerializeField] private float m_BulldozerSpeedMultiplier = 0.5f;

        [Header("Vehicle Components")]
        [SerializeField] private GameObject m_TruckVisuals = default;
        [SerializeField] private GameObject m_BulldozerVisuals = default;
        [SerializeField] private LevelPathCutter m_ForestCutterComp = default;
        [SerializeField] private CollisionInteractions m_InteractionsComp = default;

        private Vector3 m_LastLookDir = Vector3.forward;
        private bool m_IsTruckForm = true;

        [SerializeField] private GameObject arrow;
        private Dictionary<Int32, GameObject> m_PlayerList = new Dictionary<int, GameObject>();

        //--- Unity Methods ---//
        private void Update()
        {
            float speedMultiplier = (m_IsTruckForm) ? 1.0f : m_BulldozerSpeedMultiplier;
            Vector3 m_velDir = Vector3.Normalize(new Vector3(Input.GetAxis(m_InputAxisX), 0.0f, Input.GetAxis(m_InputAxisZ)));
            if (Input.GetAxisRaw(m_InputAxisX) != 0 || Input.GetAxisRaw(m_InputAxisZ) != 0)
            {
                m_LastLookDir = m_velDir;
            }
            this.GetComponent<Rigidbody>().velocity += m_Acceleration * m_velDir * Time.deltaTime * speedMultiplier;
            UpdateRotation(m_LastLookDir);

            WayFinder();

            if (Input.GetButtonDown(m_TransformInput))
            {
                TryTransform(!m_IsTruckForm);
            }
        }

        //--- Public Methods ---//
        public void InitializePlayer(Int32 playerInd, Dictionary<Int32, GameObject> playerList)
        {
            m_PlayerIndex = playerInd;
            m_InputAxisX = "Horizontal" + playerInd.ToString();
            m_InputAxisZ = "Vertical" + playerInd.ToString();
            m_TransformInput = "Transform" + playerInd.ToString();
            m_PlayerList = playerList;
        }

        public void TryTransform(bool toTruck)
        {
            Debug.Log("Trying transform for player with index " + m_PlayerIndex);

            if (m_InteractionsComp.m_IsCarrying)
            {
                toTruck = true;
            }

            m_TruckVisuals.SetActive(toTruck);
            m_BulldozerVisuals.SetActive(!toTruck);

            m_ForestCutterComp.CanCutTrees = !toTruck;

            m_IsTruckForm = toTruck;
        }

        //--- Protected Methods ---//

        //--- Private Methods ---//
        void UpdateRotation(Vector3 lookAt)
        {
            this.transform.LookAt(this.transform.position + lookAt);
        }


        void WayFinder()
        {
            foreach (var player in m_PlayerList.Values)
            {
                if (gameObject.GetComponent<CollisionInteractions>().m_IsCarrying)
                {
                    GameObject lol = GameObject.FindGameObjectWithTag("Drop");
                    if (lol)
                    {
                        arrow.transform.LookAt(lol.transform);
                    }

                }
                else if (player.GetComponent<CollisionInteractions>().m_IsCarrying)
                {
                    arrow.transform.LookAt(player.transform);
                }
                else if (GameObject.FindGameObjectWithTag("Pickup") != null)
                {
                    arrow.transform.LookAt(GameObject.FindGameObjectWithTag("Pickup").transform);
                }

            }
        }
    }
}
