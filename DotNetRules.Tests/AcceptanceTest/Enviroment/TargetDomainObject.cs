namespace DotNetRules.Tests.AcceptanceTest.Enviroment
{
    class TargetDomainObject
    {
        public int Version { get; set; }

        public int Integer { get; set; }

        public string[] StringArray { get; set; }

        public TargetDomainObject()
        {
            StringArray = new [] { "untouched" };
        }
    }
}
