namespace TextAdventure;

// Ab hier die Modellklassen zur Story-Definition

// Das interface Story definiert Methoden und Properties zur Strukturierung einer Geschichte für das Text Adventure.
// eine Story ist dabei eine "Baum"-artige Datenstruktur zur Verbindung von Abschnitten (StoryNode) unserer Geschichte.
// Stell dir einen Baumstamm vor der in viele Äste verzweigt. Unser StartNode ist der Baumstamm und die Äste sind die weiteren StoryNodes die sich
// je nach Auswahl des Users in der Geschichte immer weiter verzweigen können. 
public interface Story
{
    // StartNode repreäsentiert den Einstieg in die Geschichte
    StoryNode StartNode { get; }

    // Die Methode NextNode gibt zur gegebenen NodeId den nächsten StoryNode zurück
    StoryNode NextNode(string nodeId);

    // Wenn kein passender StoryNode gefunden wird, dann wird der ErrorNode zurück gegeben.
    StoryNode ErrorNode { get; }
}

// Definiert die StoryNode-Klasse. StoryNodes sind die Beschreiben einen Abschnitt in einer Geschichte. 
public class StoryNode
{
    // Id ist eine interne Kennzeichnung um den StoryNode eindeutig Identifizieren zu können.
    public string? Id { get; init; }

    // Text ist der Text zu dem Abschnitt
    public string? Text { get; set; }

    // Choices definiert die Liste der Auswahlmöglichkeiten in diesem Story Abschnitt
    public List<StoryChoice>? Choices { get; set; }
}

// Definiert die Choice-Klasse. Diese beschreibt eine Auswahlmöglichkeit für einen Abschnitt in der Geschichte.
public class StoryChoice
{
    // Text der Text der bei der Auswahl angeziegt wird.
    public string? Text { get; set; }

    // Die Eingabe die der User eingeben kann um die Auswahl zu selektieren (oder er klickt auf den passenden Button in der Oberfläche)
    public string? Keyword { get; set; }

    // Die interne NodeId mit dem nächsten Story-Abschnitt passend zur Auswahl des Benutzers.
    public string? NextNodeId { get; set; }
}

// Die Klasse DemoStory implementiert das Story interface und definiert eine kurze Story mit verschiedenen Abschnitten und Varianten.
// Zur Strukturierung des Geschichtsablaufs wird im TextAdventures als Konvention für die NodeId definition folgende Struktur verwendet: Kapitel_Akt_Szene_Auswahlmöglichkeit
// mit Ausnahme des Start StoryNodes dort heißt die Id einfach "start". 
public class DemoStory : Story
{
    private readonly List<StoryNode> storieNodes;

    public DemoStory()
    {
        var nodes = new List<StoryNode>
        {
            new() // StoryNode
            {
                Id = "start",
                Text = "You find yourself at a fork in the road. Which path do you choose?",
                Choices = new List<StoryChoice>
                {
                    new() // StoryChoice
                    {
                        Text = "Take the left path", Keyword = "left", NextNodeId = "Chapter1_Act1_Scene1_C1"
                    },
                    new()
                    {
                        Text = "Take the right path", Keyword = "right", NextNodeId = "Chapter1_Act1_Scene1_C2"
                    },
                }
            },

            new()
            {
                Id = "Chapter1_Act1_Scene1_C1",
                Text = "You've chosen the left path and arrive at a river. What do you do?",
                Choices = new List<StoryChoice>
                {
                    new()
                    {
                        Text = "Swim across the river", Keyword = "swim", NextNodeId = "Chapter1_Act1_Scene2_C1"
                    },
                    new()
                    {
                        Text = "Look for a bridge", Keyword = "bridge", NextNodeId = "Chapter1_Act1_Scene2_C2"
                    },
                }
            },
            new()
            {
                Id = "Chapter1_Act1_Scene1_C2",
                Text = "You've chosen the right path and encounter a bear. What do you do?",
                Choices = new List<StoryChoice>
                {
                    new() { Text = "Climb a tree", Keyword = "climbTree", NextNodeId = "..." },
                    new() { Text = "Try to scare the bear", Keyword = "scareBear", NextNodeId = "..." },
                }
            },

            new()
            {
                Id = "Chapter1_Act1_Scene2_C1",
                Text = "You arrive at the other side of the river. What do you do?",
                Choices = new List<StoryChoice>
                {
                    new() { Text = "Look at the flowers", Keyword = "look", NextNodeId = "..." },
                    new() { Text = "Lay down and sleep", Keyword = "sleep", NextNodeId = "..." },
                }
            },

            new()
            {
                Id = "Chapter1_Act1_Scene2_C2",
                Text = "You arrived at the bridge, it looks derelict. Do you walk across, or walk back?",
                Choices = new List<StoryChoice>
                {
                    new()
                    {
                        Text = "Walk across the bridge", Keyword = "across", NextNodeId = "Chapter1_Act1_Scene3_C1"
                    },
                    new() { Text = "Walk back", Keyword = "back", NextNodeId = "Chapter1_Act1_Scene1_C1" },
                }
            },

            // Add additional nodes as needed
        };

        storieNodes = nodes;
    }

    public StoryNode StartNode => storieNodes[0];

    public StoryNode ErrorNode =>
        new()
        {
            Id = "Error",
            Text = "Something went wrong, sorry!",
            Choices = new List<StoryChoice>()
        };

    public StoryNode NextNode(string nodeId)
    {
        StoryNode? storyNode = storieNodes.Find(node => node.Id == nodeId);
        return storyNode ?? ErrorNode;
    }
}