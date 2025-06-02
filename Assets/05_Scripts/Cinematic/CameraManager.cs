using System.Collections.Generic;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using System.Data;

[System.Serializable]
public enum CameraType
{ 
    EncounterThird,
    EncounterCinematic,
    BattleCenter,
    BattleEnemy,
    BattleCamera01,
    BattleCamera02,
    BattleCamera03,
    BattlePlayer,
    None,
}

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [Header("Encounter Camera")]
    [SerializeField] private CinemachineCamera EncounterThirdCamera;
    [SerializeField] private CinemachineCamera EncounterCinematicCamera;

    [Header("BattleField Camera")]
    [SerializeField] private CinemachineCamera BattleCenterCamera;
    [SerializeField] private CinemachineCamera BattleEnemyCamera;
    [SerializeField] private CinemachineCamera BattlePlayerCamera;
    [SerializeField] private List<CinemachineCamera> BattleCameraList;

    [Header("CurrentCamera Parameter")]
    [SerializeField] private CinemachineCamera CurrentCamera;

    [Header("Camera Effector")]
    [SerializeField] public FadeCanvas fadeCanvas;
    [SerializeField] public CinematicCanvas cinematicCanvas;
    public bool shakeTest;

    public void ShakeCamera(CameraType _type = CameraType.None)
    {
        CinemachineCamera targetCamera = GetCamera(_type);
        targetCamera.TryGetComponent<CinemachineBasicMultiChannelPerlin>(out CinemachineBasicMultiChannelPerlin Perlin);
        if (Perlin != null)
        {
            Perlin.AmplitudeGain = 5.0f;
            Perlin.FrequencyGain = 2.5f;
        }
    }

    public void CalmCamera(CameraType _type = CameraType.None)
    {
        CinemachineCamera targetCamera = GetCamera(_type);
        targetCamera.TryGetComponent<CinemachineBasicMultiChannelPerlin>(out CinemachineBasicMultiChannelPerlin Perlin);
        if (Perlin != null)
        {
            Perlin.AmplitudeGain = 0.0f;
            Perlin.FrequencyGain = 0.0f;
        }
    }

    public void OnShake(CameraType _type = CameraType.None, float _time = 0.5f)
    {
        StartCoroutine(ShakeEffect(_type, _time));
    }

    IEnumerator ShakeEffect(CameraType _type = CameraType.None, float _time = 0.5f)
    {
        ShakeCamera(_type);
        yield return new WaitForSeconds(_time);
        CalmCamera(_type);
    }

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(gameObject); }
    }

    private void Update()
    {
        if (shakeTest)
        {
            shakeTest = false;
            ShakeCamera();
        }
    }

    public CinemachineCamera GetCamera(CameraType _type = CameraType.None)
    {
        switch (_type)
        {
            case CameraType.EncounterThird:
                return EncounterThirdCamera;
            case CameraType.EncounterCinematic:
                return EncounterCinematicCamera;
            case CameraType.BattleCenter:
                return BattleCenterCamera;
            case CameraType.BattleEnemy:
                return BattleEnemyCamera;
            case CameraType.BattlePlayer:
                return BattlePlayerCamera;
            case CameraType.BattleCamera01:
                return BattleCameraList[0];
            case CameraType.BattleCamera02:
                return BattleCameraList[1];
            case CameraType.BattleCamera03:
                return BattleCameraList[2];
            default:
                return CurrentCamera;
        }
    }

    public void OnLiveCamera(CameraType _type = CameraType.None)
    {
        EncounterThirdCamera.Priority = 0;
        // EncounterCinematicCamera.Priority = 0;
        BattleCenterCamera.Priority = 0;
        BattleEnemyCamera.Priority = 0;
        BattlePlayerCamera.Priority = 0;
        BattleCameraList[0].Priority = 0;
        BattleCameraList[1].Priority = 0;
        BattleCameraList[2].Priority = 0;

        GetCamera(_type).Priority = 1;
        CurrentCamera = GetCamera(_type);
    }

    public void CenterCameraSlideAction(float time = 2.5f, bool LeftToRight = false)
    {
        if (LeftToRight)
        {
            BattleCenterCamera.GetComponent<CinemachineSplineDolly>().CameraPosition = 1f;
            StartCoroutine(CenterCameraSliding(time, LeftToRight));
        }
        else
        {
            BattleCenterCamera.GetComponent<CinemachineSplineDolly>().CameraPosition = 0f;
            StartCoroutine(CenterCameraSliding(time, LeftToRight));
        }
    }

    IEnumerator CenterCameraSliding(float time = 2.5f, bool LeftToRight = false)
    {
        float curTime = 0f;
        if (LeftToRight)
        {
            while (curTime < 1f)
            {
                curTime += Time.deltaTime / time;
                if (curTime > 1f) curTime = 1f;
                BattleCenterCamera.Lens.FieldOfView = 60f - 10f * Mathf.Sin(180f * curTime * Mathf.Deg2Rad);
                BattleCenterCamera.GetComponent<CinemachineSplineDolly>().CameraPosition = (1 - curTime);
                yield return null;
            }
        }
        else
        {
            while (curTime < 1f)
            {
                curTime += Time.deltaTime / time;
                if (curTime > 1f) curTime = 1f;
                BattleCenterCamera.Lens.FieldOfView = 60f - 10f * Mathf.Sin(180f * curTime * Mathf.Deg2Rad);
                BattleCenterCamera.GetComponent<CinemachineSplineDolly>().CameraPosition = curTime;
                yield return null;
            }
        }
    }

    public void CommandPosChange(SkillGroup command)
    {
        switch (command)
        {
            case SkillGroup.None:
                StartCoroutine(CommandPosMove(0f));
                break;
            case SkillGroup.Attack:
                StartCoroutine(CommandPosMove(0.3f));
                break;
            case SkillGroup.Support:
                StartCoroutine(CommandPosMove(1f));
                break;
        }
    }

    IEnumerator CommandPosMove(float TargetPoint)
    {
        float StartPoint = CurrentCamera.GetComponent<CinemachineSplineDolly>().CameraPosition;
        CinemachineCamera targetCam = CurrentCamera;
        if (StartPoint == TargetPoint)
        {
            yield return null;
        }
        else
        {
            float curTime = 0f;
            while (curTime < 1f)
            { 
                curTime += Time.deltaTime * (1 + Mathf.Abs(StartPoint - TargetPoint) / 1f);
                if (curTime > 1f) curTime = 1f;
                float changed = Mathf.Lerp(StartPoint, TargetPoint, curTime);
                targetCam.GetComponent<CinemachineSplineDolly>().CameraPosition = changed;
                yield return null;
            }
        }
    }

    public void PlayerAttackCameraAction()
    {
        StartCoroutine(AttackCameraEffect());
    }

    IEnumerator AttackCameraEffect()
    {

        float curTime = 0f;
        Vector3 StartPoint = new Vector3(3f, 2.5f, 3.5f);
        Vector3 ChangePoint = StartPoint;
        CinemachineFollow followCam = CurrentCamera.GetComponent<CinemachineFollow>();
        CinemachineHardLookAt lookatCam = CurrentCamera.GetComponent<CinemachineHardLookAt>();
        lookatCam.LookAtOffset = new Vector3(0.5f, 1f, 0f);
        Vector3 LookAtVec = new Vector3(0.5f, 1f, 0f);
        CurrentCamera.Lens.FieldOfView = 30f;
        followCam.FollowOffset = StartPoint;
        // 45 => 60
        // 3f, 2.5f, 3.5f
        // 3.5f, 0.5f, -1.5f
        while (curTime < 1f) 
        {
            curTime += Time.deltaTime / 3.6f;
            if (curTime > 1f) curTime = 1f;
            ChangePoint.x = 3.5f - 0.5f + (curTime * 0.5f);
            ChangePoint.y = 0.5f + 2f - (curTime * 2f);
            ChangePoint.z = -1.5f + 4.5f - (curTime * 4.5f);
            CurrentCamera.Lens.FieldOfView = 60f - 30f + curTime * 30f;

            LookAtVec.x = 1.5f - 1f + (curTime * 1f);

            lookatCam.LookAtOffset = LookAtVec;
            followCam.FollowOffset = ChangePoint;
            yield return null;
        }
    }

    public void OnEaseChange(bool isOn)
    {
        CinemachineBrain brainCam = Camera.main.GetComponent<CinemachineBrain>();
        if (isOn)
        {
            brainCam.DefaultBlend.Time = 2f;
        }
        else
        {
            brainCam.DefaultBlend.Time = 0f;
        }
    }
}
