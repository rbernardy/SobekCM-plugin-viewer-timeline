using System;
using System.Collections.Generic;
using System.IO;
using SobekCM.Core.BriefItem;
using SobekCM.Core.FileSystems;
using SobekCM.Core.Navigation;
using SobekCM.Core.Users;
using SobekCM.Library.ItemViewer.Viewers;
using SobekCM.Tools;
using SobekCM.Engine_Library.Endpoints;
using System.Xml;
using System.Web;
using System.Text;

namespace TIMELINE
{
    public class TIMELINE_ItemViewer : abstractNoPaginationItemViewer
    {
        /// <summary> Constructor for a new instance of the TIMELINE_ItemViewer class, used to display a timeline </summary>
        /// <param name="BriefItem"> Digital resource object </param>
        /// <param name="CurrentUser"> Current user, who may or may not be logged on </param>
        /// <param name="CurrentRequest"> Information about the current request </param>
        /// <param name="Tracer"> Trace object keeps a list of each method executed and important milestones in rendering </param>
        public TIMELINE_ItemViewer(BriefItemInfo BriefItem, User_Object CurrentUser, Navigation_Object CurrentRequest, Custom_Tracer Tracer)
        {
            this.BriefItem = BriefItem;
            this.CurrentRequest = CurrentRequest;
            this.CurrentUser = CurrentUser;
        }

        public override void Add_Main_Viewer_Section(System.Web.UI.WebControls.PlaceHolder MainPlaceHolder, Custom_Tracer Tracer)
        {
            // do nothing
        }

        public override void Write_Main_Viewer_Section(TextWriter Output, Custom_Tracer Tracer)
        {
            Output.WriteLine("<script type=\"text/javascript\">");
            Output.WriteLine("  jQuery('#itemNavForm').prop('action','').submit(function(event){ event.preventDefault(); });");
            Output.WriteLine("</script>");

            string network_location = SobekFileSystem.Resource_Network_Uri(BriefItem);
            String baseurl = CurrentRequest.Base_URL + "plugins/TIMELINE/";

            // Fit into SobekCM item page
            Output.WriteLine("  <td>");

            // timeline-19-20161103-034529-1036546140.json
            
            Output.WriteLine("      <div id='timeline-embed' style='width:100%; height:100%;'></div>");

            //Output.WriteLine("<p>There are " + BriefItem.Description.Count + " elements in the description.</p>");

            List<BriefItem_DescriptiveTerm> bidt = BriefItem.Description;

            BriefItem_DescriptiveTerm dt = bidt.Find(x => x.Term.Equals("Resource Identifier"));

            String tlsn="N/A";

            foreach (BriefItem_DescTermValue dtv in dt.Values)
            {
                if (dtv.Authority=="timelineset")
                {
                    tlsn = dtv.Value;
                    break;
                }
            }

            //Output.WriteLine("<p>tlsn=[" + tlsn + "].</p>");

            // http://digital.lib.usf.edu/engine/search/results/xml?t=TL1

            String myurl = "http://digital.lib.usf.edu/engine/search/results/xml?t=" + tlsn;

            //Output.WriteLine("<p>myurl=[" + myurl + "].</p>");
            String data;
            String json;

            json = "{\"title\":{\"media\":{\"url\":\"http://digital.lib.usf.edu/content/SF/S0/03/63/40/00001/J12-00053.jpg\",";
            json += "\"caption\":\"Blah blah blah\",";
            json += "\"credit\":\"Copyright 2016\"},";
            json += "\"text\":{";
            json += "\"headline\":\"This is my timeline\",";
            json += "\"text\":\"Nam porttitor purus eget tempor egestas. Donec laoreet bibendum ante sed fermentum. Maecenas ut scelerisque.\"";
            json += "}},";
            json += "\"events\":[";

            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] raw = wc.DownloadData(myurl);
                data = System.Text.Encoding.UTF8.GetString(raw);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data);
                XmlNodeList nodes;
                XmlAttribute attr;
                XmlAttributeCollection attrs;
                String bibid;
                String vid;
                String packageid;
                XmlNode node;
                XmlNodeList filenodes;
                nodes = doc.SelectNodes("//results/title");
                int mymonth, myday, myyear;
                mymonth = 1;
                myyear = 1945;
                Random rand = new Random();

                foreach (XmlNode titlenode in nodes)
                {
                    attrs = titlenode.Attributes;
                    bibid = attrs["bibid"].Value.ToString();
                    
                    node=titlenode.SelectSingleNode("//items/item");
                    attrs = node.Attributes;
                    vid = attrs["vid"].Value.ToString();

                    //Output.WriteLine("<p>" + bibid + ":" + vid + "</p>");

                    myurl = "http://digital.lib.usf.edu/engine/items/brief/xml/" + bibid + "/" + vid;

                    wc = new System.Net.WebClient();
                    raw = wc.DownloadData(myurl);
                    data = System.Text.Encoding.UTF8.GetString(raw);
                    doc.LoadXml(data);

                    String mytitle = doc.SelectSingleNode("//item/title/text()").Value.ToString();
                    //Output.WriteLine("<p>mytitle=[" + mytitle + "].</p>");
                    // descriptiveTerm term="Abstract

                    filenodes = doc.SelectNodes("//item/images/fileGroup/files/file/text()");
                    //Output.WriteLine("<p>There are " + filenodes.Count + " file nodes.</p>");
                    String myfilename;

                    if (filenodes.Count > 0)
                    {
                        myfilename = filenodes.Item(0).Value.ToString();
                    }
                    else
                    {
                        myfilename = "unknown.jpg";
                    }

                    String mediaURL = "http://digital.lib.usf.edu/content/" + bibid.Substring(0, 2) + "/" + bibid.Substring(2, 2) + "/" + bibid.Substring(4, 2) + "/" + bibid.Substring(6, 2) + "/" + bibid.Substring(8, 2) + "/" + vid + "/" + myfilename;
                    
                    String myabstract = doc.SelectSingleNode("//item/description/descriptiveTerm[@term='Abstract']/properties/property/@value").Value.ToString();
                    String mycaption = "No caption";
                    String credit = doc.SelectSingleNode("//item/description/descriptiveTerm[@term='Rights Management']/properties/property/@value").Value.ToString();

                    json += "{";

                    json += "\"media\":{";
                    json+="\"url\":\"" + mediaURL + "\",";
                    json += "\"caption\":\"" + mycaption + "\",";
                    json += "\"credit\":\"" + credit + "\"";
                    json += "},";

                    mymonth++;
                    myday= rand.Next(1, 28);
                    
                    json += "\"start_date\":{";
                    json +="\"month\":\"" + mymonth + "\",";
                    json += "\"day\":\"" + myday + "\",";
                    json += "\"year\":\"" + myyear + "\"";
                    json += "},";

                    json += "\"text\":{";
                    json += "\"headline\":\"" + mytitle + "\",";
                    json += "\"text\":\"" + myabstract + "\"";
                    json += "}},";
                }

                json = json.Substring(0, json.Length - 1);
                json += "]}";
            }
            catch (Exception e)
            {
                Output.WriteLine("<p>Error : " + e.Message + " [" + e.StackTrace + "].</p>");
            }

            File.WriteAllText(@"C:\Users\" + Environment.UserName + @"\Dropbox\timeline.json", json);
            File.WriteAllText(@"C:\inetpub\wwwroot\temp\" + tlsn + ".json", json);
            /*
            foreach (BriefItem_DescriptiveTerm mydt in bidt)
            {
                Output.WriteLine("<h3>" + mydt.Term + "</h3><ul>");

                foreach (BriefItem_DescTermValue dtv in mydt.Values)
                {
                    Output.WriteLine("<li>" + dtv.Authority + " : " + dtv.Value + "</li>");
                }

                Output.WriteLine("</ul>");
            }
            */

            if (tlsn == "N/A")
            {
                // if no timeline set # in an identifier use the default (USF History)
                Output.WriteLine("       <script type=\"text/javascript\">\r\n");
                Output.WriteLine("          timeline=new TL.Timeline('timeline-embed','" + baseurl + "examples/timeline-19-20161103-034529-1036546140.json');\r\n");
                Output.WriteLine("       </script>");
            }
            else
            {
                Output.WriteLine("      <script type=\"text/javascript\">\r\n");
                //Output.WriteLine("          var myjson='" + json.Replace("'","''") + "';");
                Output.WriteLine("          timeline=new TL.Timeline('timeline-embed','" + CurrentRequest.Base_URL + "temp/" + tlsn + ".json')");
                Output.WriteLine("      </script>");
            }

            // end td for SobekCM item page
            Output.WriteLine("  </td>");

            Output.WriteLine("<script type=\"text/javascript\">");
            Output.WriteLine("jQuery(document).ready(function()");
            Output.WriteLine("{");
            Output.WriteLine("\tvar myheight=window.innerHeight;");
            Output.WriteLine("\tjQuery(\"#timeline-embed\").css(\"height\",(myheight-250) + \"px\")");
            Output.WriteLine("\tjQuery(\".sbkIsw_DocumentDisplay2\").css(\"width\",\"100%\").css(\"height\",(myheight-250) + \"px\");");
            Output.WriteLine("});");
            Output.WriteLine("</script>");
        }

        /// <summary> Write any additional values within the HTML Head of the final served page </summary>
        /// <param name="Output"> Output stream currently within the HTML head tags </param>
        /// <param name="Tracer"> Trace object keeps a list of each method executed and important milestones in rendering </param>
        /// <remarks> By default this does nothing, but can be overwritten by all the individual item viewers </remarks>
        public override void Write_Within_HTML_Head(TextWriter Output, Custom_Tracer Tracer)
        {
            String baseurl = CurrentRequest.Base_URL + "plugins/TIMELINE/";

            Output.WriteLine("  <link rel=\"stylesheet\" type=\"text/css\" href=\"http://cdn.knightlab.com/libs/timeline3/latest/css/timeline.css\"/>");

            //Output.WriteLine("  <link rel=\"stylesheet\" type=\"text/css\" href=\"" + baseurl + "css/superfish/megafish.css\"/>");
            //Output.WriteLine("  <link rel=\"stylesheet\" type=\"text/css\" href=\"" + baseurl + "css/superfish/superfish.css\"/>");
            //Output.WriteLine("  <link rel=\"stylesheet\" type=\"text/css\" href=\"" + baseurl + "css/superfish/superfish-navbar.css\"/>");
            //Output.WriteLine("  <link rel=\"stylesheet\" type=\"text/css\" href=\"" + baseurl + "css/superfish/superfish-vertical.css\"/>");

            //Output.WriteLine("  <script type=\"text/javascript\" src=\"http://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js\"></script>");

            //Output.WriteLine("  <script type=\"text/javascript\" src=\"" + baseurl + "js/superfish/superfish.min.js\"></script>");
            //Output.WriteLine("  <script type=\"text/javascript\" src=\"" + baseurl + "js/superfish/supersubs.js\"></script>");
            //Output.WriteLine("  <script type=\"text/javascript\" src=\"" + baseurl + "js/superfish/hoverIntent.js\"></script>");

            Output.WriteLine("  <script type=\"text/javascript\" src=\"http://cdn.knightlab.com/libs/timeline3/latest/js/timeline.js\"></script>");

            Output.WriteLine("  <script type=\"text/javascript\" src=\"" + baseurl + "js/timeline-support.js\"></script>");
        }
    }
}