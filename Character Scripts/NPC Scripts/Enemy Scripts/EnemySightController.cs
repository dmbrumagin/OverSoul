using UnityEngine;

public class EnemySightController : MonoBehaviour
{
    public float warnedDistanceThreshold, alertedDistanceThreshold;
    private bool isPlayerInSightTrigger, isWarned, isAlerted = false;
    void Awake() {
        Debug.Log(new { isPlayerInSightTrigger, isWarned, isAlerted });
        // TODO consider setting trigger dimensions based on warning distance?
        // will probably require some constant for each sense trigger shape
        // TODO pshychic sense can be a ridiculous shape 
        //   (like a dramatic asterisk, and is setup rotated slightly at random)
        // TODO smell can just be a rectangle that gets directly above/below the current enemy platform
        // TODO all non-vision senses also should not require line of site for warning/alerting
        //   smell is an immediate warning, or alerting for specific enemies
        // each perception needs a seed value to determine the likelihood of warning/alerting
        // warning/alerting should be slightly random within a threshold
        
    }

    void Start() {

    }

    void Update() {
        // TODO warning distance should be much further than alerting distance
        if (isPlayerInSightTrigger && IsPlayerInLineOfSight()) {
            if (IsPlayerInWarningDistance()) isWarned = true;
            else if (IsPlayerInAlertingDistance()) isAlerted = true;
        }
    }

    void OnTriggerEnter(Collider collider) {
        // TODO create ice-cream cone trigger that is only on the player layer (also maybe arrows)
        isPlayerInSightTrigger = true;
    }

    void OnTriggerExit(Collider collider) {
        isPlayerInSightTrigger = false;
    }

    private bool IsPlayerInLineOfSight() {
        // TODO get player position and do a raycast at them
        // determine if any walls/etc are in the way
        return false; // REMOVE
    }

    private bool IsPlayerInAlertingDistance() {
        // TODO measure distance to player and check against sight thresholds
        return false; // REMOVE
    }
    private bool IsPlayerInWarningDistance() {
        // TODO measure distance to player and check against sight thresholds
        return false; // REMOVE
    }

    public bool IsWarned() {
        return isWarned;
    }
    public bool IsAlerted() {
        return isAlerted;
    }
}