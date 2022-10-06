using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public enum animationState { idle, walkingForward, walkingBackwards, strafingLeft, strafingRight };

    public bool isWalkingForward;
    public bool isWalkingBackwards;

    private int isWalkingForwardHash;
    private int isWalkingBackwardsHash;

    private void Start()
    {
        isWalkingForwardHash = Animator.StringToHash(Properties.isWalkingForward);
        isWalkingBackwardsHash = Animator.StringToHash(Properties.isWalkingBackwards);
    }

    // Update is called once per frame
    void Update()
    {
        switch(PlayerMovement.animationState)
        {
            case animationState.walkingForward:
            {
                animator.SetBool(isWalkingForwardHash, true);
                break;
            }

            case animationState.walkingBackwards:
            {
                animator.SetBool(isWalkingBackwardsHash, true);
                break;
            }

            // Same as idle
            default:
            {
                animator.SetBool(isWalkingForwardHash, false);
                animator.SetBool(isWalkingBackwardsHash, false);
                break;
            }
        }
    }
}
