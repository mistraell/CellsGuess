using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FinishScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _loadingScreen;
    [SerializeField] private CanvasGroup _restartScreen;
    [SerializeField] private Level level;

    private void OnEnable()
    {
        _restartScreen.DOFade(1f, 1).SetEase(Ease.InOutQuad);
    }

    public void Restart()
    {
        _loadingScreen.gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_restartScreen.DOFade(0f, 1).SetEase(Ease.InOutQuad))
            .Append(_loadingScreen.DOFade(1f, .1f).SetEase(Ease.InOutQuad)).
            AppendInterval(1.5f).Append(_loadingScreen.DOFade(0f, 1).SetEase(Ease.InOutQuad))
            .OnComplete(() =>
            {
                _loadingScreen.gameObject.SetActive(false);
                level.RegenerateLevel();
                gameObject.SetActive(false);
            });
    }
}