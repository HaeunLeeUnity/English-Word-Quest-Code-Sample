using System.Collections;
using LHEPackage.Helper;
using TMPro;
using UnityEngine;

namespace LHEPackage.Helper
{
    public class FrameChecker : MonoBehaviour
    {
        [SerializeField] private GameObject framePanel;
        [SerializeField] private TextMeshProUGUI _frameCheckerText;
    
        private float _worstFrame;
        private float _bestFrame;
        private float _totalFrame;
        private int _frameCheckingCount;
        
        private bool _isActive = false;
    
        private void Awake()
        {
            _worstFrame = float.MaxValue;
            _bestFrame = float.MinValue;
            _totalFrame = 0;
            _frameCheckingCount = 0;
            StartCoroutine(CoFrameCheck());
        }
    
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                _isActive = !_isActive;
                framePanel.SetActive(_isActive);
            }
        }
    
        private IEnumerator CoFrameCheck()
        {
            yield return YieldInstructionCache.WaitForSeconds(3f);
            
            while (true)
            {
                yield return YieldInstructionCache.WaitForSeconds(0.2f);
    
                if (_frameCheckingCount >= 40)
                {
                    _worstFrame = float.MaxValue;
                    _bestFrame = float.MinValue;
                    _totalFrame = 0;
                    _frameCheckingCount = 0;
                }
                
                var currentFrame =  1.0f / Time.deltaTime;
                _totalFrame += currentFrame;
                _frameCheckingCount++;
    
                var averageFrame = _totalFrame / _frameCheckingCount;
            
                if (currentFrame < _worstFrame)
                {
                    _worstFrame = currentFrame;
                }
            
                if (currentFrame > _bestFrame)
                {
                    _bestFrame = currentFrame;
                }
                
                _frameCheckerText.text = $"현재 {currentFrame:F1}FPS\n평균 {averageFrame:F1}FPS\n최저: {_worstFrame:F1}FPS\n최대: {_bestFrame:F1}FPS";
            }
        }
    }
}

