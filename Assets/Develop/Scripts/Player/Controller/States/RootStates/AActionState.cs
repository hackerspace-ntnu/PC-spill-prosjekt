using System;
using Unity;
using UnityEngine;

public abstract class AActionState : AState
{
    private AActionState targetTransitionState;
    protected virtual AActionState TargetTransitionState
    {
        get => targetTransitionState;
        set
        {
            if (value == null)
            {
                targetTransitionState = value;
            }
            else if (value is AActionState)
            {
                targetTransitionState = value;
            }
            else
            {
                return;
            }
        }
    }
    //  TODO: Add virtual methods/fields relevant only to action states.
    protected abstract AActionState CheckTriggers();
    /*   {
           T temp = null; // defaults to null, but have to be assigned because compiler is annoying me.
           if (typeof(T) == typeof(OnDashState) || typeof(T) == typeof(OnJumpState) || typeof(T) == typeof(OnAirJumpState)
               || typeof(T) == typeof(OnNoActionState) || typeof(T) == typeof(OnWallClingState) || typeof(T) == typeof(OnWallJump))
           {
               // Player on ground..
               if (PlayerModel.IsGrounded)
               {
                   if (StateMachine.JumpInput)
                   {
                       temp = StateMachine.OnJumpState as T;
                   }
                   else if (StateMachine.DashInput && (Time.time - PlayerModel.LastDashTime <= PlayerModel.DashDuration))
                   {
                       temp = StateMachine.OnDashState as T;
                   }
                   else
                   {
                       temp = StateMachine.OnNoActionState as T;
                   }
               }
               // Player in air, and not close to any walls
               else if (!PlayerModel.IsGrounded && PlayerModel.WallTrigger == 0)
               {
                   if (!PlayerModel.HasAirJumped && StateMachine.JumpInput
                       && (Time.time >= PlayerModel.JumpTime + PlayerModel.MinimumTimeBeforeAirJump && !PlayerModel.HasAirJumped))
                   {
                       temp = StateMachine.OnAirJumpState as T;
                   }
                   else if (StateMachine.DashInput && (Time.time - PlayerModel.LastDashTime <= PlayerModel.DashDuration))
                   {
                       temp = StateMachine.OnDashState as T;
                   }
                   else
                   {
                       temp = StateMachine.OnNoActionState as T;
                   }
               }
               // player close to wall
               else if (PlayerModel.WallTrigger != 0 && !PlayerModel.IsGrounded)
               {
                   if (Math.Abs(body.velocity.y) <= 6 && PlayerModel.WallTrigger == -Math.Sign(StateMachine.HorizontalInput* PlayerModel.FlipGravityScale))
                   {
                       temp = StateMachine.OnWallClingState as T;
                   }
                   else if (StateMachine.DashInput && (Time.time - PlayerModel.LastDashTime <= PlayerModel.DashDuration))
                   {
                       temp = StateMachine.OnDashState as T;
                   }
                   else if (StateMachine.JumpInput)
                   {
                       if (Math.Abs(StateMachine.HorizontalInput) >= 0.3)
                       {
                           temp = StateMachine.OnJumpState as T;
                       }
                       else
                       {
                           temp = StateMachine.OnWallJump as T;
                       }
                   }
                   else
                   {
                       temp = StateMachine.OnNoActionState as T;
                   }
               }
           }
           return temp;
       }*/

}

