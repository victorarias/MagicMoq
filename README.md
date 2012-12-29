#MagicMoq
========

##Mocking dependencies with style 8-)

MagicMoq is a extension to the Moq mocking framework – It is like a IoC containers that injects mocked dependencies when resolving a type, making the job of creating testing subjects much more easy (and fun).

It is so easy to use as the Moq framework. Check it out:


###Assume that you want to test this class
```C#

 public class Foo
        {
            private readonly IFooDependency dependency;

            public Foo(IFooDependency dependency, IAnotherFooDependency anotherDependency)
            {
                if (null == dependency) throw new ArgumentNullException();
                if (null == anotherDependency) throw new ArgumentNullException();

                this.dependency = dependency;
            }

            public int DoSomething()
            {
                return this.dependency.DoSomethingDifferent();
            }
        }
```
