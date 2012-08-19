# OAuth-WinRT

A quick sample demonstrating migrating a simple .NET library to Windows Runtime.

This is the original source: [http://oauth.googlecode.com/svn/code/csharp/OAuthBase.cs](http://oauth.googlecode.com/svn/code/csharp/OAuthBase.cs).

## Notable changes

### Cryptography API Changes

 - TODO: before and after code snippets
 
### Access Modifiers

#### No public fields allowed

This requires some notes on how the Windows Runtime type system works...

#### `protected` and `virtual` are frowned upon

Consider a class which is intended to be overriden:

    protected class QueryParameter
    {
        public QueryParameter(string key, string value)
        {
            Name = key;
            Value = val;
        }

        ...
    }

This will raise a compiler error:

    Elements defined in a namespace cannot be explicitly declared as private, protected, or protected internal

And the fix for that is fairly easy:

    internal class QueryParameter
    {
        public QueryParameter(string key, string value)
        {
            Name = key;
            Value = val;
        }

        ...
    }

#### public classes must be sealed

    public class QueryParameter
    {
        public QueryParameter(string key, string value)
        {
            Name = key;
            Value = val;
        }

        ...
    }

This code raises a compiler error:

    Exporting unsealed types is not supported. Please mark type 'OAuth.QueryParameter' as sealed.

And the fix for that is fairly easy:

    public sealed class QueryParameter
    {
        public QueryParameter(string key, string value)
        {
            Name = key;
            Value = val;
        }

        ...
    }

**TODO:** other samples

### "value" is a dirty word

This code was valid before:

    public sealed class QueryParameter
    {
        public QueryParameter(string key, string value)
        {
            Name = key;
            Value = val;
        }

        ...
    }

But now it spits out a compiler error in RTM:

    The parameterized constructor 'OAuth.QueryParameter..ctor(System.String, System.String)' has a parameter named 'value' which is the same as the default return value name. Consider using another name for the parameter.

And the fix is reasonably simple:

    public sealed class QueryParameter
    {
        public QueryParameter(string key, string val)
        {
            Name = key;
            Value = val;
        }

        ...
    }