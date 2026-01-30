namespace BlowUp.Models
{
    public class DocumentDynamicImageInput
    {
        public string Arguments { get; set; }

        public string Description { get; set; }

        public int? ExpectedExitCode { get; set; }

        public string ExpectedFileName { get; set; }

        public string Runner { get; set; }
    }
}