using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FCore.Class;
using FCore.Enum;
using FCore.Helper;
using FUIControls.Context;
using FUIControls.Settings;
using ICSharpCode.SharpZipLib.Zip;


namespace FUIControls.FormControl
{
    [ToolboxData("<fr:FileUploader runat=\"server\" ID=\"FileUploader1\" />")]
    public sealed class FileUploader : AbsractBasicControl
    {
        #region Delegate & Event

        #region Delegates

        public delegate void OnUploadDelegate();

        #endregion

        public event OnUploadDelegate OnUpload;

        #endregion

        #region Variables

        private LiteralControl _flpFileUploader;
        private TextBox _txtFileSize;
        private TextBox _txtFileTypes;
        private TextBox _txtUploadFolder;
        private TextBox _txtWidth;
        private TextBox _txtTooLargeErrorMessage;
        private TextBox _txtInCorrectExtensionErrorMessage;

        #endregion

        #region Properties

        public string UploadFolder
        {
            get
            {
                object o = ViewState["__file_uploader_upload_folder"];
                if (o == null)
                    return "";
                return o.ToString();
            }
            set { ViewState["__file_uploader_upload_folder"] = value; }
        }

        public decimal MaxFileSize
        {
            get
            {
                object o = ViewState["__file_uploader_file_size"];
                if (o == null)
                    return (4 * 1048576);
                return (decimal)o;
            }
            set { ViewState["__file_uploader_file_size"] = value; }
        }

        public string FileTypes
        {
            get
            {
                object o = ViewState["__file_uploader_file_types"];
                if (o == null)
                    return "*";
                return o.ToString();
            }
            set { ViewState["__file_uploader_file_types"] = value; }
        }

        private string PrivateValue
        {
            get
            {
                object o = ViewState["__file_uploader_private_value"];
                if (o == null)
                    return string.Empty;
                return o.ToString();
            }
            set { ViewState["__file_uploader_private_value"] = value; }
        }

        public string Width
        {
            get
            {
                object o = ViewState["__file_uploader_width"];
                if (o == null)
                    return "200";
                return o.ToString();
            }
            set { ViewState["__file_uploader_width"] = value; }
        }

        public string TooLargeErrorMessage
        {
            get
            {
                object o = ViewState["__file_uploader_toolarge_error"];
                if (o == null)
                    return "";
                return o.ToString();
            }
            set { ViewState["__file_uploader_toolarge_error"] = value; }
        }

        public string InCorrectExtensionErrorMessage
        {
            get
            {
                object o = ViewState["__file_uploader_extension_error"];
                if (o == null)
                    return "";
                return o.ToString();
            }
            set { ViewState["__file_uploader_extension_error"] = value; }
        }

        #endregion

        #region Constructors

        public FileUploader()
            : this(null, null)
        {
        }

        public FileUploader(string controlId, string options)
        {
            if (!string.IsNullOrEmpty(controlId))
                ID = controlId;
            if (!string.IsNullOrEmpty(options))
                SetOptions(options);
        }

        #endregion

        #region Methods

        public override void SetValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (ViewMode == FormControlViewMode.Development)
                {
                    Dictionary<string, string> options = GetOptionsFromXml(value);
                    if (options.ContainsKey("UploadFolder") && !string.IsNullOrEmpty(options["UploadFolder"]))
                        UploadFolder = options["UploadFolder"];
                    if (options.ContainsKey("MaxFileSize") && !string.IsNullOrEmpty(options["MaxFileSize"]))
                        MaxFileSize = decimal.Parse(options["MaxFileSize"]);
                    if (options.ContainsKey("FileTypes") && !string.IsNullOrEmpty(options["FileTypes"]))
                        FileTypes = options["FileTypes"];
                    if (options.ContainsKey("Width") && !string.IsNullOrEmpty(options["Width"]))
                        Width = options["Width"];
                    if (options.ContainsKey("TooLargeErrorMessage") && !string.IsNullOrEmpty(options["TooLargeErrorMessage"]))
                        TooLargeErrorMessage = options["TooLargeErrorMessage"];
                    if (options.ContainsKey("InCorrectExtensionErrorMessage") && !string.IsNullOrEmpty(options["InCorrectExtensionErrorMessage"]))
                        InCorrectExtensionErrorMessage = options["InCorrectExtensionErrorMessage"];
                }
                else
                {
                    PrivateValue = value;
                }
            }
        }

        public override void SetOptions(string xmloptions)
        {
            if (!string.IsNullOrEmpty(xmloptions))
            {
                Dictionary<string, string> options = GetOptionsFromXml(xmloptions);
                if (options.ContainsKey("UploadFolder") && !string.IsNullOrEmpty(options["UploadFolder"]))
                    UploadFolder = options["UploadFolder"];
                if (options.ContainsKey("MaxFileSize") && !string.IsNullOrEmpty(options["MaxFileSize"]))
                    MaxFileSize = decimal.Parse(options["MaxFileSize"]);
                if (options.ContainsKey("FileTypes") && !string.IsNullOrEmpty(options["FileTypes"]))
                    FileTypes = options["FileTypes"];
                if (options.ContainsKey("Width") && !string.IsNullOrEmpty(options["Width"]))
                    Width = options["Width"];
                if (options.ContainsKey("TooLargeErrorMessage") && !string.IsNullOrEmpty(options["TooLargeErrorMessage"]))
                    TooLargeErrorMessage = options["TooLargeErrorMessage"];
                if (options.ContainsKey("InCorrectExtensionErrorMessage") && !string.IsNullOrEmpty(options["InCorrectExtensionErrorMessage"]))
                    InCorrectExtensionErrorMessage = options["InCorrectExtensionErrorMessage"];
            }
        }

        public override string GetValue()
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                var resultDictionary = new Dictionary<string, string>
                                           {
                                               {"UploadFolder", UploadFolder},
                                               {"MaxFileSize", MaxFileSize.ToString()},
                                               {"FileTypes", FileTypes},
                                               {"Width",Width},  
                                               {"TooLargeErrorMessage",TooLargeErrorMessage} ,
                                               {"InCorrectExtensionErrorMessage",InCorrectExtensionErrorMessage}
                                           };
                return GetXmlFromOptions(resultDictionary);
            }
            string file = UploadFile();
            if (!string.IsNullOrEmpty(file))
                return file;

            string result = PrivateValue;
            PrivateValue = string.Empty;
            return result;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Page.Form.Enctype = "multipart/form-data";
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            EnsureControls();

            if (Page.IsPostBack)
            {
                if (ViewMode == FormControlViewMode.Development)
                {
                    var resultDictionary = new Dictionary<string, string>();
                    resultDictionary.Add("UploadFolder", GetControlValue(_txtUploadFolder.ID));
                    resultDictionary.Add("MaxFileSize", GetControlValue(_txtFileSize.ID));
                    resultDictionary.Add("FileTypes", GetControlValue(_txtFileTypes.ID));
                    resultDictionary.Add("Width", GetControlValue(_txtWidth.ID));
                    resultDictionary.Add("TooLargeErrorMessage", GetControlValue(_txtTooLargeErrorMessage.ID));
                    resultDictionary.Add("InCorrectExtensionErrorMessage", GetControlValue(_txtInCorrectExtensionErrorMessage.ID));
                    SetValue(GetXmlFromOptions(resultDictionary));
                }
            }
        }

        protected override void CreateChildControls()
        {
            EnsureControls();
            if (ViewMode == FormControlViewMode.Development)
            {
                var table = new HtmlTable();
                table.ID = "tblOptions";
                table.Width = "100%";
                table.Attributes["class"] = CssClass;

                table.Rows.Add(new HtmlTableRow());
                table.Rows[0].Cells.Add(new HtmlTableCell());
                table.Rows[0].Cells[0].Controls.Add(new LiteralControl("Upload folder:"));
                table.Rows[0].Cells[0].Attributes.Add("class", "label");
                table.Rows[0].Cells.Add(new HtmlTableCell());
                table.Rows[0].Cells[1].Controls.Add(_txtUploadFolder);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[1].Cells.Add(new HtmlTableCell());
                table.Rows[1].Cells[0].Controls.Add(new LiteralControl("Max file size:"));
                table.Rows[1].Cells[0].Attributes.Add("class", "label");
                table.Rows[1].Cells.Add(new HtmlTableCell());
                table.Rows[1].Cells[1].Controls.Add(_txtFileSize);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[2].Cells.Add(new HtmlTableCell());
                table.Rows[2].Cells[0].Controls.Add(new LiteralControl("File types:"));
                table.Rows[2].Cells[0].Attributes.Add("class", "label");
                table.Rows[2].Cells.Add(new HtmlTableCell());
                table.Rows[2].Cells[1].Controls.Add(_txtFileTypes);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[3].Cells.Add(new HtmlTableCell());
                table.Rows[3].Cells[0].Controls.Add(new LiteralControl("Width:"));
                table.Rows[3].Cells[0].Attributes.Add("class", "label");
                table.Rows[3].Cells.Add(new HtmlTableCell());
                table.Rows[3].Cells[1].Controls.Add(_txtWidth);


                table.Rows.Add(new HtmlTableRow());
                table.Rows[4].Cells.Add(new HtmlTableCell());
                table.Rows[4].Cells[0].Controls.Add(new LiteralControl("To Large Error Message:"));
                table.Rows[4].Cells[0].Attributes.Add("class", "label");
                table.Rows[4].Cells.Add(new HtmlTableCell());
                table.Rows[4].Cells[1].Controls.Add(_txtTooLargeErrorMessage);

                table.Rows.Add(new HtmlTableRow());
                table.Rows[5].Cells.Add(new HtmlTableCell());
                table.Rows[5].Cells[0].Controls.Add(new LiteralControl("Incorrect FileType Error Message:"));
                table.Rows[5].Cells[0].Attributes.Add("class", "label");
                table.Rows[5].Cells.Add(new HtmlTableCell());
                table.Rows[5].Cells[1].Controls.Add(_txtInCorrectExtensionErrorMessage);

                Controls.Add(table);
            }
            else
            {
                Controls.Add(_flpFileUploader);
            }
        }

        protected override void EnsureControls()
        {
            if (ViewMode == FormControlViewMode.Development)
            {
                if (_txtUploadFolder == null)
                {
                    _txtUploadFolder = new TextBox();
                    _txtUploadFolder.ID = string.Format("_txtUploadFolder{0}", ID);
                }
                _txtUploadFolder.Text = UploadFolder;

                if (_txtFileSize == null)
                {
                    _txtFileSize = new TextBox();
                    _txtFileSize.ID = string.Format("_txtFileSize{0}", ID);
                }
                _txtFileSize.Text = MaxFileSize.ToString();

                if (_txtFileTypes == null)
                {
                    _txtFileTypes = new TextBox();
                    _txtFileTypes.ID = string.Format("_txtFileTypes{0}", ID);
                }
                _txtFileTypes.Text = FileTypes;

                if (_txtWidth == null)
                {
                    _txtWidth = new TextBox();
                    _txtWidth.ID = string.Format("_txtWidth{0}", ID);
                }
                _txtWidth.Text = Width;

                if (_txtTooLargeErrorMessage == null)
                {
                    _txtTooLargeErrorMessage = new TextBox();
                    _txtTooLargeErrorMessage.ID = string.Format("_txtTooLargeErrorMessage{0}", ID);
                    _txtTooLargeErrorMessage.TextMode = TextBoxMode.MultiLine;
                    _txtTooLargeErrorMessage.Width = 200;
                    _txtTooLargeErrorMessage.Height = 70;
                }
                _txtTooLargeErrorMessage.Text = TooLargeErrorMessage;

                if (_txtInCorrectExtensionErrorMessage == null)
                {
                    _txtInCorrectExtensionErrorMessage = new TextBox();
                    _txtInCorrectExtensionErrorMessage.ID = string.Format("_txtInCorrectExtensionErrorMessage{0}", ID);
                    _txtInCorrectExtensionErrorMessage.TextMode = TextBoxMode.MultiLine;
                    _txtInCorrectExtensionErrorMessage.Width = 200;
                    _txtInCorrectExtensionErrorMessage.Height = 70;
                }
                _txtInCorrectExtensionErrorMessage.Text = InCorrectExtensionErrorMessage;

            }
            else
            {
                if (_flpFileUploader == null)
                {
                    _flpFileUploader = new LiteralControl();
                    _flpFileUploader.ID = string.Format("_flpFileUploader{0}", ID);
                    _flpFileUploader.Text = string.Format("<input type=\"file\" id=\"in{0}\" style=\"width:{1}px;\" name=\"in{0}\"/>",
                                                          _flpFileUploader.ID, Width);
                    string className = string.Empty;
                    if (SiteHelper.IsImageFile(PrivateValue))
                        className = "class=\"view\"";
                    if (!string.IsNullOrEmpty(PrivateValue))
                        _flpFileUploader.Text += string.Format("<div class=\"file-upload-attachment\"><a target=\"_blank\" " + className + " href=\"{0}{1}{2}\">view</a></div>", SiteHelper.GetSiteUrl(), CoreSettings.CurrentSite.RootFolder, PrivateValue);
                    if (!string.IsNullOrEmpty(className))
                        Page.ClientScript.RegisterStartupScript(typeof(TextBoxControl), "fancy", "<script>$(document).ready(function(){ $('a.view').fancybox();});</script>");

                }
            }
        }

        private string UploadFile()
        {
            string resultpath = string.Empty;
            string uploadPath = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath + UploadFolder);

            string controlId = "in" + _flpFileUploader.ID;
            HttpPostedFile uploadFile = HttpContext.Current.Request.Files[controlId];
            if (uploadFile.ContentLength > 0)
            {
                if (OnUpload != null)
                    OnUpload();

                string fileName = uploadFile.FileName;
                string tempName = fileName.Substring(0, fileName.LastIndexOf("."));
                string extension = fileName.Substring(fileName.LastIndexOf("."));
                string path = Path.Combine(uploadPath, fileName);
                if (File.Exists(path))
                {
                    int counter = 1;
                    while (File.Exists(path))
                    {
                        fileName = string.Format("{0}{1}{2}", tempName, counter, extension);
                        path = Path.Combine(uploadPath, fileName);
                        counter++;
                    }

                }
                uploadFile.SaveAs(path);
                if (!UploadFolder.EndsWith("/"))
                    resultpath = UploadFolder + @"/" + fileName;
                else
                    resultpath = UploadFolder + fileName;
                PrivateValue = resultpath;

            }
            return resultpath;
        }

        private void UnZip(HttpPostedFile uploadFile, string outputFolder)
        {
            ZipInputStream s = new ZipInputStream(uploadFile.InputStream);
            ZipEntry theEntry;
            string tmpEntry = String.Empty;
            while ((theEntry = s.GetNextEntry()) != null)
            {
                string directoryName = outputFolder;
                string fileName = Path.GetFileName(theEntry.Name);

                // create directory 
                if (directoryName != "")
                {
                    Directory.CreateDirectory(directoryName);
                }
                if (fileName != String.Empty)
                {
                    if (theEntry.Name.IndexOf(".ini") < 0)
                    {
                        string fullPath = directoryName + "\\" + theEntry.Name;
                        fullPath = fullPath.Replace("\\ ", "\\");
                        string fullDirPath = Path.GetDirectoryName(fullPath);
                        if (!Directory.Exists(fullDirPath)) Directory.CreateDirectory(fullDirPath);
                        FileStream streamWriter = File.Create(fullPath);
                        int size = 2048;
                        byte[] data = new byte[size];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                        streamWriter.Close();
                    }
                }
            }
            s.Close();
        }

        public override bool Validate()
        {
            string controlId = "in" + _flpFileUploader.ID;
            HttpPostedFile uploadFile = HttpContext.Current.Request.Files[controlId];
            bool require = true;
            bool maxsize = true;
            bool fileValid = false;
            if (IsRequired)
            {
                require = uploadFile.ContentLength > 0 || !string.IsNullOrEmpty(PrivateValue);
                if (!require)
                {
                    ErrorInfo error = new ErrorInfo();
                    error.Source = FieldName;
                    error.Message = RequiredErrorMessage;
                    RegisterError(error);
                }
            }
            if (uploadFile != null)
            {
                if (uploadFile.ContentLength > (MaxFileSize * 1048576))
                {
                    maxsize = false;
                    ErrorInfo error = new ErrorInfo();
                    error.Source = FieldName;
                    error.Message = TooLargeErrorMessage;
                    RegisterError(error);
                }


                string filename = uploadFile.FileName.ToLower();
                string[] filetypes = FileTypes.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (filetypes.Any(item => !filename.Contains(item.ToLower())))
                {
                    fileValid = true;
                }
                if (!fileValid)
                {
                    ErrorInfo error = new ErrorInfo();
                    error.Source = FieldName;
                    error.Message = InCorrectExtensionErrorMessage;
                    RegisterError(error);
                }
            }

            return require && maxsize && fileValid;
        }

        #endregion
    }
}