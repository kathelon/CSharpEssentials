using System.Collections;

namespace CSharpEssentials.RequestResponseLogging.LogWriters;

internal sealed class RequestResponseLogState : IReadOnlyList<KeyValuePair<string, object?>>
{
    private const string MessagePropertyName = "RequestResponseLog";
    private const string OriginalFormatKey = "{OriginalFormat}";
    private const string MessageFormat = "{" + MessagePropertyName + ":l}";
    private const string NullValue = "(null)";

    private readonly KeyValuePair<string, object?>[] _properties;
    private readonly string _message;

    private RequestResponseLogState(KeyValuePair<string, object?>[] properties, string message)
    {
        _properties = properties;
        _message = message;
    }

    public int Count => _properties.Length;

    public KeyValuePair<string, object?> this[int index] => _properties[index];

    public static RequestResponseLogState FromMessage(string message) =>
        new(
            [
                new KeyValuePair<string, object?>(MessagePropertyName, message),
                new KeyValuePair<string, object?>(OriginalFormatKey, MessageFormat)
            ],
            message);

    public static RequestResponseLogState FromFields(string format, IReadOnlyList<LogFields> fields, IReadOnlyList<string?> values)
    {
        int count = Math.Min(fields.Count, values.Count);
        var properties = new KeyValuePair<string, object?>[count + 1];
        var message = new StringBuilder();

        for (int i = 0; i < count; i++)
        {
            string name = fields[i].ToString();
            properties[i] = new KeyValuePair<string, object?>(name, values[i]);
            message.Append(name).Append(": ").Append(values[i] ?? NullValue).Append(Environment.NewLine);
        }

        properties[count] = new KeyValuePair<string, object?>(OriginalFormatKey, format);
        return new RequestResponseLogState(properties, message.ToString());
    }

    public static string Format(RequestResponseLogState state, Exception? _) => state._message;

    public override string ToString() => _message;

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() =>
        ((IEnumerable<KeyValuePair<string, object?>>)_properties).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
