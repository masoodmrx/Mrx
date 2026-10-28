namespace Mrx.AspNetCore.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "ajax")]
    public class AAjaxAttributesTagHelper : TagHelper
    {
        #region Ajax
        /// <summary>
        /// Must be set to true to activate unobtrusive Ajax on the target element.
        /// </summary>
        public bool Ajax { get; set; }
        /// <summary>
        /// Gets or sets the message to display in a confirmation window before a request is submitted.
        /// </summary>
        public string AjaxConfirm { get; set; }
        /// <summary>
        /// Gets or sets the HTTP request method ("Get" or "Post").
        /// </summary>
        public string AjaxMethod { get; set; }
        /// <summary>
        /// Gets or sets the mode that specifies how to insert the response into the target DOM element. Valid values are before, after and replace. Default is replace
        /// </summary>
        public string AjaxMode { get; set; }
        /// <summary>
        /// Gets or sets a value, in milliseconds, that controls the duration of the animation when showing or hiding the loading element.
        /// </summary>
        public string AjaxLoadingDuration { get; set; }
        /// <summary>
        /// Gets or sets the id attribute of an HTML element that is displayed while the Ajax function is loading.
        /// </summary>
        public string AjaxLoading { get; set; }
        /// <summary>
        /// Gets or sets the name of the JavaScript function to call immediately before the page is updated.
        /// </summary>
        public string AjaxBegin { get; set; }
        /// <summary>
        /// Gets or sets the JavaScript function to call when response data has been instantiated but before the page is updated.
        /// </summary>
        public string AjaxComplete { get; set; }
        /// <summary>
        /// Gets or sets the JavaScript function to call if the page update fails.
        /// </summary>
        public string AjaxFailure { get; set; }
        /// <summary>
        /// Gets or sets the JavaScript function to call after the page is successfully updated.
        /// </summary>
        public string AjaxSuccess { get; set; }
        /// <summary>
        /// Gets or sets the ID of the DOM element to update by using the response from the server.
        /// </summary>
        public string AjaxUpdate { get; set; }
        /// <summary>
        /// Gets or sets the URL to make the request to.
        /// </summary>
        public string AjaxUrl { get; set; }
        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            #region Ajax
            if (Ajax)
            {
                output.Attributes.SetAttribute("data-ajax", "true");
                if (AjaxConfirm.IsNotNullOrEmpty())
                {
                    output.Attributes.SetAttribute("data-ajax-confirm", AjaxConfirm);
                }

                if (AjaxMethod.IsNotNullOrEmpty())
                {
                    output.Attributes.SetAttribute("data-ajax-method", AjaxMethod);
                }

                if (AjaxMode.IsNotNullOrEmpty())
                {
                    output.Attributes.SetAttribute("data-ajax-mode", AjaxMode);
                }

                if (AjaxLoadingDuration.IsNotNullOrEmpty())
                {
                    output.Attributes.SetAttribute("data-ajax-loading-duration", AjaxLoadingDuration);
                }

                if (AjaxLoading.IsNotNullOrEmpty())
                {
                    output.Attributes.SetAttribute("data-ajax-loading", AjaxLoading);
                }

                if (AjaxBegin.IsNotNullOrEmpty())
                {
                    output.Attributes.SetAttribute("data-ajax-begin", AjaxBegin);
                }

                if (AjaxComplete.IsNotNullOrEmpty())
                {
                    output.Attributes.SetAttribute("data-ajax-complete", AjaxComplete);
                }

                if (AjaxFailure.IsNotNullOrEmpty())
                {
                    output.Attributes.SetAttribute("data-ajax-failure", AjaxFailure);
                }

                if (AjaxSuccess.IsNotNullOrEmpty())
                {
                    output.Attributes.SetAttribute("data-ajax-success", AjaxSuccess);
                }

                if (AjaxUpdate.IsNotNullOrEmpty())
                {
                    output.Attributes.SetAttribute("data-ajax-update", AjaxUpdate);
                }

                if (AjaxUrl.IsNotNullOrEmpty())
                {
                    output.Attributes.SetAttribute("data-ajax-url", AjaxUrl);
                }
            }
            #endregion
        }
    }
}
