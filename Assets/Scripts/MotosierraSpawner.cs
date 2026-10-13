using UnityEngine;

public class MotosierraSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Camera camara;
    RoundManager roundManager;
    GameObject instance;
    Vector3 position;
    Quaternion rotation;
    int tiempoEntreSpawns = 10;
    bool invoked = false;
    bool firstInvoked = false;
    public AudioClip sonidoSierra;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        roundManager = FindAnyObjectByType<RoundManager>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = sonidoSierra;
    }

    // Update is called once per frame
    void Update()
    {
        if (roundManager.currentRound >= 2 && !invoked)
        {
            InvokeRepeating(nameof(spawn), 1f, tiempoEntreSpawns);
            invoked = true;
            if(firstInvoked == false)
            {
                firstInvoked = true;
            }
        }
        if(!audioSource.isPlaying)
        {
            if(firstInvoked == true)
            {
                audioSource.Play();
            }
        }

    }

    void spawn()
    {
        int x = Random.Range(0, 2);
        int y = Random.Range(0, 2);
        float angulo;
        position = camara.ViewportToWorldPoint(new Vector3(x, y, camara.nearClipPlane));
        angulo = Mathf.Atan2((position - camara.transform.position).normalized.y, (position - camara.transform.position).normalized.x) * Mathf.Rad2Deg;
        rotation = Quaternion.Euler(0, 0, angulo);
        instance = Instantiate(prefab, position, rotation);
        instance.transform.GetComponent<Motosierra>().SetRotationOffset(angulo);
    }
}
