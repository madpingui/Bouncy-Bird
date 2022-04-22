using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public static MobileInput Instance { set; get; }

    private Vector2 swipeDelta, startTouch;

    private Camera camera;

    public Vector2 SwipeDelta { get { return swipeDelta; } }

    private float tapTimer;
    private bool timerOn;

    private LineRenderer lineRend;

    private void Awake()
    {
        Instance = this;
        camera = Camera.main;

        lineRend = GetComponent<LineRenderer>();
        lineRend.enabled = false;
    }

    private void Update()
    {
        if (timerOn)
        {
            tapTimer += Time.fixedDeltaTime;
        }

        if (tapTimer > 0.3f)
        {
            if(Player.Instance.CanStopTime)
            {
            Time.timeScale = 0.3f;
            lineRend.enabled = true;
            lineRend.SetPosition(0, transform.position);
            lineRend.SetPosition(1, camera.ScreenToWorldPoint(Input.mousePosition));
            }           
        }

        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0))
        {
            startTouch = Input.mousePosition;
            timerOn = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (tapTimer > 0.4f && Player.Instance.CanStopTime)
            {
                if (swipeDelta.magnitude > 5)
                {
                    Player.Instance.MoveSwipePlayer(camera.ScreenToWorldPoint(Input.mousePosition));
                }
                else
                {
                    Player.Instance.MoveTapPlayer();
                }
            }
            else
            {
                Player.Instance.MoveTapPlayer();
            }

            Time.timeScale = 1;
            timerOn = false;
            tapTimer = 0;

            lineRend.enabled = false;
            lineRend.SetPosition(0, Vector3.zero);
            lineRend.SetPosition(1, Vector3.zero);

            startTouch = swipeDelta = Vector2.zero;
        }
        #endregion

        #region Mobile Inputs
        if (Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                startTouch = Input.mousePosition;
                tapTimer += Time.fixedDeltaTime;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                if (tapTimer > 0.4f)
                {
                    if (swipeDelta.magnitude > 5)
                    {
                        Player.Instance.MoveSwipePlayer(camera.ScreenToWorldPoint(Input.mousePosition));
                    }
                    else
                    {
                        Player.Instance.MoveTapPlayer();
                    }
                }
                else
                {
                    Player.Instance.MoveTapPlayer();
                }

                Time.timeScale = 1;
                timerOn = false;
                tapTimer = 0;
                startTouch = swipeDelta = Vector2.zero;

                lineRend.enabled = false;
                lineRend.SetPosition(0, Vector3.zero);
                lineRend.SetPosition(1, Vector3.zero);
            }
        }
        #endregion

        swipeDelta = Vector2.zero;

        if (startTouch != Vector2.zero)
        {
            if (Input.touches.Length != 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }
            else if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
        }
    }
}
