using System.Collections;
using System.Collections.Generic;
using RubberDucks.KenneyJam.GameManager;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void SetPlayerCount(int count)
    {
        GameManager.m_NumPlayers = count;
    }
}
