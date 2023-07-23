/*
 * GameManager.cs
 * 
 * Description:
 * - Script that manages the initialization of main objects in the scene.
 * - Handles game logic like end game, winner and players
 * 
 * Author(s): 
 * - Kody Wood
*/

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using RubberDucks.Utilities;
using RubberDucks.Utilities.Timing;
using RubberDucks.KenneyJam.Jam;
using RubberDucks.KenneyJam.Player;

namespace RubberDucks.KenneyJam.GameManager
{
    public class GameManager : Singleton<GameManager>
    {
        //--- Events ---//
        [System.Serializable]
        public class EventList
        {
        }
        [Header("Events")]
        public EventList Events = default;

        //--- Properties ---//
        public int NumPlayers
        {
            set => m_NumPlayers = value;
        }
        public Dictionary<int, GameObject> PlayerList
        {
            get => m_PlayerList;
        }
        public Timer GameTimer
        {
            get => m_GameTimer;
        }

        //--- Public Variables ---//
        public static int m_NumPlayers = 4;
        //--- Protected Variables 

        //--- Private Variables ---//
        [Header("Player Variables")]
        //public static int m_NumPlayers = 4;
        [SerializeField] private int m_MaxPlayers = 4;
        [SerializeField] private int m_CurrentPlayers = 0;
        Dictionary<int, GameObject> m_PlayerList = new Dictionary<int, GameObject>();

        [Header("Timer Variables")]
        [SerializeField] private Timer m_GameTimer = new Timer();
        [SerializeField] private float m_GameDuration = 180.0f;

        //--- Unity Methods ---//
        private void OnValidate()
        {
            m_NumPlayers = (m_NumPlayers > m_MaxPlayers) ? m_MaxPlayers : m_NumPlayers;
        }

        private void Start()
        {
            Time.timeScale = 1.0f;
            StartGame();

        }
        private void Update()
        {
            m_GameTimer.UpdateTimer(Time.deltaTime);
        }

        //--- Public Methods ---//

        //--- Protected Methods ---//

        //--- Private Methods ---//
        [EditorFunctionCall("Start Game")]
        private void StartGame()
        {
            for (int i = 0; i < m_NumPlayers; ++i)
            {
                GameObject newPlayer = PlayerManager.Instance.NewPlayer(m_CurrentPlayers);
                m_PlayerList[m_CurrentPlayers] = newPlayer;
                ++m_CurrentPlayers;
            }

            m_GameTimer.StartTimer(m_GameDuration);
            m_GameTimer.m_Events.OnFinished.AddListener(CheckWinner);
            Debug.Log(m_PlayerList.Count);
        }

        private void CheckWinner()
        {
            Time.timeScale = 0.0f;
            List<int> currentLeaderIndex = new List<int>(0);
            currentLeaderIndex.Add(0);
            for (int i = 1; i < m_CurrentPlayers; i++)
            {
                int score1 = m_PlayerList[currentLeaderIndex[0]].GetComponent<PlayerController>().Score;
                int score2 = m_PlayerList[i].GetComponent<PlayerController>().Score;
                if (score1 > score2)
                {
                    //currentLeaderIndex[0] = currentLeaderIndex[0]; Keep the current leader
                }
                else if (score1 < score2)
                {
                    currentLeaderIndex.Clear();
                    currentLeaderIndex.Add(i);
                }
                else if (score1 == score2)
                {
                    currentLeaderIndex.Add(i);
                }
            }
            Debug.Log($"There are {currentLeaderIndex.Count} Winners");
            for (int i = 0; i < currentLeaderIndex.Count; ++i)
            {
                Debug.Log($"Player {m_PlayerList[currentLeaderIndex[i]].GetComponent<PlayerController>().PlayerIndex + 1} wins");
            }
        }
    }
}
