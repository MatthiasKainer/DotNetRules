namespace DotNetRules.Tests.AcceptanceTest.Enviroment
{
    public class LegacyItem
    {
        public LegacyItem()
        {
            Text = "sometext";
            Number = "0";
            Version = "0";
        }

        public string Version { get; set; }

        public string Text { get; set; }

        public string Number { get; set; }
    }
}
