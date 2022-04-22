using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawner : MonoBehaviour
{

    public int level;
    [SerializeField]
    private GameObject spikePrefab, spikesTopBot, safeZone;
    private List<GameObject> spikes = new List<GameObject>();

    public float playWidth;
    private float distandeBetweenSpikes = 1f;
    PoolEnemies poolEnemies;
    bool obstacleCActive = false;
    private void Awake()
    {

        //Se instancian todos los spikes posibles que se puedan en cada pared y se desactivan, el limite de instancias lo da el playWidth, entregandonos que area tiene para instanciar los prefabs.
        for (int i = 0; i < playWidth; i++)
        {
            GameObject go = Instantiate(spikePrefab, GetPosition(i), Quaternion.identity);
            go.transform.parent = transform;
            if (this.gameObject.name == "LeftSpikeSpawner")
            {
                go.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180)); //se rota el sprite en caso de que sea la pared de la izquierda.
            }
            go.SetActive(false);
            spikes.Add(go); //se añaden a la lista todas las instancias para poder tener un control de ellas.
        }
    }
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        poolEnemies = PoolEnemies.Instance;
    }

    //Se utiliza este metodo para acceder a la lista y activar aleatoriamente los spikes de esta, por lo tanto nunca salen repetidos y no se consume memoria con la instanciacion.
    public void ActivateSpikes()
    {
        for (int i = 0; i < spikes.Count; i++)
        {
            if (Random.Range(0, 100) <= 30)
            {
                spikes[i].SetActive(true);
                spikes[i].transform.GetChild(0).GetComponent<Animator>().SetTrigger("On");
            }
        }

        if (GameManager.instance.score >= 3)
        {
            if (Random.Range(0, 2) == 0)
            {
                if (this.gameObject.name == "LeftSpikeSpawner")
                {
                    poolEnemies.SpawnFromPool("ObstacleB", transform.position, new Quaternion(0, 0, 180, 0));
                }
                else
                {
                    poolEnemies.SpawnFromPool("ObstacleB", transform.position, Quaternion.identity);
                }
            }
        }
        if (GameManager.instance.score >= 5)
        {
            float x = Random.Range(-1.5f, 1.5f);
            float y = Random.Range(-2f, 2f);
            Vector3 obstaclePosition = new Vector3(x, y, 0);

            if (Random.Range(0, 2) == 0)
            {
                poolEnemies.SpawnFromPool("ObstacleA", obstaclePosition, Quaternion.identity);
            }
        }
        if (GameManager.instance.score >= 10)
        {
            if (Random.Range(0, 2) == 0 && !obstacleCActive)
            {
                obstacleCActive = true;
                if (Random.Range(0, 2) == 1)
                {
                    poolEnemies.SpawnFromPool("ObstacleC", Vector3.zero, new Quaternion(0, 0, 180, 0));
                }
                else
                {
                    poolEnemies.SpawnFromPool("ObstacleC", Vector3.zero, Quaternion.identity);
                }
            }
        }
        if (GameManager.instance.score >= 7)
        {
            if (Random.Range(0, 2) == 0)
            {
                StartCoroutine(spikesTopBot.GetComponent<SpikesTopBot>().Attack1());
                safeZone.transform.GetChild(0).localScale = new Vector3(Random.Range(0.2f, 0.5f), 0.3f, 1);
            }
        }
    }

    //Desactivar todos los spikes de una pared, esto sucede cuando el jugador rebota contra esta.
    public void DeactivateSpikes()
    {
        for (int i = 0; i < spikes.Count; i++)
        {
            spikes[i].SetActive(false);
        }
        obstacleCActive = false;
        poolEnemies.ReturnToPool("ObstacleA");
        poolEnemies.ReturnToPool("ObstacleB");
        poolEnemies.ReturnToPool("ObstacleC");
    }

    //Se utiliza para darle la posicion correcta a cada spike en el primer momento que se instancian.
    private Vector3 GetPosition(int i)
    {
        Vector3 position = new Vector3(transform.position.x,transform.position.y - 3.5f,transform.position.z);
        position += Vector3.up * i * distandeBetweenSpikes;
        return position;
    }
    
}