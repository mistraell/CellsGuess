using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _icon;
    [SerializeField] private GameObject _particles;
    private ObjectPair _objectPair;
    private Vector3 originalScale;
    private bool _isRight;
    private bool _isBlocked;
    public Action RightAnswerClicked;
    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnMouseUp()
    {
        if (!_isBlocked)
        {
            if (_isRight)
            {
                Bounce();
                Instantiate(_particles, transform);
                RightAnswerClicked?.Invoke();
            }
            else
            {
                Shake();
            }
        }
    }

    public void BlockCell(bool isBlocking)
    {
        _isBlocked = isBlocking;
    }
    public void Initialize(ObjectPair objectPair, bool isRight)
    {
        _isRight = isRight;
        _objectPair = objectPair;
        _icon.sprite = _objectPair.sprite;
        _icon.transform.rotation = Quaternion.Euler(0, 0, objectPair.rotationAngle);
    }

    public void Bounce()
    {
        transform.DOScale(originalScale * 1.2f, 0.2f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            transform.DOScale(originalScale * 0.8f, 0.2f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutBounce);
            });
        });

    }

    private void Shake()
    {
        transform.DOShakePosition(0.5f, new Vector3(1, 0, 0), 20,
            1, false, true).SetEase(Ease.InBounce);
    }
}