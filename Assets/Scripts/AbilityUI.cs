using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class AbilityUI : MonoBehaviour
{
    public static AbilityUI instance;

    [Header("Dash Ability")]
    [Space(10)]
    [SerializeField] private Image dashAbilityImage;
    [SerializeField] private float dashCooldown = 5;
    [SerializeField] private KeyCode dashKey;
    [SerializeField] private TextMeshProUGUI dashText;
    public DashAbility dashAbility;
    private bool dashIsCooldown = false;

    [Header("Blink Ability")]
    [Space(10)]
    [SerializeField] private Image firstCastImage;
    [SerializeField] private Image firstCastImageCD;
    [SerializeField] private Image secondCastImage;
    [SerializeField] private Image secondCastImageCD;
    [SerializeField] private TextMeshProUGUI blinkText;
    [SerializeField] private KeyCode blinkKey;
    [SerializeField] private float blinkCooldown = 5;
    [SerializeField] private float firstCastWindow = 3;

    private BlinkAbility blinkAbility;
    private bool firstCastActivated = false;
    private bool blinkisCooldown = false;
    

    public KeyCode GetBlinkKey
    {
        get { return blinkKey; }
    }
    private void Awake()
    {
        instance = this;
        dashAbility = FindObjectOfType<Player>().GetComponent<DashAbility>();
        blinkAbility = FindObjectOfType<Player>().GetComponent<BlinkAbility>();
        blinkAbility.enabled = false;
        dashAbility.enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        dashAbilityImage.fillAmount = 0;
        firstCastImageCD.fillAmount = 0;
        secondCastImageCD.fillAmount = 0;
        secondCastImage.enabled = false;
        secondCastImageCD.enabled = false;
        dashText.text = dashKey.ToString();
        blinkText.text = blinkKey.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        DashAbility();
        BlinkAbility();
    }

    private void BlinkAbility()
    {
        if (Input.GetKeyDown(blinkKey) && !blinkAbility.enabled && !blinkisCooldown)
        {
            blinkAbility.enabled = true;
            secondCastImage.enabled = true;
            secondCastImageCD.enabled = true;
            secondCastImageCD.fillAmount = 0;
            firstCastActivated = true;

        }
        if (firstCastActivated)
        {
            secondCastImageCD.fillAmount += 1 / firstCastWindow * Time.unscaledDeltaTime;
        }
        
        // secondCast Window
        if (secondCastImageCD.fillAmount == 1 || blinkAbility.secondCastActivated)
        {
            secondCastImageCD.fillAmount = 0;
            blinkAbility.secondCastActivated = false;
            blinkAbility.enabled = false;
            Time.timeScale = 1f;

            secondCastImage.enabled = false;
            secondCastImageCD.enabled = false;
            blinkisCooldown = true;
            firstCastImageCD.fillAmount = 1;
            
            firstCastActivated = false;
        }

        if (blinkisCooldown)
        {
            
            firstCastImageCD.fillAmount -= (1 / blinkCooldown) * Time.deltaTime;

            if (firstCastImageCD.fillAmount <= 0)
            {
                firstCastImageCD.fillAmount = 0;
                blinkisCooldown = false;
            }
        }
    }

    private void DashAbility()
    {
        if (Input.GetKeyDown(dashKey) && !dashIsCooldown)
        {
            dashIsCooldown = true;
            dashAbility.enabled = true;
            dashAbilityImage.fillAmount = 1;
        }

        if (dashIsCooldown)
        {
            dashAbilityImage.fillAmount -= 1 / dashCooldown * Time.deltaTime;
            if (dashAbilityImage.fillAmount <= 0)
            {
                dashAbilityImage.fillAmount = 0;
                dashIsCooldown = false;
            }
        }
    }
}
