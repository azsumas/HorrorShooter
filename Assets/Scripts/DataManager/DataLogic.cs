using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataLogic
{
    public int slot;
    public GameData.GameState state;

    public DataLogic()
    {
    }    

    public void InitData()
    {
        state = GameData.LoadGame(slot);
    }

    public void SetEnergy(float e)
    {
        state.energy = e;
    }

    public void SetAmmo(float m)
    {
        state.energy = m;
    }

    public void SetPlayerPos()
    {
        state.playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    public void SaveState()
    {
        SetPlayerPos();

        GameData.gameState = state;
        GameData.SaveGame(slot);        
    }
}
