/*
 * Copyright (c) 2019 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;

namespace RayWenderlich.Unity.StatePatternInUnity
{
    //ducking state is a sub-state of grounded state
    public class DuckingState : GroundedState
    {
        private bool belowCeiling;
        private bool crouchHeld;

        public DuckingState(Character character, StateMachine stateMachine) : base(character, stateMachine)
        {
        }

        //override certain methods from parent class while keeping desired functionality
        public override void Enter()
        {
            //crounch enabled,, boolean set to true, values assigned to speeds
            base.Enter();
            character.SetAnimationBool(character.crouchParam, true);
            speed = character.CrouchSpeed;
            rotationSpeed = character.CrouchRotationSpeed;
            //change collider height
            character.ColliderSize = character.CrouchColliderHeight;
            belowCeiling = false;
        }

        public override void Exit()
        {
            //stop crouching, boolean set to false
            base.Exit();
            character.SetAnimationBool(character.crouchParam, false);
            //back to normal collider height
            character.ColliderSize = character.NormalColliderHeight;
        }

        public override void HandleInput()
        {
            //user input
            base.HandleInput();
            crouchHeld = Input.GetButton("Fire3");
        }

        public override void LogicUpdate()
        {
            //if not crouching and no collision (juump) state changes to standing
            base.LogicUpdate();
            if (!(crouchHeld || belowCeiling))
            {
                stateMachine.ChangeState(character.standing);
            }
        }

        public override void PhysicsUpdate()
        {
            //check collision over head of character for collider height, via vector3 point.
            base.PhysicsUpdate();
            belowCeiling = character.CheckCollisionOverlap(character.transform.position +
                Vector3.up * character.NormalColliderHeight);
        }

    }
}
