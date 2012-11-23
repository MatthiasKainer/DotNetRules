namespace DotNetRules.Tests.AcceptanceTest.Enviroment
{
    class ExampleTargetObject
    {
        public int Version { get; set; }

        public int Integer { get; set; }

        public string[] StringArray { get; set; }

        public ExampleTargetObject()
        {
            StringArray = new [] { "untouched" };
        }
    }
}
