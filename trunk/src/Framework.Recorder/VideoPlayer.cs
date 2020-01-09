using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ST.Common;
using Framework.Logging;
using Vlc.DotNet.Core.Interops.Signatures;
using Vlc.DotNet.Wpf;

namespace Framework.Recorder
{
    /// <summary>
    /// Vlc Player
    /// </summary>
    public class VideoPlayer: IDisposable
    {

        private static readonly VideoPlayer m_Instance = new VideoPlayer();

        private VlcControl vlc;

        public static VideoPlayer Instance { get { return m_Instance; } }
        public VlcControl Vlc { get { return vlc; } set { vlc = value; } }
        public MediaTrackInfosStructure[] mediaInformations { get { return vlc.MediaPlayer.GetCurrentMedia().TracksInformations; } }


        public event PlayFinishedHandler OnPlayFinished;

        public void InitVlc()
        {
            Dispose();
            vlc = new VlcControl();
            vlc.MediaPlayer.VlcLibDirectoryNeeded += OnVlcControlNeedsLibDirectory;
            vlc.MediaPlayer.EndInit();
        }

        private void OnVlcControlNeedsLibDirectory(object sender, Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs e)
        {
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            if (currentDirectory == null)
                return;
            if (AssemblyName.GetAssemblyName(currentAssembly.Location).ProcessorArchitecture == ProcessorArchitecture.X86)
                e.VlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, @"lib\x86\"));
            else
                e.VlcLibDirectory = new DirectoryInfo(Path.Combine(currentDirectory, @"lib\x64\"));
            if (!e.VlcLibDirectory.Exists)
            {
                //提示 log
                //var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                //folderBrowserDialog.Description = "Select Vlc libraries folder.";
                //folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
                //folderBrowserDialog.ShowNewFolderButton = true;
                //if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                //    e.VlcLibDirectory = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                //}
                Log4NetHelper.ErrorFormat("无效的播放组件路径.");
            }
        }

        #region << Video Play >>

        public void Load(string fileName)
        {
            
        }

        public void Play(string fileName, VideoPlayFile vPlayFile, PlayFinishedHandler onPlayFinished = null)
        {
            switch (vPlayFile)
            {
                case VideoPlayFile.FileInfo:
                    vlc.MediaPlayer.Play(new FileInfo(fileName));
                    break;
                case VideoPlayFile.URI:
                    vlc.MediaPlayer.Play(new Uri(fileName));
                    break;
                case VideoPlayFile.NONE:
                    vlc.MediaPlayer.Play();
                    break;
            }

            if (onPlayFinished != null)
            {
                OnPlayFinished = onPlayFinished;
                vlc.MediaPlayer.EndReached += MediaPlayer_EndReached;
            }
        }

        public void Pause()
        {
            if (vlc != null && vlc.MediaPlayer != null)
            {
                vlc.MediaPlayer.Pause();
            }
        }

        public void Stop()
        {
            if (vlc != null && vlc.MediaPlayer != null)
            {
                if (vlc.MediaPlayer.IsPlaying)
                    vlc.MediaPlayer.Stop();

                try
                {
                    vlc.MediaPlayer.EndReached -= MediaPlayer_EndReached;
                }
                catch (Exception) { }
            }
        }
        public void Dispose()
        {
            Stop();

            if (vlc != null)
            {

                vlc.MediaPlayer.Dispose();

                vlc = null;
            }
        }

        private void MediaPlayer_EndReached(object sender, Vlc.DotNet.Core.VlcMediaPlayerEndReachedEventArgs e)
        {
            //if (e.Exception != null)
            //{
            //    Log4NetHelper.InfoFormat("播放异常停止. cause by:{0}", e.Exception.ToString());
            //}

            Stop();
            //CloseFile();

            if (OnPlayFinished != null)
            {
                OnPlayFinished.Invoke();
            }
            else
            {
                Log4NetHelper.Info("no play finished handler...");
            }
        }


        #endregion



    }
}
