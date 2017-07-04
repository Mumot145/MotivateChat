using System;
using System.Linq;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MotivationAdmin.Models
{
	public class TodoItem
	{
		string id;
        int user;
        string todo;
		bool done;
        bool deleted;
        int lbl;
        private MessageLabels msgLabels = new MessageLabels();
		[JsonProperty(PropertyName = "id")]
		public string Id
		{
			get { return id; }
			set { id = value;}
		}
        public Lbl getLabelInfo
        {
            get
            {
                return msgLabels.getLabelWithId(lbl);
            }           
        }
		[JsonProperty(PropertyName = "text")]
		public string ToDo
		{
			get { return todo; }
			set { todo = value;}
		}
        [JsonProperty(PropertyName = "userId")]
        public int UserId
        {
            get { return user; }
            set { user = value; }
        }

        [JsonProperty(PropertyName = "complete")]
		public bool Done
		{
			get { return done; }
			set { done = value;}
		}
        [JsonProperty(PropertyName = "deleted")]
        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }
        [JsonProperty(PropertyName = "lbl")]
        public int MessageLabel
        {
            get { return lbl; }
            set { lbl = value; }
        }

        [Version]
        public string Version { get; set; }
	}
}

