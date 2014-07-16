using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranModelEng.ConfigParser;
using TranModelEng.importJob;
using TranModelEng.Properties;
using TranModelEng.transJob;
using TranModelEng.ui;
using Util.util;

namespace TranModelEng
{
    public class TranModelEng
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
                /* parseTransJobMenu */
                Hashtable tjm = new Hashtable();
                try
                {
                    tjm = ConfigParse.parseTransJobMenu();
                }
                catch (IMDAException e)
                {
                    Tools.writerOutput(Repository, e.Message);
                }
                string[] tar = new String[tjm.Count];
                if (tjm != null && tjm.Count > 0)
                {
                    int i = 0;
                    foreach (String key in tjm.Keys)
                    {
                        tar[i] = "&" + tjm[key].ToString();
                        i++;
                    }
                }

                /* parseImportJobMenu */
                Hashtable ijm = new Hashtable();
                try
                {
                    ijm = ConfigParse.parseImportJobMenu();
                }
                catch (IMDAException e)
                {
                    Tools.writerOutput(Repository, e.Message);
                }
                string[] iar = new String[ijm.Count];
                if (ijm != null && ijm.Count > 0)
                {
                    int i = 0;
                    foreach (String key in ijm.Keys)
                    {
                        iar[i] = "&" + ijm[key].ToString();
                        i++;
                    }
                }

                string[] aro1 = { "-" };
                string[] aro2 = { "-", "&" + IMDAResources.about };
                List<String> r = new List<String>();
                r.AddRange(tar);
                r.AddRange(aro1);
                r.AddRange(iar);
                r.AddRange(aro2);

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

                /* parseTransJobMenu */
                Hashtable tjm = new Hashtable();
                try
                {
                    tjm = ConfigParse.parseTransJobMenu();
                }
                catch (IMDAException e)
                {
                    Tools.writerOutput(Repository, e.Message);
                }

                String currKey = "";
                String job_type = "trans";

                if (tjm != null && tjm.Keys.Count > 0)
                {
                    foreach (String key in tjm.Keys)
                    {
                        if (ItemName.Equals("&" + tjm[key]))
                        {
                            currKey = key;
                            break;
                        }
                    }
                }

                /* parseImportJobMenu */
                if (currKey == null || "".Equals(currKey))
                {
                    Hashtable ijm = new Hashtable();
                    try
                    {
                        ijm = ConfigParse.parseImportJobMenu();
                    }
                    catch (IMDAException e)
                    {
                        Tools.writerOutput(Repository, e.Message);
                    }

                    if (ijm != null && ijm.Keys.Count > 0)
                    {
                        foreach (String key in ijm.Keys)
                        {
                            if (ItemName.Equals("&" + ijm[key]))
                            {
                                currKey = key;
                                job_type = "import";
                                break;
                            }
                        }
                    }
                }

                /* MenuClick */
                if ("" != currKey)
                {
                    ConfigData config_data = new ConfigData();

                    if ("trans".Equals(job_type))
                    {
                        TransJobData trans_job = new TransJobData();
                        try
                        {
                            trans_job = ConfigParse.parseTransJob(currKey);
                            config_data = ConfigParse.parseConfig();
                        }
                        catch (IMDAException e)
                        {
                            Tools.writerOutput(Repository, e.Message);
                        }

                        if (trans_job != null)
                        {
                            try
                            {
                                //transJob
                                Tools.writerOutput(Repository, trans_job.MenuName + IMDAResources.job_starting);
                                TransJob tjob = new TransJob(Repository, trans_job, config_data);
                                tjob.runJob();
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
                    }else if("import".Equals(job_type)){
                        ImportJobData import_job = new ImportJobData();
                        try
                        {
                            import_job = ConfigParse.parseImportJob(currKey);
                        }
                        catch (IMDAException e) 
                        {
                            Tools.writerOutput(Repository, e.Message);
                        }

                        if (import_job != null)
                        {
                            try
                            {
                                //importJob
                                Tools.writerOutput(Repository, import_job.MenuName + IMDAResources.job_starting);
                                ImportJob ijob = new ImportJob(Repository, import_job, config_data);
                                ijob.runJob();
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
