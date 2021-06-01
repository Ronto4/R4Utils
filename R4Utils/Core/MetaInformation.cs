namespace R4Utils.Core
{
    /// <summary>
    /// A class containing basic information about the current version of R4Utils. 
    /// </summary>
    public static class MetaInformation
    {
        /// <summary>
        /// A number that gets higher as time passes and never decreases. Can be used to check compatibility, etc.
        /// </summary>
        public const ulong Iteration = 0;

        /// <summary>
        /// The hash of the commit that was used to build.
        /// </summary>
        public const string CommitHash = "";
    }
}
