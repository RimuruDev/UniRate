﻿using UnityEngine;

namespace UniRate {

    [RequireComponent(typeof(Animation))]
    public class RateRequestAnimationComponent : RateRequestComponent {

        #region <<---------- Properties and Fields ---------->>

        [SerializeField][HideInInspector] private string _clipName;

        private bool IsPlaying {
            get => this._isPlaying;
            set {
                if (this._isPlaying == value) return;
                this._isPlaying = value;
                this.OnIsPlayingChanged(this._isPlaying);
            }
        }
        private bool _isPlaying;
        
        private Animation _animation;
        
        #endregion <<---------- Properties and Fields ---------->>




        #region <<---------- MonoBehaviour ---------->>
        
        private void Awake() {
            this.CacheManager();
            this._animation = this.GetComponent<Animation>();
        }

        private void OnEnable() {
            this.IsPlaying = this.GetIsPlaying(this._animation, this._clipName);
        }

        private void Update() {
            this.IsPlaying = this.GetIsPlaying(this._animation, this._clipName);
            if (this._isPlaying || !this.IsRequesting || this.ElapsedSecondsSinceRequestsStarted <= this.DelaySecondsToStopRequests) return;
            this.StopRequests();
        }

        private void OnDisable() {
            this._isPlaying = false;
            this.StopRequests();
        }
        
        #endregion <<---------- MonoBehaviour ---------->>




        #region <<---------- Callbacks ---------->>
        
        private void OnIsPlayingChanged(bool isPlaying) {
            if (isPlaying) {
                this.StartRequests(this.Manager, this.GetCurrentPreset());
                this.Manager.ApplyTargetsIfDirty();
                return;
            }
            if (!this.IsRequesting || this.ElapsedSecondsSinceRequestsStarted <= this.DelaySecondsToStopRequests) return;
            this.StopRequests();
        }

        private bool GetIsPlaying(Animation animation, string clipName) {
            if (string.IsNullOrEmpty(clipName)) {
                return animation.isPlaying;
            }
            return animation.IsPlaying(clipName);
        }
        
        #endregion <<---------- Callbacks ---------->>
    }
}