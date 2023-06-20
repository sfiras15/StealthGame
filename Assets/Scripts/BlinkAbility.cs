using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlinkAbility : MonoBehaviour
{
    [Header("References")]
    [Space(10)]

    private Player player;
    [SerializeField] private Transform telportLocation;

    [SerializeField] private GunSystem gun2;
    [SerializeField] private TextMeshProUGUI gunText;

    [SerializeField] private GameObject fps_arm;

    [SerializeField] private GameObject blinkSphere;


    [SerializeField] private float maxBlinkDistance = 10f;

    public bool secondCastActivated = false;

    public bool playAudio = false;



    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void OnEnable()
    {
        Time.timeScale = 0.5f;
        fps_arm.SetActive(true);
        blinkSphere.SetActive(true);
        gun2.gameObject.SetActive(false);
        secondCastActivated = false;
        gunText.enabled = false;
        telportLocation.gameObject.SetActive(true);

    }
    private void OnDisable()
    {
        Time.timeScale = 1f;
        fps_arm.SetActive(false);
        blinkSphere.SetActive(false);
        gun2.enabled = true;
        gun2.gameObject.SetActive(true);
        gunText.enabled = true;
        telportLocation.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float mousePositionDistance = Vector3.Distance(transform.position, player.mousePosition);
        if (mousePositionDistance <= maxBlinkDistance)
        {
            telportLocation.position = player.mousePosition;
            if (Input.GetKeyDown(AbilityUI.instance.GetBlinkKey) )
            {
                
                secondCastActivated = true;
                StartCoroutine(Blink(player.mousePosition));
                playAudio = true;
                this.enabled = false;
            }
        }
        
    }
    public IEnumerator Blink(Vector3 mousePosition)
    {

        float elapsed = 0f;
        float duration = 0.5f;
        Vector3 position = transform.position;
        while (elapsed < duration)
        {
            position = Vector3.Lerp(position, mousePosition, elapsed / duration);

            elapsed += Time.deltaTime;

            transform.position = position;
            yield return null;
        }
        transform.position = mousePosition;
    }
}
