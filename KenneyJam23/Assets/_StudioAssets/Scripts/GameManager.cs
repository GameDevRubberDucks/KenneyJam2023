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

using UnityEngine;
using UnityEngine.Events;

using RubberDucks.Utilities;
using System.Collections.Generic;
using RubberDucks.KenneyJam.Player;
using RubberDucks.KenneyJam.Jam;


namespace RubberDucks.KenneyJam.GameManager
{
	public class GameManager : PersistentSingleton<GameManager>
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

        //--- Public Variables ---//

        //--- Protected Variables 

        //--- Private Variables ---//
        [Header("Game Settings")]
		

		[Header("Player Variables")]
        [SerializeField] private int m_NumPlayers = 4;
        [SerializeField] private int m_MaxPlayers = 4;
        [SerializeField] private int m_CurrentPlayers = 0;
        Dictionary<int, GameObject> m_PlayerList = new Dictionary<int, GameObject>();

        //--- Unity Methods ---//
        private void OnValidate()
        { 
            m_NumPlayers = (m_NumPlayers > m_MaxPlayers) ? m_MaxPlayers : m_NumPlayers;
        }

        private void Start()
        {
            StartGame();
        }

        //--- Public Methods ---//

        //--- Protected Methods ---//

        //--- Private Methods ---//
        [EditorFunctionCall("Start Game")]
        private void StartGame()
		{
			for(int i =0; i < m_NumPlayers; ++i)
			{
                GameObject newPlayer = PlayerManager.Instance.NewPlayer(m_CurrentPlayers);
                m_PlayerList[m_CurrentPlayers] = newPlayer;
                ++m_CurrentPlayers;
            }
		}

        private void CheckWinner()
        {
            List<int> currentLeaderIndex = new List<int>();
            for(int i = 1; i<m_CurrentPlayers; i++)
            {
                int score1 = m_PlayerList[currentLeaderIndex[0]].GetComponent<PlayerController>().Score;
                int score2 = m_PlayerList[i].GetComponent<PlayerController>().Score;
                if (score1 > score2)
                {
                    //currentLeaderIndex[0] = currentLeaderIndex[0]; Keep the current leader
                }
                else if( score1 < score2)
                {
                    currentLeaderIndex.Clear();
                    currentLeaderIndex.Add(i);
                }
                else if (score1 == score2)
                {
                    currentLeaderIndex.Add(i);
                }
            }
        }
	}
}
