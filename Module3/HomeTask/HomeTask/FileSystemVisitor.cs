using System;
using System.Collections.Generic;
using System.IO;

namespace HomeTask
{

    public class FileSystemEventArgs : EventArgs
    {
        public string Path { get; }
        public bool AbortSearch { get; set; }
        public bool Exclude { get; set; }

        public FileSystemEventArgs(string path)
        {
            Path = path;
        }
    }

    public delegate bool FileSystemFilter(string filter);

    public class FileSystemVisitor
    {
        private readonly string _rootPath;
        private readonly FileSystemFilter _filter;
        private readonly IDirectoryWrapper _directoryWrapper;

        public event EventHandler Start;
        public event EventHandler Finish;
        public event EventHandler<FileSystemEventArgs> FileFound;
        public event EventHandler<FileSystemEventArgs> DirectoryFound;
        public event EventHandler<FileSystemEventArgs> FilteredFileFound;
        public event EventHandler<FileSystemEventArgs> FilteredDirectoryFound;

        public FileSystemVisitor(string rootPath, FileSystemFilter? filter = null, IDirectoryWrapper? directoryWrapper = null)
        {
            _rootPath = rootPath;
            _filter = filter;
            _directoryWrapper = directoryWrapper ?? new DirectoryWrapper();
        }

        public IEnumerable<string> Read()
        {
            OnStart();
            foreach (var entry in _directoryWrapper.EnumerateFileSystemEntries(_rootPath))
            {
                var args = new FileSystemEventArgs(entry);
                if (Directory.Exists(entry))
                {
                        OnDirectoryFound(args);
                        if (args.AbortSearch) yield break;
                        {
                            if (_filter != null)
                            {
                                if(_filter(entry))
                                {
                                    OnFilteredDirectoryFound(args);
                                    if(!args.Exclude) yield return entry;
                                }
                                
                            }
                            else
                            {
                                yield return entry;
                            }
                        }

                }
                else
                {
                        OnFileFound(args);
                        if (args.AbortSearch) yield break;

                        if (_filter != null)
                        {
                            if (_filter(entry))
                            {
                                OnFilteredFileFound(args);
                                if (!args.Exclude) yield return entry;

                            }
                            
                        }
                        else
                        {
                            yield return entry;
                        }

                }
            }
            OnFinish();
        }


        protected virtual void OnStart()
        {
            Start?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnFinish()
        {
            Finish?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnFileFound(FileSystemEventArgs e)
        {
            FileFound?.Invoke(this, e);
        }

        protected virtual void OnDirectoryFound(FileSystemEventArgs e)
        {
            DirectoryFound?.Invoke(this, e);
        }

        protected virtual void OnFilteredFileFound(FileSystemEventArgs e)
        {
            FilteredFileFound?.Invoke(this, e);
        }

        protected virtual void OnFilteredDirectoryFound(FileSystemEventArgs e)
        {
            FilteredDirectoryFound?.Invoke(this, e);
        }
    }

}
