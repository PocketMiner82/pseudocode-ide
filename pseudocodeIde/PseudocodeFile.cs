namespace pseudocode_ide
{
    public class PseudocodeFile
    {
        public bool singleEqualIsCompareOperator { get; set; }
        public string[] fileContent { get; set; }

        public PseudocodeFile(bool singleEqualIsCompareOperator, string[] fileContent)
        {
            this.singleEqualIsCompareOperator = singleEqualIsCompareOperator;
            this.fileContent = fileContent;
        }
    }
}
