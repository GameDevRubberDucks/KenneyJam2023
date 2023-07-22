/*
 * PlayerManager.cs
 * 
 * Description:
 * - Handles Players joining,  droping  and Assigning player input
 * 
 * Author(s): 
 * - Kody Wood
*/

using UnityEngine;
using UnityEngine.Events;

using RubberDucks.Utilities;
using System;
using System.Collections.Generic;
using RubberDucks.KenneyJam.Player;
using FullscreenEditor;

namespace RubberDucks.KenneyJam.Jam
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        //--- Events ---//
        [System.Serializable]
        public class EventList
        {
        }
        [Header("Events")]
        public EventList Events = default;

        //--- Properties ---//

        //--- Public Variables ---//

        //--- Protected Variables ---//

        //--- Private Variables ---//
        [Header("Dependencies")]
        [SerializeField] private GameObject m_PlayerPrefab = default;
        [SerializeField] private List<Transform> m_SpawnLocations = new List<Transform>();

        [Header("Player Variables")]
        [SerializeField] private Int32 m_MaxPlayers = 4;
        [SerializeField] private Int32 m_CurrentPlayers = 0;
        Dictionary<Int32, GameObject> m_PlayerList = new Dictionary<Int32, GameObject>();

        //--- Unity Methods ---//
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P) && m_CurrentPlayers + 1 <= m_MaxPlayers)
            {
                GameObject newPlayer = Instantiate(m_PlayerPrefab, m_SpawnLocations[m_CurrentPlayers].position, Quaternion.identity);
                m_PlayerList[m_CurrentPlayers] = newPlayer;
                newPlayer.GetComponent<PlayerController>().InitializePlayer(m_CurrentPlayers, m_PlayerList);
                ++m_CurrentPlayers;
            }
            if (Input.GetKeyDown(KeyCode.O) && m_CurrentPlayers - 1 != 0)
            {
                if (m_PlayerList.ContainsKey(m_CurrentPlayers - 1))
                {
                    --m_CurrentPlayers;
                    GameObject tempPlayer = m_PlayerList[m_CurrentPlayers];
                    m_PlayerList.Remove(m_CurrentPlayers);
                    Destroy(tempPlayer);
                }
            }
        }

        //--- Public Methods ---//

        //--- Protected Methods ---//

        //--- Private Methods ---//
        private void NewPlayer()
        {

        }
    }
}
