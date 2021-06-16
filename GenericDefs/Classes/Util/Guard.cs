using System;

namespace GenericDefs.Util
{
    /// <summary>
    /// Performs common argument validation.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Checks a string argument to ensure it isn't null or empty.
        /// </summary>
        /// <param name="argumentValue">The argument value to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="argumentValue" /> is a null reference.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="argumentValue" /> is <see cref="F:System.String.Empty" />.</exception>
        public static void ArgumentNotNullOrEmptyString(string argumentValue, string argumentName)
        {
            Guard.ArgumentNotNull(argumentValue, argumentName);
            if (argumentValue.Length == 0)
            {
                throw new ArgumentException("String cannot be empty.", argumentName);
            }
        }

        /// <summary>
        /// Checks an argument to ensure it isn't null.
        /// </summary>
        /// <param name="argumentValue">The argument value to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="argumentValue" /> is a null reference.</exception>
        public static void ArgumentNotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}