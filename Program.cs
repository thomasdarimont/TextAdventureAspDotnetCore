// Verwendet die Microsoft.AspNetCore.Mvc-Namespace
// die übrigen "usings" sind bei ASP.NET Core "Minimal APIs" implizit definiert (siehe Datei TextAdventure.csproj -> ImplicitUsings)
// die impliziten usings können in der Datei obj/Debug/net7.0/TextAdventure.GlobalUsings.g.cs eingesehen werden
using Microsoft.AspNetCore.Mvc;

using TextAdventure;

// Erstellt einen WebApplication-Builder zur Definition einer Web Anwendung
var builder = WebApplication.CreateBuilder(args);

// Fügt den NewtonsoftJson-Controller hinzu -> dieser Ermöglicht die Verarbeitung von Anfragen und Erzeugung von Antworten im JSON Format.
// Hinweis: Bei der Konvertierung von C# Objekten nach JSON werden die Properties automatisch kleingeschrieben!
builder.Services.AddControllers()
                .AddNewtonsoftJson();

// Fügt Session-Unterstützung hinzu - damit kann pro Benutzer auf dem Server Zustand über mehrere Anfragen verwaltet werden
// Wir brauchen die Session um Server-seitig zu Speichern an welcher Stelle von der Story der User sich befindet. 
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    // Setzt die Session-Timeout-Zeit auf 30 Minuten
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    // Setzt das HttpOnly-Flag für den Cookie (der Cookie wird dadurch für JavaScript nicht sichtbar gemacht! Das ist ein Security Feature)
    options.Cookie.HttpOnly = true;
    // Markiert den Cookie als essentiell und für die Anwendung benötigt der cookie ist damit nicht optional.
    options.Cookie.IsEssential = true;
});

// Erstellt die Anwendung
var app = builder.Build();
// Verwendet die Standarddateien mit Standard Konventiionen siehe https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files?source=recommendations&view=aspnetcore-7.0#serve-default-documents
app.UseDefaultFiles();
// Verwendet statische Dateien  siehe https://learn.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-7.0
app.UseStaticFiles();
// Verwendet das Session-Middleware hiermit wird die oben definierte Session Unterstützung genutzt.
app.UseSession();

// Erstellt eine neue DemoStory-Instanz - In dieser Beispielanwendung wird die Geschichte und möglichen Interaktionspfade mit ihren Verzweigungen als "Story" beschrieben.
// Die DemoStory demonstriert die Definition einer Geschichte.
// Siehe Story.cs
var story = new DemoStory();

// Definiert die Konstante für die aktuelle Node-ID -> mit diesem Namen merken wir uns in der Session an welcher Stelle der User in der Geschichte steht.
const string CURRENT_NODE_ID = "currentNodeId";

// Ab hier definieren wir unsere "Handler" für die API Endpunkte die wir über HTTP aufrufbar machen wollen.

// Mappt den Anfrage-Pfad: "/api/story/start" für HTTP-GET Aufrufe
app.MapGet("/api/story/start", async (HttpContext context) => {
        
    Console.WriteLine($"Starte Sitzung mit SessionID={context.Session.Id}");

    // Holt die aktuelle Node-ID aus der Session oder startet mit der ersten Node
    var currentNodeId = context.Session.GetString(CURRENT_NODE_ID) ?? story.StartNode.Id;

    Console.WriteLine($"Aktuelle Node={currentNodeId} SessionID={context.Session.Id}");

    return story.NextNode(currentNodeId);
});

// Mappt den Anfrage-Pfad: "/api/story/proceed" für HTTP-POST Aufrufe
app.MapPost("/api/story/proceed", async (HttpContext context, [FromBody] UserInput userInput) =>
{
    // Bestimmt die nächste Story-Node basierend auf der Benutzereingabe
    var nextNode = story.NextNode(userInput.Text);

    Console.WriteLine($"Aktuelle Node={nextNode.Id} SessionID={context.Session.Id}");

    // Speichert die aktuelle Node-ID in der Session
    context.Session.SetString(CURRENT_NODE_ID, nextNode.Id);

    return nextNode;
});

// Mappt den Anfrage-Pfad: "/api/story/reset" für HTTP-GET Aufrufe
app.MapGet("/api/story/reset", async (HttpContext context) => {

    Console.WriteLine($"Beende Sitzung mit SessionID={context.Session.Id}");

    // Leert die Session
    context.Session.Clear();

    // Löscht den aktuellen Session-Cookie
    context.Response.Cookies.Delete(".AspNetCore.Session");

    // Erzeugt eine neue Session-ID durch Erstellen eines neuen Session-Cookies
    context.Session.SetString("dummy", "dummy");
});

// Startet die Anwendung
app.Run();

// Ab hier definieren wir das Klassen Modell für Benutzereingaben, Antworten sowie Story-Definition

// Definiert den UserInput-Datentyp als c# record
record UserInput(string Text);

// Definiert den Response-Datentyp als  c# record
record Response(string Message);
