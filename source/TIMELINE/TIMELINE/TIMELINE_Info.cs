using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SobekCM.Resource_Object;

namespace TIMELINE
{
    [Serializable]
    public class TIMELINE_Info : SobekCM.Resource_Object.Metadata_Modules.iMetadata_Module
    {
        private String doi;
        private String bibid;
        private String vid;
        private String title;
        private String credit;
        private String caption;
        private String headline;
        private String maintext;
        private String year;
        private String month;
        private String day;
        private String isHome;

        private List<TIMELINE_Entry_Info> entries;

        #region Entries properties and methods

        public int Entries_Count
        {
            get { return entries == null ? 0 : entries.Count; }
        }

        public ReadOnlyCollection<TIMELINE_Entry_Info> Entries
        {
            get
            {
                if (entries == null)
                {
                    entries = new List<TIMELINE_Entry_Info>();
                }

                return new ReadOnlyCollection<TIMELINE_Entry_Info>(entries);
            }
        }

        public void Clear_Entries()
        {
            //pclogme.logme("PostcardCore_Info: Clear_Subjects...");

            if (entries != null)
            {
                entries.Clear();
            }
        }

        public void Add_Entry(String doi,String bibid, String vid, String title, String caption, String credit, String headline, String maintext, String year, String month, String day, String isHome)
        {
            //pclogme.logme("PostcardCore_Info: Add_Subjects...");

            if (entries == null)
            {
                //pclogme.logme("PostcardCore_Info: subjects was null.");
                entries = new List<TIMELINE_Entry_Info>();
            }

            if (!entries.Contains(new TIMELINE_Entry_Info(doi,bibid,vid,title,caption,credit,headline,maintext,year,month,day)))
            {
                //pclogme.logme("PostcardCore_Info: subjects was Not null and this subject had not already been added.");
                //pclogme.logme("PostcardCore_Info: adding subject: name=[" + name + "], perspective=[" + perspective + "], topic=[" + topic + "].");
                entries.Add(new TIMELINE_Entry_Info(doi, bibid, vid, title, caption, credit, headline, maintext,year, month, day));
                //pclogme.logme("PostcardCore_Info: There are now [" + subjects.Count + "](element count) (" + Subjects_Count + ") (method returned count) subjects.");
            }
        }

        #endregion

        public List<KeyValuePair<string, string>> Metadata_Search_Terms
        {
            get
            {
                List<KeyValuePair<String, String>> metadataTerms = new List<KeyValuePair<String, String>>();
                
                if (entries != null && Entries_Count > 0)
                {
                    foreach (TIMELINE_Entry_Info entry in entries)
                    {
                        metadataTerms.Add(new KeyValuePair<String, String>("Entry DOI", entry.Entry_DOI));
                        metadataTerms.Add(new KeyValuePair<string, string>("Entry BibID", entry.Entry_BibID));
                        metadataTerms.Add(new KeyValuePair<string, string>("Entry VID", entry.Entry_VID));
                        metadataTerms.Add(new KeyValuePair<string, string>("Entry Title", entry.Entry_Title));
                        metadataTerms.Add(new KeyValuePair<string, string>("Entry Caption", entry.Entry_Caption));
                        metadataTerms.Add(new KeyValuePair<string, string>("Entry Credit", entry.Entry_Credit));
                        metadataTerms.Add(new KeyValuePair<string, string>("Entry Headline", entry.Entry_Headline));
                        metadataTerms.Add(new KeyValuePair<string, string>("Entry Text", entry.Entry_Maintext));
                        metadataTerms.Add(new KeyValuePair<string, string>("Entry Year", entry.Entry_Year));
                        metadataTerms.Add(new KeyValuePair<string, string>("Entry Month", entry.Entry_Month));
                        metadataTerms.Add(new KeyValuePair<string, string>("Entry Day", entry.Entry_Day));
                    }
                }

                return metadataTerms;
            }
        }

        public bool hasData
        {
            get
            {
                return (!String.IsNullOrEmpty(doi)) || (!String.IsNullOrEmpty(bibid)) ||
                        (!String.IsNullOrEmpty(vid)) || (!String.IsNullOrEmpty(title)) ||
                        (!String.IsNullOrEmpty(caption)) || (!String.IsNullOrEmpty(credit) ||
                        (!String.IsNullOrEmpty(headline)) || (!String.IsNullOrEmpty(year)) ||
                        (!String.IsNullOrEmpty(month)) || (!String.IsNullOrEmpty(day)) ||
                        ((entries != null) && (Entries_Count > 0)
                 ));
            }
        }

        public string Module_Name
        {
            get
            {
                { return "TimelineCore"; }
            }
        }

        public bool Retrieve_Additional_Info_From_Database(int ItemID, string DB_ConnectionString, SobekCM_Item BibObject, out string Error_Message)
        {
            Error_Message = String.Empty;
            return true;
        }

        public bool Save_Additional_Info_To_Database(int ItemID, string DB_ConnectionString, SobekCM_Item BibObject, out string Error_Message)
        {
            Error_Message = String.Empty;
            return true;
        }
    }
}