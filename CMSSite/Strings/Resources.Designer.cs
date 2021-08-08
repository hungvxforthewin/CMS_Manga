using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMSite.Strings
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources
    {

        private static global::System.Resources.ResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources()
        {
        }

        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CRMSite.Strings.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Your username is in used.
        /// </summary>
        internal static string REGISTER_DUPLICATE_USER_NAME
        {
            get
            {
                return ResourceManager.GetString("REGISTER_DUPLICATE_USER_NAME", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to You must enter valid email address to register.
        /// </summary>
        internal static string REGISTER_REQUIRED_EMAIL
        {
            get
            {
                return ResourceManager.GetString("REGISTER_REQUIRED_EMAIL", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to You must enter your firstname.
        /// </summary>
        internal static string REGISTER_REQUIRED_FIRST_NAME
        {
            get
            {
                return ResourceManager.GetString("REGISTER_REQUIRED_FIRST_NAME", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to You must enter your lastname.
        /// </summary>
        internal static string REGISTER_REQUIRED_LAST_NAME
        {
            get
            {
                return ResourceManager.GetString("REGISTER_REQUIRED_LAST_NAME", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Request invalid.
        /// </summary>
        internal static string REQUEST_INVALID
        {
            get
            {
                return ResourceManager.GetString("REQUEST_INVALID", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Opp!!!Please contact to administrators!!.
        /// </summary>
        internal static string UNKNOWN_ERROR
        {
            get
            {
                return ResourceManager.GetString("UNKNOWN_ERROR", resourceCulture);
            }
        }
    }
}
