using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [Header("Params")]

    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Load Globals JSON")]

    [SerializeField] private TextAsset loadGlobalsJson;


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

    //Solución para evitar salto de texto
    //private bool canSkip;

    //private bool submitSkip;

    private Coroutine displayLineCoroutine;

    private static DialogueManager instance;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";

    private DialogueVariables dialogueVariables;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Hay más de uno Dialogue Manager en la Escena");
        }

        instance = this; 
        dialogueVariables = new DialogueVariables(loadGlobalsJson);
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
        //regresar inmediatamente si el diálogo no se reproduce

        //if (controls.UI.Submit.triggered)
        //Añadido
        /*if(Input.GetKeyDown(KeyCode.Z))
        {
            submitSkip = true;
        }*/

        if (!dialogueIsPlaying)
        {
            return;
        }
        //Manejar continuar a la siguiente línea en el diálogo cuando se presiona enviar
        //Note: the 'currentStory.currentChoices.Count == 0' part was to fix a bug
        //after the youtube was made
        if (canContinueToNextLine && currentStory.currentChoices.Count == 0 &&
            InputManager.GetInstance().GetSubmitPressed())
        {
            
            ContinueStory();
            //Debug.Log("Continuar historia");
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        //inicia una nueva historia
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        dialogueVariables.StartListening(currentStory);

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

        //Deja de escuchar la historia.
        dialogueVariables.StopListening(currentStory);

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
        //set the text to the full line, but set the visible character to 0
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;


        //hide items while text is typing
        continueIcon.SetActive(false);
        HideChoices();
        //submitSkip = false;
        canContinueToNextLine = false;

        bool isAddingRichTextTag = false;

        //StartCoroutine(CanSkip());

        //display each letter one at a time
        foreach (char letter in line.ToCharArray())
        {
            //if the submit button is pressed, finish up displaying the line right away
            //if(InputManager.GetInstance().GetSubmitPressed())

            /*if(canSkip && submitSkip)
            {
                submitSkip =false;
                dialogueText.maxVisibleCharacters = line.Length;
                break;
            }*/

            if (InputManager.GetInstance().GetSubmitPressed())
            {
                dialogueText.maxVisibleCharacters = line.Length;
                break;
            }

            //check for rich text tag, if found, add it without waiting
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


    

        //actions to take after the entire lines has finished displaying
        continueIcon.SetActive(true);
        DisplayChoices();

        canContinueToNextLine= true;
        //añadido
        //canSkip = false;
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
        //Recorre cada tag del archivo INK y manejala en consecuencia
        foreach (string tag in currentTags)
        {
            //parseo the tag
            string[] splitTag = tag.Split(':');
            //Primera es la clave y el segundo es el valor
            if(splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch(tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    Debug.Log("speaker =" + tagValue);
                    break;
                case PORTRAIT_TAG:
                    portaitAnimator.Play(tagValue);
                    //Debug.Log("portrait =" + tagValue);
                    break;
                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    //Debug.Log("layout =" + tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }

        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;
        ///verificación para asegurarnos de que nuestra interfaz de usuario pueda admitir la cantidad de opciones entrantes.
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

            //Note: The below two lines were added to fix a bug
            InputManager.GetInstance().RegisterSubmitPressed();
            ContinueStory();
        }
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if(variableValue == null)
        {
            Debug.LogWarning("Ink Variable was found to be null: " + variableName);
        }
        return variableValue;
    }



    /*private IEnumerator CanSkip()
    {
        canSkip = false; 
        yield return new WaitForSeconds(0.04f);
        canSkip = true;
    }*/
}
