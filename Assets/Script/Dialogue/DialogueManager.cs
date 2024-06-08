using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{

    [SerializeField] private TopDown_Transition transition;

    [Header("Params")]

    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Dialogue UI")]

    [SerializeField] private GameObject dialoguePanel;

    [SerializeField] private GameObject continueIcon;

    [SerializeField] private TextMeshProUGUI dialogueText;
    
    [SerializeField] private TextMeshProUGUI displayNameText;

    [SerializeField] private Animator portaitAnimator;

    private Animator layoutAnimator;
    
    [Header("Choices UI")]
    
    [SerializeField] private GameObject[] choices;
    
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    //Para que no pueda pasar la siguiente linea hasta que termine la frase.
    private bool canContinueToNextLine = false;

    private Coroutine displayLineCoroutine;

    private static DialogueManager instance;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    private const string CHANGE_SCENE_TAG = "change_scene";

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Hay más de uno Dialogue Manager en la Escena");
        }

        instance = this; 
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        layoutAnimator = dialoguePanel.GetComponent<Animator>();

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }
        //Manejar continuar a la siguiente línea en el diálogo cuando se presiona enviar
        if (canContinueToNextLine && currentStory.currentChoices.Count == 0 &&
            InputManager.GetInstance().GetSubmitPressed())
        {
          
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        //inicia una nueva historia
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);


        //reset portrait, layout , and speaker
        displayNameText.text = "???";
        portaitAnimator.Play("default");
        layoutAnimator.Play("left");

        ContinueStory();
    }
   
    private IEnumerator ExitDialogueMode()
    {
        //Da un poco espacio al terminar el dialogo
        yield return new WaitForSeconds(0.2f);



        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        //Lo ponemos vacío por si acaso.
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //Es como sacar linea de pila como tal para dar la siguiente linea de dialogo
            //Si intentamos parar una corrutina = null dará error.
            if (displayLineCoroutine!= null)
            {
  
                StopCoroutine(displayLineCoroutine);
            }

            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));

            //Handle tags
            HandleTags(currentStory.currentTags);
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    //para hacer el efecto de dialogo
    private IEnumerator DisplayLine(string line)
    {
        //establezca el texto en la línea completa, pero establezca el carácter visible en 0
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        //ocultar elementos mientras se escribe texto
        continueIcon.SetActive(false);
        HideChoices();
        canContinueToNextLine = false;

        bool isAddingRichTextTag = false;

        //StartCoroutine(CanSkip());

        //mostrar cada letra una a la vez
        foreach (char letter in line.ToCharArray())
        {

            if (InputManager.GetInstance().GetSubmitPressed())
            {
                dialogueText.maxVisibleCharacters = line.Length;
                break;
            }

            //verifique la etiqueta de texto enriquecido; si la encuentra, agréguela sin esperar
            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;

                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            else
            {
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }


        //Acciones a tomar después de que todas las líneas hayan terminado de mostrarse.
        continueIcon.SetActive(true);
        DisplayChoices();

        canContinueToNextLine= true;
    }


    //Para tener oculto las opciones hasta que aparezcan
    private void HideChoices()
    {
        foreach(GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        //Recorre cada tag del archivo INK y maneja en consecuencia
        foreach (string tag in currentTags)
        {
            //parseo the tag
            string[] splitTag = tag.Split(':');
            //Primera es la clave y el segundo es el valor
            if(splitTag.Length != 2)
            {
                Debug.LogError("La etiqueta no se pudo parsear adecuadamente: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch(tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    //Debug.Log("speaker =" + tagValue);
                    break;
                case PORTRAIT_TAG:
                    portaitAnimator.Play(tagValue);
                    //Debug.Log("portrait =" + tagValue);
                    break;
                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    //Debug.Log("layout =" + tagValue);
                    break;
                case CHANGE_SCENE_TAG:
                    ChangeScene(tagValue);
                    break;
                default:
                    Debug.LogWarning("La etiqueta llegó pero no se está manejando actualmente: " + tag);
                    break;
            }

        }
    }

    private void ChangeScene(string sceneName)
    {
        transition.FadeOutAndLoadScene(sceneName);
    }


    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        //verificación para asegurarnos de que nuestra interfaz de usuario pueda admitir la cantidad de opciones entrantes.
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("Se ofrecieron más opciones de las que la interfaz de usuario puede admitir. Número de opciones: " + currentChoices.Count);
        }

        int index = 0;
        //habilitar e inicializar las opciones hasta la cantidad de opciones para esta línea de diálogo
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            //Debug.Log(choice.text);
            index++;
        }

        //revisa las opciones restantes que admite la interfaz de usuario y asegúrate de que estén ocultas
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        //Event System requiere que lo borremos primero y luego esperemos 
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);

            InputManager.GetInstance().RegisterSubmitPressed();
            ContinueStory();
        }
    }
}
