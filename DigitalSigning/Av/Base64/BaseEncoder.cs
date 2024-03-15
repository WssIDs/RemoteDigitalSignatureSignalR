namespace DigitalSigning.Av.Base64;

/// <remarks>Based on http://www.csharp411.com/convert-binary-to-base64-string/</remarks>
public class BaseEncoder
{
    private const char PaddingChar = '=';
    private readonly byte[] _map;

    public readonly char[] CharacterSet;
    public readonly bool PaddingEnabled;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="characterSet"></param>
    /// <param name="paddingEnabled"></param>
    public BaseEncoder(char[] characterSet, bool paddingEnabled)
    {
        PaddingEnabled = paddingEnabled;
        CharacterSet = characterSet;
        _map = Create(CharacterSet);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public string ToBase(byte[] data)
    {
        int length;
        if (data == null || 0 == (length = data.Length))
            return string.Empty;

        unsafe
        {
            fixed (byte* _d = data)
            fixed (char* cs = CharacterSet)
            {
                var d = _d;

                var padding = length % 3;
                if (padding > 0)
                    padding = 3 - padding;
                var blocks = (length - 1) / 3 + 1;

                var l = blocks * 4;

                var s = new char[l];

                fixed (char* _sp = s)
                {
                    var sp = _sp;
                    byte b1, b2, b3;

                    for (var i = 1; i < blocks; i++)
                    {
                        b1 = *d++;
                        b2 = *d++;
                        b3 = *d++;

                        *sp++ = cs[(b1 & 0xFC) >> 2];
                        *sp++ = cs[(b2 & 0xF0) >> 4 | (b1 & 0x03) << 4];
                        *sp++ = cs[(b3 & 0xC0) >> 6 | (b2 & 0x0F) << 2];
                        *sp++ = cs[b3 & 0x3F];
                    }

                    var pad2 = padding == 2;
                    var pad1 = padding > 0;

                    b1 = *d++;
                    b2 = pad2 ? (byte) 0 : *d++;
                    b3 = pad1 ? (byte) 0 : *d++;

                    *sp++ = cs[(b1 & 0xFC) >> 2];
                    *sp++ = cs[(b2 & 0xF0) >> 4 | (b1 & 0x03) << 4];
                    *sp++ = pad2 ? '=' : cs[(b3 & 0xC0) >> 6 | (b2 & 0x0F) << 2];
                    *sp++ = pad1 ? '=' : cs[b3 & 0x3F];

                    if (PaddingEnabled) return new string(s, 0, l);
                    if (pad2) l--;
                    if (pad1) l--;
                }

                return new string(s, 0, l);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public byte[] FromBase(string data)
    {
        var length = data?.Length ?? 0;

        if (length == 0)
            return Array.Empty<byte>();

        unsafe
        {
            fixed (char* p = data?.ToCharArray())
            {
                var p2 = p;

                var blocks = (length - 1) / 4 + 1;
                var bytes = blocks * 3;

                var padding = length switch
                {
                    > 2 when p2 != null && p2[length - 2] == PaddingChar => 2,
                    > 1 when p2 != null && p2[length - 1] == PaddingChar => 1,
                    _ => blocks * 4 - length
                };

                var newData = new byte[bytes - padding];

                fixed (byte* d = newData)
                {
                    var dp = d;

                    byte temp1;
                    byte temp2;
                    for (var i = 1; i < blocks; i++)
                    {
                        temp1 = _map[*p2++];
                        temp2 = _map[*p2++];

                        *dp++ = (byte) ((temp1 << 2) | ((temp2 & 0x30) >> 4));
                        temp1 = _map[*p2++];
                        *dp++ = (byte) (((temp1 & 0x3C) >> 2) | ((temp2 & 0x0F) << 4));
                        temp2 = _map[*p2++];
                        *dp++ = (byte) (((temp1 & 0x03) << 6) | temp2);
                    }

                    temp1 = _map[*p2++];
                    temp2 = _map[*p2++];

                    *dp++ = (byte) ((temp1 << 2) | ((temp2 & 0x30) >> 4));

                    temp1 = _map[*p2++];

                    if (padding != 2)
                        *dp++ = (byte) (((temp1 & 0x3C) >> 2) | ((temp2 & 0x0F) << 4));


                    temp2 = _map[*p2++];
                    if (padding == 0)
                        *dp++ = (byte) (((temp1 & 0x03) << 6) | temp2);


                }

                return newData;
            }
        }
    }

    private static byte[] Create(IReadOnlyList<char> characterSet)
    {
        var x = new byte[123];

        for (byte i = 0; i < characterSet.Count; i++)
        {
            x[(byte) characterSet[i]] = i;
        }

        return x;
    }
}