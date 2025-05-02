using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_Joystick : UI_Scene
{
	enum GameObjects
	{
        JoystickBG,
        Handler,
    }

    private GameObject _handler;
    private GameObject _joystickBG;
    private Vector2 _moveDir { get; set; }
    private Vector2 _joystickTouchPos;
    private Vector2 _joystickOriginalPos;
    private float _joystickRadius;

    private void OnDestroy()
    {
        Managers.UI.OnTimeScaleChanged -= OnTimeScaleChanged;
    }

    protected override void Awake()
	{
		base.Awake();

        Managers.UI.OnTimeScaleChanged += OnTimeScaleChanged;

        BindObjects(typeof(GameObjects));
        _handler = GetObject((int)GameObjects.Handler);

        _joystickBG = GetObject((int)GameObjects.JoystickBG);
        _joystickOriginalPos = _joystickBG.transform.position;
        _joystickRadius = _joystickBG.GetComponent<RectTransform>().sizeDelta.y / 5;
        gameObject.BindEvent(OnPointerDown, type: Define.ETouchEvent.PointerDown);
        gameObject.BindEvent(OnPointerUp, type: Define.ETouchEvent.PointerUp);
        gameObject.BindEvent(OnDrag, type: Define.ETouchEvent.Drag);

        SetActiveJoystick(false);
    }

	#region Event

	public void OnPointerDown(PointerEventData evt)
	{
        SetActiveJoystick(true);

        _joystickTouchPos = Input.mousePosition;

        if (Managers.Game.JoystickType == Define.EJoystickType.Flexible)
        {
            _handler.transform.position = Input.mousePosition;
            _joystickBG.transform.position = Input.mousePosition;
        }
    }

    public void OnPointerUp()
    {
        _moveDir = Vector2.zero;
        _handler.transform.position = _joystickOriginalPos;
        _joystickBG.transform.position = _joystickOriginalPos;
        Managers.Game.MoveDir = _moveDir;
        SetActiveJoystick(false);
    }

    public void OnPointerUp(PointerEventData evt)
	{
        OnPointerUp();
    }

	public void OnDrag(PointerEventData eventData)
	{
        Vector2 dragePos = eventData.position;

        _moveDir = Managers.Game.JoystickType == Define.EJoystickType.Fixed
            ? (dragePos - _joystickOriginalPos).normalized
            : (dragePos - _joystickTouchPos).normalized;

        // 조이스틱이 반지름 안에 있는 경우
        float joystickDist = (dragePos - _joystickOriginalPos).sqrMagnitude;

        Vector3 newPos;
        // 조이스틱이 반지름 안에 있는 경우
        if (joystickDist < _joystickRadius)
        {
            newPos = _joystickTouchPos + _moveDir * joystickDist;
        }
        else // 조이스틱이 반지름 밖에 있는 경우
        {
            newPos = Managers.Game.JoystickType == Define.EJoystickType.Fixed
                ? _joystickOriginalPos + _moveDir * _joystickRadius
                : _joystickTouchPos + _moveDir * _joystickRadius;
        }

        _handler.transform.position = newPos;
        Managers.Game.MoveDir = _moveDir;
    }

    void SetActiveJoystick(bool isActive)
    {
        if (isActive == true)
        {
            _handler.GetComponent<Image>().DOFade(1, 0.5f);
            _joystickBG.GetComponent<Image>().DOFade(1, 0.5f);
        }
        else
        {
            _handler.GetComponent<Image>().DOFade(0, 0.5f);
            _joystickBG.GetComponent<Image>().DOFade(0, 0.5f);
        }
    }

    public void OnTimeScaleChanged(int timeScale)
    {
        if (timeScale == 1)
        {
            gameObject.SetActive(true);
            OnPointerUp();
        }
        else
            gameObject.SetActive(false);
    }
    #endregion
}
