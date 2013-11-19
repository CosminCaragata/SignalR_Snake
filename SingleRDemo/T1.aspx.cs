using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SingleRDemo
{
    public partial class T1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SnakeGame sg = new SnakeGame(20, 20);
            //sg.AddPlayerToBoard("");
            var planboard = sg.GetGameTable();
        }
    }
}