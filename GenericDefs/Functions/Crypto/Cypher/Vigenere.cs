using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDefs.Functions.Crypto.Cypher
{
    public class Vigenere
    {
        private static int Mod(int a, int b)
        {
            return (a % b + b) % b;
        }

        private static string Cypher(string input, string key, bool encipher)
        {
            for (int i = 0; i < key.Length; ++i)
                if (!char.IsLetter(key[i]))
                    return null; // Error

            string output = string.Empty;
            int nonAlphaCharCount = 0;

            for (int i = 0; i < input.Length; ++i)
            {
                if (char.IsLetter(input[i])) {
                    bool cIsUpper = char.IsUpper(input[i]);
                    char offset = cIsUpper ? 'A' : 'a';
                    int keyIndex = (i - nonAlphaCharCount) % key.Length;
                    int k = (cIsUpper ? char.ToUpper(key[keyIndex]) : char.ToLower(key[keyIndex])) - offset;
                    k = encipher ? k : -k;
                    char ch = (char)((Mod(((input[i] + k) - offset), 26)) + offset);
                    output += ch;
                } else {
                    output += input[i];
                    ++nonAlphaCharCount;
                }
            }

            return output;
        }

        public static string Encipher(string input, string key)
        {
            return Cypher(input, key, true);
        }

        public static string Decipher(string input, string key)
        {
            //return Cipher(input, key, false);
            return Keyless.DecipherVigenere(input, key);
        }

        public static string FindKey(string VigenereCryptogram, int keyLen)
        {
            return Keyless.FindKey(VigenereCryptogram, keyLen);
        }

        internal class Keyless
        {
            internal static List<string> SplitTextWithKeyLen(string text, int keyLen)
            {
                List<string> result = new List<string>();
                StringBuilder[] sb = new StringBuilder[keyLen];

                for (int i = 0; i < keyLen; i++)
                {
                    sb[i] = new StringBuilder();
                }

                for (int i = 0; i < text.Length; i++)
                {
                    sb[i % keyLen].Append(text[i]);
                }

                foreach (var item in sb)
                {
                    result.Add(item.ToString());
                }

                return result;
            }

            internal static Dictionary<char, double> AnalyseFrequency(string text)
            {
                if (text == null) return null;

                Dictionary<char, double> frequencies = new Dictionary<char, double>();
                int textLength = text.Length;

                for (int i = 0; i < textLength; i++)
                {
                    char c = text[i];
                    char key = '#';

                    //ignore chars that are not letters
                    if ((c >= 'a' && c <= 'z'))
                        key = c;

                    if (c >= 'A' && c <= 'Z')
                        key = (char)(c + 'a' - 'A');

                    if (frequencies.Keys.Contains(key)) frequencies[key] = frequencies[key] + 1;
                    else frequencies[key] = 1;
                }

                //cannot enumerate throught the dictionnay keys directly.
                List<char> keys = frequencies.Keys.ToList();

                foreach (char c in keys)
                {
                    frequencies[c] /= textLength;
                }

                return frequencies;
            }

            internal static string DecipherVigenere(string text, string key)
            {
                StringBuilder result = new StringBuilder();
                int keyLength = key.Length;
                int diff;
                char decoded;

                for (int i = 0; i < text.Length; i++)
                {
                    diff = text[i] - key[i % keyLength];

                    //diff should never be more than 25 or less than -25
                    if (diff < 0) diff += 26;

                    decoded = (char)(diff + 'a');
                    result.Append(decoded);
                }

                return result.ToString();
            }

            internal static string FindKey(string VigenereCryptogram, int keyLen)
            {
                Dictionary<char, double> frequencies;
                List<char> keyChars = new List<char>();

                VigenereCryptogram.Replace(" ", "").Replace("\n", "").Replace("\r", "");
                List<string> substitutionTexts = SplitTextWithKeyLen(VigenereCryptogram, keyLen);

                foreach (string caesarCryptogram in substitutionTexts)
                {
                    Console.WriteLine(caesarCryptogram);
                    Console.WriteLine(string.Format("Length: {0}", caesarCryptogram.Length));

                    frequencies = AnalyseFrequency(caesarCryptogram);

                    double maxFreq = frequencies.Values.Max();
                    char maxChar = frequencies.Keys.Where(c => frequencies[c] == maxFreq).FirstOrDefault();

                    //'E' is the maxChar
                    Console.WriteLine(string.Format("E is probably: {0} - {1}", maxChar, maxFreq));
                    Console.WriteLine(string.Format("Key would be: {0}", (char)(maxChar - 'e' + 'A')));

                    keyChars.Add((char)(maxChar - 'e' + 'A'));

                    foreach (var item in frequencies)
                    {
                        Console.WriteLine(string.Format("{0}: {1:0.00}", item.Key, item.Value));
                    }

                    Console.WriteLine("******************************");
                    Console.WriteLine("");
                }

                string VigenereKEY = "";
                foreach (char c in keyChars)
                {
                    VigenereKEY += c;
                }

                Console.WriteLine("Key found: {0}", VigenereKEY);
                return VigenereKEY;
            }
        }
    }
}