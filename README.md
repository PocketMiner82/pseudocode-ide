# pseudocode-ide
An Editor (and Interpreter) for the Pseudo Code defined for the 2024 Abitur.<br>
The remaining part of the README will be in German.

## Grundlegende Verwendung
Dieses Programm ermöglicht die Ausführung von Pseudocode nach der [Formelsammlung 1.5.2 TG Informationstechnik](https://www.schule-bw.de/faecher-und-schularten/berufliche-schularten/berufliches-gymnasium-oberstufe/musterpruefungsaufgaben-neue-bildungsplaene-abitur-2024/formelsammlung-it.pdf) für das Abitur 2024 in Baden Württemberg.

### Zusätzlich zu der Definition in der Formelsammlung wurden folgende Operationen definiert
* `UND` - Und-Vergleich
* `ODER` - Oder-Vergleich
* `schreibe(text, neueZeile = true)` - Schreibt den gegeben Text in das Ausgabefenster.
  - `text`: Der auszugebende Text
  - `neueZeile`: Wenn `WAHR`: Eine neue Zeile wird am Ende angehängt
* `warte(zeitMs)` - Unterbricht die Ausführung des Programms für eine bestimmte Zeit.
  - `zeitMs`: Die Zeit in Millisekunden.
* `benutzereingabe<Typ>(text, titel):Typ` - Öffnet ein Dialogfenster.
  - `text`: Der im Dialogfenster angezeigte Infotext
  - `titel`: Der Titel des Dialogfensters
  - Rückgabetyp: Der Rückgabetyp ist der in `<` und `>` angegebene `Typ`
* Nicht implementiert:
  - Unterstützung für mehrere Dateien
  - Klassen/Objekte - erfordert Unterstützung für mehrere Dateien; außerdem sind Klassen/Objekte nicht in der Formelsammlung definiert.
