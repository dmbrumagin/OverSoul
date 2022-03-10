using UnityEngine;
using System.Collections;
using System;

/* notes on strategy enum:
socializing can occur randomly based on a few factors:
  proximity to enemies that the crow has an affinity for
  the crow's inate preference for socializing
  how long the crow has been socializing
  whether or not the crow hears/sees something
  whether or not the social targets are also socializing
    a crow that decides to patrol while socializing could cause the other crow to wait or patrol
    or the other crow could be VERY social and just follow the now patrolling crow
    this should decrease the ability to sense the presence of the player (ohhhh yes)
    one of them being alerted should result in the other social enemies being alerted

rallying can probably occur based on the crow's awareness of other enemies
  so if a crow sees the player, and has recently seen an enemy nearby,
  it can go toward where the enemy was and call to it.
rallying could also occur after retreating if the crow sees fellow enemies that aren't alerted

when created, crows should have some random traits that dictate how likely they are to choose certain strategies
so most decisions will be partly random 
  (a coward crow will run after getting hit once, 
   a lazy crow won't patrol, 
   a perceptive crow will be easier to warn/alert)
   might have some small aethetic change to help the player discern the strat like eye color(not necessary though)
   I think having aesethic differences for traits would be cooler, especially if we just have trait tags/strings 
     that seed the actual trait stats (since it would be hard to do visuals for varying numbers)
*/

public enum CrowTraits // TODO probably make this generic - EnemyPersonalityTraits
{
    Brave, // affects retreating, investigating
    //Cowardly, // affects retreating, investigating
    Diligent, // affects patrolling, investigating
    //Lazy, // affects patrolling, investigating
    Social, // affects socializing, rallying
    //Asocial, // affects socializing, rallying
    Agile, // affects dodge ability and speed
    Perceptive, // affects sight/hearing
    // Oblivious // affects sight/hearing
}

public enum CrowStrategy // TODO probably make this generic - EnemyStrategy
{
    Waiting, // crow is resting between patrol routines
    Patrolling, // crow is on a routine patrol
    Socializing, // crow is near/following other enemies that have affinity
    Investigating, // crow heard or saw something, but is not alerted
    Searching, // crow is alerted, may or may not know where to search
    Pursuing, // crow has seen the player and is giving chase
    Retreating, // crow is afraid or nearly dead
    Rallying, // crow is trying to alert other enemies
    Overdrive // enemy will use do or die tactics, which should have different movement/timing/damage/etc

    //might be funny to have an enemy that kills itself from dishonor instead of retreat, also goes hand in hand with any kamikazi enemies.
    // what about an overdrive strat that brave or kamikazi enemies choose at low health?
    //yes sounds like a good fit for the enemy type - if we wanted to start an enemy in suicide mode we would just have it under the health threshold.
}

// TODO this seems a but unnecessary? idk yet tho
public enum CrowCombatState {
    PrepareDive,
    Dive,
    PreparePeck,
    Peck,
    Dodge
}

public class CrowAIController : MonoBehaviour {
    public float moveForceMultiplier = 15;
    private new Transform transform;
    private new Rigidbody2D rigidbody;
    private Transform playerTransform;
    private CrowDiveAttack crowDiveAttack;
    private IEnumerator dive;
    private CrowPeckAttack crowPeckAttack;
    private IEnumerator peck;
    private CrowMovementController crowMovementController;
    private EnemyHearingController hearingController;
    private EnemySightController sightController;
    private CrowStrategy strategy;
    private bool isWarned = false;
    private bool isAlerted = false;

    void Awake() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody2D>();
        crowDiveAttack = GetComponent<CrowDiveAttack>();
        crowPeckAttack = GetComponent<CrowPeckAttack>();
        crowMovementController = GetComponent<CrowMovementController>();
        hearingController = GetComponent<EnemyHearingController>();
        sightController = GetComponent<EnemySightController>();

        // TODO InitializeTraitValues
        ChooseStartingStrategy();
    }
    private void SelectRandomTraits() {
        // TODO pick one or two random traits
    }

    private void InitializeTraitValues() {
        // TODO set partially random numeric stats for traits
    }

    private void ChooseStartingStrategy() {
        // TODO randomly choose waiting or patrolling based on traits
        strategy = CrowStrategy.Pursuing;
    }

    private Vector2 GetVectorToPlayer() {
        var vector = playerTransform.position - transform.position;
        // Debug.Log(new { vector, playerPos = playerTransform.position, crowPos = transform.position });
        return vector;
    }

    private bool isFlapping = false;
    IEnumerator Flap(Vector2 playerVector) {
        isFlapping = true;
        // Debug.Log("FLAPPING");
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.01f, 0.2f));
        playerVector.y = playerVector.y + 1f;
        var lrDirection = playerVector.x >= 0 ? 1 : -1;
        if (Math.Abs(playerVector.x) < 0.2f) lrDirection = 0;
        var flapForce = playerVector.y > -0.5f ? 1200f : -100f;
        rigidbody.AddForce(new Vector2(lrDirection * moveForceMultiplier, flapForce));
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("DONE FLAPPING");
        isFlapping = false;
    }

    void FixedUpdate() { // TODO should be update?
        // switch statements tend to be more code (you have to use break), and less flexible (single condition)
        // they are slightly faster for large amounts of cases though, so worth considering
        // fall-through is also a bit cumbersome imo, but might be useful since searching/investigating will use some shared logic
        //yeah I messed a bit with fall through, it can be a bit hard to visualize at times but with us using the enum and the large amount of cases it seemed like a decent spot to try it out

        CheckAndSetWarnedAndAlerted();
        // TODO ExecuteAttackIfPlayerIsInRange

        // TODO ChooseTargetLocation
        // TODO FindWaypointForTargetLocation

        if (isFlapping || crowPeckAttack.IsAttacking() || crowDiveAttack.IsAttacking()) {
            // TODO cancel attacks if necessary (especially dive)
            return;
        }

        if (strategy == CrowStrategy.Pursuing) {
            // TODO determine whether or not to retreat and/or rally
            
            var vectorToPlayer = GetVectorToPlayer();
            var diveVector = crowDiveAttack.attackVector;
            var peckVector = crowPeckAttack.attackVector;
            // Debug.Log(new { vectorToPlayer, diveVector, peckVector });
            // Debug.DrawRay(transform.position, diveVector, Color.red, 0.2f);
            // Debug.DrawRay(transform.position, vectorToPlayer, Color.green, 0.2f);
            // Debug.Log(new { vtp = vectorToPlayer.magnitude, div = diveVector.magnitude });
            if (vectorToPlayer.magnitude > diveVector.magnitude) {
                // doesn't do pathfinding, just goes straight to player, which should be in line-of-sight
                StartCoroutine(Flap(vectorToPlayer));
            } else if (!crowPeckAttack.IsReady() && !crowDiveAttack.IsReady()) {
                var direction = new Vector2(vectorToPlayer.x * -1, Math.Abs(vectorToPlayer.y));
                StartCoroutine(Flap(direction)); // flap away on x, but always up; no moves are ready
            } else if (vectorToPlayer.y < 0
              && vectorToPlayer.y > diveVector.y
              && Math.Abs(vectorToPlayer.x) < 1
              && diveVector.magnitude > vectorToPlayer.magnitude
              && crowDiveAttack.IsReady()) {
                // player is in range of dive
                dive = crowDiveAttack.Attack(vectorToPlayer);
                StartCoroutine(dive);
            } else if (crowPeckAttack.IsReady() && peckVector.magnitude > vectorToPlayer.magnitude) {
                //StartCoroutine(Flap(playerVector)); // TODO REMOVE
                peck = crowPeckAttack.Attack(vectorToPlayer.normalized);
                StartCoroutine(peck);
            } else if (peckVector.magnitude * 2 > vectorToPlayer.magnitude) {
                var direction = new Vector2(vectorToPlayer.x / 3, Math.Abs(vectorToPlayer.y));
                StartCoroutine(Flap(direction)); // flap less on x, but always up; no moves are ready or in range, but player is in close range
            } else {
                StartCoroutine(Flap(vectorToPlayer));
            }
        } else if (strategy == CrowStrategy.Retreating) {
            // crow runs away from player and tries to hide
        } else if (strategy == CrowStrategy.Rallying) {
            // crow tries to find non-alerted enemies and calls to them
        } else if (strategy == CrowStrategy.Searching) {
            // check vision and hearing for player/arrows/etc
            // if alerted by other enemy, follow that enemy
            // if alerted by hearing, find path to location
            // if alerted by vision, find path to location
            // set pursuing strategy if player is within sight threshold
        } else if (strategy == CrowStrategy.Investigating) {
            // use most logic from searching AI
            // after some time investigating, should ChooseStartingStrategy
        } else if (strategy == CrowStrategy.Socializing) {
            // crow goes to nearest enemy with greatest affinity
            // may cause other enemy to choose socializing strat depending on traits, etc
        } else if (strategy == CrowStrategy.Patrolling) {
            // crow flies around the nearest (large?) platform
        } else if (strategy == CrowStrategy.Waiting) {
            // crow finds nearest place to land and chill out
        }
    }

    private void CheckAndSetWarnedAndAlerted() {
        if (!isAlerted) { // isAlerted can't reset once true (unless the player fakes their death? or dies and comes back to life?)

            // TODO check if alerted by other enemy rally calls

            isAlerted = sightController.IsAlerted() || hearingController.IsAlerted();
            if (isAlerted) {
                isWarned = true;
                strategy = CrowStrategy.Searching;
                return;
            }
        } else if (!isWarned) {
            isWarned = sightController.IsWarned() || hearingController.IsWarned();
            if (isWarned) strategy = CrowStrategy.Investigating;
        }
    }

    private void ExecuteAttackIfPlayerIsInRange() {
        // check attack vectors against current player location
        // should have some slight randomness depending on attackDuration
        //   (longer attack times are easier to avoid, so shouldn't be too predictable)
        //   (could also account for player dash exhaustion)
        // account for player movement direction
    }

    private Vector2 FindWaypointForTargetLocation(Vector2 location) {
        // cast ray towards player (or investigation/search location)
        // get dimensions of any platform in the way
        // find which edge is closest
        // for flying types, set target some (semi-random) distance away from the corner of the platform
        // otherwise, set target on edge of platform (distance depends on enemy jump ability/etc)
        // if there is no platform in the way
        //   calculate target based on player location and chosen attack vector(s)
        //   (attack vectors should represent direction/distance from player in order to land the attack successfully, assuming player doesn't move)
        // if the player is above the enemy
        //   raycast in a few directions to locate platforms (maybe up and +- 30 degrees, possibly also semi-random)
        //   sort located platforms by distance to player
        //   semi-randomly select a target platform (weighted towards least distance)
        //   for flying types, set target location some distance away from the nearest corner of the target platform
        //   otherwise, calculate target using jump vector (similar to attack vector)
        //   


        return new Vector2(); // TODO default to current move target, if it exists
    }
}