using System;

namespace Snowball.Content
{
    /// <summary>
    /// Used to specify a format for content types which can be loaded from different file formats.
    /// </summary>
    public enum ContentFormat
    {
        /// <summary>
        /// Use the default file format for the content type.
        /// </summary>
        Default,

        /// <summary>
        /// Use the XML file format for the content type.
        /// </summary>
        Xml
    }
}
