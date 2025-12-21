using System;

namespace CloudNimble.BlazorEssentials.TursoDb
{

    /// <summary>
    /// Represents an error that occurred during a Turso database operation.
    /// </summary>
    /// <param name="message">The error message describing the exception.</param>
    public class TursoDbException(string message) : Exception(message)
    {
    }

}
