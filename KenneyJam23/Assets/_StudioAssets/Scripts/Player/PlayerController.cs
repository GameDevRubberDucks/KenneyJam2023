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

using RubberDucks.Utilities;

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
        public int Score
        {
            get => m_Score;
            set => m_Score = value;
        }
        public bool IsCuttingTrees
        {
            get => m_IsCuttingTrees;
            set => m_IsCuttingTrees = value;
        }

        //--- Public Variables ---//

        //--- Protected Variables ---//

        //--- Private Variables ---//
        [Header("Player Variables")]
        [SerializeField] private int m_PlayerIndex = -1;
        [SerializeField] private int m_Score = 0;
        [SerializeField] private string m_InputAxisX = string.Empty;
        [SerializeField] private string m_InputAxisZ = string.Empty;

        [Header("Movement Variables")]
        [SerializeField] private float m_Acceleration = 300.0f;
        [SerializeField] private float m_CuttingTreesSpeedMultiplier = 0.5f;

        private Vector3 m_LastLookDir = Vector3.forward;
        private bool m_IsCuttingTrees = false;

        [SerializeField] private GameObject arrow;
        private GameObject[] playerList;
        private Dictionary<int, GameObject> m_PlayerList = new Dictionary<int, GameObject>();

        //--- Unity Methods ---//

        private void Start()
        {
            playerList = new GameObject[GameObject.FindGameObjectsWithTag("Player").Length];
            playerList = GameObject.FindGameObjectsWithTag("Player");
        }

        private void Update()
        {
            float speedMultiplier = (IsCuttingTrees) ? m_CuttingTreesSpeedMultiplier : 1.0f;
            Vector3 m_velDir = Vector3.Normalize(new Vector3(Input.GetAxis(m_InputAxisX), 0.0f, Input.GetAxis(m_InputAxisZ)));
            if (Input.GetAxisRaw(m_InputAxisX) != 0 || Input.GetAxisRaw(m_InputAxisZ) != 0)
            {
                m_LastLookDir = m_velDir;
            }
            this.GetComponent<Rigidbody>().velocity += m_Acceleration * m_velDir * Time.deltaTime * speedMultiplier;
            UpdateRotation(m_LastLookDir);

            //WayFinder();

        }

        //--- Public Methods ---//
        public void InitializePlayer(int playerInd, ref Dictionary<int, GameObject> playerList)
        {
            m_PlayerIndex = playerInd;
            m_InputAxisX = "Horizontal" + playerInd.ToString();
            m_InputAxisZ = "Vertical" + playerInd.ToString();
            m_PlayerList = playerList;
        }
        //--- Protected Methods ---//

        //--- Private Methods ---//
        void UpdateRotation(Vector3 lookAt)
        {
            this.transform.LookAt(this.transform.position + lookAt);
        }

        /*
        void WayFinder()
        {
            Debug.Log(playerList.Length);
            foreach (var player in playerList)
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
        */
    }
}
