﻿using UnityEngine;
using XXLMod3.Controller;
using XXLMod3.Core;

namespace XXLMod3.PlayerStates
{
    class Custom_Lateflip : PlayerState_OnBoard
    {
        private StickInput _flipStick;
        private bool _noseManualling;
        private bool _manualling;
        private StickInput _popStick;

        private bool _flipDetected;
        private bool _forwardLoad;
        private bool _isLateflipping;
        private bool _isRespawning;
        private bool _noComply;
        private bool _potentialFlip;
        private bool _wasGrinding;
        private bool _wasGrindingBackwards;
        private bool _wasManualling = false;

        private Vector2 _initialFlipDir = Vector2.zero;
        private Vector3 _velocityOnBeginPop = Vector3.zero;

        private int _flipFrameCount;
        private int _flipFrameMax = 25;

        private float _augmentedLeftAngle;
        private float _augmentedRightAngle;
        private float _inAirTurnDelta;
        private float _invertVel;
        private float _toeAxis;
        private float _flip;
        private float _flipVel;
        private float _kickAddSoFar;
        private float _popDir;
        private float _popForce = Main.settings.DefaultPopForce; //MODDED
        private float _popVel;
        private float _timer;
        private float _lateflipOffset = -0.03f;
        private bool _backfootfFlip = false;
        private bool _primoflip;

        private PlayerController.SetupDir _setupDir;
        private bool _predictedCollision;
        private bool _fingerflip;

        public Custom_Lateflip(StickInput p_popStick, StickInput p_flipStick, float p_popForce, bool p_forwardLoad, float p_invertVel, PlayerController.SetupDir p_setupDir, float p_augmentedLeftAngle, float p_augmentedRightAngle, float p_popVel, float p_toeAxis, float p_popDir)
        {
            _popStick = p_popStick;
            _flipStick = p_flipStick;
            _popForce = p_popForce;
            _forwardLoad = p_forwardLoad;
            _invertVel = p_invertVel;
            _setupDir = p_setupDir;
            _augmentedLeftAngle = p_augmentedLeftAngle;
            _augmentedRightAngle = p_augmentedRightAngle;
            _popVel = p_popVel;
            _toeAxis = p_toeAxis;
            _popDir = p_popDir;
            PlayerController.Instance.animationController.skaterAnim.CrossFadeInFixedTime("Pop", 0.1f);
            PlayerController.Instance.animationController.ikAnim.CrossFadeInFixedTime("Pop", 0.1f);
        }

        public Custom_Lateflip(StickInput p_popStick, StickInput p_flipStick, float p_popForce, bool p_forwardLoad, float p_invertVel, PlayerController.SetupDir p_setupDir, float p_augmentedLeftAngle, float p_augmentedRightAngle, float p_popVel, float p_toeAxis, float p_popDir, bool p_noComply)
        {
            _popStick = p_popStick;
            _flipStick = p_flipStick;
            _popForce = p_popForce;
            _forwardLoad = p_forwardLoad;
            _invertVel = p_invertVel;
            _setupDir = p_setupDir;
            _augmentedLeftAngle = p_augmentedLeftAngle;
            _augmentedRightAngle = p_augmentedRightAngle;
            _popVel = p_popVel;
            _toeAxis = p_toeAxis;
            _popDir = p_popDir;
            _noComply = p_noComply;
            PlayerController.Instance.animationController.skaterAnim.CrossFadeInFixedTime("PushMongo", 0.1f);
            PlayerController.Instance.animationController.ikAnim.CrossFadeInFixedTime("PushMongo", 0.1f);
        }

        public Custom_Lateflip(StickInput p_popStick, StickInput p_flipStick, float p_popForce, bool p_wasGrinding, bool p_forwardLoad, float p_invertVel, PlayerController.SetupDir p_setupDir, float p_augmentedLeftAngle, float p_augmentedRightAngle, float p_popVel, float p_toeAxis, float p_popDir, int p_wasGrindingBackwards)
        {
            _popStick = p_popStick;
            _flipStick = p_flipStick;
            _popForce = p_popForce;
            _wasGrinding = p_wasGrinding;
            _forwardLoad = p_forwardLoad;
            _invertVel = p_invertVel;
            _setupDir = p_setupDir;
            _augmentedLeftAngle = p_augmentedLeftAngle;
            _augmentedRightAngle = p_augmentedRightAngle;
            _popVel = p_popVel;
            _toeAxis = p_toeAxis;
            _popDir = p_popDir;
            _wasGrindingBackwards = (p_wasGrindingBackwards == 1);
        }

        public Custom_Lateflip(StickInput p_popStick, StickInput p_flipStick, float p_popForce, bool p_wasGrinding, bool p_forwardLoad, float p_invertVel, PlayerController.SetupDir p_setupDir, float p_augmentedLeftAngle, float p_augmentedRightAngle, float p_popVel, float p_toeAxis, float p_popDir, bool p_wasManualling)
        {
            _popStick = p_popStick;
            _flipStick = p_flipStick;
            _popForce = p_popForce;
            _wasGrinding = p_wasGrinding;
            _forwardLoad = p_forwardLoad;
            _invertVel = p_invertVel;
            _setupDir = p_setupDir;
            _augmentedLeftAngle = p_augmentedLeftAngle;
            _augmentedRightAngle = p_augmentedRightAngle;
            _popVel = p_popVel;
            _toeAxis = p_toeAxis;
            _popDir = p_popDir;
            _wasManualling = p_wasManualling;
        }

        public Custom_Lateflip(StickInput p_popStick, StickInput p_flipStick, Vector2 p_initialFlipDir, float p_flipVel, float p_popVel, float p_toeAxis, float p_popDir, bool p_flipDetected, float p_flip, float p_popForce, bool p_forwardLoad, float p_invertVel, PlayerController.SetupDir p_setupDir, float p_augmentedLeftAngle, float p_augmentedRightAngle)
        {
            _popStick = p_popStick;
            _flipStick = p_flipStick;
            _potentialFlip = false;
            _flipDetected = p_flipDetected;
            _initialFlipDir = p_initialFlipDir;
            _toeAxis = p_toeAxis;
            _popDir = p_popDir;
            _flipVel = p_flipVel;
            _popVel = p_popVel;
            _flip = p_flip;
            _popForce = p_popForce;
            _forwardLoad = p_forwardLoad;
            _invertVel = p_invertVel;
            _setupDir = p_setupDir;
            _augmentedLeftAngle = p_augmentedLeftAngle;
            _augmentedRightAngle = p_augmentedRightAngle;
            PlayerController.Instance.animationController.skaterAnim.CrossFadeInFixedTime("Pop", 0.1f);
            PlayerController.Instance.animationController.ikAnim.CrossFadeInFixedTime("Pop", 0.1f);
        }
        public Custom_Lateflip(StickInput p_popStick, StickInput p_flipStick, Vector2 p_initialFlipDir, float p_flipVel, float p_popVel, float p_toeAxis, float p_popDir, bool p_flipDetected, float p_flip, float p_popForce, bool p_forwardLoad, float p_invertVel, PlayerController.SetupDir p_setupDir, float p_augmentedLeftAngle, float p_augmentedRightAngle, bool p_primoflip)
        {
            _popStick = p_popStick;
            _flipStick = p_flipStick;
            _potentialFlip = false;
            _flipDetected = p_flipDetected;
            _initialFlipDir = p_initialFlipDir;
            _toeAxis = p_toeAxis;
            _popDir = p_popDir;
            _flipVel = p_flipVel;
            _popVel = p_popVel;
            _flip = p_flip;
            _popForce = p_popForce;
            _forwardLoad = p_forwardLoad;
            _invertVel = p_invertVel;
            _setupDir = p_setupDir;
            _augmentedLeftAngle = p_augmentedLeftAngle;
            _augmentedRightAngle = p_augmentedRightAngle;
            _primoflip = p_primoflip;
            PlayerController.Instance.animationController.skaterAnim.CrossFadeInFixedTime("Pop", 0.1f);
            PlayerController.Instance.animationController.ikAnim.CrossFadeInFixedTime("Pop", 0.1f);
        }
        //MODDED CONSTRUCTOR
        public Custom_Lateflip(StickInput p_popStick, StickInput p_flipStick, Vector2 p_initialFlipDir, float p_flipVel, float p_popVel, float p_toeAxis, float p_popDir, bool p_flipDetected, float p_flip, float p_popForce, bool p_forwardLoad, float p_invertVel, PlayerController.SetupDir p_setupDir, float p_augmentedLeftAngle, float p_augmentedRightAngle, bool p_isLateflipping, bool p_primoflip)
        {
            _popStick = p_popStick;
            _flipStick = p_flipStick;
            _potentialFlip = false;
            _flipDetected = p_flipDetected;
            _initialFlipDir = p_initialFlipDir;
            _toeAxis = p_toeAxis;
            _popDir = p_popDir;
            _flipVel = p_flipVel;
            _popVel = p_popVel;
            _flip = p_flip;
            _popForce = p_popForce;
            _forwardLoad = p_forwardLoad;
            _invertVel = p_invertVel;
            _setupDir = p_setupDir;
            _augmentedLeftAngle = p_augmentedLeftAngle;
            _augmentedRightAngle = p_augmentedRightAngle;
            _isLateflipping = p_isLateflipping;
            _primoflip = p_primoflip;
            PlayerController.Instance.animationController.skaterAnim.CrossFadeInFixedTime("Pop", 0.3f);
            PlayerController.Instance.animationController.ikAnim.CrossFadeInFixedTime("Pop", 0.3f);
        }

        public Custom_Lateflip(StickInput p_popStick, StickInput p_flipStick, Vector2 p_initialFlipDir, float p_flipVel, float p_popVel, float p_toeAxis, float p_popDir, bool p_flipDetected, float p_flip, float p_popForce, bool p_wasGrinding, bool p_forwardLoad, float p_invertVel, PlayerController.SetupDir p_setupDir, float p_augmentedLeftAngle, float p_augmentedRightAngle, int p_wasGrindingBackwards)
        {
            _popStick = p_popStick;
            _flipStick = p_flipStick;
            _potentialFlip = false;
            _flipDetected = p_flipDetected;
            _initialFlipDir = p_initialFlipDir;
            _toeAxis = p_toeAxis;
            _popDir = p_popDir;
            _flipVel = p_flipVel;
            _popVel = p_popVel;
            _flip = p_flip;
            _popForce = p_popForce;
            _wasGrinding = p_wasGrinding;
            _forwardLoad = p_forwardLoad;
            _invertVel = p_invertVel;
            _setupDir = p_setupDir;
            _augmentedLeftAngle = p_augmentedLeftAngle;
            _augmentedRightAngle = p_augmentedRightAngle;
            _wasGrindingBackwards = (p_wasGrindingBackwards == 1);
        }

        public Custom_Lateflip(StickInput p_popStick, StickInput p_flipStick, Vector2 p_initialFlipDir, float p_flipVel, float p_popVel, float p_toeAxis, float p_popDir, bool p_flipDetected, float p_flip, float p_popForce, bool p_wasGrinding, bool p_forwardLoad, float p_invertVel, PlayerController.SetupDir p_setupDir, float p_augmentedLeftAngle, float p_augmentedRightAngle, bool p_wasManualling)
        {
            _popStick = p_popStick;
            _flipStick = p_flipStick;
            _potentialFlip = false;
            _flipDetected = p_flipDetected;
            _initialFlipDir = p_initialFlipDir;
            _toeAxis = p_toeAxis;
            _popDir = p_popDir;
            _flipVel = p_flipVel;
            _popVel = p_popVel;
            _flip = p_flip;
            _popForce = p_popForce;
            _wasGrinding = p_wasGrinding;
            _forwardLoad = p_forwardLoad;
            _invertVel = p_invertVel;
            _setupDir = p_setupDir;
            _augmentedLeftAngle = p_augmentedLeftAngle;
            _augmentedRightAngle = p_augmentedRightAngle;
            _wasManualling = p_wasManualling;
        }

        public override void Enter()
        {
            PlayerController playerController = PlayerController.Instance;
            XXLController.CurrentState = CurrentState.Lateflip;
            if (_isLateflipping)
            {
                XXLController.Instance.IsLateFlip = true;
                PlayerController.Instance.SetMaxSteeze(1f);
                PlayerController.Instance.SetRightIKWeight(1);
                PlayerController.Instance.SetLeftIKWeight(1);
            }
            PlayerController.Instance.cameraController.NeedToSlowLerpCamera = true;
            PlayerController.Instance.ToggleFlipTrigger(true);
            PlayerController.Instance.ToggleFlipColliders(true);
            _velocityOnBeginPop = PlayerController.Instance.boardController.boardRigidbody.velocity;
            if (_wasGrinding)
            {
                _velocityOnBeginPop = Vector3.Project(_velocityOnBeginPop, PlayerController.Instance.boardController.triggerManager.grindDirection);
            }
            if (!_isLateflipping)
            {
                SoundManager.Instance.PlayMovementFoleySound(0.3f, true);
                PlayerController.Instance.SetIKLerpSpeed(4f);
                if (_primoflip)
                {
                    PlayerController.Instance.boardController.SetBoardControllerUpVector(PlayerController.Instance.skaterController.skaterTransform.right);
                }
                else
                {
                    PlayerController.Instance.boardController.SetBoardControllerUpVector(PlayerController.Instance.skaterController.skaterTransform.up);
                }
                PlayerController.Instance.inputController.ResetInAirTurn();
                PlayerController.Instance.SetKneeBendWeightManually(1f);
                PlayerController.Instance.CorrectHandIKRotation(PlayerController.Instance.GetBoardBackwards());
            }
            bool flag;
            if (_wasGrinding)
            {
                flag = _wasGrindingBackwards;
            }
            else
            {
                flag = PlayerController.Instance.IsSwitch;
            }
            PlayerController.Instance.skaterController.rightFootCollider.isTrigger = true;
            PlayerController.Instance.skaterController.leftFootCollider.isTrigger = true;
            //EventManager.Instance.EnterAir(popType, _popForce);
            PlayerController.Instance.respawn.behaviourPuppet.BoostImmunity(1000f);
            if (!_isLateflipping)
            {
                if (_noComply)
                {
                    PlayerController.Instance.CrossFadeAnimation("Pop", 0.3f);
                }
                else
                {
                    PlayerController.Instance.CrossFadeAnimation("Pop", 0.04f);
                }
                PlayerController.Instance.ScaleDisplacementCurve(Vector3.ProjectOnPlane(PlayerController.Instance.skaterController.skaterTransform.position - PlayerController.Instance.boardController.boardTransform.position, PlayerController.Instance.skaterController.skaterTransform.forward).magnitude + 0.0935936f);
                //PlayerController.Instance.boardController.ResetTweakValues();
            }

            //PlayerController.Instance.ForceBoardPosition();
            if (!_isLateflipping)
            {
                PlayerController.Instance.boardController.CacheBoardUp();
                PlayerController.Instance.boardController.UpdateReferenceBoardTargetRotation();
                //KickAdd();
            }
        }

        private void KickAdd()
        {
            if (!_backfootfFlip)
            {
                float num = 5f;
                float num2 = Mathf.Clamp(Mathf.Abs(_popVel) / num, -0.7f, 0.7f);
                float num3 = 1.1f;
                if (_wasGrinding)
                {
                    num3 *= 0.5f;
                }
                float num4 = num3 - num3 * num2 - _kickAddSoFar;
                _kickAddSoFar += num4;
                PlayerController.Instance.DoKick(_forwardLoad, num4);
            }
        }

        public override void Exit()
        {
            if (_isLateflipping)
            {
                PlayerController.Instance.SetRightIKWeight(1);
                PlayerController.Instance.SetLeftIKWeight(1);
            }
            _isLateflipping = false;
            PlayerController.Instance.cameraController.NeedToSlowLerpCamera = false;
            PlayerController.Instance.ToggleFlipTrigger(false);
        }

        public override void Update()
        {
            base.Update();
            SoundManager.Instance.SetRollingVolumeFromRPS(PlayerController.Instance.GetSurfaceTag(PlayerController.Instance.boardController.GetSurfaceTagString()), PlayerController.Instance.boardController._rollSoundSpeed);
            if (Mathf.Abs(PlayerController.Instance.boardController.secondVel) > 5f)
            {
                PlayerController.Instance.SetIKLerpSpeed(8f);
            }
        }

        public override void FixedUpdate()
        {
            if (_isRespawning)
            {
                return;
            }
            PlayerController.Instance.ScalePlayerCollider();
            PlayerController.Instance.SetRotationTarget();
            if (_timer > 1f)
            {
                PlayerController.Instance.AnimPopInterruptedTransitions(true);
                PlayerController.Instance.SetTurningMode(InputController.TurningMode.Grounded);
                PlayerController.Instance.SetBoardToMaster();
                base.DoTransition(typeof(Custom_Riding), null);
            }
            PlayerController.Instance.comController.UpdateCOM();
            _timer += Time.deltaTime;
            if (!_isLateflipping)
            {
                PlayerController.Instance.AddUpwardDisplacement(_timer);
            }

            if (_timer > 0.06f)
            {
                SendEventPop(0f);
            }
            else
            {
                if (!_isLateflipping)
                {
                    KickAdd();
                }
                //PlayerController.Instance.UpdateSkaterDuringPop();
                PlayerController.Instance.MoveCameraToPlayerFixedUpdate();
                PlayerController.Instance.boardController.Rotate(true, false);
                float magnitude = (PlayerController.Instance.boardController.boardRigidbody.position - PlayerController.Instance.boardController.boardTargetPosition.position).magnitude;
                if (magnitude > 0.2f)
                {
                    PlayerController.Instance.UpdateBoardPosition();
                    if (magnitude > 0.5f)
                    {
                        PlayerController.Instance.ForceBail();
                    }
                }
                if (_wasGrinding)
                {
                    PlayerController.Instance.ForceBoardPosition();
                }
            }
            Vector3 vector = SkaterXL.Core.Mathd.LocalAngularVelocity(PlayerController.Instance.skaterController.skaterRigidbody);
            _inAirTurnDelta += 57.29578f * vector.y * Time.deltaTime;
        }

        public override void OnCollisionEnterEvent(Vector3 _impulse, bool _isBoard, Collision c)
        {
            if (_isBoard)
            {
                SoundManager.Instance.PlayBoardHit(_impulse.magnitude);
            }
        }

        public override float GetAugmentedAngle(StickInput p_stick)
        {
            if (p_stick.IsRightStick)
            {
                return _augmentedRightAngle;
            }
            return _augmentedLeftAngle;
        }

        public override void OnStickUpdate(StickInput p_leftStick, StickInput p_rightStick)
        {
            PlayerController.Instance.SetLeftIKOffset(p_leftStick.ToeAxis, p_leftStick.ForwardDir, p_leftStick.PopDir, p_leftStick.IsPopStick, false, PlayerController.Instance.GetAnimReleased());
            PlayerController.Instance.SetRightIKOffset(p_rightStick.ToeAxis, p_rightStick.ForwardDir, p_rightStick.PopDir, p_rightStick.IsPopStick, false, PlayerController.Instance.GetAnimReleased());
        }

        public override void OnFlipStickUpdate()
        {
            float num = 0f;
            PlayerController.Instance.OnFlipStickUpdate(ref _flipDetected, ref _potentialFlip, ref _initialFlipDir, ref _flipFrameCount, ref _flipFrameMax, ref _toeAxis, ref _flipVel, ref _popVel, ref _popDir, ref _flip, _flipStick, true, false, ref _invertVel, _popStick.IsRightStick ? _augmentedLeftAngle : _augmentedRightAngle, false, _forwardLoad, ref num);
        }

        public override void OnPopStickUpdate()
        {
            PlayerController.Instance.OnPopStickUpdate(0.1f, PlayerController.Instance.IsGrounded(), _popStick, ref _popVel, 10f, _forwardLoad, ref _setupDir, ref _invertVel, _popStick.IsRightStick ? _augmentedRightAngle : _augmentedLeftAngle);

        }

        public override StickInput GetPopStick()
        {
            return _popStick;
        }

        public override bool CanGrind()
        {
            return false;
        }

        public override bool IsInPopState()
        {
            return true;
        }

        public override void SendEventPop(float p_value)
        {
            if (!_wasGrinding)
            {
                PlayerController.Instance.AnimSetupTransition(false);
                if (_isLateflipping)
                {
                    PlayerController.Instance.OnPop(-1f, _popVel);
                    object[] args3 = new object[]
{
                _popStick,
                _flipStick,
                _initialFlipDir,
                _flipVel,
                _popVel,
                _toeAxis,
                0,
                _flipDetected,
                0,
                _forwardLoad,
                0,
                _setupDir,
                _augmentedLeftAngle,
                _augmentedRightAngle,
                0,
                _inAirTurnDelta
};
                    base.DoTransition(typeof(Custom_Pop), args3);
                }
            
                object[] args = new object[]
{
                _popStick,
                _flipStick,
                _initialFlipDir,
                _flipVel,
                _popVel,
                _toeAxis,
                0,
                _flipDetected,
                0,
                _forwardLoad,
                0,
                _setupDir,
                _augmentedLeftAngle,
                _augmentedRightAngle,
                _kickAddSoFar,
                _inAirTurnDelta
};
                base.DoTransition(typeof(Custom_Pop), args);
                return;

            }
            Vector3 vector = Vector3.ProjectOnPlane(PlayerController.Instance.GetGrindRight(), Vector3.up).normalized;
            Debug.DrawRay(PlayerController.Instance.GetGrindContactPosition(), vector * 5f, Color.magenta, 15f);
            if (PlayerController.Instance.boardController.triggerManager.sideEnteredGrind == TriggerManager.SideEnteredGrind.Left)
            {
                vector = -vector;
            }
            Vector3 vector2 = vector;
            Debug.DrawRay(PlayerController.Instance.GetGrindContactPosition(), vector2, Color.cyan, 15f);
            PlayerController.Instance.AnimSetupTransition(false);
            if (PlayerController.Instance.boardController.triggerManager.sideEnteredGrind != TriggerManager.SideEnteredGrind.Center)
            {
                PlayerController.Instance.OnPop(_popForce, _popStick.AugmentedToeAxisVel, vector2);
            }
            else
            {
                PlayerController.Instance.OnPop(_popForce, _popVel);
            }
            object[] args2 = new object[]
            {
            _popStick,
            _flipStick,
            _initialFlipDir,
            _flipVel,
            _popVel,
            _toeAxis,
            _popDir,
            _flipDetected,
            _flip,
            true,
            _forwardLoad,
            _invertVel,
            _setupDir,
            _augmentedLeftAngle,
            _augmentedRightAngle,
            _kickAddSoFar,
            _inAirTurnDelta
            };
            base.DoTransition(typeof(Custom_Pop), args2);
        }

        public override void OnRespawn()
        {
            _isRespawning = true;
            base.OnRespawn();
        }

        public override void OnPlayerTriggerEnterEvent(Collider c)
        {
            PlayerController.Instance.ForceBail();
        }

        public override void OnPlayerTriggerStayEvent(Collider c)
        {
            PlayerController.Instance.ForceBail();
        }

        public override bool IsInBeginPopState()
        {
            return true;
        }

        public override void OnFlipTriggerEnter(Collider c)
        {
            PlayerController.Instance.ToggleFlipColliders(false);
        }

        public override void OnFlipTriggerStay(Collider c)
        {
            if (PlayerController.Instance.IsFlipCollisionTriggerActive())
            {
                PlayerController.Instance.ToggleFlipColliders(false);
            }
        }

        public override void OnFlipTriggerEmpty()
        {
            PlayerController.Instance.ToggleFlipColliders(true);
        }

        public override bool IsInImpactState()
        {
            return this._predictedCollision;
        }
    }
}
