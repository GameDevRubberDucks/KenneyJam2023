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
using DG.Tweening;

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
        [Header("Movement Variables")]
        [SerializeField] private float m_Acceleration = 300.0f;
        private Vector3 m_LastLookDir = Vector3.forward;

        [SerializeField] private GameObject arrow;
        private GameObject[] playerList;
        private Dictionary<Int32, GameObject> m_PlayerList = new Dictionary<int, GameObject>();

        //--- Unity Methods ---//

        private void Start()
        {
            playerList = new GameObject[GameObject.FindGameObjectsWithTag("Player").Length];
            playerList = GameObject.FindGameObjectsWithTag("Player");
        }

        private void Update()
        {
            Vector3 m_velDir = new Vector3(Input.GetAxis(m_InputAxisX), 0.0f, Input.GetAxis(m_InputAxisZ));
            if (Input.GetAxisRaw(m_InputAxisX) != 0 || Input.GetAxisRaw(m_InputAxisZ) != 0)
            {
                m_LastLookDir = m_velDir;
            }
            this.GetComponent<Rigidbody>().velocity += m_Acceleration * m_velDir * Time.deltaTime;
            UpdateRotation(m_LastLookDir);

            //WayFinder();

        }

        //--- Public Methods ---//
        public void InitializePlayer(Int32 playerInd, ref Dictionary<Int32, GameObject> playerList)
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
