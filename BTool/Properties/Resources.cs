﻿namespace BTool.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
    internal class Resources
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal Resources()
        {
        }

        internal static Icon btw
        {
            get
            {
                return (Icon) ResourceManager.GetObject("btw", resourceCulture);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
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

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    System.Resources.ResourceManager manager = new System.Resources.ResourceManager("BTool.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = manager;
                }
                return resourceMan;
            }
        }

        internal static Bitmap ti_banner
        {
            get
            {
                return (Bitmap) ResourceManager.GetObject("ti_banner", resourceCulture);
            }
        }

        internal static Icon ti_icon
        {
            get
            {
                return (Icon) ResourceManager.GetObject("ti_icon", resourceCulture);
            }
        }
    }
}
