# pseudocode-ide
An Editor (and Interpreter) for the Pseudo Code defined for the 2024 Abitur.<br>
The remaining part of the README will be in German.

## Grundlegende Verwendung
Dieses Programm ermöglicht die Ausführung von Pseudocode nach der [Formelsammlung 1.5.2 TG Informationstechnik](https://www.schule-bw.de/faecher-und-schularten/berufliche-schularten/berufliches-gymnasium-oberstufe/musterpruefungsaufgaben-neue-bildungsplaene-abitur-2024/formelsammlung-it.pdf) für das Abitur 2024 in Baden-Württemberg.

### Zusätzlich zu den Definitionen in der Formelsammlung wurden folgendes hinzugefügt:
* `UND` - Und-Vergleich
* `ODER` - Oder-Vergleich
* `schreibe(wert, neueZeile = wahr)` oder `print(wert, neueZeile = wahr)` - Schreibt den gegeben Text in das Ausgabefenster.
  - `wert`: Der auszugebende Text
  - `neueZeile`: Wenn `wahr`: Eine neue Zeile wird am Ende angehängt
* `warte(zeitMs)` - Unterbricht die Ausführung des Programms für eine bestimmte Zeit.
  - `zeitMs`: Die Zeit in Millisekunden.
* `benutzereingabe<Typ>(nachricht, titel):Typ` - Öffnet ein Eingabefenster.
  - `nachricht`: Der im Eingabefenster angezeigte Infotext
  - `titel`: Der Titel des Eingabefensters
  - Rückgabetyp: Der Rückgabetyp ist der in `<` und `>` angegebene `Typ`. Sollte die Konvertierung fehlschlagen, wird NICHTS zurückgegeben.
  - z.B. würde `benutzereingabe<GZ>("Gib eine Zahl ein", "Zahl")` ein Eingabefenster mit dem Titel "Zahl" und dem Infotext "Gib eine Zahl ein" öffnen, welches den eingebenen Text als ganze Zahl (Integer) zurückgibt.
* Nicht implementiert:
  - Unterstützung für mehrere Dateien
  - Klassen/Objekte - erfordert Unterstützung für mehrere Dateien; außerdem sind Klassen/Objekte nicht in der Formelsammlung definiert.
