using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CharacterController : MonoBehaviour
{
    [SerializeField] int moveSpeed;
    [SerializeField] int jumpHeight;
    Animator animator;
    AudioSource audioSource;
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 moveChar = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), 0, CrossPlatformInputManager.GetAxis("Vertical"));
        if (moveChar != Vector3.zero)
        {
            animator.SetBool("isWalking", true);
            Quaternion targetRotation = Quaternion.LookRotation(moveChar, Vector3.up);
            transform.rotation = targetRotation;
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        transform.position += moveChar * Time.deltaTime * moveSpeed;
        if (GameManager.Instance.IsJumping)
        {
            animator.SetTrigger("Jump");
            audioSource.PlayOneShot(AudioManager.Instance.Jump);
            transform.Translate(Vector3.up * jumpHeight * Time.deltaTime, Space.World);
            GameManager.Instance.IsJumping = false;
        }
        if (GameManager.Instance.IsPunching)
        {
            animator.SetTrigger("Punch");
            audioSource.PlayOneShot(AudioManager.Instance.Hit);
            ModifyTerrain.Instance.DestroyBlock(10f, (byte)TextureType.air.GetHashCode());
            GameManager.Instance.IsPunching = false;
        }
        if (GameManager.Instance.IsBuilding)
        {
            animator.SetTrigger("Punch");
            audioSource.PlayOneShot(AudioManager.Instance.Build);
            ModifyTerrain.Instance.AddBlock(10f, (byte)TextureType.rock.GetHashCode());
            GameManager.Instance.IsBuilding = false;
        }
    }
}
