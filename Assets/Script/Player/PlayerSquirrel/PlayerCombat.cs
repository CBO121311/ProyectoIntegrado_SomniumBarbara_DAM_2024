using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{

    [SerializeField] private int maxLife;
    [SerializeField] private float controlLossTime;
    [SerializeField] private Image heartLifePanel;
    [SerializeField] private Image faceLifePanel;

    [SerializeField] private List<Sprite> faceSprites;
    [SerializeField] private List<Sprite> heartSprites;

    private int life = 3;
    private ControllerPlayerSquirrel playerMovement;
    private Animator squirrelAnimator;
    private SpriteRenderer spriteRenderer;
    private bool isInvulnerable = false;


    [Header("Audio")]
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip teleportSound;
    private AudioSource audioSource;

    private void Start()
    {
        playerMovement = GetComponent<ControllerPlayerSquirrel>();
        squirrelAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        life = maxLife;
    }


    /// <summary>
    /// Aplica daño al jugador, activa una animación de golpe, y realiza acciones adicionales.
    /// </summary>
    /// <param name="damage">Cantidad de daño que se aplica al jugador.</param>
    /// <param name="position">Posición del golpe recibido.</param>
    public void takeDamage(int damage, Vector2 position, bool ignoreInvulnerability = false)
    {
        if (isInvulnerable && !ignoreInvulnerability) return;

        audioSource.PlayOneShot(damageSound);
        life -= damage;

        UpdateLifeSprite();

        squirrelAnimator.SetTrigger("Hit");
        StartCoroutine(LoseControl());
        StartCoroutine(DisableCollision());
        StartCoroutine(SetInvulnerable(1f));

        if (life <= 0)
        {
            StartCoroutine(HandleDeath(position));
            return;
        }
        playerMovement.BounceOnDamage(position);
    }

    private void UpdateLifeSprite()
    {
        if (life >= 0 && life < heartSprites.Count)
        {
            heartLifePanel.sprite = heartSprites[life];
            faceLifePanel.sprite = faceSprites[life];
        }
    }

    /// <summary>
    /// Corrutina que maneja la muerte del jugador.
    /// </summary>
    /// <param name="position">Posición del golpe recibido.</param>
    private IEnumerator HandleDeath(Vector2 position)
    {
        
        yield return new WaitForSeconds(0.5f);
        PlayerSpawnSquirrel playerSpawn = GetComponent<PlayerSpawnSquirrel>();
        playerSpawn.Death();
        GameEventsManager.instance.HitEnemy(20f);
        // Restablecer la vida del jugador
        life = maxLife;
        UpdateLifeSprite();
    }

    private IEnumerator DisableCollision()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        yield return new WaitForSeconds(controlLossTime);
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

    private IEnumerator LoseControl()
    {
        playerMovement.canMove = false;
        yield return new WaitForSeconds(controlLossTime);
        playerMovement.canMove = true;
    }

    private IEnumerator SetInvulnerable(float duration)
    {
        isInvulnerable = true;
        StartCoroutine(FlashSprite(duration)); // Iniciar el parpadeo del sprite
        yield return new WaitForSeconds(duration);
        isInvulnerable = false;
    }

    private IEnumerator FlashSprite(float duration)
    {
        float flashDelay = 0.1f;
        while (isInvulnerable)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashDelay);
        }
        spriteRenderer.enabled = true; // Asegurarse de que el sprite esté visible al final
    }
}
