// Pseudocode IDE - Execute Pseudocode for the German (BW) 2024 Abitur
// Copyright (C) 2024  PocketMiner82
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY

using System.Collections.Generic;
using static pseudocode_ide.interpreter.pseudocode.TokenType;

namespace pseudocode_ide.interpreter.pseudocode
{
    public static class PseudocodeKeywords
    {
        /// <summary>
        /// All the defined keywords in pseudocode and their corresponding token.
        /// </summary>
        public static readonly PseudocodeTypeDictionary KEYWORDS = new PseudocodeTypeDictionary();

        /// <summary>
        /// Add the keywords contents to the readonly static list
        /// </summary>
        static PseudocodeKeywords()
        {
            // code block snippets
            KEYWORDS.Add(new PseudocodeType("WENN", IF, new List<PseudocodeAutocompleteItem>()
            {
                new PseudocodeAutocompleteItem("WENN ^bedingung^\n\t^code^\nENDE WENN", "WENN Block"),
                new PseudocodeAutocompleteItem("WENN ^bedingung^\n\t^code^\nSONST\n\t^code^\nENDE WENN", "WENN-SONST Block")
            }));
            KEYWORDS.Add(new PseudocodeType("FALLS", SWITCH_PREFIX, "FALLS ^variable^ GLEICH\n\t^bedingung1^:\n\t\t^code^\n\tSONST:\n\t\t^code^\nENDE FALLS"));
            KEYWORDS.Add(new PseudocodeType("SOLANGE", WHILE, "SOLANGE ^bedingung^\n\t^code^\nENDE SOLANGE"));
            KEYWORDS.Add(new PseudocodeType("WIEDERHOLE", DO, "WIEDERHOLE\n\t^code^\nSOLANGE ^bedingung^"));
            KEYWORDS.Add(new PseudocodeType("FÜR", FOR, new List<PseudocodeAutocompleteItem>()
            {
                new PseudocodeAutocompleteItem("FÜR ^variable^ BIS ^endwert^ SCHRITT ^erhöhung^\n\t^code^\nENDE FÜR", "FÜR-BIS Block"),
                new PseudocodeAutocompleteItem("FÜR ^variable^ IN ^liste^\n\t^code^\nENDE FÜR", "FÜR-IN Block")
            }));
            KEYWORDS.Add(new PseudocodeType("OPERATION", FUNCTION, "OPERATION ^name^()\n\t^code^"));
            KEYWORDS.Add(new PseudocodeType("Liste", TYPE_LIST, "Liste<^Typ^>"));

            // built-in methods
            KEYWORDS.Add(new PseudocodeType("schreibe", IDENTIFIER, "schreibe(^wert^)"));
            KEYWORDS.Add(new PseudocodeType("warte", IDENTIFIER, "warte(^millisekunden^)"));
            KEYWORDS.Add(new PseudocodeType("benutzereingabe", IDENTIFIER, "benutzereingabe<^Rückgabetyp^>(^nachricht^, ^titel^)"));


            // break
            KEYWORDS.Add(new PseudocodeType("ABBRUCH", BREAK));

            // function
            KEYWORDS.Add(new PseudocodeType("RÜCKGABE", RETURN));

            // booleans
            KEYWORDS.Add(new PseudocodeType("wahr", TRUE));
            KEYWORDS.Add(new PseudocodeType("true", TRUE));
            KEYWORDS.Add(new PseudocodeType("falsch", FALSE));
            KEYWORDS.Add(new PseudocodeType("false", FALSE));

            // compare operators
            KEYWORDS.Add(new PseudocodeType("UND", AND));
            KEYWORDS.Add(new PseudocodeType("ODER", OR));

            // boolean types
            KEYWORDS.Add(new PseudocodeType("bool", TYPE_BOOL));
            KEYWORDS.Add(new PseudocodeType("Boolean", TYPE_BOOL));
            KEYWORDS.Add(new PseudocodeType("boolean", TYPE_BOOL));

            // int types
            KEYWORDS.Add(new PseudocodeType("GZ", TYPE_INT));
            KEYWORDS.Add(new PseudocodeType("int", TYPE_INT));
            KEYWORDS.Add(new PseudocodeType("Integer", TYPE_INT));

            // double types
            KEYWORDS.Add(new PseudocodeType("FKZ", TYPE_DOUBLE));
            KEYWORDS.Add(new PseudocodeType("double", TYPE_DOUBLE));
            KEYWORDS.Add(new PseudocodeType("Real", TYPE_DOUBLE));

            // char types
            KEYWORDS.Add(new PseudocodeType("Zeichen", TYPE_CHAR));
            KEYWORDS.Add(new PseudocodeType("char", TYPE_CHAR));

            // string types
            KEYWORDS.Add(new PseudocodeType("Text", TYPE_STRING));
            KEYWORDS.Add(new PseudocodeType("string", TYPE_STRING));
            KEYWORDS.Add(new PseudocodeType("String", TYPE_STRING));

            // null
            KEYWORDS.Add(new PseudocodeType("NICHTS", NULL));

            // new keyword
            KEYWORDS.Add(new PseudocodeType("NEU", NEW));

            // if/else
            KEYWORDS.Add(new PseudocodeType("SONST", ELSE));
            KEYWORDS.Add(new PseudocodeType("ENDE WENN", END_IF));

            // switch/case
            KEYWORDS.Add(new PseudocodeType("GLEICH", SWITCH_SUFFIX));
            KEYWORDS.Add(new PseudocodeType("ENDE FALLS", END_SWITCH));

            // (do) while
            KEYWORDS.Add(new PseudocodeType("ENDE SOLANGE", END_WHILE));

            // for
            KEYWORDS.Add(new PseudocodeType("BIS", FOR_TO));
            KEYWORDS.Add(new PseudocodeType("SCHRITT", FOR_STEP));
            KEYWORDS.Add(new PseudocodeType("IN", FOR_IN));
            KEYWORDS.Add(new PseudocodeType("ENDE FÜR", END_FOR));
        }
    }
}
