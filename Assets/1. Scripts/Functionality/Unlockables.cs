using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlockables : MonoBehaviour
{
    public Hashtable UnlockableTable;

    public List<GameObject> Guns;

    public List<int> Waves;

    public List<int> Quantities;

    public GameObject GetObjectForWave(int wave)
    {
        if (Waves.IndexOf(wave) > -1)
        {
            return Guns[Waves.IndexOf(wave)];
        }

        return null;
    }

    public int GetWuantityForWave(int wave)
    {
        if (Waves.IndexOf(wave) > -1)
        {
            return Quantities[Waves.IndexOf(wave)];
        }

        return 0;
    }
}
