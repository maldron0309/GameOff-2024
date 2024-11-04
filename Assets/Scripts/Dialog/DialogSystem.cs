using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class DialogSystem : MonoBehaviour
{
    public static DialogSystem instance;
    public GameObject dialogWindow;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI speakerNameText;
    public Image speakerImage;
    public GameObject decisionPanel;
    public Button optionButtonPrefab;
    public Transform optionsContainer;
    public GameStateData gameData;

    private BaseDialogNode currentNode;
    private bool isDecision;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        dialogWindow.SetActive(false);
    }

    void Update()
    {
        if (dialogWindow.activeInHierarchy && !isDecision && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            AdvanceDialog();
        }
    }

    public void StartDialog(DialogGraph dialogGraph)
    {
        // might need to disable player charecter movment when dialog is open
        //PlayerCharacter.instance.DisableInteractions();
        
        dialogWindow.SetActive(true);
        ShowNode(dialogGraph.startNode);
    }

    void ShowNode(BaseDialogNode nextNode)
    {
        currentNode = nextNode;
        if (currentNode is DialogNode dialogNode)
        {
            ShowDialogNode(dialogNode);
        }
        else if (currentNode is DecisionNode decisionNode)
        {
            ShowDecisionNode(decisionNode);
        }
        else if (currentNode is ModifyStateNode actionNode)
        {
            PerformActionNode(actionNode);
        }
    }

    void ShowDialogNode(DialogNode dialogNode)
    {
        dialogWindow.SetActive(true);
        speakerNameText.text = dialogNode.speakerName;
        dialogText.text = dialogNode.dialogText;
        speakerImage.sprite = dialogNode.speakerImage;

        isDecision = false;
        decisionPanel.SetActive(false);

    }

    void ShowDecisionNode(DecisionNode decisionNode)
    {
        isDecision = true;
        decisionPanel.SetActive(true);
        foreach (Transform child in optionsContainer)
        {
            Destroy(child.gameObject);
        }

        int validDecisions = 0;
        // Iterate over all decision options (NextNodes)
        for (int i = 0; i < decisionNode.nextNodes.Count; i++)
        {
            BaseDialogNode nextNode = decisionNode.nextNodes[i];
            bool allConditionsMet = true;

            // Check all conditions for this next node
            foreach (var condition in nextNode.conditions)
            {
                if (!condition.Evaluate(gameData))
                {
                    allConditionsMet = false;
                    break;
                }
            }

            // If all conditions are met, display the option
            if (allConditionsMet)
            {
                Button optionButton = Instantiate(optionButtonPrefab, optionsContainer);
                TextMeshProUGUI optionText = optionButton.GetComponentInChildren<TextMeshProUGUI>();                
                optionText.text = decisionNode.options[i];

                int capturedIndex = i;
                optionButton.transform.Translate(Vector2.down * 60 * validDecisions);
                optionButton.onClick.AddListener(() => OnOptionSelected(capturedIndex));
                validDecisions++;
            }
        }
    }

    void OnOptionSelected(int index)
    {
        ShowNode(currentNode.nextNodes[index]);
    }

    void PerformActionNode(ModifyStateNode actionNode)
    {
        actionNode.TriggerEvent();
        AdvanceDialog();
    }

    void AdvanceDialog()
    {
        BaseDialogNode nextNode = null;
        nextNode = null;
        if (currentNode.nextNodes.Count > 0)
        {
            foreach (var node in currentNode.nextNodes)
            {
                bool nodePassed = true;
                // if all conditions pass then bool value stays true and we found the next node
                foreach (var condition in node.conditions)
                {
                    nodePassed = nodePassed && condition.Evaluate(gameData);
                }
                //node.stateConditions.Where(x => x.Evaluate(GameStateData)).FirstOrDefault();
                if (nodePassed)
                {
                    nextNode = node;
                    break;
                }
            }
        }

        if (nextNode == null)
        {
            EndDialog();
            return;
        }
        else
        {
            ShowNode(nextNode);
        }
    }

    void EndDialog()
    {
        dialogWindow.SetActive(false);
        //PlayerCharacter.instance.EnableInteractions();
        // Re-enable player controls here
    }
}
