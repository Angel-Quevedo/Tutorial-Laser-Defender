using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] Player player;

    Text healthText;

    private void Start()
    {
        healthText = GetComponent<Text>();
    }

    private void Update()
    {
        healthText.text = player.GetHealth().ToString();
    }
}
