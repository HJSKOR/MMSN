using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static int Count
    {
        get { return GameManager.Instance.PlayerID == 0 ? 1 : 0; }
    }
    public static float WeaoponSpeed
    {
        get { return GameManager.Instance.PlayerID == 1 ? 1.1f : 1f; }
    }

    public static float WeaoponRate
    {
        get { return GameManager.Instance.PlayerID == 1 ? 0.9f : 1f; }
    }

    public static int Hard
    {
        get { return GameManager.Instance.PlayerID == 2 ? 1 : 0; }
    }
}
