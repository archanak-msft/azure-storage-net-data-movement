//------------------------------------------------------------------------------
// <copyright file="ShouldTransferCallback.cs" company="Microsoft">
//    Copyright (c) Microsoft Corporation
// </copyright>
//------------------------------------------------------------------------------
namespace Microsoft.WindowsAzure.Storage.DataMovement
{
    /// <summary>
    /// Callback invoked to tell whether to copy given file
    /// </summary>
    /// <param name="sourcePath">Path of the source file</param>
    /// <param name="destinationPath">Path of the destination file.</param>
    /// <returns>True if the file should be copied; otherwise false.</returns>
    public delegate bool ShouldTransferCallback(
        string sourcePath,
        string destinationPath);
}
