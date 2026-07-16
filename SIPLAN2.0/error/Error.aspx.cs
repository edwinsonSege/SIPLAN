using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIPLAN2._0.error
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["Log"] != null)
                Memo.Text = HttpContext.Current.Session["Log"].ToString();
        }

        protected void ClearLinkButton_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["Log"] = "";
            Memo.Text = "";
        }

    }
}