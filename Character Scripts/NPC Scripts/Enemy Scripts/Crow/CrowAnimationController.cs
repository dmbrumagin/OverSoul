using UnityEngine;

public class CrowAnimationController : MonoBehaviour {
    private CrowDiveAttack crowDiveAttack;
    private CrowPeckAttack crowPeckAttack;
    private CrowMovementController crowMovementController;

    void Awake() {
        crowDiveAttack = GetComponent<CrowDiveAttack>();
        crowPeckAttack = GetComponent<CrowPeckAttack>();
        crowMovementController = GetComponent<CrowMovementController>();
    }

}