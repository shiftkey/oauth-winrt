# OAuth-WinRT

A quick sample demonstrating migrating a simple .NET library to Windows Runtime.

This is the original source: [http://oauth.googlecode.com/svn/code/csharp/OAuthBase.cs](http://oauth.googlecode.com/svn/code/csharp/OAuthBase.cs).

## Notable changes

### Cryptography API Changes

These are a little bit more verbose now...

#### Setting up the hash algorithm

Before:

    HMACSHA1 hmacsha1 = new HMACSHA1();
    hmacsha1.Key = Encoding.ASCII.GetBytes(string.Format("{0}&{1}", UrlEncode(consumerSecret), string.IsNullOrEmpty(tokenSecret) ? "" : UrlEncode(tokenSecret)));

After:

    var algorithm = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha1);
    var value = string.Format("{0}&{1}", UrlEncode(consumerSecret), string.IsNullOrEmpty(tokenSecret) ? "" : UrlEncode(tokenSecret));
    
    var keymaterial = CryptographicBuffer.ConvertStringToBinary(value, BinaryStringEncoding.Utf8);
    var hmacKey = algorithm.CreateKey(keymaterial);

#### Computing the hash


Before:

    byte[] dataBuffer = System.Text.Encoding.ASCII.GetBytes(data);
    byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);

    return Convert.ToBase64String(hashBytes);


After:

	var signature = CryptographicEngine.Sign(
                hashAlgorithm,
                CryptographicBuffer.ConvertStringToBinary(data, BinaryStringEncoding.Utf8));

    return signature;

### Working with Streams

`Windows.Storage.Streams.IBuffer` is a new interface to represent an array of bytes. This gets used in much of the cryptography APIs (pass in a buffer, receieve a buffer) rather than working with primitive structures.

Its actually very plain:

    public interface IBuffer
    {
        uint Capacity { get; }
        uint Length { get; set; }
    }

I need to do some more digging into this class, I'm sure there's more to it than this...


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