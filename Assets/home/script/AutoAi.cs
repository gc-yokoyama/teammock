using UnityEngine;
using System.Collections;

public class AutoAi : MonoBehaviour {

    private Animator anim;
    private AnimatorStateInfo currentState;     // 現在のステート状態を保存する参照
    private AnimatorStateInfo previousState;    // ひとつ前のステート状態を保存する参照

    private bool m_doAi = false;
    private int m_aiType = 0;

    private Timer aiTimer = new Timer();
    private Timer preAiTimer = new Timer();
    private Timer relaxTimer = new Timer();
    private Timer animationStartTimer = new Timer();
    private Timer nodoDeleteTimer = new Timer();


    private int juiceState = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        currentState = anim.GetCurrentAnimatorStateInfo(0);
        previousState = currentState;

        startAi();
    }

	void Update ()
    {
        if (m_doAi) {
//            updateArrow();
//            updateAnimation();
            updateRotation();
            updatePosition();

            if (aiTimer.Update()) {
                aiTimer.IsEnable = false;
            }
            if (preAiTimer.Update()) {
                preAiTimer.IsEnable = false;
            }
            if (relaxTimer.Update()) {
                relaxTimer.IsEnable = false;
            }
            if (animationStartTimer.Update()) {
                animationStartTimer.IsEnable = false;
            }
            if (nodoDeleteTimer.Update()) {
                nodoDeleteTimer.IsEnable = false;
            }
        }
	}

    private void animationStartCall(int opaqueIntValue, bool opaqueBoolValue)
    {
        if (opaqueIntValue == 1) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Standing(loop)")) {
                anim.Play("Standing(loop)");
            }
        } else if (opaqueIntValue == 2) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Walking(loop)")) {
                anim.Play("Walking(loop)");
            }
        } else if (opaqueIntValue == 3) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("KneelDown")) {
                anim.Play("KneelDown");
            }
        } else if (opaqueIntValue == 4) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("KneelDownToUp")) {
                anim.Play("KneelDownToUp");
            }
        } else if (opaqueIntValue == 5) {
        } else if (opaqueIntValue == 6) {
        }
    }

    private void animationTimerStart(int opaqueIntValue, float limitTime = 0.01f)
    {
        animationStartTimer = new Timer();
        animationStartTimer.LimitTime = limitTime;
        animationStartTimer.OpaqueIntValue = opaqueIntValue;
        animationStartTimer.FireDelegate = animationStartCall;
    }

    private void preAiTimerCall(int intValue, bool boolValue)
    {
        preAiTimer = new Timer();
        preAiTimer.LimitTime = 0.01f;
        preAiTimer.FireDelegate = nextAi;
    }

    private void relaxCall(float limitTime = 3.0f)
    {
        relaxTimer = new Timer();
        relaxTimer.LimitTime = limitTime;
        relaxTimer.FireDelegate = nextTime;
    }

    private void nextTime(int intValue, bool boolValue)
    {
        if (m_aiType == 5) {
            if (juiceState == 3) {
                animationTimerStart(2);
                offLight();
                juiceState++;
            } else if (juiceState == 5) {
                animationTimerStart(3);
                juiceState++;
            } else if (juiceState == 7) {
                animationTimerStart(4);
                juiceState++;
            } else if (juiceState == 9) {
                aiTimer = new Timer();
                aiTimer.LimitTime = 1.0f;
                aiTimer.FireDelegate = preAiTimerCall;
            }
        }
    }

    private void nodoDelete(int intValue, bool boolValue)
    {
        GameObject.Find("nodoRoot(Clone)").GetComponent<NodoRootScript>().delete();;
    }

    public void startAi()
    {
        m_doAi = true;
        animationTimerStart(1);

        aiTimer = new Timer();
        aiTimer.LimitTime = 5.0f;
        aiTimer.FireDelegate = preAiTimerCall;
    }

    private void nextAi(int intValue, bool boolValue)
    {
        m_aiType = new System.Random().Next(0, 6);

        m_aiType = 5;

        if (m_aiType == 5) {
            GameObject prefab = (GameObject)Resources.Load("nodoRoot");
            GameObject child = Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;

            nodoDeleteTimer = new Timer();
            nodoDeleteTimer.LimitTime = 3.0f;
            nodoDeleteTimer.FireDelegate = nodoDelete;

            if (isDrinkRight()) {
                juiceState = 0;
            } else {
                juiceState = 1;
            }
        }

        startAiAnimation();

        if (m_aiType != 5) {
            aiTimer = new Timer();
            aiTimer.LimitTime = 5.0f;
            aiTimer.FireDelegate = preAiTimerCall;
        }
    }

    private void startAiAnimation()
    {
        if (m_aiType == 0) {
            animationTimerStart(1);
        } else if (m_aiType == 1) {
            animationTimerStart(2);
        } else if (m_aiType == 2) {
            animationTimerStart(2);
        } else if (m_aiType == 3) {
            animationTimerStart(2);
        } else if (m_aiType == 4) {
            animationTimerStart(2);
        } else if (m_aiType == 5) {
            animationTimerStart(2);
        }
    }

    private void updateArrow()
    {
    }

    private void startAnimation()
    {

    }

/*
    private void updateAnimation()
    {
        if (m_aiType == 0) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Standing(loop)")) {
                anim.Play("Standing(loop)");
            }
        } else if (m_aiType == 1) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Walking(loop)")) {
                anim.Play("Walking(loop)");
            }
        } else if (m_aiType == 2) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Walking(loop)")) {
                anim.Play("Walking(loop)");
            }
        } else if (m_aiType == 3) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Walking(loop)")) {
                anim.Play("Walking(loop)");
            }
        } else if (m_aiType == 4) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Walking(loop)")) {
                anim.Play("Walking(loop)");
            }
        } else if (m_aiType == 5) {
        }
    }
*/

    private void downRotation()
    {
        GameObject unityChan = GameObject.Find("unityChan");
        Vector3 rotation = unityChan.transform.eulerAngles;

        // down
        if (rotation.y > 0.0f && rotation.y <= 180.0f) {
            rotation.y += 1.5f;
        } else if (rotation.y > 180.0f && rotation.y < 360.0f) {
            rotation.y -= 1.5f;
        }
        if (rotation.y < 0.0f) {
            rotation.y  = 0.0f;
        }
        if (rotation.y >= 360.0f) {
            rotation.y = 0.0f;
        }
        unityChan.transform.eulerAngles = rotation;
    }

    private void leftRotation()
    {
        GameObject unityChan = GameObject.Find("unityChan");
        Vector3 rotation = unityChan.transform.eulerAngles;

        // left
        if (rotation.y >= 0.0f && rotation.y <= 90.0f) {
            rotation.y -= 1.5f;
        } else if (rotation.y > 90.0f && rotation.y <= 270.0f) {
            rotation.y += 1.5f;
        } else if (rotation.y > 270.0f && rotation.y <= 360.0f) {
            rotation.y -= 1.5f;
        }
        if (rotation.y < 0.0f) {
            rotation.y += 360.0f;
        }
        unityChan.transform.eulerAngles = rotation;
    }

    private void upRotation()
    {
        GameObject unityChan = GameObject.Find("unityChan");
        Vector3 rotation = unityChan.transform.eulerAngles;

        // up
        if (rotation.y > 0.0f && rotation.y <= 180.0f) {
            rotation.y -= 1.5f;
        } else if (rotation.y > 180.0f && rotation.y < 360.0f) {
            rotation.y += 1.5f;
        }
        if (rotation.y < 0.0f) {
            rotation.y  = 0.0f;
        }
        if (rotation.y >= 360.0f) {
            rotation.y = 0.0f;
        }
        unityChan.transform.eulerAngles = rotation;
    }

    private void rightRotation()
    {
        GameObject unityChan = GameObject.Find("unityChan");
        Vector3 rotation = unityChan.transform.eulerAngles;

        // right
        if (rotation.y >= 0.0f && rotation.y <= 90.0f) {
            rotation.y += 1.5f;
        } else if (rotation.y > 90.0f && rotation.y <= 270.0f) {
            rotation.y -= 1.5f;
        } else if (rotation.y > 270.0f && rotation.y < 360.0f) {
            rotation.y += 1.5f;
        }
        if (rotation.y < 0.0f) {
            rotation.y  = 0.0f;
        }
        if (rotation.y >= 360.0f) {
            rotation.y = 0.0f;
        }
        unityChan.transform.eulerAngles = rotation;
    }

    private void updateRotation()
    {
        if (m_aiType == 0) {
            return;
        }
        if (m_aiType == 1) {
            downRotation();
        } else if (m_aiType == 2) {
            leftRotation();
        } else if (m_aiType == 3) {
            upRotation();
        } else if (m_aiType == 4) {
            rightRotation();
        } else if (m_aiType == 5) {
            if (juiceState == 0) {
                rightRotation();
            } else if (juiceState == 1) {
                if (isDrinkUp()) {
                    upRotation();
                } else {
                    downRotation();
                }
            } else if (juiceState == 2) {
                leftRotation();
            } else if (juiceState == 4) {
                rightRotation();
            }
        }
    }


    private void updatePosition()
    {
        if (m_aiType == 0) {
            return;
        }
        GameObject unityChan = GameObject.Find("unityChan");
        Vector3 pos = unityChan.transform.position;
        if (m_aiType == 1) {
            // down
            if (pos.z <= -8.0f) {
                // stop
                m_aiType = 0;
                animationTimerStart(1);
                return;
            }
            pos.z -= 0.02f;
            transform.position = pos;
        } else if (m_aiType == 2) {
            // left
            pos.x -= 0.02f;
            transform.position = pos;
        } else if (m_aiType == 3) {
            // up
            pos.z += 0.02f;
            transform.position = pos;
        } else if (m_aiType == 4) {
            // right
            if (pos.x >= 8.0f) {
                // stop
                m_aiType = 0;
                animationTimerStart(1);
                return;
            }
            pos.x += 0.02f;
            transform.position = pos;
        } else if (m_aiType == 5) {
            GameObject nodoRoot = GameObject.Find("nodoRoot(Clone)");
            if (juiceState == 0) {
                pos.x += 0.02f;
                transform.position = pos;

                if (nodoRoot != null) {
                    Vector3 nodoPos = nodoRoot.transform.position;
                    nodoPos.x += 0.02f;
                    nodoRoot.transform.position = nodoPos;
                }

                if (!isDrinkRight()) {
                    juiceState = 1;
                }
            } else if (juiceState == 1) {
                if (isDrinkUp()) {
                    // up
                    pos.z += 0.02f;
                    transform.position = pos;

                    if (nodoRoot != null) {
                        Vector3 nodoPos = nodoRoot.transform.position;
                        nodoPos.z += 0.02f;
                        nodoRoot.transform.position = nodoPos;
                    }

                    if (!isDrinkUp()) {
                        juiceState = 2;
                    }
                } else {
                    // down
                    pos.z -= 0.02f;
                    transform.position = pos;

                    if (nodoRoot != null) {
                        Vector3 nodoPos = nodoRoot.transform.position;
                        nodoPos.z -= 0.02f;
                        nodoRoot.transform.position = nodoPos;
                    }

                    if (isDrinkUp()) {
                        juiceState = 2;
                    }
                }
            } else if (juiceState == 2) {
                if (!isDrinkRight()) {
                    pos.x -= 0.02f;
                    transform.position = pos;

                    if (nodoRoot != null) {
                        Vector3 nodoPos = nodoRoot.transform.position;
                        nodoPos.x -= 0.02f;
                        nodoRoot.transform.position = nodoPos;
                    }

                    if (isDrinkRight()) {
                        juiceState = 3;
                        animationTimerStart(1);
                        onLight();
                        relaxCall();
                    }
                }
            } else if (juiceState == 4) {
                pos.x += 0.02f;
                transform.position = pos;

                if (nodoRoot != null) {
                    Vector3 nodoPos = nodoRoot.transform.position;
                    nodoPos.x += 0.02f;
                    nodoRoot.transform.position = nodoPos;
                }

                if (isDrinkPoint()) {
                    animationTimerStart(1);
                    relaxCall();
                    juiceState++;
                }
            } else if (juiceState == 6) {
                relaxCall(6.0f);
                juiceState++;
            } else if (juiceState == 8) {
                relaxCall();
                juiceState++;
            }
        }
    }

    private bool isDrinkUp()
    {
        GameObject drink = GameObject.Find("cold_drinks");
        Vector3 drinkPos = drink.transform.position;
        GameObject unityChan = GameObject.Find("unityChan");
        Vector3 unityPos = unityChan.transform.position;
        return unityPos.z < drinkPos.z;
    }

    private bool isDrinkRight()
    {
        GameObject drink = GameObject.Find("cold_drinks");
        Vector3 drinkPos = drink.transform.position;
        GameObject unityChan = GameObject.Find("unityChan");
        Vector3 unityPos = unityChan.transform.position;
        return unityPos.x < -5.3f;
    }

    private bool isDrinkPoint()
    {
        GameObject unityChan = GameObject.Find("unityChan");
        Vector3 unityPos = unityChan.transform.position;
        return unityPos.x >= 0.5f;
    }

    private void onLight()
    {
        GameObject prefab = (GameObject)Resources.Load("light");
        Instantiate(prefab);
        /*
        Vector3 pos = new Vector3(-5.05f, 4.33f, -0.52f);
        34.1227f, -43.846f, 0
        */
    }

    private void offLight()
    {
        GameObject light = GameObject.Find("light(Clone)");
        LightScript script = light.GetComponent<LightScript>();
        script.delete();
    }

}
