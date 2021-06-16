namespace GenericDefs.Functions.Crypto.Cypher
{
    public class Caeser
    {
        public static string Encipher(string value, int shift)
        {
            char[] buffer = value.ToCharArray();
            if (shift > 26) { shift = shift % 26; }
            if (shift < 26) { shift = shift % -26; }
            for (int i = 0; i < buffer.Length; i++)
            {
                // Letter.
                char letter = buffer[i];
                // Add shift to all.
                letter = (char)(letter + shift);
                // Subtract 26 on overflow.
                // Add 26 on underflow.
                if (letter > 'z')
                {
                    letter = (char)(letter - 26);
                }
                else if (letter < 'a')
                {
                    letter = (char)(letter + 26);
                }
                // Store.
                buffer[i] = letter;
            }
            return new string(buffer);
        }
    }
}