namespace Mrx.AspNetCore.Enums
{
    public enum LinkTarget
    {
        None,
        /// <summary>
        /// Opens the linked document in a new window or tab
        /// </summary>
        [Data("_blank")]
        Blank,
        /// <summary>
        /// Opens the linked document in the same frame as it was clicked (this is default)
        /// </summary>
        [Data("_self")]
        Self,
        /// <summary>
        /// Opens the linked document in the parent frame
        /// </summary>
        [Data("_parent")]
        Parent,
        /// <summary>
        /// Opens the linked document in the full body of the window
        /// </summary>
        [Data("_top")]
        Top
    }
}
