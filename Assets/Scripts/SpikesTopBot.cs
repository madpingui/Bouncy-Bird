using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesTopBot : MonoBehaviour
{
    public GameObject safeZone;
    bool attacking;
    // Start is called before the first frame update

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Attack1()
    {
        if (!attacking)
        {
            attacking = true;
            safeZone.SetActive(true);
            float posX = Random.Range(-2.2f + (2 * safeZone.transform.localScale.y), 2.2f - (2 * safeZone.transform.localScale.y));

            safeZone.GetComponent<Animator>().SetTrigger("Begin");
            yield return new WaitForSeconds(0.5f);
            gameObject.GetComponent<Animator>().SetTrigger("Attack1");
            yield return new WaitForSeconds(3.21f);
            safeZone.GetComponent<Animator>().SetTrigger("End");
            safeZone.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            attacking = false;
        }

    }
}
