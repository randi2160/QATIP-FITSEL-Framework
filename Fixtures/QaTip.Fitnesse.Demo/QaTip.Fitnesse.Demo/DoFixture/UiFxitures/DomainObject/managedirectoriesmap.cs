using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fit;
using System.IO;
using System.Diagnostics;

namespace QaTip.Fitnesse.Demo.DoFixture.UiFxitures.DomainObject
{
    public class managedirectoriesmap:ColumnFixture
    {
        StreamReader sr;

        private string _newfilename;
       public string targetfile{ get; set; }
       public string DirectoryPath { get; set; }
       public string SourcePath { get; set; }
       public string DestinationPath { get; set; }
       public string results { get; set; }
       public string WorkingDirectory { get; set; }
       public string outputfilename { get; set; }
        /// <summary>
        /// Name of the Original Directory.  For use when renaming existing directories
        /// </summary>
        public string OriginalDirectoryName
        { get; set; }

        /// <summary>
        /// Name of the New Directory. 
        /// </summary>
        public string NewDirectoryName
        { get; set; }

        /// <summary>
        /// Name of the Original File.  For use when renaming existing files
        /// </summary>
        public string OriginalFileName
        { get; set; }

        public string filetocopy { get; set; }
        /// <summary>
        /// Name of the New File. 
        /// </summary>
        public string NewFileName
        {
            get { return _newfilename; }
            set { _newfilename = value; }
        }

        /// <summary>
        /// Name of the Source File.
        /// If intended to be a relatve path from FitNesseRoot, specify IsRelativePath == YES
        /// </summary>
        public string SourceFile
        { get; set; }

        /// <summary>       
        /// Designates that SourceFile is a relative path from FitNesseRoot dir
        /// </summary>
        public string IsSourcePathRelative
        { get; set; }

        /// <summary>
        /// Name of the destination file. 
        /// </summary>
        public string DestinationFile
        { get; set; }

        /// <summary>
        /// String1 used in ConcatenateString
        /// </summary>
        public string String1
        { get; set; }

        /// <summary>
        /// String2 used in ConcatenateString
        /// </summary>
        public string String2
        { get; set; }
        
        /// <summary>
        /// Creates a directory        
        /// </summary>
        public void CreateDirectory()
        {
            if (NewDirectoryName == null)
            {
                throw new ApplicationException("NewDirectoryName is null - Must supply a directory name in order to create a directory");
            }

            CoreHelpers.LogMessage(String.Format("CreateDirectory: Creating {0}", NewDirectoryName));
            Directory.CreateDirectory(NewDirectoryName);

            if (!Directory.Exists(NewDirectoryName))
            {
                throw new ApplicationException(string.Format("Unable to create directory {0}", NewDirectoryName));
            }
        }


       //Copy file from one location to next:
        public void CopyFilefromOneLocationToNext()
        {
            string fileName = filetocopy;
            string sourcePath = SourcePath;
            string targetPath = DestinationPath;
            try
            {
                // Use Path class to manipulate file and directory paths.
                string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                string destFile = System.IO.Path.Combine(targetPath, fileName);

                // To copy a folder's contents to a new location:
                // Create a new target folder, if necessary.
                if (!System.IO.Directory.Exists(targetPath))
                {
                    System.IO.Directory.CreateDirectory(targetPath);

                }

                if (System.IO.Directory.Exists(sourcePath))
                {
                    string[] files = System.IO.Directory.GetFiles(sourcePath);

                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        fileName = System.IO.Path.GetFileName(s);
                        destFile = System.IO.Path.Combine(targetPath, fileName);
                        if (fileName == filetocopy)
                        {
                            System.IO.File.Copy(s, destFile, true);
                            results = "Copied" + " " + fileName + "  " + "From:" + " " + sourcePath + " " + "To:" + " " + destFile;
                        }
                    }
                }
                else
                {
                    results = "Source path does not exist!";
                }

            }

            catch (Exception e)
            {
                results = "An error occurred: '{0}'" + e;
            }

        }
       




        /// <summary>
        /// Creates a blank file
        /// </summary>
        public string CreateFile()
        {
            File.Create(NewFileName).Close();

            if (!File.Exists(NewFileName))
            {
                throw new ApplicationException(string.Format("Unable to create file {0}", NewFileName));
            }

            return "YES";
        }

        /// <summary>
        /// Creates a file that contains its name
        /// </summary>
        public string CreateNonEmptyFile()
        {
            if (File.Exists(NewFileName))
            {
                throw new ApplicationException(string.Format("new file {0} already exists!", NewFileName));
            }

            using( StreamWriter streamWriter = File.CreateText(NewFileName) )
                streamWriter.Write( NewFileName );

            return Constants.YES;
        }

        /// <summary>
        /// Deletes a directory        
        /// </summary>
        public void DeleteDirectory()
        {
            if (String.IsNullOrEmpty(OriginalDirectoryName))
            {
                throw new ApplicationException("OriginalDirectoryName is null - Must supply a directory name in order to delete a directory");
            }

            if (Directory.Exists(OriginalDirectoryName))
            {
                if (Directory.GetParent(OriginalDirectoryName) == null)
                {
                    throw new ApplicationException("OriginalDirectoryName is root directory - Cannot delete root directory");
                }
                CoreHelpers.LogMessage(String.Format("DeleteDirectory: Deleting {0}", OriginalDirectoryName));
                Directory.Delete(OriginalDirectoryName, true);
               
            }
            else
            {
                CoreHelpers.LogMessage(String.Format("DeleteDirectory: Skipping Delete {0} does not exist", OriginalDirectoryName));
            }
        }

       /// <summary>
        /// Copies a file from a source path to destination path
        /// </summary>
        public void CopyFile()
        {
            if (IsSourcePathRelative != null && IsSourcePathRelative.ToUpper() == Constants.YES)
            {
                SourceFile = Environment.CurrentDirectory + @"\FitNesseRoot\" + SourceFile;
            }

            if (!File.Exists(SourceFile))
            {
                throw new ApplicationException(string.Format("Source file {0} does not exist!", SourceFile));
            }

            File.Copy(SourceFile, DestinationFile, true);

            if (!File.Exists(DestinationFile))
            {
                throw new ApplicationException(string.Format("Unable to copy file {0}!", SourceFile));
            }
        }
        
        /// <summary>
        /// Deletes a file        
        /// </summary>
        public void DeleteFile()
        {
            if (OriginalFileName == null)
            {
                throw new ApplicationException("OriginalFileName is null - Must supply a file name in order to delete a directory");
            }

            if (File.Exists(OriginalFileName))
            {
                CoreHelpers.LogMessage(String.Format("DeleteFile: Deleting {0}", OriginalFileName));
                File.Delete(OriginalFileName);
            }
            else
            {
                CoreHelpers.LogMessage(String.Format("DeleteFile: Skipping Delete {0} does not exist", OriginalFileName));
            }
        }

        /// <summary>
        /// Sets readonly attribute on a file
        /// </summary>
        public string SetFileReadOnly()
        {
            if (OriginalFileName == null)
            {
                throw new ApplicationException("OriginalFileName is null - Must supply a file name in order to set readonly on a file");
            }

            if (File.Exists(OriginalFileName))
            {
                File.SetAttributes(OriginalFileName, FileAttributes.ReadOnly);

                //sr = new StreamReader(OriginalFileName);
                return Constants.YES;
            }
            else
            {
                CoreHelpers.LogMessage(String.Format("SetFileReadOnly: Skipping set of readonly. {0} does not exist", OriginalFileName));
            }
            return Constants.NO;
        }

        /// <summary>
        /// Locks a file        
        /// </summary>
        public string LockFile()
        {
            if (OriginalFileName == null)
            {
                throw new ApplicationException("OriginalFileName is null - Must supply a file name in order to lock a file");
            }

            if (File.Exists(OriginalFileName))
            {
                sr = new StreamReader(OriginalFileName);
                return Constants.YES;
            }
            else
            {
                CoreHelpers.LogMessage(String.Format("LockFile: Skipping lock {0} does not exist", OriginalFileName));
            }
            return Constants.NO;
        }

        /// <summary>
        /// Sets file attribute to Normal
        /// </summary>
        public string SetFileNormal()
        {
            if (OriginalFileName == null)
            {
                throw new ApplicationException("OriginalFileName is null - Must supply a file name in order to set attribute on a file");
            }

            if (File.Exists(OriginalFileName))
            {
                File.SetAttributes(OriginalFileName, FileAttributes.Archive);
                return Constants.YES;
            }
            else
            {
                CoreHelpers.LogMessage(String.Format("SetFileNormal: Skipping setting attribute on file. {0} does not exist", OriginalFileName));
            }
            return Constants.NO;
        }

        /// <summary>
        /// Unlocks the file        
        /// </summary>
        public void UnlockFile()
        {
            sr.Close();
        }

        /// <summary>
        /// Unzips a compressed file        
        /// </summary>
        public void UnzipFile()
        {
            if (IsSourcePathRelative != null && IsSourcePathRelative.ToUpper() == Constants.YES)
            {
                SourceFile = Environment.CurrentDirectory + @"\FitNesseRoot\" + SourceFile;
            }

            if (!File.Exists(SourceFile))
            {
                throw new ApplicationException(string.Format("Source file {0} does not exist!", SourceFile));
            }

            if (!SourceFile.EndsWith(".zip") && !SourceFile.EndsWith(".gz"))
            {
                throw new ApplicationException("SourceFile is not of the right file type.  Can only uncompress .zip or .gz files");
            }

            //CompressionLib compressionLib = new CompressionLib();
            //compressionLib.Extract(SourceFile, NewDirectoryName);
        }

       //Get latest files
        public string getlatestcreatedfile()
        {

            FileInfo newestFile = GetNewestFile(new DirectoryInfo(DirectoryPath));
            return newestFile.ToString();
        }

        private static FileInfo GetNewestFile(DirectoryInfo directory)
        {
            return directory.GetFiles()
                .Union(directory.GetDirectories().Select(d => GetNewestFile(d)))
                .OrderByDescending(f => (f == null ? DateTime.MinValue : f.LastWriteTime))
                .FirstOrDefault();
        }


       //Execute a command
        private void ExecuteCommand(string command)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            var process = Process.Start(processInfo);

            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                Console.WriteLine("output>>" + e.Data);
            process.BeginOutputReadLine();

            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                Console.WriteLine("error>>" + e.Data);
            process.BeginErrorReadLine();

            process.WaitForExit();

            Console.WriteLine("ExitCode: {0}", process.ExitCode);
            process.Close();
        }

       //execute batch

        public void executebatch()
        {
            ExecuteCommand(targetfile);

        }
    }
}
