using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy_AirplaneMinigame : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    public SkeletonAnimation anim;
    [SpineAnimation] public string anim_Idle;

    private void Start()
    {
        //anim.state.Complete += AnimComplete;
        speed = 7.5f;
        rb.velocity = -transform.right * speed;
        Destroy(gameObject, 5f);
    }
    private void AnimComplete(Spine.TrackEntry trackEntry)
    {
        //if (trackEntry.Animation.Name == anim_BellRun)
        //{
        //    PlayAnim(anim, anim_Run, true);
        //}
    }

    public void PlayAnim(SkeletonAnimation anim, string nameAnim, bool loop)
    {
        anim.state.SetAnimation(0, nameAnim, loop);
    }



}
