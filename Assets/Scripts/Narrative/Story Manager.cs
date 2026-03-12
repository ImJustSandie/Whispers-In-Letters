using Ink.Runtime;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;

    public TextAsset inkJSON;

    private Story story;

    void Awake()
    {
        Instance = this;

        if (inkJSON == null)
        {
            Debug.LogError("[StoryManager] inkJSON no está asignado en el Inspector.");
            return;
        }

        story = new Story(inkJSON.text);
        Debug.Log("[StoryManager] Story inicializado correctamente.");
    }

    public void StartStory(string knot)
    {
        Debug.Log("[StoryManager] StartStory llamado con knot: '" + knot + "'");

        if (string.IsNullOrEmpty(knot))
        {
            Debug.LogWarning("[StoryManager] El knot está vacío o nulo.");
            return;
        }

        story.ChoosePathString(knot);
        ContinueStory();
    }

    public void ContinueStory()
    {
        while (story.canContinue)
        {
            string line = story.Continue();
            Debug.Log(line);
        }

        if (story.currentChoices.Count > 0)
        {
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                Debug.Log($"[{i}] {story.currentChoices[i].text}");
            }
        }
    }
}