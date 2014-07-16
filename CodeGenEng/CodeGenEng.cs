using CodeGenEng.ConfigParser;
using CodeGenEng.Properties;
using CodeGenEng.ui;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Util.util;

namespace CodeGenEng
{
    public class CodeGenEng
    {
        private bool m_ShowFullMenus = false;

        //Called Before EA starts to check Add-In Exists
        public String EA_Connect(EA.Repository Repository)
        {
            //No special processing required.

            return "a string";
        }

        //Called when user Click Add-Ins Menu item from within EA.
        //Populates the Menu with our desired selections.
        public object EA_GetMenuItems(EA.Repository Repository, string Location, string MenuName)
        {
            ConfigData c = new ConfigData();
            try
            {
                c = ConfigParse.parseConfig();
            }
            catch (IMDAException e)
            {
                Tools.writerOutput(Repository, e.Message);
            }


            EA.Package aPackage = Repository.GetTreeSelectedPackage();
            if ("".Equals(MenuName))
            {
                return "-&" + c.Plugin_name;
            }
            else if (("-&" + c.Plugin_name).Equals(MenuName))
            {
                Hashtable jm = new Hashtable();
                try
                {
                    jm = ConfigParse.parseJobMenu();
                }
                catch (IMDAException e)
                {
                    Tools.writerOutput(Repository, e.Message);
                }
                string[] ar = new String[jm.Count];
                if (jm != null && jm.Count > 0)
                {
                    int i = 0;
                    foreach (String key in jm.Keys)
                    {
                        ar[i] = "&" + jm[key].ToString();
                        i++;
                    }
                }

                string[] aro = { "-", "&" + IMDAResources.about };
                List<String> r = new List<String>();
                r.AddRange(ar);
                r.AddRange(aro);

                return r.ToArray<String>();
            }
            return "";
        }

        //Sets the state of the menu depending if there is an active project or not
        bool IsProjectOpen(EA.Repository Repository)
        {
            try
            {
                EA.Collection c = Repository.Models;
                return true;
            }
            catch
            {
                return false;
            }
        }

        bool IsEnabledMenu(EA.Repository Repository)
        {
            try
            {

                String t = Repository.GetTreeSelectedItemType().ToString();

                if ("otPackage" == t || "otElement" == t)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        //Called once Menu has been opened to see what menu items are active.
        public void EA_GetMenuState(EA.Repository Repository, string Location, string MenuName, string ItemName, ref bool IsEnabled, ref bool IsChecked)
        {

            if (IsProjectOpen(Repository))
            {
                if (!IsEnabledMenu(Repository))
                {
                    IsEnabled = m_ShowFullMenus;
                }
            }
            else
                // If no open project, disable all menu options
                IsEnabled = false;
        }

        //Called when user makes a selection in the menu.
        //This is your main exit point to the rest of your Add-in
        public void EA_MenuClick(EA.Repository Repository, string Location, string MenuName, string ItemName)
        {
            try
            {
                EA.Package aPackage;
                aPackage = Repository.GetTreeSelectedPackage();
                Hashtable jm = new Hashtable();
                try
                {
                    jm = ConfigParse.parseJobMenu();
                }
                catch (IMDAException e)
                {
                    Tools.writerOutput(Repository, e.Message);
                }

                String currKey = "";
                if (jm != null && jm.Keys.Count > 0)
                {
                    foreach (String key in jm.Keys)
                    {
                        if (ItemName.Equals("&" + jm[key]))
                        {
                            currKey = key;
                            break;
                        }
                    }
                }

                if ("" != currKey)
                {
                    GenJobData gen_job = new GenJobData();
                    try
                    {
                        gen_job = ConfigParse.parseGenJob(currKey);
                    }
                    catch (IMDAException e)
                    {
                        Tools.writerOutput(Repository, e.Message);
                    }

                    if (gen_job != null)
                    {
                        try
                        {
                            Tools.writerOutput(Repository, gen_job.MenuName + IMDAResources.job_starting);
                            ConfigData c = ConfigParse.parseConfig();
                            TemplateJob tj = new TemplateJob(Repository, gen_job, c);
                            tj.runJob();
                        }
                        catch (IMDAException e)
                        {
                            Tools.writerOutput(Repository, e.Message);
                        }
                    }
                    else
                    {
                        Tools.writerOutput(Repository, IMDAResources.job_not_exist);
                    }
                }
                else if (ItemName.Equals("&" + IMDAResources.about))
                {
                    About anAbout = new About();
                    anAbout.ShowDialog();
                }
            }
            catch (Exception e)
            {
                Tools.writerOutput(Repository, e.Message);
            }
        }

        public void EA_Disconnect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }


    }
}
