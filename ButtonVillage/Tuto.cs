using DigitalRuby.Tween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tuto : MonoBehaviour
{
    bool Death = false;
    List<string> _tutosDone;

    private float _timeForDeathTuto;
    private Image _overdriveImage;

    private bool _tutoOpen;

	// Use this for initialization
	void Start ()
    {
        _tutosDone = new List<string>();

        _timeForDeathTuto = Time.time + 7f;
        _overdriveImage = GameObject.FindGameObjectWithTag("Resource Canvas").transform.Find("Ressources Panel/Overdrive").GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (GameManager.Instance.TutoFinished || _tutoOpen)
            return;

        if (Time.time > _timeForDeathTuto && !_tutosDone.Exists(s => s == "Death"))
            StartTuto("Death");
        else if (!_tutosDone.Exists(s => s == "Touch"))
            StartTuto("Touch");
        else if (_overdriveImage.fillAmount < 0.4f && !_tutosDone.Exists(s => s == "Overdrive"))
            StartTuto("Overdrive");
	}

    void StartTuto(string tuto)
    {
        if (_tutosDone.Contains(tuto))
            return;

        transform.Find("Blocker").gameObject.SetActive(true);

        GameManager.Instance.IsPaused = true;
        _tutoOpen = true;
        _tutosDone.Add(tuto);

        Transform tf = transform.Find(tuto);
        if (tf == null)
            return;

        tf.gameObject.SetActive(true);
        CanvasGroup grp = tf.GetComponent<CanvasGroup>();
        gameObject.Tween("Tuto" + tuto, grp.alpha, 1, 2f, TweenScaleFunctions.CubicEaseOut, t =>
        {
            grp.alpha = t.CurrentValue;
        }, t2 =>
        {
            grp.transform.Find("Button").gameObject.SetActive(true);
        });
    }

    public void EndTuto(string tuto)
    {
        if (!_tutosDone.Contains(tuto))
            return;
        
        CanvasGroup grp = transform.Find(tuto).GetComponent<CanvasGroup>();
        grp.transform.Find("Button").gameObject.SetActive(false);

        gameObject.Tween("TutoEnd" + tuto, grp.alpha, 0, 1f, TweenScaleFunctions.CubicEaseOut, t =>
        {
            grp.alpha = t.CurrentValue;
        }, t2 =>
        {
            grp.gameObject.SetActive(false);
            GameManager.Instance.IsPaused = false;
            _tutoOpen = false;
            transform.Find("Blocker").gameObject.SetActive(false);
        });
    }
}
