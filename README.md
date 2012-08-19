# OAuth-WinRT

A quick sample demonstrating migrating a simple .NET library to Windows Runtime.

This is the original source: [http://oauth.googlecode.com/svn/code/csharp/OAuthBase.cs](http://oauth.googlecode.com/svn/code/csharp/OAuthBase.cs).

## Notable changes

### Cryptography API Changes

 - TODO: before and after code snippets
 
### Access Modifiers

 - No public fields allowed
 - `protected` and `virtual` are frowned upon
 - TODO: other samples

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