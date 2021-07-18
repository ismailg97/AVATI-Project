using System.ComponentModel;

namespace AVATI.Pages
{
    public enum TypeButton
    {
        [Description("btn-primary")]
        Primary,
        [Description("btn-secondary")]
        Secondary,
        [Description("btn-success")]
        Success,
        [Description("btn-danger")]
        Danger,
        [Description("btn-warning")]
        Warning,
        [Description("btn-info")]
        Info,
        [Description("btn-light")]
        Light,
        [Description("btn-dark")]
        Dark,
        [Description("btn-outline-primary")]
        OutlinePrimary,
        [Description("btn-outline-secondary")]
        OutlineSecondary,
        [Description("btn-outline-success")]
        OutlineSuccess,
        [Description("btn-outline-danger")]
        OutlineDanger,
        [Description("btn-outline-warning")]
        OutlineWarning,
        [Description("btn-outline-info")]
        OutlineInfo,
        [Description("btn-outline-light")]
        OutlineLight,
        [Description("btn-outline-dark")]
        OutlineDark
    }
}