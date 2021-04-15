using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using IndieMarc.StealthLOS;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    private Vector3 _mousePosToWorldPoint;
    private Vector2 _mousePos;
    private Vector3 _mouseDownUpdatePos;
    private RaycastHit2D _hit;
    private GameObject _hitGO;
    private bool _selecting;
    private bool _justPicked;
    public float rotateSpeed;

    public GameObject toolBarCanvas;
    public GameObject targetToolBarElement;
    public ScrollRect scrollRect;

    public ToolBarController toolBarController;
    public GameObject toolBarWhole;
    private bool _flag;

    public BehaviorCD behaviorCd;
    private bool _IJustRegret;

    public GameObject player;
    public float ableDistance;

    public Image zawaludoooo;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    void Start()
    {
        _selecting = false;
        _justPicked = false;
        _flag = false;
    }


    void Update()
    {
        DragBehavior();
        PutBackRightClick();
        CallOutToolBar();
    }


    private void DragBehavior()
    {
        if (Input.GetMouseButtonDown(0) && !behaviorCd.startCountingDown)
        {
            _mousePosToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _mousePos = new Vector2(_mousePosToWorldPoint.x, _mousePosToWorldPoint.y);
            LayerMask layerMask = 1 << 10;
            _hit = Physics2D.Raycast(_mousePos, Vector2.zero, 1000, layerMask);
            if (_hit.collider && CheckIfToolInPlayerRange(_hit.collider.gameObject.transform.position))
            {
                Zawaludo();
                // Debug.Log(_hit.collider.name);
                _selecting = true;
                _hitGO = _hit.collider.gameObject;
                _hitGO.layer = LayerMask.NameToLayer("Controllable");
                _hitGO.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                _hitGO.GetComponent<Rigidbody2D>().angularVelocity = 0;
            }
            else
            {
                GameObject selectToolInScene = PickBehavior();
                if (selectToolInScene)
                {
                    Zawaludo();
                    _hitGO = selectToolInScene;
                    _selecting = true;
                    _justPicked = true;
                    scrollRect.enabled = false;
                    _hitGO.layer = LayerMask.NameToLayer("Controllable");
                    _hitGO.transform.position =
                        new Vector3(_mousePosToWorldPoint.x, _mousePosToWorldPoint.y);
                    _hitGO.transform.eulerAngles = new Vector3(0, 0, 0);
                }
            }
        }

        if (Input.GetMouseButton(0) && _selecting)
        {
            _mouseDownUpdatePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _hitGO.transform.position = new Vector3(_mouseDownUpdatePos.x, _mouseDownUpdatePos.y);
            _hitGO.GetComponent<Rigidbody2D>().angularVelocity = 0;
            if (CheckIfToolInPlayerRange(_hitGO.transform.position))
            {
                _hitGO.GetComponent<SpriteRenderer>().color = Color.green;
            }
            else
            {
                _hitGO.GetComponent<SpriteRenderer>().color = Color.red;
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                _hitGO.transform.eulerAngles = new Vector3(0, 0,
                    _hitGO.transform.eulerAngles.z + Input.GetAxis("Mouse ScrollWheel") * rotateSpeed);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_selecting)
            {
                Mudamuda();
                Debug.Log("------_justPicked:" + _justPicked + "//_IJustRegret:" + _IJustRegret);
                _hitGO.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0.1f);
                _hitGO.layer = LayerMask.NameToLayer("AsDefault");
                _hitGO.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                _IJustRegret = false;
                if (_justPicked)
                {
                    PutBackBehaviorImm();
                    if (_justPicked && !CheckIfToolInPlayerRange(_hitGO.transform.position))
                    {
                        toolBarController.AddToolToList(_hitGO);
                        _IJustRegret = true;
                        _justPicked = false;
                    }
                }


                if (!_IJustRegret)
                {
                    behaviorCd.startCountingDown = true;
                    if (!CheckIfToolInPlayerRange(_hitGO.transform.position))
                    {
                        toolBarController.AddToolToList(_hitGO);
                    }

                    _justPicked = false;
                }

                _selecting = false;
                _hitGO = null;
                Debug.Log("_justPicked:" + _justPicked + "//_IJustRegret:" + _IJustRegret);
            }

            scrollRect.enabled = true;
        }
    }


    private void PutBackBehaviorImm()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            GraphicRaycaster gr = toolBarCanvas.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pointerEventData, results);
            foreach (var item in results)
            {
                if (item.gameObject == targetToolBarElement)
                {
                    Debug.Log("GotIt!");
                    toolBarController.AddToolToList(_hitGO);
                    _IJustRegret = true;
                    _justPicked = false;
                }
            }
        }
    }

    private void PutBackRightClick()
    {
        if (Input.GetMouseButtonDown(1) && !behaviorCd.startCountingDown)
        {
            _mousePosToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _mousePos = new Vector2(_mousePosToWorldPoint.x, _mousePosToWorldPoint.y);
            LayerMask layerMask = 1 << 10;
            _hit = Physics2D.Raycast(_mousePos, Vector2.zero, 1000, layerMask);
            if (_hit.collider)
            {
                _hitGO = _hit.collider.gameObject;
                if (CheckIfToolInPlayerRange(_hitGO.transform.position))
                {
                    toolBarController.AddToolToList(_hitGO);
                    behaviorCd.startCountingDown = true;
                }
            }
        }
    }


    private GameObject PickBehavior()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            GraphicRaycaster gr = toolBarCanvas.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pointerEventData, results);
            foreach (var item in results)
            {
                foreach (var toolInToolBar in toolBarController.toolDic.Keys)
                {
                    if (item.gameObject == toolInToolBar)
                    {
                        Debug.Log("GotIt!" + toolInToolBar);
                        return toolBarController.PickToolFromList(item.gameObject);
                    }
                }
            }
        }

        return null;
    }

    public bool CheckIfToolInPlayerRange(Vector3 ToolPos)
    {
        float distance = Vector3.Distance(player.transform.position, ToolPos);
        if (distance <= ableDistance)
        {
            return true;
        }

        return false;
    }

    private void Zawaludo()
    {
        Time.timeScale = 0.1f;
        player.GetComponent<CharacterControls2D>().enabled = false;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        zawaludoooo.gameObject.SetActive(true);
        zawaludoooo.DOFade(0.4f, 0.05f);
    }

    private void Mudamuda()
    {
        Time.timeScale = 1f;
        player.GetComponent<CharacterControls2D>().enabled = true;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        zawaludoooo.DOFade(0f, 0.5f).OnComplete(Mudamudamuda);
    }

    private void Mudamudamuda()
    {
        zawaludoooo.gameObject.SetActive(false);
    }

    private void CallOutToolBar()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !_flag)
        {
            toolBarWhole.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-22.9f, -39.8f), 1)
                .SetEase(Ease.OutCubic);
            _flag = true;
        }else if (Input.GetKeyDown(KeyCode.Tab) && _flag)
        {
            
            Debug.Log("fuck!");
            toolBarWhole.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-22.9f, 27f), 1)
                .SetEase(Ease.OutCubic);
            _flag = false;
        }
    }
}