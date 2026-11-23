using UnityEngine;

// Galaxia espiral feita com o Particle System existente na cena.
[RequireComponent(typeof(ParticleSystem))]
public class GalaxiaEspiral : MonoBehaviour
{
    [Header("Formato")]
    [Range(2, 6)] public int bracos = 3;
    [Range(2f, 12f)] public float raio = 5.2f;
    [Range(1f, 9f)] public float voltas = 4.7f;
    [Range(0.05f, 1f)] public float dispersao = 0.34f;
    [Header("Quantidade")]
    [Range(500, 12000)] public int estrelas = 5500;
    [Range(0, 8000)] public int gas = 2600;
    [Range(0, 2000)] public int nucleo = 900;
    [Header("Movimento")]
    public float velocidadeAngular = 0.13f;
    [Range(0f, 1f)] public float inclinacao = 0.16f;

    private ParticleSystem sistema;
    private ParticleSystem.Particle[] particulas;
    private Vector3[] posicoes;
    private float[] velocidades;
    private int[] tipos;
    private float tempo;
    private const float Vida = 999999f;

    private void Awake()
    {
        sistema = GetComponent<ParticleSystem>();
        Criar();
    }

    private void OnValidate()
    {
        estrelas = Mathf.Max(1, estrelas);
        bracos = Mathf.Max(2, bracos);
    }

    private void Criar()
    {
        Random.InitState(2026);
        int total = estrelas + gas + nucleo;
        particulas = new ParticleSystem.Particle[total];
        posicoes = new Vector3[total];
        velocidades = new float[total];
        tipos = new int[total];

        var main = sistema.main;
        main.loop = false;
        main.playOnAwake = false;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
        main.startSpeed = 0f;
        main.startLifetime = Vida;
        main.maxParticles = total + 100;
        main.gravityModifier = 0f;
        main.startSize = 0.05f;

        var emission = sistema.emission;
        emission.enabled = false;
        var shape = sistema.shape;
        shape.enabled = false;
        var velocity = sistema.velocityOverLifetime;
        velocity.enabled = false;
        var force = sistema.forceOverLifetime;
        force.enabled = false;
        var colorOverLifetime = sistema.colorOverLifetime;
        colorOverLifetime.enabled = false;
        var sizeOverLifetime = sistema.sizeOverLifetime;
        sizeOverLifetime.enabled = false;

        for (int i = 0; i < total; i++)
        {
            int tipo = i < estrelas ? 0 : i < estrelas + gas ? 1 : 2;
            tipos[i] = tipo;
            float r;
            float angulo;

            if (tipo == 2)
            {
                r = Mathf.Sqrt(Random.value) * raio * 0.19f;
                angulo = Random.value * Mathf.PI * 2f;
            }
            else
            {
                r = Mathf.Lerp(0.22f, raio, Mathf.Pow(Random.value, 0.8f));
                int braco = Random.Range(0, bracos);
                angulo = braco * Mathf.PI * 2f / bracos +
                         (r / raio) * voltas +
                         Random.Range(-dispersao, dispersao) * (0.45f + r / raio);
                if (Random.value < 0.13f)
                    angulo = Random.value * Mathf.PI * 2f;
            }

            float altura = Random.Range(-1f, 1f) * inclinacao * (1f - r / (raio * 1.3f));
            posicoes[i] = new Vector3(Mathf.Cos(angulo) * r, Mathf.Sin(angulo) * r, altura);
            velocidades[i] = velocidadeAngular * Mathf.Lerp(2.5f, 0.6f, r / raio);

            Color cor;
            float tamanho;
            if (tipo == 2)
            {
                float t = Random.value;
                cor = Color.Lerp(new Color(1f, 0.52f, 0.19f, 0.65f),
                                 new Color(1f, 0.98f, 0.83f, 1f), t);
                tamanho = Random.Range(0.035f, 0.12f);
            }
            else if (tipo == 1)
            {
                cor = Color.Lerp(new Color(0.08f, 0.25f, 0.8f, 0.07f),
                                 new Color(0.35f, 0.7f, 1f, 0.2f), Random.value);
                tamanho = Random.Range(0.18f, 0.58f);
            }
            else
            {
                float escolha = Random.value;
                cor = escolha < 0.68f
                    ? new Color(0.37f, 0.68f, 1f, Random.Range(0.55f, 1f))
                    : escolha < 0.91f
                        ? new Color(0.94f, 0.95f, 1f, 1f)
                        : new Color(1f, 0.75f, 0.43f, 1f);
                tamanho = Random.value < 0.025f
                    ? Random.Range(0.11f, 0.21f)
                    : Random.Range(0.014f, 0.065f);
            }

            particulas[i].position = posicoes[i];
            particulas[i].startColor = cor;
            particulas[i].startSize = tamanho;
            particulas[i].remainingLifetime = Vida;
            particulas[i].startLifetime = Vida;
            particulas[i].randomSeed = (uint)(i + 1);
        }

        sistema.Clear();
        sistema.Play();
        sistema.SetParticles(particulas, particulas.Length);
    }

    private void LateUpdate()
    {
        if (particulas == null) return;
        float dt = Mathf.Min(Time.deltaTime, 0.05f);
        tempo += dt;
        for (int i = 0; i < particulas.Length; i++)
        {
            Vector3 p = posicoes[i];
            float a = velocidades[i] * tempo;
            float c = Mathf.Cos(a);
            float s = Mathf.Sin(a);
            particulas[i].position = new Vector3(
                p.x * c - p.y * s,
                p.x * s + p.y * c,
                p.z);
            particulas[i].remainingLifetime = Vida;
        }
        sistema.SetParticles(particulas, particulas.Length);
    }
}
