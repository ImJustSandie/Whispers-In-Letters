using UnityEngine;

/// <summary>
/// Componente reutilizable de animación de pulso por emisión.
/// Extrae la lógica de "emission pulse" de PrologueItemInteractable
/// para poder aplicarla a cualquier objeto con Renderer de forma independiente.
///
/// CONFIGURACIÓN EN INSPECTOR:
///   emissionColor   → Color HDR de la emisión
///   pulseSpeed      → Velocidad de la pulsación
///   maxIntensity    → Intensidad máxima de la emisión
/// </summary>
public class PulseAnimation : MonoBehaviour
{
    [Header("Visual Feedback (Emission Pulse)")]
    [Tooltip("Color de la emisión (soporta HDR).")]
    [ColorUsage(true, true)]
    [SerializeField] private Color emissionColor = Color.white;

    [Tooltip("Velocidad de la pulsación.")]
    [SerializeField] private float pulseSpeed = 2f;

    [Tooltip("Intensidad máxima de la emisión.")]
    [SerializeField] private float maxIntensity = 1.5f;

    [Header("Pulse Interval")]
    [Tooltip("Duración en segundos que dura activa la animación de pulso.")]
    [SerializeField] private float pulseDuration = 2f;

    [Tooltip("Duración en segundos de la pausa entre ciclos de pulso.")]
    [SerializeField] private float pauseDuration = 1f;

    private Material _material;
    private static readonly int EmissionColorProperty = Shader.PropertyToID("_EmissionColor");

    private float _timer = 0f;
    private bool _isPulsing = true;

    private void Start()
    {
        SetupEmission();
    }

    private void SetupEmission()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            // Acceder a .material crea una instancia única para este objeto
            _material = rend.material;
            _material.EnableKeyword("_EMISSION");
        }
        else
        {
            Debug.LogWarning("[PulseAnimation] No se encontró un Renderer en este GameObject.");
        }
    }

    private void Update()
    {
        if (_material == null) return;

        _timer += Time.deltaTime;

        if (_isPulsing)
        {
            // Efecto de pulsación suave usando Seno basado en el tiempo (rango 0..1)
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            float currentIntensity = pulse * maxIntensity;
            _material.SetColor(EmissionColorProperty, emissionColor * currentIntensity);

            if (_timer >= pulseDuration)
            {
                // Termina el ciclo activo → apagar emisión y esperar
                _material.SetColor(EmissionColorProperty, Color.black);
                _isPulsing = false;
                _timer = 0f;
            }
        }
        else
        {
            // En pausa: esperar pauseDuration antes de volver a pulsar
            if (_timer >= pauseDuration)
            {
                _isPulsing = true;
                _timer = 0f;
            }
        }
    }

    private void OnDestroy()
    {
        // Limpiar la instancia de material para evitar fugas de memoria
        if (_material != null)
        {
            Destroy(_material);
        }
    }
}
