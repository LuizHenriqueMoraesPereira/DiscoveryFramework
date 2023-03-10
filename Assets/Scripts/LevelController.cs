using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : SceneController
{
    public static LevelController CurrentLevel;

    public static float LevelTimer;

    public static float GlobalTimer;

    public static bool AllowTime;
    public static bool AllowPause;
    public static bool PauseTrigger;
    public static bool Paused;
    public static bool Boss;

    public static bool CheckPoint;
    public static float CheckPointX;
    public static float CheckPointY;
    public static float CheckPointLevelTime;
    public static float CheckPointGameTime;

    public bool Water;
    public float WaterLevel;
    
    public float GameTimer;
    public int Milliseconds;
    public int Seconds;
    public int Minutes;

    public int Rings;

    private static int RingsInThisRow;
    private static float CreationStrength;
    private static int CreationDirection = 1;
    private static float CreationAngle = 101.25f;

    private new void Awake()
    {
        Paused = false;
        AllowTime = AllowPause = true;

        base.Awake();

        CurrentLevel = this;
    }

    private new void Start()
    {
        CreationAngle = 101.25f;

        base.Start();
    }

    private void FixedUpdate()
    {
        Rings = Mathf.Clamp(Rings, 0, 999);

        if (AllowPause)
        {
            if (PauseTrigger)
            {
                Paused = !Paused;

                foreach (BaseObject objRef in ObjectList)
                {
                    objRef.enabled = !Paused;
                }

                foreach (Animator animator in FindObjectsOfType<Animator>())
                {
                    animator.enabled = !Paused;
                }

                /*foreach (Attacher attacher in FindObjectsOfType<Attacher>())
                {
                    attacher.enabled = !Paused;
                }*/

                if (Paused) AudioController.Pause();
                else AudioController.Resume();

                PauseTrigger = false;
            }
        }
        else
        {
            if (Paused || PauseTrigger)
            {
                foreach (BaseObject objRef in ObjectList)
                {
                    objRef.enabled = true;
                }

                foreach (Animator animator in FindObjectsOfType<Animator>())
                {
                    animator.enabled = true;
                }

                /*foreach (Attacher attacher in FindObjectsOfType<Attacher>())
                {
                    attacher.enabled = true;
                }*/

                AudioController.Resume();

                PauseTrigger = false;
                Paused = false;
            }
        }

        if (AllowTime && !Paused)
        {
            LevelTimer += 1f;
            GlobalTimer += 1f;

            GameTimer += 1000f / 60f;

            Milliseconds = Mathf.FloorToInt(GameTimer / 10) % 100;
            Seconds = Mathf.FloorToInt(GameTimer / 1000) % 60;
            Minutes = Mathf.FloorToInt(GameTimer / 60000);
        }
    }

    public static void RingLoss(int ringsToCreate, float creationX, float creationY)
    {
        CreationAngle = 101.25f;
        CreationDirection = 1;
        CreationStrength = 4f;

        for (int i = 0; i < 32; i++)
        {
            if (ringsToCreate > 0)
            {
                Ring movingRing = CreateStageObject("Moving Ring", creationX, creationY) as Ring;
                movingRing.MovementActivated = true;
                movingRing.XPosition = creationX;
                movingRing.YPosition = creationY;
                movingRing.XSpeed = Mathf.Cos(CreationAngle * Mathf.Deg2Rad) * CreationStrength * CreationDirection;
                movingRing.YSpeed = Mathf.Sin(CreationAngle * Mathf.Deg2Rad) * CreationStrength;
                movingRing.transform.position = new Vector3(movingRing.XPosition, movingRing.YPosition, 0f);
                CreationDirection *= -1;
                CreationAngle += 22.5f + Mathf.Max(0, CreationDirection);
                ringsToCreate--;
            }
            if (i == 15)
            {
                CreationAngle = 101.25f;
                CreationDirection = 1;
                CreationStrength = 2f;
            }
        }
    }
}
