using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml;
using filesigner2.Properties;
using CapiComRCW;
using System.IO;
using System.Runtime.InteropServices;
using Ionic.Zip;
using System.Diagnostics;
using Microsoft.Win32;
using System.Threading;
using System.IO.Pipes;

namespace filesigner2
{
    public partial class MainForm : Form
    {
        XmlDocument xdEkuOids;
        Thread pipe;
        List<string> errlist = new List<string>();
        public delegate void UpdateList();
        public UpdateList myUpdate;
        public MainForm()
        {
            InitializeComponent();
 //           StartPipe();
        }

        private void StartPipe()
        {
            //Path.GetFullPath(;
            pipe = new Thread(PipeReader);
            pipe.Start();
        }
        
        public void PipeReader()
        {
            var server = new NamedPipeServerStream("FileSignerPipe");
            StreamReader reader = new StreamReader(server);
            string s = "";
            myUpdate = () => { lbFiles.Items.Add(s); };
            while (pipe.ThreadState!=System.Threading.ThreadState.StopRequested)
            {
                try
                {
                    server.WaitForConnection();
                    s = reader.ReadLine();
                    //Delegate m =Delegate.CreateDelegate(  (t, n) => { lbFiles.Items.Add(s); });
                    Thread.Sleep(100);
                    if (s != null)
                        lbFiles.Invoke(myUpdate);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("pipe error!" + e.Message);
                }
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.xdEkuOids = new XmlDocument();
            this.xdEkuOids.LoadXml(Resources.EKU_OIDs);
            LoadCerts();
            if (cbCerts.Items.Count > 0)
            {
                cbCerts.SelectedIndex = 0;
            }
            string [] args=Environment.GetCommandLineArgs();
            if (args.Length>1)
            {
                lbFiles.Items.Add(Path.GetFullPath( args[1]));
            }
        }

        private void LoadCerts()
        {
            cbCerts.DropDownWidth = cbCerts.Width + 200;
            X509Store x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            x509Store.Open(OpenFlags.ReadOnly);
            X509Certificate2Enumerator enumerator = x509Store.Certificates.GetEnumerator();
            while (enumerator.MoveNext())
            {
                X509Certificate2 current = enumerator.Current;
                if (true )//!(current.GetKeyAlgorithm() != "1.2.643.2.2.19"))
                {
                    if (current.HasPrivateKey && current.NotAfter>DateTime.Now)
                    {
                        X509EnhancedKeyUsageExtension x509EnhancedKeyUsageExtension = current.Extensions["2.5.29.37"] as X509EnhancedKeyUsageExtension;
                        if (true)// x509EnhancedKeyUsageExtension != null)
                        {
                            //int num = 0;
                            //string sEKU = "";
                            //OidEnumerator enumerator2 = x509EnhancedKeyUsageExtension.EnhancedKeyUsages.GetEnumerator();
                            //while (enumerator2.MoveNext())
                            //{
                            //    Oid current2 = enumerator2.Current;
                            //    if (this.OidToString(current2.Value) != "Не определен")
                            //    {
                            //        sEKU = current2.Value;
                            //        num++;
                            //    }
                            //}
                            //Regex regex = new Regex("\nCN=(.*)\n");
                            //string sName = current.SubjectName.Name;
                            //Match match = regex.Match("\n" + current.SubjectName.Format(true).Replace("\r", "") + "\n");
                            //if (match.Success)
                            //{
                            //    sName = match.Groups[1].Value;
                            //}
                            cbCerts.Items.Add(new CertificateItem( current));
                            
                            //item = default(UserSelectForm.info);
                            //item.sName = sName;
                            //item.cert = current;
                            //item.sEMail = UserSelectForm.GetEMail(current.SubjectName.Format(true));
                            //item.sEKU = sEKU;
                            //item.dtStartDate = current.NotBefore.ToLocalTime();
                            //item.dtEndDate = current.NotAfter.ToLocalTime();
                            //item.bValid = Program.ValidateCertificate(this.log, current, "^.+\\.1\\.3\\..+$", true, out item.sTooltip);
                            //this.l.Add(item);
                        }
                    }
                }
            }
            x509Store.Close();
        }
        private string OidToString(string sOID)
        {
            string result;
            foreach (XmlNode xmlNode in this.xdEkuOids.SelectNodes("//EKU_OIDs/EKU[@OID]"))
            {
                Regex regex = new Regex(xmlNode.Attributes["OID"].Value);
                if (regex.IsMatch(sOID))
                {
                    result = xmlNode.InnerText;
                    return result;
                }
            }
            result = "Не определен";
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = true;
            if (of.ShowDialog() == DialogResult.OK)
            {
                lbFiles.Items.AddRange(of.FileNames);
            }
            of.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            errlist.Clear();
            SignFiles();
            lbErrs.Items.AddRange(errlist.ToArray());
        }

        private void SignFiles()
        {
            foreach (string item in lbFiles.Items)
            {
                
                SignFile(item);
            }
            MessageBox.Show("Завершено!"+String.Join("\r\n", errlist.ToArray()));
        }

        private void SignFile(string sFileIn)
        {
            try
            {
                string sigfile = sFileIn + ".sig";
                if (Path.GetExtension(sFileIn).ToUpper().Equals(".ZIP"))
                {
                    SignZip(sFileIn);
                    if (!cbExtSignZIP.Checked) return;
                    File.Delete(sigfile);
                }
                X509Certificate2 m_cert = (cbCerts.SelectedItem as CertificateItem).certificate;// X509Certificate2;
                if (m_cert == null)
                {
                    throw new ApplicationException("Сформировать ЭЦП невозможно: не найден действительный сертификат отправителя!");
                }
                SignedData signedData = new SignedDataClass();
                Utilities utilities = new UtilitiesClass();
                byte[] array;
                using (FileStream fileStream = new FileStream(sFileIn, FileMode.Open, FileAccess.Read))
                {
                    array = new byte[fileStream.Length];
                    fileStream.Position = 0L;
                    fileStream.Read(array, 0, (int)fileStream.Length);
                    fileStream.Close();
                }
                byte[] array2 = null;
                bool flag = false;
                if (File.Exists(sFileIn + ".sig") && cbCoSign.Checked)
                {
                    flag = true;
                    using (FileStream fileStream = new FileStream(sFileIn + ".sig", FileMode.Open, FileAccess.Read))
                    {
                        array2 = new byte[fileStream.Length];
                        fileStream.Position = 0L;
                        fileStream.Read(array2, 0, (int)fileStream.Length);
                        fileStream.Close();
                    }
                }
                Signer signer = new SignerClass();
                IStore store = new StoreClass();
                bool flag2 = false;
                store.Open(CAPICOM_STORE_LOCATION.CAPICOM_CURRENT_USER_STORE, "My", CAPICOM_STORE_OPEN_MODE.CAPICOM_STORE_OPEN_READ_ONLY);
                foreach (ICertificate certificate in store.Certificates)
                {
                    if (certificate.Thumbprint == m_cert.Thumbprint)
                    {
                        signer.Certificate = certificate;
                        flag2 = true;
                        break;
                    }
                }
                if (!flag2)
                {
                    throw new Exception("Не удалось найти сертификат подписи!");
                }
                CapiComRCW.Attribute attribute = new AttributeClass();
                attribute.Name = CAPICOM_ATTRIBUTE.CAPICOM_AUTHENTICATED_ATTRIBUTE_SIGNING_TIME;
                attribute.Value = DateTime.Now.ToUniversalTime();
                signer.AuthenticatedAttributes.Add(attribute);
                byte[] array3;
                if (flag && cbCoSign.Checked)
                {
                    // signedData.Content = "";

                    //signedData.Content = Marshal.PtrToStringBSTR( utilities.ByteArrayToBinaryString(array));
                    ((CapiComRCW.ISignedData)signedData).set_Content(utilities.ByteArrayToBinaryString(array));
                    try
                    {
                        signedData.Verify(Convert.ToBase64String(array2), true, CAPICOM_SIGNED_DATA_VERIFY_FLAG.CAPICOM_VERIFY_SIGNATURE_ONLY);
                    }
                    catch (Exception e)
                    {
                        errlist.Add("Ошибка проверки подписи!" + sFileIn + ":" + e.Message);
                    }
                    //Store store2 = new StoreClass();
                    //store2.Open(CAPICOM_STORE_LOCATION.CAPICOM_CURRENT_USER_STORE, "AddressBook", CAPICOM_STORE_OPEN_MODE.CAPICOM_STORE_OPEN_READ_WRITE);
                    //for (int i = 1; i <= signedData.Signers.Count; i++)
                    //{
                    //    Signer signer2 = (Signer)signedData.Signers[i];
                    //    Certificate pVal = (Certificate)signer2.Certificate;
                    //    store2.Add(pVal);
                    //}
                    //store2.Close();
                    string s = signedData.CoSign(signer, CAPICOM_ENCODING_TYPE.CAPICOM_ENCODE_BASE64);
                    array3 = Convert.FromBase64String(s);
                }
                else
                {
                    //signedData.Content = utilities.ByteArrayToBinaryString(array);
                    ((CapiComRCW.ISignedData)signedData).set_Content(utilities.ByteArrayToBinaryString(array));

                    string s = signedData.Sign(signer, true, CAPICOM_ENCODING_TYPE.CAPICOM_ENCODE_BASE64);
                    array3 = Convert.FromBase64String(s);
                }
                using (FileStream fileStream = new FileStream(sFileIn + ".sig", FileMode.Create, FileAccess.Write))
                {
                    fileStream.Write(array3, 0, array3.Length);
                    fileStream.Close();
                }
            } catch (Exception e)
            {
                MessageBox.Show("Ошибка подписывания!" + e.ToString());
            }
        }

        private void SignZip(string sFileIn)
        {
            try
            {
                ZipFile zf = new ZipFile(sFileIn);
                List<ZipEntry> sl = new List<ZipEntry>();
                List<ZipEntry> dell = new List<ZipEntry>();
                string fn;
                foreach (ZipEntry ze in zf.Entries)
                {
                    fn = ze.FileName.ToLower();
                    if (fn.EndsWith(".sig"))
                    {
                        if (cbNoEntryZIPSIGDelete.Checked && fn.EndsWith(".zip.sig"))
                        {
                            continue;
                        }
                        if (cbPDFSignDelete.Checked && fn.EndsWith(".pdf.sig"))
                        {
                            continue;
                        }
                        if (!cbCoSign.Checked)
                            dell.Add(ze);
                        continue;
                    }
                    sl.Add(ze);
                }
                for (int i = 0; i < dell.Count; i++)
                {
                    zf.RemoveEntry(dell[i]);
                }
                foreach (ZipEntry ze in sl)
                {
                    try
                    {
                        if (ze.IsDirectory) continue;
                        MemoryStream ms = new MemoryStream();
                        MemoryStream mssign = new MemoryStream();
                        ze.Extract(ms);
                        byte[] sarr;// = SignBuffer(ms.ToArray());
                        if (cbCoSign.Checked && zf.EntryFileNames.Contains(ze.FileName + ".sig"))
                        {
                            ZipEntry signze = zf[ze.FileName + ".sig"];
                            signze.Extract(mssign);
                            sarr = CoSignBuffer(ms.ToArray(), mssign.ToArray());
                            zf.RemoveEntry(signze);

                        }
                        else
                        {
                            sarr = SignBuffer(ms.ToArray());
                            if (sarr == null) return;
                            if (zf.EntryFileNames.Contains(ze.FileName + ".sig")) continue;
                        }
                        ZipEntry sz = zf.AddEntry(ze.FileName + ".sig", sarr);
                    }
                    catch (Exception e)
                    {
                        errlist.Add("Sign zip error!" + ze.FileName + " ");
                    }
                }

                zf.Save();
            } catch (Exception e)
            {
                MessageBox.Show("Ошибка подписи ZIP!" + e.ToString());
            }
        }

        private byte[] CoSignBuffer(byte[] data, byte[] signdata)
        {
            X509Certificate2 m_cert = ((CertificateItem)cbCerts.SelectedItem).certificate;// as X509Certificate2;
            if (m_cert == null)
            {
                MessageBox.Show("не найден сертификат!");
                return null;
            }
            SignedData signedData = new SignedDataClass();
            Utilities utilities = new UtilitiesClass();
            byte[] array = data;

            Signer signer = new SignerClass();
            IStore store = new StoreClass();
            bool flag2 = false;
            store.Open(CAPICOM_STORE_LOCATION.CAPICOM_CURRENT_USER_STORE, "My", CAPICOM_STORE_OPEN_MODE.CAPICOM_STORE_OPEN_READ_ONLY);
            foreach (ICertificate certificate in store.Certificates)
            {
                if (certificate.Thumbprint == m_cert.Thumbprint)
                {
                    signer.Certificate = certificate;
                    flag2 = true;
                    break;
                }
            }
            if (!flag2)
            {
                throw new Exception("Не удалось найти сертификат подписи!");
            }
            CapiComRCW.Attribute attribute = new AttributeClass();
            attribute.Name = CAPICOM_ATTRIBUTE.CAPICOM_AUTHENTICATED_ATTRIBUTE_SIGNING_TIME;
            attribute.Value = DateTime.Now.ToUniversalTime();
            signer.AuthenticatedAttributes.Add(attribute);
            byte[] array3;
            byte[] array2 = signdata;
            ((CapiComRCW.ISignedData)signedData).set_Content(utilities.ByteArrayToBinaryString(array));

            signedData.Verify(Convert.ToBase64String(array2), true, CAPICOM_SIGNED_DATA_VERIFY_FLAG.CAPICOM_VERIFY_SIGNATURE_ONLY);
            Store store2 = new StoreClass();
            store2.Open(CAPICOM_STORE_LOCATION.CAPICOM_CURRENT_USER_STORE, "AddressBook", CAPICOM_STORE_OPEN_MODE.CAPICOM_STORE_OPEN_READ_WRITE);
            for (int i = 1; i <= signedData.Signers.Count; i++)
            {
                Signer signer2 = (Signer)signedData.Signers[i];
                Certificate pVal = (Certificate)signer2.Certificate;
                store2.Add(pVal);
            }
            store2.Close();

            string s = signedData.CoSign(signer, CAPICOM_ENCODING_TYPE.CAPICOM_ENCODE_BASE64);
            array3 = Convert.FromBase64String(s);
            return array3;
        }

        private byte[] SignBuffer(byte[] arr)
        {
            X509Certificate2 m_cert = ((CertificateItem)cbCerts.SelectedItem).certificate;// as X509Certificate2;
            if (m_cert == null)
            {
               MessageBox.Show("не найден сертификат!");
               return null;
            }
            SignedData signedData = new SignedDataClass();
            Utilities utilities = new UtilitiesClass();
            byte[] array=arr;
            //using (FileStream fileStream = new FileStream(sFileIn, FileMode.Open, FileAccess.Read))
            //{
            //    array = new byte[fileStream.Length];
            //    fileStream.Position = 0L;
            //    fileStream.Read(array, 0, (int)fileStream.Length);
            //    fileStream.Close();
            //}
            //byte[] array2 = null;
            //bool flag = false;
            //if (File.Exists(sFileIn + ".sig"))
            //{
            //    flag = true;
            //    using (FileStream fileStream = new FileStream(sFileIn + ".sig", FileMode.Open, FileAccess.Read))
            //    {
            //        array2 = new byte[fileStream.Length];
            //        fileStream.Position = 0L;
            //        fileStream.Read(array2, 0, (int)fileStream.Length);
            //        fileStream.Close();
            //    }
            //}
            Signer signer = new SignerClass();
            IStore store = new StoreClass();
            bool flag2 = false;
            store.Open(CAPICOM_STORE_LOCATION.CAPICOM_CURRENT_USER_STORE, "My", CAPICOM_STORE_OPEN_MODE.CAPICOM_STORE_OPEN_READ_ONLY);
            foreach (ICertificate certificate in store.Certificates)
            {
                if (certificate.Thumbprint == m_cert.Thumbprint)
                {
                    signer.Certificate = certificate;
                    flag2 = true;
                    break;
                }
            }
            if (!flag2)
            {
                throw new Exception("Не удалось найти сертификат подписи!");
            }
            CapiComRCW.Attribute attribute = new AttributeClass();
            attribute.Name = CAPICOM_ATTRIBUTE.CAPICOM_AUTHENTICATED_ATTRIBUTE_SIGNING_TIME;
            attribute.Value = DateTime.Now.ToUniversalTime();
            signer.AuthenticatedAttributes.Add(attribute);
            byte[] array3;
            //if (flag)
            //{
            //    ((CapiComRCW.ISignedData)signedData).set_Content(utilities.ByteArrayToBinaryString(array));
            //    signedData.Verify(Convert.ToBase64String(array2), true, CAPICOM_SIGNED_DATA_VERIFY_FLAG.CAPICOM_VERIFY_SIGNATURE_ONLY);
            //    Store store2 = new StoreClass();
            //    store2.Open(CAPICOM_STORE_LOCATION.CAPICOM_CURRENT_USER_STORE, "AddressBook", CAPICOM_STORE_OPEN_MODE.CAPICOM_STORE_OPEN_READ_WRITE);
            //    for (int i = 1; i <= signedData.Signers.Count; i++)
            //    {
            //        Signer signer2 = (Signer)signedData.Signers[i];
            //        Certificate pVal = (Certificate)signer2.Certificate;
            //        store2.Add(pVal);
            //    }
            //    store2.Close();
            //    string s = signedData.CoSign(signer, CAPICOM_ENCODING_TYPE.CAPICOM_ENCODE_BASE64);
            //    array3 = Convert.FromBase64String(s);
            //}
//            else
            {
                //signedData.Content = utilities.ByteArrayToBinaryString(array);
                ((CapiComRCW.ISignedData)signedData).set_Content(utilities.ByteArrayToBinaryString(array));

                string s = signedData.Sign(signer, true, CAPICOM_ENCODING_TYPE.CAPICOM_ENCODE_BASE64);
                array3 = Convert.FromBase64String(s);
            }
            return array3;
            //using (FileStream fileStream = new FileStream(sFileIn + ".sig", FileMode.Create, FileAccess.Write))
            //{
            //    fileStream.Write(array3, 0, array3.Length);
            //    fileStream.Close();
            //}
        }

        private void cbCerts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCerts.SelectedIndex!=-1)
            {
                X509Certificate2 cert = (cbCerts.SelectedItem as CertificateItem).certificate;//  X509Certificate2;
                tbCertInfo.Text = cert.ToString();// cbCerts.SelectedItem.ToString();
              //  Debug.WriteLine(cert.FriendlyName);
              //  Debug.WriteLine(cert.SubjectName.Name);
              //  Debug.WriteLine(cert.IssuerName.Name);
              // Debug.WriteLine(cert.GetNameInfo(X509NameType.UrlName,true));
                Debug.WriteLine(cert.GetNameInfo(X509NameType.SimpleName, false));
            }
        }

        private void cbCoSign_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddFolder();
        }

        private void AddFolder()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                string sfolder = fbd.SelectedPath;
                AddAllFiles(sfolder);
            }

        }

        private void AddAllFiles(string sfolder)
        {
            string[] files = Directory.GetFiles(sfolder);
            foreach (string fname in files)
            {
                if ((Path.GetExtension(fname).ToUpper() == ".ZIP") ||
                     (Path.GetExtension(fname).ToUpper() == ".XML"))
                    lbFiles.Items.Add(fname);
            }
            string[] dirs = Directory.GetDirectories(sfolder);
            foreach (string dir in dirs)
            {
                AddAllFiles(dir);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var slist=lbFiles.SelectedItems;
            while (slist.Count>0)
            {
                lbFiles.Items.Remove(slist[0]);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MakeShellRegister();
        }

        private void MakeShellRegister()
        {
            if (MessageBox.Show("Добавить подпись в контекстное меню? Требуется запуск от администратора!", "Контекстное меню", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    string commpath = Environment.GetCommandLineArgs()[0];
                    RegistryKey reg = Registry.ClassesRoot.CreateSubKey("*");
                    reg = reg.CreateSubKey("shell");
                    RegistryKey reg2 = reg.CreateSubKey("Sign");
                    //if (reg2 == null)
                    //{
                    //    MessageBox.Show("Не удалось!");
                    //    return;
                    //}
                    reg2.SetValue("", "Подписать ЭЦП");
                    RegistryKey reg3 = reg2.CreateSubKey("command");
                    reg3.SetValue("", commpath + " " + "\"%1\"");
                    MessageBox.Show("Успешно!");
                } catch(Exception e)
                {
                    MessageBox.Show("Не удалось!" + e.Message);
                }
            }
        }
    }
}
