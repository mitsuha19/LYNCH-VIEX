using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    [Header("Stamina Main Parameters")]
    public float playerStamina = 100.0f;
    [SerializeField] private float maxStamina = 100.0f;
    [SerializeField] private float jumpCost = 20;
    [SerializeField] public bool hasRegenerated = true;
    [SerializeField] public bool weAreSprinting = false;

    [Header("Stamina Regen Parameters")]
    [SerializeField, Range(0, 50)] private float staminaDrain = 0.5f;
    [SerializeField, Range(0, 50)] private float staminaRegen = 0.5f;

    [Header("Stamina Speed Parameters")]
    [SerializeField, Range(0, 50)] private int slowedRunSpeed = 4;
    [SerializeField, Range(0, 50)] private int normalRunSpeed = 8;

    [Header("Stamina UI Elements")]
    [SerializeField, Range(0, 50)] private Image staminaProgressUI = null;
    [SerializeField, Range(0, 50)] private CanvasGroup sliderCanvasGroup = null;

    private FirstPersonController playerController;

    private void Awake()
    {
        playerController = GetComponent<FirstPersonController>();
    }

    private void Update()
    {
        if (!weAreSprinting)
        {
            if(playerStamina <= maxStamina - 0.01)
            {
                playerStamina += staminaRegen * Time.deltaTime;

                if(playerStamina >= maxStamina)
                {
                    hasRegenerated = true;
                }
            }
        }
    }

    public void Sprinting()
    {
        if (hasRegenerated)
        {
            weAreSprinting = true;
            playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina(1);

            if(playerStamina <= 0)
            {
                hasRegenerated = false;
                sliderCanvasGroup.alpha = 0; // We might change to a 5s delay
            }
        }
    }

    public void StaminaJump()
    {
        if(playerStamina >= (maxStamina * jumpCost / maxStamina))
        {
            playerStamina -= jumpCost;
            playerController.HandleJump();
            UpdateStamina(1);
        }
    }

    void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;

        if(value == 0)
        {
            sliderCanvasGroup.alpha = 0;
        }
        else
        {
            sliderCanvasGroup.alpha = 1;
        }
    }
}
