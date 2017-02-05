using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMELINE
{
    [Serializable]
    public class TIMELINE_Entry_Info
    {
        private String entryDOI;
        private String entryBibID;
        private String entryVID;
        private String entryTitle;
        private String entryCaption;
        private String entryCredit;
        private String entryHeadline;
        private String entryMaintext;
        private String entryYear;
        private String entryMonth;
        private String entryDay;
  
        public TIMELINE_Entry_Info()
        {
        }

        public TIMELINE_Entry_Info(String EntryDOI, String EntryBibID, String EntryVID, String EntryTitle, String EntryCaption, String EntryCredit, String EntryHeadline, String EntryMaintext, String EntryYear, String EntryMonth, String EntryDay)
        {
            entryDOI = EntryDOI;
            entryBibID = EntryBibID;
            entryVID = EntryVID;
            entryTitle = EntryTitle;
            entryCaption = EntryCaption;
            entryCredit = EntryCredit;
            entryHeadline = EntryHeadline;
            entryMaintext = EntryMaintext;
            entryYear = EntryYear;
            entryMonth = EntryMonth;
            entryDay = EntryDay;
        }

        public String Entry_DOI
        {
            get { return entryDOI ?? String.Empty; }
            set { entryDOI = value; }
        }

        public String Entry_BibID
        {
            get { return entryBibID ?? String.Empty;  }
            set { entryBibID = value; }
        }

        public String Entry_VID
        {
            get { return entryVID ?? String.Empty; }
            set { entryVID = value; }
        }

        public String Entry_Title
        {
            get { return entryTitle ?? String.Empty; }
            set { entryTitle = value; }
        }

        public String Entry_Caption
        {
            get { return entryCaption ?? String.Empty; }
            set { entryCaption = value; }
        }

        public String Entry_Credit
        {
            get { return entryCredit ?? String.Empty; }
            set { entryCredit = value; }
        }

        public String Entry_Headline
        {
            get { return entryHeadline ?? String.Empty; }
            set { entryHeadline = value; }
        }

        public String Entry_Maintext
        {
            get { return entryMaintext ?? String.Empty; }
            set { entryMaintext = value; }
        }

        public String Entry_Year
        {
            get { return entryYear ?? String.Empty; }
            set { entryYear = value; }
        }

        public String Entry_Month
        {
            get { return entryMonth ?? String.Empty; }
            set { entryMonth = value; }
        }

        public String Entry_Day
        {
            get { return entryDay ?? String.Empty; }
            set { entryDay = value; }
        }
    }
}