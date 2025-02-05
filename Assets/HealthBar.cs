using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform progress;
    [Range(0,6)]
    public int lifes = 6;

    public void SetLifes(int lifes)
    {
        progress.localScale = new Vector2((6 - lifes) * 5f / 16f, 5f / 16f);
    }

    private void OnValidate()
    {
        SetLifes(lifes);
    }
}
