﻿namespace HopSharp.WebExample
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Xml.Linq;
    using HopSharp.ConsoleExample;

    /// <summary>
    /// A super page that throws an error when loaded :)
    /// </summary>
    public partial class GetPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            new Exceptioner().Throw();
        }
    }
}