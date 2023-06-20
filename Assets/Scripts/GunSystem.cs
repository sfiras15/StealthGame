using UnityEngine;
using TMPro;
using System.Timers;
using Unity.Collections;
using System.Collections;

public class GunSystem : MonoBehaviour
{
    //Gun stats

    [SerializeField] private int damage;
    [SerializeField] private float timeBetweenShooting, MovingSpread, spread, range, reloadTime, timeBetweenShots;
    [SerializeField] private int magazineSize, bulletsPerTap;
    [SerializeField] private bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //bools 

    private bool shooting, readyToShoot, reloading;

    //Reference
    [SerializeField] private GunSystem nextGun;
    [SerializeField] private Camera fpsCam;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask whatIsEnemy;
    private CharacterController characterController;

    private RaycastHit rayHit;
    //Graphics

    [SerializeField] private GameObject muzzleFlash, bulletHoleGraphic;
    [SerializeField] private CameraShake camShake;

    [SerializeField] private float camShakeMagnitude, camShakeDuration;
    [SerializeField] private TextMeshProUGUI text;

    //Timer

    [SerializeField] private float spreadResetTime;
    private bool timerIsRunning;
    private float lastMovedTime;

    //Audio
    [SerializeField] private AudioClip blastAudio;
    [SerializeField] private float blastVolume = 0.2f;

    [SerializeField] private AudioClip reloadAudio;
    [SerializeField] private float reloadVolume = 0.3f;

    public int GetGunDamage
    {
        get { return damage; }
    }

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }
    private void Update()
    {
        MyInput();

        //SetText
        text.SetText(bulletsLeft + " / " + magazineSize);

    }
    private void MyInput()
    {
        float playerVelocity = characterController.velocity.magnitude;

        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if ((Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) || bulletsLeft == 0) Reload();

        // If the player is moving set a fixed spreadValue
        if (playerVelocity != 0f)
        { 
            spread = MovingSpread;
            lastMovedTime = Time.time;
        }
        else if (playerVelocity == 0 && !timerIsRunning)
        {
            
            timerIsRunning = true;
            
        }

        if (timerIsRunning)
        {
            if (Time.time - lastMovedTime >= spreadResetTime)
            {
                spread = 0f;
                timerIsRunning = false;
            }
            
        }
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
            if (nextGun.gameObject.activeSelf)
            {
                nextGun.enabled = true;
                this.enabled = false;
            }


        }
    }
    private void Shoot()
    {
        readyToShoot = false;

        //Spread

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            //Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Enemy"))
                PlayerManager.instance.HitEnemy(rayHit.collider.gameObject.GetComponent<Enemy>().id);
        }

        //ShakeCamera
        CameraShake.instance.ShakeCam(camShakeDuration, camShakeMagnitude);
        

        //Graphics
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.FromToRotation(Vector3.forward, rayHit.normal));
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.FromToRotation(Vector3.forward, rayHit.normal));

        AudioSource.PlayClipAtPoint(blastAudio, transform.position, blastVolume);
        bulletsLeft--;
        bulletsShot--;
        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
        
    }
    private void Reload()
    {
        reloading = true;
        AudioSource.PlayClipAtPoint(reloadAudio, transform.position, reloadVolume);
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
