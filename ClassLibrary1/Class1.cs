namespace Lib;

public class Base
{
}

public class Foo1 : Base
{
}

public class Foo2 : IFoo
{
}

public class Foo3 : Base, IFoo
{
}

public class Foo4 : IFoo
{
}

public interface IFoo
{
}