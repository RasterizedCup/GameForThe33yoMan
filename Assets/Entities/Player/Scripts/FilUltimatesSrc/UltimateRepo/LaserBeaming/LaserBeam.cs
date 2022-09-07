using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LaserBeam : MonoBehaviour
{
    public Camera OrthoWeaponCam;
    public float laserTotalDuration;
    public float laserWindupDuration;
    bool laserEnabled = false;
    public LayerMask contactFilterPriming, contactFilterFiring;
    public GameObject LaserSprite;
    public AudioSource LaserSound;
    Vector2 posDelta;
    public float laserSpriteXScalar;
    public float laserSpriteYScalar;
    public float firstMaxIntensity;
    public float maxIntensity, minIntensity, flickerRate;
    bool flickerIsDescending;
    Light2D LaserLight;
    float laserStartTimeDelta;
    bool initLaser;
    bool isCurrentlyLasering;
    public float damageVal;

    bool toggleParticles;
    public ParticleSystem laserParticles;
    public Light2D worldLight;
    public float worldLightMin, worldLightMax;
    public float worldLightChangeRate;

    Camera attackOrientationCam;
    private void Start()
    {
        toggleParticles = true;
        isCurrentlyLasering = false;
        LaserLight = GameObject.Find("LaserLight").GetComponent<Light2D>();
        LaserLight.intensity = firstMaxIntensity;
        LaserLight.enabled = false;
        flickerIsDescending = true;
        initLaser = true;
        laserEnabled = true;
        attackOrientationCam = GameObject.Find("OrthoTrackingCamera").GetComponent<Camera>();
    }
    public bool HandleLaserBeam()
    {
        if (laserEnabled)
        {
            if (initLaser)
            {
                AdjustUltimateBar.killDisplay = false; // let AUB scripts handle with this
                FilUltimates.currentUltCharge = 0;
                LaserSound.Play();
                laserStartTimeDelta = Time.time;
                initLaser = false;
            }
            if (laserStartTimeDelta + laserWindupDuration < Time.time && laserStartTimeDelta + laserTotalDuration > Time.time)
            {
                isCurrentlyLasering = true;
                LaserLight.enabled = true;
                LaserSprite.GetComponent<SpriteRenderer>().enabled = true;

                // set laser rotation
                Vector3 worldPoint = GetScreenToRayPoint(Input.mousePosition);
                posDelta = worldPoint - transform.position;
                posDelta.Normalize();
                float rotZ = Mathf.Atan2(posDelta.y, posDelta.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

                // handle laser impact data
                RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, posDelta, 1000000, contactFilterPriming);
                setBeamSprite(hit);

                // init laser particles after first adjustment ray adjustment
                if (!laserParticles.isPlaying)
                    laserParticles.Play();

                // handle laser flicker
                handleLaserFlicker();
            }
            else if(laserStartTimeDelta + laserWindupDuration > Time.time)
            {
                worldLight.intensity = Mathf.Max(worldLightMin, worldLight.intensity - (worldLightChangeRate * Time.deltaTime));
            }
            else if(laserStartTimeDelta + laserTotalDuration < Time.time)
            {
                LaserLight.enabled = false;
                LaserSprite.GetComponent<SpriteRenderer>().enabled = false;
                laserEnabled = false;
                isCurrentlyLasering = false;
                if (!laserParticles.isStopped)
                    laserParticles.Stop();
                toggleParticles = true;
            }
        }
        else
        {
            if(worldLight.intensity != 1)
            {
                worldLight.intensity = Mathf.Min(worldLightMax, worldLight.intensity + (worldLightChangeRate * Time.deltaTime));
            }
            else
            {
                laserEnabled = true;
                initLaser = true; // reset laser status cycle
                return false; // attack complete
            }
        }
        return true;
    }

    void handleLaserFlicker()
    {
        if (flickerIsDescending)
        {
            LaserLight.intensity -= (flickerRate * Time.deltaTime);
            if (LaserLight.intensity <= minIntensity)
                flickerIsDescending = false;
        }
        else
        {
            LaserLight.intensity += (flickerRate * Time.deltaTime);
            if (LaserLight.intensity >= maxIntensity)
                flickerIsDescending = true;
        }
    }

    void setBeamSprite(RaycastHit2D[] hit)
    {
        if(hit.Length > 0)
        {
            List<RaycastHit2D> impacts = new List<RaycastHit2D>();
            impacts.AddRange(hit);
            impacts.OrderBy(i => i.distance);
            // get mid point in worldspace
            var targetX = transform.position.x + (impacts[0].point.x - transform.position.x) / 2;
            var targetY = transform.position.y + (impacts[0].point.y - transform.position.y) / 2;

            LaserSprite.transform.position = new Vector3(targetX, targetY, 0);
            LaserSprite.transform.localScale = new Vector3(impacts[0].distance * laserSpriteXScalar, laserSpriteYScalar, 0);

            // set particle position
            laserParticles.transform.position = impacts[0].point;
            laserParticles.Play();

            // set light orientation
            List<Vector3> lightCoords = new List<Vector3>();
            lightCoords.Add(new Vector3(0, 0, 0));
            lightCoords.Add(new Vector3(0, .05f, 0));
            lightCoords.Add(new Vector3(impacts[0].distance, 0, 0));
            lightCoords.Add(new Vector3(impacts[0].distance, .1f, 0));
            for(int i=0; i<lightCoords.Count; i++)
            {
                LaserLight.shapePath[i] = lightCoords[i];
            }

            // check impacts (update to list or tag)
            if (impacts[0].transform.gameObject.name.Contains("MotBot") || impacts[0].transform.gameObject.name.Contains("MottTurret"))
            {
                impacts[0].transform.gameObject.GetComponent<HealthLogicBase>().CurrentHealth -= damageVal * Time.deltaTime;
            }
            if (impacts[0].transform.CompareTag("Missile"))
            {
                impacts[0].transform.gameObject.GetComponent<MissileLogic>().SetExplosionFromExternal();
            }
        }
    }

    Vector3 GetScreenToRayPoint(Vector3 mousePos)
    {
        Ray ray = OrthoWeaponCam.ScreenPointToRay(mousePos);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, posDelta, Color.red);
    }


}
