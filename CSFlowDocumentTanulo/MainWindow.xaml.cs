
using CSAc4yObjectObjectService.Association;
using CSAc4yObjectObjectService.Object;
using CSFlowDocumentTanulo;
using Modul.Final.Class;
using Modul.PersistentService.Class;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace CSFlowDocumentTry1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public string APPSETTING_SQLCONNECTIONSTRING = ConfigurationManager.AppSettings["sqlConnectionString"];
        public Dictionary<string, Tanulo> TanuloDictionary = new Dictionary<string, Tanulo>();
        public List<Nyelv> NyelvList = new List<Nyelv>();
        public TanuloKontener tanuloKontener = new TanuloKontener()
        {
            TanuloLista = new List<Tanulo>()
        };
        public int SectionSorszam = 1;
        public int NyelvSorszam = 1;

        SerializeMethods SerializeMethods = new SerializeMethods();

        public MainWindow()
        {
            InitializeComponent();

           // uiListViewFlowDocument.Items.Add(uiListViewItem);
           // uiListViewFlowDocument.Items.Add(uiListViewItem);
        }

        private void AddTextBox(object subject, RoutedEventArgs e)
        {
            AddForm("", "", "", "");
        }

        private void ButtonAction(object subject, RoutedEventArgs e)
        {
            tanuloKontener.TanuloLista.Clear();
            /*
            Tanulo tanulo = new Tanulo()
            {
                Vezeteknev = uiTextBoxVezetekNev.Text.ToString(),
                Keresztnev = uiTextBoxKeresztNev.Text.ToString(),
                Kor = uiTextBoxKor.Text.ToString(),
                Cim = uiTextBoxCim.Text.ToString(),
                NyelvList = new List<Nyelv>()
            };

            tanuloKontener.TanuloLista.Add(tanulo);

            List<Nyelv> nyelvList = new List<Nyelv>();
            */
            foreach (var block in uiFlowDocument.Blocks)
            {
                foreach (var dictionary in TanuloDictionary)
                {
                    if (block.Name.Equals(dictionary.Key))
                    {
                        Section sectionBlock = (Section)block;
                        BlockUIContainer uiContainmer = (BlockUIContainer)sectionBlock.Blocks.FirstBlock;
                        WrapPanel uiMainWrapPanel = (WrapPanel)uiContainmer.Child;
                        UIElementCollection uiInnerWrapPanels = uiMainWrapPanel.Children;
                        foreach(var uiInnerWrapPanel in uiInnerWrapPanels)
                        {
                            UIElementCollection uiWrapPanelElements = uiMainWrapPanel.Children;
                            foreach(var element in uiWrapPanelElements)
                            {
                                if (element.GetType().Name.Equals("WrapPanel"))
                                {
                                    WrapPanel wrapPanel = (WrapPanel)element;
                                    foreach (Nyelv nyelv in dictionary.Value.NyelvList)
                                    {

                                        if (wrapPanel.Name.Equals(nyelv.WrapPanelSorszam))
                                        {
                                            foreach (var elem in wrapPanel.Children)
                                            {
                                                if (elem.GetType().Name.Equals("TextBox"))
                                                {
                                                    TextBox uiTextBox = (TextBox)elem;
                                                    if (uiTextBox.Name.Equals("uiTextBoxNyelv"))
                                                    {
                                                        nyelv.Nev = uiTextBox.Text;
                                                    }
                                                    else if (uiTextBox.Name.Equals("uiTextBoxSzint"))
                                                    {
                                                        nyelv.Szint = uiTextBox.Text;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    foreach (var elem in wrapPanel.Children)
                                    {
                                        if (elem.GetType().Name.Equals("TextBox"))
                                        {
                                            TextBox uiTextBox = (TextBox)elem;
                                            if (uiTextBox.Name.Equals("uiTextBoxVezetekNev"))
                                            {
                                                dictionary.Value.Vezeteknev = uiTextBox.Text;
                                            }
                                            else if (uiTextBox.Name.Equals("uiTextBoxKeresztNev"))
                                            {
                                                dictionary.Value.Keresztnev = uiTextBox.Text;
                                            }
                                            else if (uiTextBox.Name.Equals("uiTextBoxKor"))
                                            {
                                                dictionary.Value.Kor = uiTextBox.Text;
                                            }
                                            else if (uiTextBox.Name.Equals("uiTextBoxCim"))
                                            {
                                                dictionary.Value.Cim = uiTextBox.Text;
                                            }
                                        }
                                    }
                                    
                                }
                            }
                        }
                    }
                }
            }
            foreach (var dictionary in TanuloDictionary)
            {
                tanuloKontener.TanuloLista.Add(dictionary.Value);
            }

            uiTextBlock.Text = SerializeMethods.serialize(tanuloKontener, typeof(TanuloKontener));

            UploadTanuloKontener(tanuloKontener);
            UploadKontenerTanuloAssociation(tanuloKontener);

            /*
            uiTextBoxName.Text = "";
            uiTextBoxAncestor.Text = "";
            uiTextBoxGUID.Text = "";
            uiTextBoxNamespace.Text = "";
            */
        }
    
        public void XmlBetoltes(object subject, RoutedEventArgs e)
        {
            OpenXML();

            List<Block> flowDocumentBlockList = uiFlowDocument.Blocks.ToList();
            uiFlowDocument.Blocks.Clear();

            foreach (var block in flowDocumentBlockList)
            {
                if (block.Name.Equals("section0"))
                {
                    uiFlowDocument.Blocks.Add(block);
                }
            }

            SectionSorszam = 1;
            NyelvSorszam = 1;

            TanuloDictionary.Clear();
            tanuloKontener.TanuloLista.Clear();

            string xml = uiTextBlock.Text;
            TanuloKontener kontenerUj = SerializeMethods.Deserialize(xml);
            int x = 0;
            if (kontenerUj != null)
            {
                foreach (var tanulo in kontenerUj.TanuloLista)
                {
                    AddForm(tanulo.Vezeteknev, tanulo.Keresztnev, tanulo.Kor, tanulo.Cim);
                    foreach(Nyelv nyelv in tanulo.NyelvList)
                    {
                        AddNyelvekForm(nyelv.Nev, nyelv.Szint, nyelv.SectionName);
                    }
                }
            }
        }

        public void AddForm(string vNev, string kNev, string kor, string cim)
        {
            WrapPanel uiMainWrapPanel = new WrapPanel()
            {
                Orientation = Orientation.Vertical,
                Width = 750,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            WrapPanel uiInnerWrapPanel1 = new WrapPanel()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness()
                {
                    Top = 12,
                    Left = 25
                }
            };

            uiInnerWrapPanel1.Children.Add(
                new Label()
                {
                    Content = "Vezetéknév: ",
                    Width = 100
                });

            uiInnerWrapPanel1.Children.Add(
                new TextBox()
                {
                    Name = "uiTextBoxVezetekNev",
                    Width = 250,
                    Height = 25,
                    Text = vNev
                });

            uiInnerWrapPanel1.Children.Add(
                new Label()
                {
                    Content = "Keresztnév: ",
                    Width = 100
                });

            uiInnerWrapPanel1.Children.Add(
                new TextBox()
                {
                    Name = "uiTextBoxKeresztNev",
                    Width = 250,
                    Height = 25,
                    Text = kNev
                });

            WrapPanel uiInnerWrapPanel2 = new WrapPanel()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness()
                {
                    Top = 12,
                    Left = 25
                }
            };

            uiInnerWrapPanel2.Children.Add(
                new Label()
                {
                    Content = "Kor: ",
                    Width = 100
                });

            uiInnerWrapPanel2.Children.Add(
                new TextBox()
                {
                    Name = "uiTextBoxKor",
                    Width = 250,
                    Height = 25,
                    Text = kor
                });

            uiInnerWrapPanel2.Children.Add(
                new Label()
                {
                    Content = "Cím: ",
                    Width = 100
                });

            uiInnerWrapPanel2.Children.Add(
                new TextBox()
                {
                    Name = "uiTextBoxCim",
                    Width = 250,
                    Height = 25,
                    Text = cim
                });
            BlockUIContainer uiContainer = new BlockUIContainer()
            {
                Child = uiMainWrapPanel,
                BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                BorderThickness = new Thickness()
                {
                    Bottom = 1,
                    Left = 1,
                    Top = 1,
                    Right = 1
                }

            };
            Section sectionFromCode = new Section()
            {
                Background = new SolidColorBrush(Color.FromRgb(248, 248, 255)),
                Name = "section" + SectionSorszam
            };

            Button uiTorlesButton = new Button()
            {
                Width = 200,
                Content = "Törlés",
                Tag = sectionFromCode.Name
            };

            uiTorlesButton.Click += deleteButton;
            
            Button uiAddNyelvButton = new Button()
            {
                Width = 100,
                Height = 25,
                Content = "Nyelv +",
                Margin = new Thickness()
                    {
                        Top = 12,
                        Left = 25
                    },
                HorizontalAlignment = HorizontalAlignment.Left,
                Tag = sectionFromCode.Name
            };

            uiAddNyelvButton.Click += AddNyelv;


            uiMainWrapPanel.Children.Add(uiInnerWrapPanel1);
            uiMainWrapPanel.Children.Add(uiInnerWrapPanel2);
            uiMainWrapPanel.Children.Add(uiAddNyelvButton);
            uiMainWrapPanel.Children.Add(uiTorlesButton);


            sectionFromCode.Blocks.Add(uiContainer);
            uiFlowDocument.Blocks.Add(sectionFromCode);


            TanuloDictionary.Add(sectionFromCode.Name, new Tanulo() {
                    NyelvList = new List<Nyelv>(),
                    VegzettsegList = new List<Vegzettseg>()
                });

            SectionSorszam++;
        }

        public void AddNyelv(object subject, RoutedEventArgs e)
        {
            var clickedButton = subject as Button;
            string sectionName = clickedButton.Tag.ToString();

            AddNyelvekForm("","", sectionName);
        }

        public void AddNyelvekForm(string nev, string szint, string sectionName)
        {

            WrapPanel uiInnerWrapPanel1 = new WrapPanel()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness()
                {
                    Top = 12,
                    Left = 25
                },
                Background = new SolidColorBrush(Color.FromRgb(220, 220, 255)),
                Name = "nyelvWrapPanel" + NyelvSorszam
            };

            uiInnerWrapPanel1.Children.Add(
                new Label()
                {
                    Content = "Nyelv: ",
                    Width = 100
                });

            uiInnerWrapPanel1.Children.Add(
                new TextBox()
                {
                    Name = "uiTextBoxNyelv",
                    Width = 250,
                    Height = 25,
                    Text = nev
                });

            uiInnerWrapPanel1.Children.Add(
                new Label()
                {
                    Content = "Szint: ",
                    Width = 100
                });

            uiInnerWrapPanel1.Children.Add(
                new TextBox()
                {
                    Name = "uiTextBoxSzint",
                    Width = 250,
                    Height = 25,
                    Text = szint
                });
            

            Button uiTorlesButton = new Button()
            {
                Width = 150,
                Content = "Nyelv törlése",
                Tag = uiInnerWrapPanel1.Name + "," + sectionName,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness()
                {
                    Top = 12,
                    Left = 25
                },
            };

            uiTorlesButton.Click += deleteNyelv;

            foreach (var block in uiFlowDocument.Blocks)
            {
                if (block.Name.Equals(sectionName))
                {
                    Section sectionBlock = (Section)block;
                    BlockUIContainer uiContainmer = (BlockUIContainer)sectionBlock.Blocks.FirstBlock;
                    WrapPanel uiMainWrapPanel = (WrapPanel)uiContainmer.Child;

                    uiInnerWrapPanel1.Children.Add(uiTorlesButton);
                    uiMainWrapPanel.Children.Add(uiInnerWrapPanel1);

                    Nyelv nyelv = new Nyelv()
                    {
                        SectionName = sectionName,
                        WrapPanelSorszam = "nyelvWrapPanel" + NyelvSorszam,
                    };

                    NyelvList.Add(new Nyelv()
                    {
                        SectionName = sectionName,
                        WrapPanelSorszam = "nyelvWrapPanel" + NyelvSorszam,
                    });

                    foreach(var dictionary in TanuloDictionary)
                    {
                        if (dictionary.Key.Equals(nyelv.SectionName))
                        {
                            dictionary.Value.NyelvList.Add(nyelv);
                        }
                    }
                }
            }
            NyelvSorszam++;
            //SectionSorszam++;

        }
        

        public void UploadTanuloKontener(TanuloKontener kontener)
        {
            SqlConnection sqlConnection = new SqlConnection(APPSETTING_SQLCONNECTIONSTRING);
            Ac4yXMLObjectPersistentService ac4YXMLObjectPersistentService = new Ac4yXMLObjectPersistentService();

            sqlConnection.Open();

            foreach(var tanulo in kontener.TanuloLista)
            {

                foreach(Nyelv nyelv in tanulo.NyelvList)
                {
                    SetByNamesResponse setByNamesResponseNyelv =
                        new Ac4yObjectObjectService(sqlConnection).SetByNames(
                            new SetByNamesRequest()
                            {
                                TemplateName = "nyelv",
                                Name = nyelv.Nev + "." + nyelv.Szint,
                            });

                    string nyelvXml = SerializeMethods.serialize(nyelv, typeof(Nyelv));
                    string nyelvGuid = setByNamesResponseNyelv.Ac4yObject.GUID;

                    ac4YXMLObjectPersistentService.Save(new Ac4yXMLObject()
                    {
                        serialization = nyelvXml,
                        GUID = nyelvGuid
                    });

                }

                tanulo.NyelvList.Clear();

                SetByNamesResponse setByNamesResponseTanulo =
                    new Ac4yObjectObjectService(sqlConnection).SetByNames(
                        new SetByNamesRequest()
                        {
                            TemplateName = "tanuló",
                            Name = tanulo.Vezeteknev + "." + tanulo.Keresztnev,
                        });

                string tanuloXml = SerializeMethods.serialize(tanulo, typeof(Tanulo));
                string tanuloGuid = setByNamesResponseTanulo.Ac4yObject.GUID;

                ac4YXMLObjectPersistentService.Save(new Ac4yXMLObject()
                {
                    serialization = tanuloXml,
                    GUID = tanuloGuid
                });
            }

            kontener.TanuloLista.Clear();

            SetByNamesResponse setByNamesResponse =
                new Ac4yObjectObjectService(sqlConnection).SetByNames(
                    new SetByNamesRequest()
                    {
                        TemplateName = "tanuló konténer",
                        Name = "associationTeszt"
                    });

            string xml = SerializeMethods.serialize(kontener, typeof(TanuloKontener));
            string guid = setByNamesResponse.Ac4yObject.GUID;
            ac4YXMLObjectPersistentService.Save(new Ac4yXMLObject()
            {
                serialization = xml,
                GUID = guid
            });
        }

        public void UploadKontenerTanuloAssociation(TanuloKontener kontener)
        {
            SqlConnection sqlConnection = new SqlConnection(APPSETTING_SQLCONNECTIONSTRING);
            sqlConnection.Open();

            foreach (var tanulo in kontener.TanuloLista) {
                Ac4yAssociationObjectService.SetByNamesResponse setByNamesResponse =
                    new Ac4yAssociationObjectService(sqlConnection).SetByNames(
                        new Ac4yAssociationObjectService.SetByNamesRequest()
                        {
                            AssociationPathName = "tanuló konténer.tanuló",
                            OriginTemplateName = "tanuló konténer",
                            OriginName = "associationTeszt",
                            TargetTemplateName = "tanuló",
                            TargetName = tanulo.Vezeteknev + "." + tanulo.Keresztnev
                        });

                foreach(Nyelv nyelv in tanulo.NyelvList)
                {
                    Ac4yAssociationObjectService.SetByNamesResponse setByNamesResponseNyelv =
                    new Ac4yAssociationObjectService(sqlConnection).SetByNames(
                        new Ac4yAssociationObjectService.SetByNamesRequest()
                        {
                            AssociationPathName = "tanuló.nyelv",
                            OriginTemplateName = "tanuló",
                            OriginName = tanulo.Vezeteknev + "." + tanulo.Keresztnev,
                            TargetTemplateName = "nyelv",
                            TargetName = nyelv.Nev + "." + nyelv.Szint
                        });
                }
            }
        }

        public void OpenXML()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                        uiTextBlock.Text = fileContent;
                    }
                }
            }
        }

        private void SaveXml(object subject, RoutedEventArgs e)
        {
            using (System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog())
            {
                // Displays a SaveFileDialog so the user can save the Image
                // assigned to Button2.

                saveFileDialog.InitialDirectory = "c:\\";
                saveFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                saveFileDialog.Title = "Save an Image File";
                saveFileDialog.ShowDialog();
                
                if(saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog.FileName, uiTextBlock.Text);
                }
            }
        }

        public void DeleteTanuloKontener(Tanulo tanulo)
        {
            SqlConnection connection = new SqlConnection(APPSETTING_SQLCONNECTIONSTRING);
            connection.Open();

            
        }


        private void deleteNyelv(object subject, RoutedEventArgs e)
        {
            var clickedButton = subject as Button;
            string wrapName = clickedButton.Tag.ToString().Substring(0, clickedButton.Tag.ToString().IndexOf(","));
            string sectionName = clickedButton.Tag.ToString().Substring(clickedButton.Tag.ToString().IndexOf(",")+1);

            foreach (var block in uiFlowDocument.Blocks)
            {
                if (block.Name.Equals(sectionName))
                {
                    Section sectionBlock = (Section)block;
                    BlockUIContainer uiContainmer = (BlockUIContainer)sectionBlock.Blocks.FirstBlock;
                    WrapPanel uiMainWrapPanel = (WrapPanel)uiContainmer.Child;
                    UIElementCollection uiInnerWrapPanels = uiMainWrapPanel.Children;
                    foreach (var uiInnerWrapPanel in uiInnerWrapPanels)
                    {
                        
                        if (uiInnerWrapPanel.GetType().Name.Equals("WrapPanel"))
                        {
                            WrapPanel wrapPanel = (WrapPanel)uiInnerWrapPanel;
                            if (wrapPanel.Name.Equals(wrapName))
                            {
                                uiMainWrapPanel.Children.Remove(wrapPanel);
                                break;
                            }
                        }
                        
                    }
                }
                
            }

            foreach(var dictionary in TanuloDictionary)
            {
                if (dictionary.Key.Equals(sectionName))
                {
                    Nyelv torolNyelv = null;
                    foreach(Nyelv nyelv in dictionary.Value.NyelvList)
                    {
                        if (nyelv.WrapPanelSorszam.Equals(wrapName))
                        {
                            torolNyelv = nyelv;
                        }
                    }
                    dictionary.Value.NyelvList.Remove(torolNyelv);
                }
            }

        }

        private void deleteButton(object subject, RoutedEventArgs e)
        {
            var clickedButton = subject as Button;
            string sectionName = clickedButton.Tag.ToString();

            List<Block> flowDocumentBlockList = uiFlowDocument.Blocks.ToList();
            uiFlowDocument.Blocks.Clear();

            foreach(var block in flowDocumentBlockList)
            {
                if (!block.Name.Equals(sectionName))
                {
                    uiFlowDocument.Blocks.Add(block);
                }
            }

            TanuloDictionary.Remove(sectionName);
            tanuloKontener.TanuloLista.Clear();
            
            
        }

        public class Person
        {
            public string name { get; set; }
            public string age { get; set; }
        }
    }
}
