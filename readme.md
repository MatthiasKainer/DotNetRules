# Abstract
The DotNetRules Library is an approach to apply policies to objects without the need to call each Policy manually from code, but with one simple call. Using this approach you can deploy your policies in an external library and deploy them independently of the core application. 

# Introduction
As a software developer, I went through a lot of situations where I had to validate conditions on an existing object. With the coming of domain driven design I found myself ending up with dozens of lines of fluent validation code that made my clean domain object look like spaghetti code all over again. 
Using a rule engine is fun, but most of the engines out there required a lot of perquisites and all I wanted was to write some code, so I started my own lightweight engine. 

## Execute all policies: 

    myObject.ApplyPolicies();

## A simple policy: 

    using DotNetRules.Runtime;
    [Policy(typeof(MyObject))]
    internal class Policy : PolicyBase<MyObject>
    {
        Given objectIsHuge = () => Subject.Size > 1000;

        Then shrinkIt = () =>
        {
            Subject.Data = Packaging.Compress(Subject.Data);
        };
    }

## A more advanced policy: 

    [Policy(typeof(Capability.Event), typeof(Guid))]
    class EventWriteAccess : RelationPolicyBase<Capability.Event, Guid>
    {
        static bool _allowed = false;

        Establish _context = target =>
        {
            _allowed = false;
        };

        Given _userCanWrite = () => Source.Owner == Target ||
                                    Source.AdministatorsId.Contains(Target) ||
                                    Source.EditorsId.Contains(Target);
    								
		Or _userIsPlattformAdmin = () => Plattform.IsAdmin(Target);

        Then _grantPermission = () => _allowed = true;

        Return<bool> _return = () => _allowed;
    }

## Use it
Open your nuget console and do 
> Install-Package DotNetRules

or 

> Install-Package DotNetRules.Web.Mvc

