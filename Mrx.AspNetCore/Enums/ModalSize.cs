namespace Mrx.AspNetCore.Enums
{
    public enum ModalSize
    {
        /// <summary>
        /// 300px
        /// </summary>
        [Data("modal-sm")]
        Small,
        /// <summary>
        /// 500px
        /// </summary>
        [Data("")]
        Default,
        /// <summary>
        /// 800px
        /// </summary>
        [Data("modal-lg")]
        Large,
        /// <summary>
        /// 1140px
        /// </summary>
        [Data("modal-xl")]
        ExtraLarge
    }
}
