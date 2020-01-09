using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using System.Text.RegularExpressions;

namespace JudgeInstallPath
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult CustomAction1(Session session)
        {
            //获取安装路径
            string installpath = session["APPDIR"];
            //MessageBox.Show(installpath);
            session.Log("APPDIR:" + installpath);
            session.Log("DesktopFolder:" + session["DesktopFolder"]);
            Regex regexzhe = new Regex("[\u4e00-\u9fa5]");

            if (regexzhe.IsMatch(installpath))
            {
                //"ISZH" 为 Advanced Installer 添加的安装参数，如有需求可自行更改
                session["ISZH"] = "true";
            }
            else
            {
                session["ISZH"] = "false";
            }

            if (installpath.Trim().Length <= 3)
            {
                session["ISGEN"] = "true";
            }
            else
            {
                session["ISGEN"] = "false";
            }

            if (installpath.Equals(session["DesktopFolder"]))
            {
                session["ISDESK"] = "true";
            }
            else
            {
                session["ISDESK"] = "false";
            }

            return ActionResult.Success;
        }
    }
}
