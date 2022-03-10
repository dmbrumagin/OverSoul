using UnityEngine;

// TODO is this useless? probably
public class EnemySmellController : MonoBehaviour {
    // TODO research how best to do this
    public float warnedDistanceThreshold, alertedDistanceThreshold;
    private bool isPlayerInHearingTrigger, isWarned, isAlerted = false;
    void Awake()
    {
        Debug.Log(new { isPlayerInHearingTrigger, isWarned, isAlerted });

    }

    void Start()
    {

    }

    void Update()
    {
        if (isPlayerInHearingTrigger && IsPlayerInLineOfSight())
        {
            if (IsPlayerInWarningDistance()) isWarned = true;
            else if (IsPlayerInAlertingDistance()) isAlerted = true;
        }
    }

    void OnTriggerEnter(Collider collider) {
        // TODO create circular trigger that is only on the player layer
        isPlayerInHearingTrigger = true;
    }

    void OnTriggerExit(Collider collider) {
        isPlayerInHearingTrigger = false;
    }

    private bool IsPlayerInLineOfSight() {
        // TODO get player position and do a raycast at them
        // determine if any walls/etc are in the way
        return false; // REMOVE
    }

    private bool IsPlayerInAlertingDistance() {
        // TODO measure distance to player and check against hearing thresholds
        return false; // REMOVE
    }
    private bool IsPlayerInWarningDistance() {
        // TODO measure distance to player and check against hearing thresholds
        return false; // REMOVE
    }

    public bool IsWarned() {
        return isWarned;
    }
    public bool IsAlerted() {
        return isAlerted;
    }
}