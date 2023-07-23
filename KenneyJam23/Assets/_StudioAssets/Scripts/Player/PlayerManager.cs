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
using TMPro;

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
        Dictionary<int, GameObject> m_PlayerList = new Dictionary<int, GameObject>();

        //--- Unity Methods ---//

        //--- Public Methods ---//
        public GameObject NewPlayer(int playerIndex)
        {
            GameObject newPlayer = Instantiate(m_PlayerPrefab, m_SpawnLocations[playerIndex].position, Quaternion.identity);
            m_PlayerList[playerIndex] = newPlayer;
            newPlayer.GetComponent<PlayerController>().InitializePlayer(playerIndex, m_PlayerList);
            return newPlayer;
        }

        //--- Protected Methods ---//

        //--- Private Methods ---//
    }
}
