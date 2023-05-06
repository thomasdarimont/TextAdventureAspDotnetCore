ASP.Net Core TextAdventure
----

Beispiel für ein kleines Brwoser gesteuertes TextAdventure mit ASP.Net Core Backend 
auf Basis von "Minimal API" im Rahmen des coderdojo coding trainings für zukünftige Softwareentwickler:innen.

# Content
- In der Datei [Program.cs](./Program.cs) findet sich die definition der Web Application mit der Backend Logik und den HTTP Endpunkten.    
- In der Datei [Story.cs](./Story.cs) findet sich das Klassenmodell zur Abbildung einer interaktiven Story.  
- In der Datei [wwwroot/index.html](./wwwroot/index.html) ist ein kleines Frontend auf Basis von HTML, JavaScript und CSS zu finden.  

# Build

```
dotnet build
```

# Run
```
dotnet run
```

Danach ist die Webanwendung unter http://localhost:5096 erreichbar.

Mit einem Click auf "start" kann man das TextAdventure starten.
Über die Buttons kann man die Auswahlmöglichkeiten der jeweiligen
anwählen.