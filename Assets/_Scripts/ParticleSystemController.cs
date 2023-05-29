using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _front;
    [SerializeField] private ParticleSystem _frontOtherSide;
    [SerializeField] private ParticleSystem _frontTrials;
    [SerializeField] private ParticleSystem _back;
    [SerializeField] private Color _changingColor;
    [SerializeField] private Material _particlesMaterial;
    private Color _startColor;


    private int _currentSystem = 1;
    void Start()
    {
        var main = _front.main;
        _startColor = _particlesMaterial.color;
        main.stopAction = ParticleSystemStopAction.Callback;
        main = _back.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            var main = _front.main;
            main.loop = false;
            main = _frontTrials.main;
            main.loop = false;
            main = _frontOtherSide.main;
            main.loop = false;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            _particlesMaterial.color = _changingColor;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _particlesMaterial.color = _startColor;
        }

        if (!_front.IsAlive() && !_back.IsAlive())
            ChangeAliveParticleSystem();

    }

    private void ChangeAliveParticleSystem()
    {
        _currentSystem *= -1;

        if (_currentSystem == 1)
            _front.Play();
        else
            _back.Play();
    }
}
