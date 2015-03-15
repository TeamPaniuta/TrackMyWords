﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentGrabber.Write
{
    public class DatabaseWriter : IWriteProvider
    {

        public WriteMode PreferredWriteMode
        {
            get
            {
                return WriteMode.Append;
            }
        }

        public void DoWrite(Dictionary<string, string> items, StreamWriter writer)
        {
            string file = ((FileStream)writer.BaseStream).Name;
            int index = file.LastIndexOf('.');
            string[] parts = file.Substring(0, index).Split('\\');
            int len = parts.Length;
            string song = parts[len - 1];
            string album = parts[len - 2];
            string artist = parts[len - 3];
            string lyrics = items["lyrics"];

            StreamWriter query;
            if (File.Exists("grabs/__query__.txt"))
            {
                query = File.AppendText("grabs/__query__.txt");
            }
            else
            {
                query = new StreamWriter("grabs/__query__.txt");
            }
            query.WriteLine(string.Format("insert into Song (title, lyrics, release_date) values ('{0}', '{1}', {2});", song, lyrics, "2012-12-12"));
            query.WriteLine(string.Format("insert into Artist (artist_name) select * from (select \'{0}\') as tmp where not exists(select artist_name from Artist where artist_name = \'{0}\') limit 1;", artist));
            query.Flush();
            query.Close();
            query.Dispose();
            query = null;
        }
    }
}
