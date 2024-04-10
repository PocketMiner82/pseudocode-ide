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

namespace pseudocode_ide
{
    public class PseudocodeFile
    {
        /// <summary>
        /// If '=' is used to compare values (e.g. 1=1 would be true)
        /// or if it is used to define variables (e.g. i:int = 1)
        /// </summary>
        public bool SingleEqualIsCompareOperator { get; set; }

        /// <summary>
        /// the pseudocode split by lines
        /// </summary>
        public string[] FileContent { get; set; }

        /// <summary>
        /// Json structure of the .pseudocode file.
        /// </summary>
        /// <param name="singleEqualIsCompareOperator">If '=' is used to compare values (e.g. 1=1 would be true)
        /// or if it is used to define variables (e.g. i:int = 1)</param>
        /// <param name="fileContent">the pseudocode split by lines</param>
        public PseudocodeFile(bool singleEqualIsCompareOperator, string[] fileContent)
        {
            SingleEqualIsCompareOperator = singleEqualIsCompareOperator;
            FileContent = fileContent;
        }
    }
}
