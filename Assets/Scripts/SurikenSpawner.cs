using UnityEngine;

public class SurikenSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Camera camara;
    RoundManager roundManager;
    GameObject instance;
    Vector3 position;
    Quaternion rotation;
    int tiempoEntreSpawns = 10;
    bool invoked = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        roundManager = FindAnyObjectByType<RoundManager>();
    }

    

    void SpawnSuriken()
    {
        Vector3 pantallaMin = camara.ViewportToWorldPoint(new Vector3(0, 0, camara.nearClipPlane));
        Vector3 pantallaMax = camara.ViewportToWorldPoint(new Vector3(1, 1, camara.nearClipPlane));

        position = new Vector3(Random.Range(pantallaMin.x, pantallaMax.x), Random.Range(pantallaMin.y, pantallaMax.y), 0);
        rotation = Quaternion.Euler(position);
        instance = Instantiate(prefab, position, rotation);
        instance.transform.GetComponent<Suriken>().SetDirection(position);
    }

    // Update is called once per frame
    void Update()
    {
        if (roundManager.currentRound >= 3 && !invoked)
        {
            InvokeRepeating(nameof(SpawnSuriken), 1f, tiempoEntreSpawns);
            invoked = true;
        }
    }
}
